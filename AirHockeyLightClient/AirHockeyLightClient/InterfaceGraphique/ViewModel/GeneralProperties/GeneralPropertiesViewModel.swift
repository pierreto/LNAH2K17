///////////////////////////////////////////////////////////////////////////////
/// @file GeneralPropertiesViewModel.swift
/// @author Pierre To
/// @date 2017-10-26
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import PromiseKit
import Foundation

class GeneralPropertiesViewModel: NSObject, IGeneralPropertiesViewModel {
    
    private var generalProperties: GeneralProperties
    
    var coefficientFrictionError: Dynamic<String>
    var coefficientRebondError: Dynamic<String>
    var coefficientAccelerationError: Dynamic<String>
    
    init(generalProperties: GeneralProperties) {
        self.generalProperties = generalProperties
        self.coefficientFrictionError = Dynamic(generalProperties.coefficientFrictionError)
        self.coefficientRebondError = Dynamic(generalProperties.coefficientRebondError)
        self.coefficientAccelerationError = Dynamic(generalProperties.coefficientAccelerationError)
        super.init()
        self.subscribeToNotifications()
    }
    
    deinit {
        unsubscribeFromNotifications()
    }
    
    func save (coefficientFriction: String, coefficientRebond: String, coefficientAcceleration: String) -> Bool {
        let valid = generalProperties.validateCoefficients(
            coefficientFriction: coefficientFriction,
            coefficientRebond: coefficientRebond,
            coefficientAcceleration: coefficientAcceleration)
        
        if valid {
            generalProperties.setCoefficientValues(coefficientFriction: coefficientFriction,
                                                   coefficientRebond: coefficientRebond,
                                                   coefficientAcceleration: coefficientAcceleration)
        }
        
        return valid
    }
    
    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(savePressed(_:)),
                                               name: NSNotification.Name(rawValue: GeneralPropertiesNotification.SaveGeneralPropertiesNotification),
                                               object: self.generalProperties)
    }
    
    fileprivate func unsubscribeFromNotifications() {
        NotificationCenter.default.removeObserver(self)
    }
    
    @objc fileprivate func savePressed(_ notification: NSNotification){
        self.coefficientFrictionError.value = self.generalProperties.coefficientFrictionError
        self.coefficientRebondError.value = self.generalProperties.coefficientRebondError
        self.coefficientAccelerationError.value = self.generalProperties.coefficientAccelerationError
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
