///////////////////////////////////////////////////////////////////////////////
/// @file ChatHub.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-11-01
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftR

class ChatHub: BaseHub {    
    init(connection: SignalR?) {
        super.init()
        self.hubProxy = connection?.createHubProxy("ChatHub")
        
        /// Transmettre un message reçu du serveur au ChatViewController
        self.hubProxy?.on("ChatMessageReceived") { args in
            ChatAreaViewController.sharedChatAreaViewController.receiveMessage(message: args?[0] as! Dictionary<String, String>)
        }
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
    public override func logout() {
        /*do {
            try chatHub!.invoke("Disconnect", arguments: [username], callback: { (response) in
                print("ChatHub Disconnect Success")
            })
        }
        catch {
            print("Error Disconnect")
        }*/
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
            try hubProxy!.invoke("SendBroadcast", arguments: [message])
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
            try hubProxy!.invoke("SendChannel", arguments: [channelName, message])
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
            try hubProxy!.invoke("SendBroadcast", arguments: [userId, message])
        }
        catch {
            print("Error SendPrivate")
        }
    }

    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
