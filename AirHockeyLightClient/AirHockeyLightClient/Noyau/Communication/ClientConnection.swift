//
//  ClientConnection.swift
//  AirHockeyLightClient
//
//  Created by Mikael Ferland and Pierre To on 17-09-22.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import Foundation
import SwiftR

class ClientConnection {
    static let sharedConnection = ClientConnection()
    private var connection: SignalR?
    private var chatHub: Hub?
    
    private var ipAddress: String?
    private var username: String?
    
    public func getConnection() -> SignalR {
        return connection!
    }
    
    public func getChatHub() -> Hub {
        return chatHub!
    }
    
    public func getIpAddress() -> String {
        return ipAddress!
    }
    
    public func setIpAddress(ipAddress: String) {
        self.ipAddress = ipAddress
    }
    
    public func getUsername() -> String {
        return username!
    }
    
    public func setUsername(username: String) {
        self.username = username
    }
    
    public func EstablishConnection(ipAddress: String, hubName: String) {
        connection = SignalR("http://" + ipAddress + ":63056")
        chatHub = Hub(hubName)
        
        connection!.starting = { print("started") }
        connection!.connected = { print("connected: \(String(describing: self.connection!.connectionID))") }
        connection!.connectionSlow = { print("connectionSlow") }
        connection!.reconnecting = { print("reconnecting") }
        connection!.reconnected = { print("reconnected") }
        connection!.disconnected = { print("disconnected") }
        
        connection!.addHub(chatHub!)
        connection!.start()
    }
    
    public func RegisterUsername(username: String) {
        
    }
    
    public func Disconnect(username: String) {
        do {
            try chatHub!.invoke("Disconnect", arguments: [username], callback: { (response) in
                self.StopConnection()
            })
        }
        catch {
            print("Error Disconnect")
        }
    }
    
    public func StopConnection() {
        ClientConnection.sharedConnection.getConnection().stop()
    }
    
    
    public func SendBroadcast(message: Any) {
        do {
            try chatHub!.invoke("SendBroadcast", arguments: [message])
        }
        catch {
            print("Error SendBroadcast")
        }
    }
    
    public func SendChannel(channelName: String, message: Any) {
        do {
            try chatHub!.invoke("SendChannel", arguments: [channelName, message])
        }
        catch {
            print("Error SendChannel")
        }
    }
    
    public func SendPrivate(userId: guid_t, message: Any) {
        do {
            try chatHub!.invoke("SendBroadcast", arguments: [userId, message])
        }
        catch {
            print("Error SendPrivate")
        }
    }
}
