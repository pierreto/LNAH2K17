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
    var ipAddressError: String
    var usernameError: String
    var passwordError: String
    
    private let clientConnection = ClientConnection.sharedConnection
    
    override init() {
        self.ipAddressError = ""
        self.usernameError = ""
        self.passwordError = ""
        super.init()
    }
    
    // Mark: Functions
    func validateFields(ipAddress: String, username: String, password: String) -> Promise<Bool> {
        let validIp = validateIPAddress(ipAddress: ipAddress)
        let validUsername = validateUsername(username: username)
        let validPassword = validatePassword(password: password)
        NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
        if(validIp && validUsername && validPassword) {
            return connectToServer(ipAddress: ipAddress, username: username, password: password)
                .then { data -> Promise<Bool> in
                    print("Connection is", data)
                    if(data) {
                        return Promise { fullfil, error in
                            let parameters: [String: String] = [
                                "Username" : username,
                                "Password" : password
                            ]
                            // self.registerUsername(ipAddress: ipAddress, username: username, password: password)
                            Alamofire.request("http://" + ipAddress + ":63056/api/login", method: .post, parameters: parameters, encoding: JSONEncoding.default)
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
                }.always {}
        } else {
            return Promise(value: false)
        }
    }
    
    // Mark: Private Functions
    fileprivate func validateIPAddress(ipAddress: String) -> Bool {
        let validIpAddressRegex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"
        let ipMatches = ipAddress.range(of: validIpAddressRegex, options: .regularExpression)
        if ipAddress.isEmpty {
            self.ipAddressError = "Adresse IP requise"
            return false
        } else if (ipMatches != nil) {
            self.ipAddressError = ""
            return true
        } else {
            self.ipAddressError = "Adresse IP invalide"
            return false
        }
    }
    
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
    
    /// Établir la connexion au serveur via SignalR
    private func connectToServer(ipAddress: String, username: String, password: String) -> Promise<Bool> {
        return Promise { fullfil, error in
            clientConnection.EstablishConnection(ipAddress: ipAddress, hubName: "ChatHub")
            /// Avertir l'utilisateur s'il n'est pas possible de se connecter au serveur après 5 secondes
            let timerTask = DispatchWorkItem {
                if !(ClientConnection.sharedConnection.getConnection().state == .connected) {
                    self.ipAddressError = "Impossible de se connecter à cette adresse"
                    NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                    fullfil(false)
                }
            }
            
            let timer = DispatchTime.now() + 5 // start a timer
            DispatchQueue.main.asyncAfter(deadline: timer, execute: timerTask)
            
            /// Avertir l'utilisateur en cas d'erreur au moment de la connexion
            clientConnection.getConnection().error = { error in
                self.ipAddressError = "Une erreur est survenue durant la connection"
                NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                fullfil(false)
                timerTask.cancel()
            }
            
            clientConnection.getConnection().connectionFailed = { error in
                print("Connection failed")
                self.ipAddressError = "La connexion a échouée"
                NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                fullfil(false)
                timerTask.cancel()
            }
            
            /// Transmettre un message reçu du serveur au ChatViewController
            clientConnection.getChatHub().on("ChatMessageReceived") { args in
                ChatViewController.sharedChatViewController.receiveMessage(message: args?[0] as! Dictionary<String, String>)
            }
            
            /// Connexion au serveur réussie
            clientConnection.getConnection().connected = {
                print("Connected with ip: " + ipAddress)
                fullfil(true)
                self.clientConnection.setIpAddress(ipAddress: ipAddress)
                self.ipAddressError = ""
                NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                timerTask.cancel()
            }
        }
    }
}
