///////////////////////////////////////////////////////////////////////////////
/// @file FriendsService.swift
/// @author Mikael Ferland
/// @date 2017-11-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Alamofire
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class FriendsService
/// @brief Classe qui supporte la gestion des amis
///
/// @author Mikael Ferland
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class FriendsService {
    
    private let clientConnection = HubManager.sharedConnection
    
    func sendFriendRequest(friendUsername: String) {
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/user/u/" + friendUsername, method: .get, parameters: nil, encoding: JSONEncoding.default).responseJSON { response in
                switch response.result {
                case .success(let value):
                    let newFriend = self.buildUserEntity(json: JSON(value))
                    HubManager.sharedConnection.getFriendsHub().sendFriendRequest(newFriend: newFriend)
                case .failure(let error):
                    print("Error sendFriendRequest FriendService: \(error)")
                }
            }
        }
    }
    
    func getAllUsers(completionHandler: @escaping ([UserEntity]?, Error?) -> ()) {
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/user", method: .get, parameters: nil, encoding: JSONEncoding.default).responseJSON { response in
                switch response.result {
                case .success(let value):
                    let usersJson = JSON(value)
                    let users = [UserEntity]()
                    for user in usersJson {
                        
                    }
                    completionHandler(users, nil)
                case .failure(let error):
                    completionHandler(nil, error)
                }
            }
        }
    }
    
    func buildUserEntity(json: JSON) -> UserEntity {
        let userEntity = UserEntity()
        
        userEntity.setId(id: json["Id"].int!)
        userEntity.setUsername(username: json["Username"].rawString()!)
        userEntity.setName(name: json["Name"].rawString()!)
        userEntity.setEmail(email: json["Email"].rawString()!)
        
        return userEntity
    }
    
    func buildFriendRequestEntity(json: JSON) -> FriendRequestEntity {
        // Build requestor object
        let requestor = UserEntity()
        requestor.setId(id: json["Requestor"]["Id"].int != nil ? json["Requestor"]["Id"].int! : -1)
        requestor.setUsername(username: json["Requestor"]["Username"].string != nil ? json["Requestor"]["Username"].string! : "")
        requestor.setName(name: json["Requestor"]["Name"].string != nil ? json["Requestor"]["Name"].string! : "")
        requestor.setEmail(email: json["Requestor"]["Email"].string != nil ? json["Requestor"]["Email"].string! : "")
        
        // Build friend object
        let friend = UserEntity()
        friend.setId(id: json["Friend"]["Id"].int != nil ? json["Friend"]["Id"].int! : -1)
        friend.setUsername(username: json["Friend"]["Username"].string != nil ? json["Friend"]["Username"].string! : "")
        friend.setName(name: json["Friend"]["Name"].string != nil ? json["Friend"]["Name"].string! : "")
        friend.setEmail(email: json["Friend"]["Email"].string != nil ? json["Friend"]["Email"].string! : "")
        
        // Build friend request object
        let status = json["Status"].int != nil ? RequestStatus(rawValue: json["Status"].int!) : nil
        return FriendRequestEntity(requestor: requestor, friend: friend, status: status!)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
