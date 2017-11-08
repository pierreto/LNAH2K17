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
}

class Map: NSObject {
    
    /// Propriétés d'une carte
    private var mapName : String?
    
    /// Message d'erreur
    var mapNameError: String
    var passwordError: String
    var passwordConfirmationError: String
    
    override init() {
        self.mapNameError = ""
        self.passwordError = ""
        self.passwordConfirmationError = ""
        super.init()
    }
    
    func validate(name: String, isPrivate: Bool, password: String, passwordConfirmation: String) -> Bool {
        NotificationCenter.default.post(name: Notification.Name(rawValue: MapNotification.SaveMapNotification), object: self)
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
            }
            if !password.isEmpty && password != passwordConfirmation {
                self.passwordConfirmationError = "Veuillez entrer le même mot de passe!"
                validated = false
            }
        }
        
        return validated
    }

    func save(name: String, isLocal: Bool, isPrivate: Bool) {
        // TODO : add other params to map
        let map = MapEntity()
        map.mapName = name
        
        DBManager.instance.sauvegarderCarte(map: map, json: nil)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
