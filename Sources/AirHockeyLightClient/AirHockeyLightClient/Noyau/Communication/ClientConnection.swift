//
//  ClientConnection.swift
//  AirHockeyLightClient
//
//  Created by Pierre To on 17-09-22.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import Foundation
import SwiftR

class ClientConnection {
    static let sharedConnection = ClientConnection()
    private var connection: SignalR?
    private var chatHub: Hub?
    
    private var username: String?
    
    public func getChatHub() -> Hub {
        return chatHub!
    }
    
    public func getUsername() -> String {
        return username!
    }
    
    public func setUsername(username: String) {
        self.username = username
    }
    
    public func EstablishConnection() {
        connection = SignalR("http://192.168.0.118:63056")
        chatHub = Hub("ChatHub")
        
        connection!.starting = { print("started") }
        connection!.connected = { print("connected: \(String(describing: self.connection!.connectionID))") }
        connection!.connectionSlow = { print("connectionSlow") }
        connection!.reconnecting = { print("reconnecting") }
        connection!.reconnected = { print("reconnected") }
        connection!.disconnected = { print("disconnected") }
        
        connection!.addHub(chatHub!)
        connection!.start()
    }
    
    public func SendBroadcast(username: String, message: String) {
        do {
            try chatHub!.invoke("SendBroadcast", arguments: [username, message])
        }
        catch {
            print("Error SendBroadcast")
        }
    }
    
    public func SendChannel(channelName: String, message: String) {
        do {
            try chatHub!.invoke("SendChannel", arguments: [channelName, message])
        }
        catch {
            print("Error SendChannel")
        }
    }
    
    public func SendPrivate(userId: guid_t, message: String) {
        do {
            try chatHub!.invoke("SendBroadcast", arguments: [userId, message])
        }
        catch {
            print("Error SendPrivate")
        }
    }
}
