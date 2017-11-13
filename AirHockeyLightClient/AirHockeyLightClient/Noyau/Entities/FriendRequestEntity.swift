///////////////////////////////////////////////////////////////////////////////
/// @file FriendRequestEntity.swift
/// @author Mikael Ferland
/// @date 2017-11-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

enum RequestStatus : Int {
    case REFUSED = -1
    case PENDING = 0
    case ACCEPTED = 1
}

///////////////////////////////////////////////////////////////////////////
/// @class FriendRequestEntity
/// @brief Classe pour encapsuler les attributs d'une demande d'amitié
///
/// @author Mikael Ferland
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class FriendRequestEntity : Entity {
    
    private var id : Int
    private var requestor : UserEntity
    private var friend : UserEntity
    private var status : RequestStatus
    
    init(id: Int, requestor: UserEntity, friend: UserEntity, status: RequestStatus) {
        self.id = id
        self.requestor = requestor
        self.friend = friend
        self.status = status
    }
    
    func getId() -> Int {
        return self.id
    }
    
    func setId(id: Int) {
        self.id = id
    }
    
    func getRequestor() -> UserEntity {
        return self.requestor
    }
    
    func setRequestor(requestor: UserEntity) {
        self.requestor = requestor
    }
    
    func getFriend() -> UserEntity {
        return self.friend
    }
    
    func setFriend(friend: UserEntity) {
        self.friend = friend
    }
    
    func getStatus() -> RequestStatus {
        return self.status
    }
    
    func setStatus(status: RequestStatus) {
        self.status = status
    }
    
    func toDictionary() -> [String: Any] {
        let request = [
            "Id": self.id,
            "Requestor": self.requestor.toDictionary(),
            "Friend": self.friend.toDictionary(),
            "Status": self.status
            ] as [String : Any]
        
        return request
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
