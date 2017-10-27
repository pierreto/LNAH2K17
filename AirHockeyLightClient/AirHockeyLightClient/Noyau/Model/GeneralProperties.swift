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
    
    /// Propriétés de la zone de jeu
    private var coefficientFriction: Float = DefaultValues.coefficientFriction;
    private var coefficientRebond: Float = DefaultValues.coefficientRebond;
    private var coefficientAcceleration: Float = DefaultValues.coefficientAcceleration;
    
    /// Messages d'erreur
    var coefficientFrictionError: String
    
    override init() {
        self.coefficientFrictionError = ""
        super.init()
    }
    
    func validateCoefficients(coefficientFriction: Float) -> Bool {
        if (!validateCoefficientFriction(coefficient: coefficientFriction)) {
            NotificationCenter.default.post(name: Notification.Name(rawValue: GeneralPropertiesNotification.SaveGeneralPropertiesNotification), object: self)
            return false
        }
        
        NotificationCenter.default.post(name: Notification.Name(rawValue: GeneralPropertiesNotification.SaveGeneralPropertiesNotification), object: self)
        return true
    }
    
    private func validateCoefficientFriction(coefficient: Float) -> Bool {
        if coefficient < self.MIN_COEFFICIENT_FRICTION {
            self.coefficientFrictionError = "Coefficient de friction doit être plus grand que 0.0"
            return false
        } else if coefficient > self.MAX_COEFFICIENT_FRICTION {
            self.coefficientFrictionError = "Coefficient de friction doit être moins grand que 10.0"
            return false
        } else {
            self.coefficientFrictionError = ""
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
    public func setCoefficientValues(coefficientFriction: Float, coefficientRebond: Float, coefficientAcceleration: Float) {
        self.coefficientFriction = coefficientFriction
        self.coefficientRebond = coefficientRebond
        self.coefficientAcceleration = coefficientAcceleration
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
