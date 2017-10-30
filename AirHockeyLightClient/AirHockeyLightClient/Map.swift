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
    private var isLocalMap : Bool?
    private var isPrivateMap : Bool?
    
    /// Message d'erreur
    var mapNameError: String
    
    override init() {
        self.mapNameError = ""
        super.init()
    }
    
    func validateName(mapName: String) -> Bool {
        NotificationCenter.default.post(name: Notification.Name(rawValue: MapNotification.SaveMapNotification), object: self)
        mapNameError = "Error"
        
        return false
    }

    func setValues(mapName: String, isLocalMap: Bool, isPrivateMap: Bool) {
        self.mapName = mapName
        self.isLocalMap = isLocalMap
        self.isPrivateMap = isPrivateMap
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
