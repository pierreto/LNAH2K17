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
    var passwordError: Dynamic<String>
    var passwordConfirmationError: Dynamic<String>
    var unlockPasswordError: Dynamic<String>
    
    init(map: Map) {
        self.map = map
        self.mapNameError = Dynamic(map.mapNameError)
        self.passwordError = Dynamic(map.passwordError)
        self.passwordConfirmationError = Dynamic(map.passwordConfirmationError)
        self.unlockPasswordError = Dynamic(map.unlockPasswordError)
        
        super.init()
        
        self.subscribeToNotifications()
    }
    
    deinit {
        unsubscribeFromNotifications()
    }
    
    func save(name: String, isPrivate: Bool, password: String, passwordConfirmation: String) -> Bool {
        let valid = self.map.validate(name: name, isPrivate: isPrivate, password: password, passwordConfirmation: passwordConfirmation)
        
        if valid {
            self.map.save(name: name, isPrivate: isPrivate, password: password)
        }
        
        return valid
    }
    
    func unlock(map: MapEntity, unlockPassword: String) -> Bool {
        return self.map.unlock(map: map, password: unlockPassword)
    }
    
    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(savePressed(_:)),
                                               name: NSNotification.Name(rawValue: MapNotification.SaveMapNotification),
                                               object: self.map)
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(unlockPressed(_:)),
                                               name: NSNotification.Name(rawValue: MapNotification.UnlockMapNotification),
                                               object: self.map)
    }
    
    fileprivate func unsubscribeFromNotifications() {
        NotificationCenter.default.removeObserver(self)
    }
    
    @objc fileprivate func savePressed(_ notification: NSNotification) {
        self.mapNameError.value = self.map.mapNameError
        self.passwordError.value = self.map.passwordError
        self.passwordConfirmationError.value = self.map.passwordConfirmationError
    }
    
    @objc fileprivate func unlockPressed(_ notification: NSNotification) {
        self.unlockPasswordError.value = self.map.unlockPasswordError
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
