///////////////////////////////////////////////////////////////////////////////
/// @file ClientConnection.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation
import SwiftR

///////////////////////////////////////////////////////////////////////////
/// @class ClientConnection
/// @brief Classe singleton pour gérer la connection avec le serveur via
///        SignalR
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-22
///////////////////////////////////////////////////////////////////////////
class ClientConnection {
    
    /// Instance singleton de l'objet ClientConnection
    static let sharedConnection = ClientConnection()
    
    private var connection: SignalR?
    private var chatHub: Hub?
    /// Adresse IP du serveur
    private var ipAddress: String?
    private var username: String?
    
    public func getConnection() -> SignalR {
        return self.connection!
    }
    
    public func getChatHub() -> Hub {
        return self.chatHub!
    }
    
    public func getIpAddress() -> String {
        return self.ipAddress!
    }
    
    public func setIpAddress(ipAddress: String) {
        self.ipAddress = ipAddress
    }
    
    public func getUsername() -> String {
        return self.username!
    }
    
    public func setUsername(username: String) {
        self.username = username
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn EstablishConnection(ipAddress: String, hubName: String)
    ///
    /// Entammer une connection avec le serveur.
    ///
    /// @param[in] ipAddress : L'adresse IP du serveur
    /// @param[in] hubName : Nom du hub hébergé dans le serveur
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    public func EstablishConnection(ipAddress: String, hubName: String) {
        self.connection = SignalR("http://" + ipAddress + ":63056")
        self.chatHub = Hub(hubName)
        
        self.connection!.starting = { print("started") }
        self.connection!.connected = { print("connected: \(String(describing: self.connection!.connectionID))") }
        self.connection!.connectionSlow = { print("connectionSlow") }
        self.connection!.reconnecting = { print("reconnecting") }
        self.connection!.reconnected = { print("reconnected") }
        self.connection!.disconnected = { print("disconnected") }
        
        self.connection!.addHub(chatHub!)
        self.connection!.start()
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn Disconnect(username: String)
    ///
    /// Dé-enregistrer l'utilisateur et fermer la connection au serveur.
    ///
    /// @param[in] username : Le nom d'usager du client
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    public func Disconnect(username: String) {
        do {
            try chatHub!.invoke("Disconnect", arguments: [username], callback: { (response) in
                self.connection!.stop()
            })
        }
        catch {
            print("Error Disconnect")
        }
    }

    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn SendBroadcast(message: Any)
    ///
    /// Envoyer un message à tous les autres utilisateurs.
    ///
    /// @param[in] message : Le message à envoyer
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    public func SendBroadcast(message: Any) {
        do {
            try chatHub!.invoke("SendBroadcast", arguments: [message])
        }
        catch {
            print("Error SendBroadcast")
        }
    }
  
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn SendChannel(channelName: String, message: Any)
    ///
    /// Envoyer un message à un canal spécifique
    ///
    /// @param[in] channelName : Le nom du canal
    /// @param[in] message : Le message à envoyer
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    public func SendChannel(channelName: String, message: Any) {
        do {
            try chatHub!.invoke("SendChannel", arguments: [channelName, message])
        }
        catch {
            print("Error SendChannel")
        }
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn SendPrivate(userId: guid_t, message: Any)
    ///
    /// Envoyer un message à un utilisateur spécifique
    ///
    /// @param[in] userId : L'identifiant de l'utilisateur
    /// @param[in] message : Le message à envoyer
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    public func SendPrivate(userId: guid_t, message: Any) {
        do {
            try chatHub!.invoke("SendBroadcast", arguments: [userId, message])
        }
        catch {
            print("Error SendPrivate")
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
