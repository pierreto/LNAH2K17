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
    
    private var maps: Maps
    
    var mapNameError: Dynamic<String>
    
    init(maps: Maps) {
        self.maps = maps
        self.mapNameError = Dynamic(maps.mapNameError)
        
        super.init()
        
        self.subscribeToNotifications()
    }
    
    deinit {
        unsubscribeFromNotifications()
    }
    
    func save(mapName: String, isLocalMap: Bool, isPrivateMap: Bool) -> Bool {
        let valid = self.maps.validateMap(mapName: mapName)
        
        if valid {
            self.maps.saveMap(mapName: mapName, isLocalMap: isLocalMap, isPrivateMap: isPrivateMap)
        }
        
        return valid
    }
    
    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(savePressed(_:)),
                                               name: NSNotification.Name(rawValue: MapNotification.SaveMapNotification),
                                               object: self.maps)
    }
    
    fileprivate func unsubscribeFromNotifications() {
        NotificationCenter.default.removeObserver(self)
    }
    
    @objc fileprivate func savePressed(_ notification: NSNotification) {
        self.mapNameError.value = self.maps.mapNameError
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
