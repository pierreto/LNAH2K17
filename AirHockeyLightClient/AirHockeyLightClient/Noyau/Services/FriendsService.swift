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
                    print(value)
                    let newFriend = self.buildUserEntity(json: JSON(value))
                    HubManager.sharedConnection.getFriendsHub().sendFriendRequest(newFriend: newFriend)
                case .failure(let error):
                    print("Error sendFriendRequest FriendService: \(error)")
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

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
