//
//  Login.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//
import Alamofire
import PromiseKit
import Foundation
import SwiftyJSON

enum LoginNotification {
    static let SubmitNotification = "SubmitNotification"
    static let LogoutNotification = "LogoutNotification"
}
class Login: NSObject {
    
    // Mark: Properties
    var usernameError: String
    var passwordError: String
    
    private let clientConnection = HubManager.sharedConnection
    
    override init() {
        self.usernameError = ""
        self.passwordError = ""
        super.init()
    }
    
    // Mark: Functions
    func validateFields(username: String, password: String) -> Promise<Bool> {
        let validUsername = validateUsername(username: username)
        let validPassword = validatePassword(password: password)
        NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
        if(validUsername && validPassword) {
            return Promise { fullfil, error in
                let parameters: [String: String] = [
                    "Username" : username,
                    "Password" : password
                ]

                Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/login", method: .post, parameters: parameters, encoding: JSONEncoding.default)
                    .responseJSON { response in
                        if(response.response?.statusCode == 200) {
                            self.clientConnection.setUsername(username: username)
                            if let result = response.result.value {
                                let id = result as! Int
                                self.clientConnection.setId(id: id)
                            }
                            
                            // Connect user to chat
                            HubManager.sharedConnection.getChatHub().subscribe()
                            
                            // Retrieve the users friends
                            HubManager.sharedConnection.getFriendsHub().initialize()
                            HubManager.sharedConnection.getFriendsHub().getAllFriends()
                            
                            // Upload local maps to server (under users id)
                            let mapService = MapService()
                            mapService.exportLocalMapsToServer()
                            
                            // Initialize instance of DBManager to start map fetching from server
                            // DBManager.instance.startMapFetching()
                            
                            fullfil(true)
                        } else {
                            //Getting error message from server
                            if let data = response.data {
                                let responseJSON = JSON(data: data)
                                
                                if let message: String = responseJSON.stringValue {
                                    if !message.isEmpty {
                                        self.usernameError = message
                                    }
                                }
                            }
                            NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                            fullfil(false)
                        }
                }
            }
        } else {
            return Promise(value: false)
        }
    }
    
    // Mark: Private Functions
    fileprivate func validateUsername(username: String) -> Bool {
        if (username.isEmpty) {
            self.usernameError = "Nom d'usager requis"
            return false
        }
        self.usernameError = ""
        return true
    }
    
    fileprivate func validatePassword(password: String) -> Bool {
        if(password.isEmpty) {
            self.passwordError = "Mot de passe requis"
            return false;
        }
        self.passwordError = ""
        return true
    }
}
