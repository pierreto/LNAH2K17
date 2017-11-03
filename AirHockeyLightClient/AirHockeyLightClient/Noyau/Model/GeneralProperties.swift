///////////////////////////////////////////////////////////////////////////////
/// @file GeneralProperties.swift
/// @author Pierre To
/// @date 2017-10-26
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation

enum GeneralPropertiesNotification {
    static let SaveGeneralPropertiesNotification = "SaveGeneralPropertiesNotification"
}

class GeneralProperties: NSObject {
    
    /// Classe contenant les valeurs par défaut utilisées
    /// pour la classe GeneralPropertiesViewController
    private class DefaultValues {
        public static let coefficientFriction: Float = 1.0
        public static let coefficientRebond: Float = 0.0
        public static let coefficientAcceleration: Float = 40.0
    }
    
    /// Valeurs maximales et minimales
    private let MIN_COEFFICIENT_FRICTION: Float = 0.0
    private let MAX_COEFFICIENT_FRICTION: Float = 10.0
    private let MIN_COEFFICIENT_REBOND: Float = 0.0
    private let MAX_COEFFICIENT_REBOND: Float = 15.0
    private let MIN_COEFFICIENT_ACCELERATION: Float = 0.0
    private let MAX_COEFFICIENT_ACCELERATION: Float = 100.0
    
    /// Propriétés de la zone de jeu
    private var coefficientFriction: Float = DefaultValues.coefficientFriction;
    private var coefficientRebond: Float = DefaultValues.coefficientRebond;
    private var coefficientAcceleration: Float = DefaultValues.coefficientAcceleration;
    
    /// Messages d'erreur
    var coefficientFrictionError: String
    var coefficientRebondError: String
    var coefficientAccelerationError: String
    
    override init() {
        self.coefficientFrictionError = ""
        self.coefficientRebondError = ""
        self.coefficientAccelerationError = ""
        super.init()
    }
    
    func validateCoefficients(coefficientFriction: String, coefficientRebond: String, coefficientAcceleration: String) -> Bool {
        if (!validateCoefficientFriction(coefficient: coefficientFriction) ||
            !validateCoefficientRebond(coefficient: coefficientRebond) ||
            !validateCoefficientAcceleration(coefficient: coefficientAcceleration)) {
            NotificationCenter.default.post(name: Notification.Name(rawValue: GeneralPropertiesNotification.SaveGeneralPropertiesNotification), object: self)
            return false
        }
        
        NotificationCenter.default.post(name: Notification.Name(rawValue: GeneralPropertiesNotification.SaveGeneralPropertiesNotification), object: self)
        return true
    }
    
    private func validateCoefficientFriction(coefficient: String) -> Bool {
        let coefficientFriction: Float? = Float.init(coefficient)
        
        if coefficientFriction == nil {
            self.coefficientFrictionError = "Entrée invalide"
            return false
        }
        else if coefficientFriction! < self.MIN_COEFFICIENT_FRICTION {
            self.coefficientFrictionError = "Coefficient de friction >= 0.0"
            return false
        }
        else if coefficientFriction! > self.MAX_COEFFICIENT_FRICTION {
            self.coefficientFrictionError = "Coefficient de friction <= 10.0"
            return false
        }
        else {
            self.coefficientFrictionError = ""
            return true
        }
    }
    
    private func validateCoefficientRebond(coefficient: String) -> Bool {
        let coefficientRebond: Float? = Float.init(coefficient)
        
        if coefficientRebond == nil {
            self.coefficientRebondError = "Entrée invalide"
            return false
        }
        else if coefficientRebond! < self.MIN_COEFFICIENT_REBOND {
            self.coefficientRebondError = "Coefficient de rebond >= 0.0"
            return false
        }
        else if coefficientRebond! > self.MAX_COEFFICIENT_REBOND {
            self.coefficientRebondError = "Coefficient de rebond <= 15.0"
            return false
        }
        else {
            self.coefficientRebondError = ""
            return true
        }
    }
    
    private func validateCoefficientAcceleration(coefficient: String) -> Bool {
        let coefficientAccceleration: Float? = Float.init(coefficient)
        
        if coefficientAccceleration == nil {
            self.coefficientAccelerationError = "Entrée invalide"
            return false
        }
        else if coefficientAccceleration! < self.MIN_COEFFICIENT_ACCELERATION {
            self.coefficientAccelerationError = "Coefficient d'accélération >= 0.0"
            return false
        }
        else if coefficientAccceleration! > self.MAX_COEFFICIENT_ACCELERATION {
            self.coefficientAccelerationError = "Coefficient d'accélération <= 100.0"
            return false
        }
        else {
            self.coefficientAccelerationError = ""
            return true
        }
    }
    
    /// Retourne les valeurs des trois coefficients de la zone en cours
    public func getCoefficientValues() -> [Float] {
        return [ self.coefficientFriction,
                 self.coefficientRebond,
                 self.coefficientAcceleration ];
    }
    
    /// Retourne les valeurs des trois coefficients de la zone en cours
    public func setCoefficientValues(coefficientFriction: String, coefficientRebond: String, coefficientAcceleration: String) {
        let coefficientFriction = Float.init(coefficientFriction)
        self.coefficientFriction = coefficientFriction == nil ? DefaultValues.coefficientFriction : coefficientFriction!
        let coefficientRebond = Float.init(coefficientRebond)
        self.coefficientRebond = coefficientRebond == nil ? DefaultValues.coefficientRebond : coefficientRebond!
        let coefficientAcceleration = Float.init(coefficientAcceleration)
        self.coefficientAcceleration = coefficientAcceleration == nil ? DefaultValues.coefficientAcceleration : coefficientAcceleration!
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
