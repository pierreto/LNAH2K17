///////////////////////////////////////////////////////////////////////////////
/// @file MapViewModel.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation

class MapViewModel: NSObject, IMapViewModel {
    
    private var map: Map
    
    var mapNameError: Dynamic<String>
    
    init(map: Map) {
        self.map = map
        self.mapNameError = Dynamic(map.mapNameError)
        
        super.init()
        
        self.subscribeToNotifications()
    }
    
    deinit {
        unsubscribeFromNotifications()
    }
    
    func save(mapName: String, isLocalMap: Bool, isPrivateMap: Bool) -> Bool {
        let valid = map.validateName(mapName: mapName)
        
        if valid {
            map.setValues(mapName: mapName, isLocalMap: isLocalMap, isPrivateMap: isPrivateMap)
        }
        
        return valid
    }
    
    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(savePressed(_:)),
                                               name: NSNotification.Name(rawValue: MapNotification.SaveMapNotification),
                                               object: self.map)
    }
    
    fileprivate func unsubscribeFromNotifications() {
        NotificationCenter.default.removeObserver(self)
    }
    
    @objc fileprivate func savePressed(_ notification: NSNotification) {
        self.mapNameError.value = self.map.mapNameError
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
