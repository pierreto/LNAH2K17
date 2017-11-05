///////////////////////////////////////////////////////////////////////////////
/// @file UserManager.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class UserManager
/// @brief Classe qui gère les utilisateurs en ligne
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class UserManager {
    
    private var users: Dictionary<String, OnlineUser>
    
    init() {
        self.users = Dictionary<String, OnlineUser>()
    }
    
    deinit {
        self.users.removeAll()
        self.users = Dictionary<String, OnlineUser>()
    }
    
    public func getUsers() -> Dictionary<String, OnlineUser> {
        return self.users
    }
    
    public func getUser(username: String) -> OnlineUser {
        return self.users[username]!
    }
    
    public func addUser(username: String, hexColor: String) {
        self.users.updateValue(OnlineUser(username: username, hexColor: hexColor), forKey: username)
    }
    
    public func removeUser(username: String) {
        let index = self.users.index(forKey: username)
        self.users.remove(at: index!)
    }
    
    public func userExist(username: String) -> Bool {
        return self.users.keys.contains(username)
    }
    
    public func clearUsers() {
        self.users.removeAll()
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
