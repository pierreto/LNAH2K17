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

class Maps: NSObject {
    
    /// Propriétés d'une carte
    private var mapName : String?
    
    /// Message d'erreur
    var mapNameError: String
    
    override init() {
        self.mapNameError = ""
        super.init()
    }
    
    func validateMap(mapName: String) -> Bool {
        NotificationCenter.default.post(name: Notification.Name(rawValue: MapNotification.SaveMapNotification), object: self)
        
        if !DBManager.instance.isMapNameUnique(mapName: mapName) {
            mapNameError = "La carte doit posséder un nom unique!"
            return false;
        }
        
        return true
    }

    func saveMap(mapName: String, isLocalMap: Bool, isPrivateMap: Bool) {
        let map = MapEntity()
        map.name = mapName
        
        DBManager.instance.sauvegarderCarte(map: map)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
