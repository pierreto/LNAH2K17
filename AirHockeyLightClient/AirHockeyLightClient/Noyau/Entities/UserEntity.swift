///////////////////////////////////////////////////////////////////////////////
/// @file UserEntity.swift
/// @author Mikael Ferland
/// @date 2017-11-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
/// @class UserEntity
/// @brief Classe pour encapsuler les attributs d'un utilisateur
///
/// @author Mikael Ferland
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class UserEntity : Entity {
    
    private var id : Int
    private var username : String
    private var name : String
    private var email : String
    
    init(id: Int, username: String, name: String, email: String) {
        self.id = id
        self.username = username
        self.name = name
        self.email = email
    }
    
    func getId() -> Int {
        return self.id
    }
    
    func setId(id: Int) {
        self.id = id
    }
    
    func getUsername() -> String {
        return self.username
    }
    
    func setUsername(username: String) {
        self.username = username
    }
    
    func getName() -> String {
        return self.name
    }
    
    func setName(name: String) {
        self.name = name
    }
    
    func getEmail() -> String {
        return self.email
    }
    
    func setEmail(email: String) {
        self.email = email
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
