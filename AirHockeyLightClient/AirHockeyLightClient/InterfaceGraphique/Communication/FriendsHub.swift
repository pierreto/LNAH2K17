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
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class FriendsHub
/// @brief Classe pour gérer les appels au serveur pour la liste d'amis
///        d'un usager
///
/// @author Mikael Ferland
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class FriendsHub: BaseHub {
    
    private let friendsService = FriendsService()
    
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
    }
    
    override func initializeHub() {
        self.hubProxy?.on("FriendRequestEvent") { args in
            let newRequestJson = JSON(args?[0] as! Dictionary<String, Any>)
            let newRequest = self.friendsService.buildFriendRequestEntity(json: newRequestJson)
            
            if FriendRequestsTableViewController.instance != nil {
                FriendRequestsTableViewController.instance?.addRequest(newRequest: newRequest)
            } else if FriendRequestsViewController.instance != nil {
                FriendRequestsViewController.instance?.displayNotification()
            }
        }
        
        self.hubProxy?.on("NewFriendEvent") { args in
            let newFriendJson = JSON(args?[0] as! Dictionary<String, Any>)
            let newFriend = self.friendsService.buildUserEntity(json: newFriendJson)
            
            FriendsTableViewController.instance.addFriend(newFriend: newFriend)
        }
        
        self.hubProxy?.on("RemovedFriendEvent") { args in
            let exFriendJson = JSON(args?[0] as! Dictionary<String, Any>)
            let exFriend = self.friendsService.buildUserEntity(json: exFriendJson)
            
            FriendsTableViewController.instance.removeFriend(exFriend: exFriend)
        }
        
        self.hubProxy?.on("NewAddableFriendEvent") { args in
            let newUserJson = JSON(args?[0] as! Dictionary<String, Any>)
            let newUser = self.friendsService.buildUserEntity(json: newUserJson)
            
            if AddFriendViewController.instance != nil {
                AddFriendViewController.instance?.addNewAddableUser(username: newUser.getUsername())
            }
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
                    let friendsJson = JSON(result as! [Dictionary<String, Any>])
                    var friends = [UserEntity]()
                    
                    for friend in friendsJson {
                        friends.append(self.friendsService.buildUserEntity(json: friend.1))
                    }
                    
                    FriendsTableViewController.instance.updateAllFriends(friends: friends)
                    
                    // Update add friend view
                    if AddFriendViewController.instance != nil {
                        AddFriendViewController.instance?.filterUserEntries(friends: friends)
                    }
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
                    print("Error GetAllPendingRequests FriendsHub: \(e)")
                }
                else {
                    let requestsJson = JSON(result as! [Dictionary<String, Any>])
                    var pendingRequests = [FriendRequestEntity]()
                    
                    for request in requestsJson {
                        let req = self.friendsService.buildFriendRequestEntity(json: request.1)
                        if req.getStatus() == RequestStatus.PENDING {
                            pendingRequests.append(req)
                        }
                    }
                    
                    // Update friend requests view
                    if pendingRequests.count > 0 {
                        FriendRequestsViewController.instance?.displayNotification()
                    }
                    if FriendRequestsTableViewController.instance != nil {
                        FriendRequestsTableViewController.instance?.updatePendingRequestsEntries(pendingRequests: pendingRequests)
                    }
                    
                    // Update add friend view
                    if AddFriendViewController.instance != nil {
                        AddFriendViewController.instance?.filterUserEntries(pendingRequests: pendingRequests)
                    }
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
                    print("Error SendFriendRequest FriendsHub: \(e)")
                }
            }
        }
        catch {
            print("Error SendFriendRequest")
        }
    }
    
    func acceptFriendRequest(request : FriendRequestEntity) {
        do {
            try self.hubProxy?.invoke("AcceptFriendRequest", arguments: [request.toDictionary()]) { (result, error) in
                if let e = error {
                    print("Error AcceptFriendRequest FriendsHub: \(e)")
                }
            }
        }
        catch {
            print("Error AcceptFriendRequest")
        }
    }
    
    func refuseFriendRequest(request : FriendRequestEntity) {
        do {
            try self.hubProxy?.invoke("RefuseFriendRequest", arguments: [request.toDictionary()]) { (result, error) in
                if let e = error {
                    print("Error RefuseFriendRequest FriendsHub: \(e)")
                }
            }
        }
        catch {
            print("Error RefuseFriendRequest")
        }
    }
    
    func removeFriend(exFriend : UserEntity) {
        do {
            let userDictionary = HubManager.sharedConnection.getUser().toDictionary()
            let exFriendDictionary = exFriend.toDictionary()
            try self.hubProxy?.invoke("RemoveFriend", arguments: [userDictionary, exFriendDictionary])
        }
        catch {
            print("Error RemoveFriend")
        }
    }
    
    override func logout() {
        print("logout friends hub")
        
        if FriendsTableViewController.instance != nil {
            FriendsTableViewController.instance.resetFriendsList()
        }
        
        if FriendRequestsTableViewController.instance != nil {
            FriendRequestsTableViewController.instance?.resetFriendRequests()
        }
        
        if AddFriendViewController.instance != nil {
            AddFriendViewController.instance?.resetFilterEntries()
        }
        
        self.hubProxy = nil
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
