///////////////////////////////////////////////////////////////////////////////
/// @file CoefficientCommand.swift
/// @author Pierre To
/// @date 2017-11-09
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class CoefficientCommand
/// @brief Classe qui s'occupe de créer une commande pour la configuration de la zone de jeu
///
/// @author Pierre To
/// @date 2017-11-09
///////////////////////////////////////////////////////////////////////////
class CoefficientCommand: EditionCommand {
    
    var coefficientFriction: Float = 0.0
    var coefficientRebond: Float = 0.0
    var coefficientAcceleration: Float = 0.0
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(coefficientFriction: Float, coefficientRebond: Float, coefficientAcceleration: Float) {
        super.init(objectUuid: "")
        self.coefficientFriction = coefficientFriction
        self.coefficientRebond = coefficientRebond
        self.coefficientAcceleration = coefficientAcceleration
    }
    
    override func executeCommand() {
        FacadeModele.instance.assignerGeneralProperties(coefficientFriction: self.coefficientFriction,
                                                        coefficientRebond: self.coefficientRebond,
                                                        coefficientAcceleration: self.coefficientAcceleration)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.COEFFICIENT_COMMAND.rawValue]).rawString()
        let data = JSON(["frictionCoeff": self.coefficientFriction,
                         "reboundCoeff": self.coefficientRebond,
                         "accCoeff": self.coefficientAcceleration]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
            JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.coefficientFriction = json["frictionCoeff"].float!
        self.coefficientRebond = json["reboundCoeff"].float!
        self.coefficientAcceleration = json["accCoeff"].float!
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
