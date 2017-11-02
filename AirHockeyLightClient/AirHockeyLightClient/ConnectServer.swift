//
//  ConnectServer.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-10-18.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//
import PromiseKit
import Foundation

enum ConnectServerNotification {
    static let ConnectServerNotification = "SubmitNotification"
}

class ConnectServer: NSObject {
    
    // Mark: Properties
    var ipAddressError: String
    private let clientConnection = HubManager.sharedConnection
    
    override init() {
        print("init")
        self.ipAddressError = ""
        super.init()
    }
    
    func validateIp(ipAddress: String) -> Promise<Bool> {
        if (!validateIPAddress(ipAddress: ipAddress)) {
            NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
            return Promise(value: false)
        }
        return connectToServer(ipAddress: ipAddress)
    }
    
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
    
    private func connectToServer(ipAddress: String) -> Promise<Bool> {
        return Promise { fullfil, error in
            clientConnection.EstablishConnection(ipAddress: ipAddress, hubName: "ChatHub")
            /// Avertir l'utilisateur s'il n'est pas possible de se connecter au serveur après 5 secondes
            let timerTask = DispatchWorkItem {
                if !(HubManager.sharedConnection.getConnection()?.state == .connected) {
                    self.ipAddressError = "Adresse non joinable"
                    NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                    fullfil(false)
                }
            }
            
            let timer = DispatchTime.now() + 5 // start a timer
            DispatchQueue.main.asyncAfter(deadline: timer, execute: timerTask)
            
            /// Avertir l'utilisateur en cas d'erreur au moment de la connexion
            clientConnection.getConnection()?.error = { error in
                self.ipAddressError = "Une erreur est survenue durant la connection"
                NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                fullfil(false)
                timerTask.cancel()
            }
            
            clientConnection.getConnection()?.connectionFailed = { error in
                print("Connection failed")
                self.ipAddressError = "La connexion a échouée"
                NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: self)
                fullfil(false)
                timerTask.cancel()
            }
            
            /// Transmettre un message reçu du serveur au ChatViewController
            clientConnection.getChatHub().getHub().on("ChatMessageReceived") { args in
                ChatViewController.sharedChatViewController.receiveMessage(message: args?[0] as! Dictionary<String, String>)
            }
            
            /// Connexion au serveur réussie
            clientConnection.getConnection()?.connected = {
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
