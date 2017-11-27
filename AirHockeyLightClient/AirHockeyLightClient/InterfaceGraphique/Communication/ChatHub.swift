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
        
        self.hubProxy?.on("ChatMessageReceivedChannel") { args in
            ChatAreaViewController.sharedChatAreaViewController.receiveMessageChannel(message: args?[0] as! Dictionary<String, String>, channelName: args?[1] as! String)
        }
        
        self.hubProxy?.on("NewJoinableChannel") { args in
             ChatAreaViewController.sharedChatAreaViewController.newJoinableChannel(channelName: args?[0] as! String)
        }
        
        self.hubProxy?.on("ChannelDeleted") { args in
             ChatAreaViewController.sharedChatAreaViewController.channelDeleted(channelName: args?[0] as! String)
        }

        self.hubProxy?.on("ChatMessageReceivedPrivate") { args in
            ChatAreaViewController.sharedChatAreaViewController.receiveMessagePrivate(message: args?[0] as! Dictionary<String, String>, senderId: args?[1] as! Int)
        }

        self.hubProxy?.on("PrivateChannelCreated") { args in
            ChatAreaViewController.sharedChatAreaViewController.newPrivateChannel(name: args?[0] as! String, othersId: args?[1] as! Int, othersProfile: args?[2] as! String)
        }
    }
    
    func subscribe(){
        do {
            try hubProxy?.invoke("Subscribe", arguments: [HubManager.sharedConnection.getId()!]) { (result,error) in
                if let e = error {
                    print("Error Subscribe ChatHub: \(e)")
                }
                else {
                    let channelNames = result as! [String]
                    for channelName in channelNames {
                        ChatAreaViewController.sharedChatAreaViewController.channelsToJoin.append(ChannelEntity(name: channelName))
                    }
                    ChatAreaViewController.sharedChatAreaViewController.joinChannelTableView.reloadData()
                }
            }
        }
        catch {
            print("Error Subscribe ChatHub")
        }
    }
    
    func joinChannel(channelName: String) {
        do {
            try hubProxy?.invoke("JoinChannel", arguments: [channelName]) { (result,error) in
                if let e = error {
                    print("Error JoinChannel ChatHub: \(e)")
                }
                else {
                    MasterViewController.sharedMasterViewController.joinChannel(channelName: channelName)
                    /*MasterViewController.sharedMasterViewController.channels.append(ChannelEntity(name: channelName))
                    MasterViewController.sharedMasterViewController.channelTableView.reloadData()
                    //Set joined channel as selected
                    let indexPath = IndexPath(row: MasterViewController.sharedMasterViewController.channels.count - 1, section: 0);
                    MasterViewController.sharedMasterViewController.channelTableView.selectRow(at: indexPath, animated: true, scrollPosition: .bottom)
                    MasterViewController.sharedMasterViewController.channelTableView.delegate?.tableView!(MasterViewController.sharedMasterViewController.channelTableView, didSelectRowAt: indexPath)*/
                    if let index = ChatAreaViewController.sharedChatAreaViewController.channelsToJoin.index(where: { $0.name == channelName }) {
                        ChatAreaViewController.sharedChatAreaViewController.channelsToJoin.remove(at: index)
                        ChatAreaViewController.sharedChatAreaViewController.joinChannelTableView.reloadData()
                    }
                }
            }
        }
        catch {
            print("Error JoinChannel ChatHub")
        }
    }
    
    func leaveRoom(roomName: String) {
        do {
            try hubProxy?.invoke("LeaveRoom", arguments: [roomName, HubManager.sharedConnection.getId()!]);
        }
        catch {
            print("Error JoinChannel ChatHub")
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
        do {
            print("logout chat hub")
            var roomNames = MasterViewController.sharedMasterViewController.channels.map({ (cE: ChannelEntity) -> String in
                cE.getName()
            })
            //Principal ne pas quitter
            roomNames.remove(at: 0)
            try hubProxy?.invoke("Disconnect", arguments: [roomNames, HubManager.sharedConnection.getId()!]);

            self.hubProxy = nil
        }
        catch {
            print("Error JoinChannel ChatHub")
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
            try hubProxy?.invoke("SendBroadcast", arguments: [message])
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
    public func SendChannel(channelName: String, message: [String : Any]) {
        do {
            try hubProxy?.invoke("SendChannel", arguments: [channelName, message])
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
    public func SendPrivate(message:[String : Any], senderId: Int, receptorId: Int) {
        do {
            try hubProxy?.invoke("SendPrivateMessage", arguments: [message, senderId, receptorId])
        }
        catch {
            print("Error SendPrivate")
        }
    }

    public func CreatePrivateChannel(othersName: String, myId: Int, othersId: Int, othersProfile: String, res: @escaping (_ message: Bool?) -> Void) {
        do {
            try hubProxy!.invoke("CreatePrivateChannel", arguments: [othersName, myId, othersId, othersProfile]) { (result, error) in
                if let e = error {
                    print("Error CreatePrivateChannel: \(e)")
                }
                else {
                    res(result as? Bool)
                }
            }
        }
        catch {
            print("Error SendPrivate")
        }
    }
    
    public func CreateChannel(channelName: String, res: @escaping (_ message: String?) -> Void) {
        var msg: String?
        do {
            try hubProxy?.invoke("CreateChannel", arguments: [channelName]) { (result, error) in
                if let e = error {
                    print("Error CreateChannel: \(e)")
                }
                else {
                    if(result as! Bool){
                        msg = ""
                    } else {
                        msg = "Canal déjà créé"
                    }
                    res(msg)
                }
            }
        }
        catch{
            print("Error CreateChannel")
        }
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

