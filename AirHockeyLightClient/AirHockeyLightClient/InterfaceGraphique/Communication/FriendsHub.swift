///////////////////////////////////////////////////////////////////////////////
/// @file FriendsHub.swift
/// @author Mikael Ferland
/// @date 2017-11-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftR

///////////////////////////////////////////////////////////////////////////
/// @class FriendsHub
/// @brief Classe pour gérer les appels au serveur pour la liste d'amis
///        d'un usager
///
/// @author Mikael Ferland
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class FriendsHub: BaseHub {
    
    init(connection: SignalR?) {
        super.init()
        self.hubProxy = connection?.createHubProxy("FriendsHub")
    }
    
    func initialize() {
        do {
            let user = HubManager.sharedConnection.getUser().toDictionary()
            try self.hubProxy?.invoke("JoinHub", arguments: [user])
        }
        catch {
            print("Error JoinHub")
        }
        
        self.InitializeEvents()
    }
    
    private func InitializeEvents() {
        self.hubProxy?.on("FriendRequestEvent") { args in
            print("FriendRequestEvent")
        }
        
        self.hubProxy?.on("NewFriendEvent") { args in
            print("NewFriendEvent")
        }
        
        self.hubProxy?.on("RemovedFriendEvent") { args in
            print("RemovedFriendEvent")
        }
        
        self.hubProxy?.on("CanceledFriendRequestEvent") { args in
            print("CanceledFriendRequestEvent")
        }
    }
    
    func getAllFriends() {
        do {
            let user = HubManager.sharedConnection.getUser().toDictionary()
            try self.hubProxy?.invoke("GetAllFriends", arguments: [user]) { (result, error) in
                if let e = error {
                    print("Error GetAllFriends FriendsHub: \(e)")
                }
                else {
                    let friends = result as! [UserEntity]
                    FriendsTableViewController.instance.updateFriendsEntries(friends: friends)
                }
            }
        }
        catch {
            print("Error GetAllFriends")
        }
    }
    
    func getAllPendingRequest() {
        do {
            let user = HubManager.sharedConnection.getUser().toDictionary()
            try self.hubProxy?.invoke("GetAllPendingRequests", arguments: [user]) { (result, error) in
                if let e = error {
                    print("Error GetAllFriends FriendsHub: \(e)")
                }
                else {
                    let pendingRequests = result as! [FriendRequestEntity]
                    FriendsTableViewController.instance.updatePendingRequestsEntries(pendingRequests: pendingRequests)
                }
            }
        }
        catch {
            print("Error GetAllPendingRequests")
        }
    }
    
    func sendFriendRequest(newFriend : UserEntity) {
        do {
            let userDictionary = HubManager.sharedConnection.getUser().toDictionary()
            let friendDictionary = newFriend.toDictionary()
            try self.hubProxy?.invoke("SendFriendRequest", arguments: [userDictionary, friendDictionary]) { (result, error) in
                if let e = error {
                    print("Error GetAllFriends FriendsHub: \(e)")
                }
            }
        }
        catch {
            print("Error SendFriendRequest")
        }
    }
    
    func acceptFriendRequest(request : FriendRequestEntity) {
        do {
            let user = HubManager.sharedConnection.getUser().toDictionary()
            try self.hubProxy?.invoke("AcceptFriendRequest", arguments: [user])
        }
        catch {
            print("Error AcceptFriendRequest")
        }
    }
    
    func refuseFriendRequest(request : FriendRequestEntity) {
        do {
            let user = HubManager.sharedConnection.getUser().toDictionary()
            try self.hubProxy?.invoke("RefuseFriendRequest", arguments: [user])
        }
        catch {
            print("Error RefuseFriendRequest")
        }
    }
    
    func removeFriend(exFriend : UserEntity) {
        do {
            let user = HubManager.sharedConnection.getUser().toDictionary()
            try self.hubProxy?.invoke("RemoveFriend", arguments: [user, exFriend])
        }
        catch {
            print("Error RemoveFriend")
        }
    }
    
    override func logout() {
        // TODO
        print("logout")
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
