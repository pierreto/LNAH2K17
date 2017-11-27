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
    
    private var id : Int = 0
    private var username : String = ""
    private var name : String = ""
    private var email : String = ""
    private var profile: String = ""
    private var alreadyUsedLightEditor: Bool = false
    
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
    
    func getProfile() -> String {
        return self.profile
    }
    
    func setProfile(profile: String) {
        self.profile = profile
    }
    
    func getAlreadyUsedLightEditor() -> Bool {
        return self.alreadyUsedLightEditor
    }
    
    func setAlreadyUsedLightEditor(alreadyUsedLightEditor: Bool) {
        self.alreadyUsedLightEditor = alreadyUsedLightEditor
    }
    
    func toDictionary() -> [String: Any] {
        let user = [
            "Id": self.id,
            "Username": self.username,
            "Name": self.name,
            "Email": self.email,
            "Profile": self.profile,
            "AlreadyUsedLightEditor": self.alreadyUsedLightEditor
        ] as [String : Any]
        
        return user
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
