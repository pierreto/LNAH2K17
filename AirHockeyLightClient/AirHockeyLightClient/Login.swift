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

enum LoginNotification {
    static let SubmitNotification = "SubmitNotification"
}
class Login: NSObject {
    
    // Mark: Properties
    var usernameError: String
    var passwordError: String
    
    private let clientConnection = ClientConnection.sharedConnection
    
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

                Alamofire.request("http://" + self.clientConnection.getIpAddress() + ":63056/api/login", method: .post, parameters: parameters, encoding: JSONEncoding.default)
                    .responseJSON { response in
                        if(response.result.value as! Int == 1) {
                            self.clientConnection.setUsername(username: username)
                            fullfil(true)
                        } else {
                            self.usernameError = "Veuillez entrer un nom d'usager valide"
                            self.passwordError = "Veuillez entrer un mot de passe valide"
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
