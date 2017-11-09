///////////////////////////////////////////////////////////////////////////////
/// @file Map.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation

enum MapNotification {
    static let SaveMapNotification = "SaveMapNotification"
    static let UnlockMapNotification = "UnlockMapNotification"
}

class Map: NSObject {
    
    /// Propriétés d'une carte
    private var mapName : String?
    
    /// Message d'erreur
    var mapNameError: String = ""
    var passwordError: String = ""
    var passwordConfirmationError: String = ""
    var unlockPasswordError: String = ""
    
    override init() {
        super.init()
        self.resetErrorMessages()
    }
    
    func validate(name: String, isPrivate: Bool, password: String, passwordConfirmation: String) -> Bool {
        self.resetErrorMessages()
        var validated = true
        
        if name.isEmpty {
            self.mapNameError = "La carte doit posséder un nom!"
            validated = false
        }
        else if !DBManager.instance.isMapNameUnique(mapName: name) {
            self.mapNameError = "La carte doit posséder un nom unique!"
            validated = false
        }
        if isPrivate {
            if password.isEmpty {
                self.passwordError = "Une carte privée doit posséder un mot de passe!"
                validated = false
            } else if password.characters.count < 5 {
                self.passwordError = "Le mot de passe doit posséder au moins 5 caractères!"
                validated = false
            } else if password != passwordConfirmation {
                self.passwordConfirmationError = "Veuillez entrer le même mot de passe!"
                validated = false
            }
        }
        
        NotificationCenter.default.post(name: Notification.Name(rawValue: MapNotification.SaveMapNotification), object: self)
        
        return validated
    }
    
    func unlock(map: MapEntity, password: String) -> Bool {
        self.resetErrorMessages()
        var unlocked = true;
        
        if map.password != password {
            self.unlockPasswordError = "Le mot de passe entré est invalide!"
            unlocked = false
        }
        
        NotificationCenter.default.post(name: Notification.Name(rawValue: MapNotification.UnlockMapNotification), object: self)
        
        return unlocked
    }

    func save(name: String, isLocal: Bool, isPrivate: Bool, password: String) {
        let map = MapEntity()
        map.id.value = 1 // TODO : générer un UUID
        map.creator = "N/A" // TODO : mettre nom de l'utilisateur
        map.mapName = name
        map.privacy.value = isPrivate
        map.password = password
        map.currentNumberOfPlayer.value = 0
        
        DBManager.instance.sauvegarderCarte(map: map, json: nil)
    }
    
    private func resetErrorMessages() {
        self.mapNameError = ""
        self.passwordError = ""
        self.passwordConfirmationError = ""
        self.unlockPasswordError = ""
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
