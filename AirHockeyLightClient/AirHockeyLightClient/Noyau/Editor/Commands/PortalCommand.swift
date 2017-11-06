///////////////////////////////////////////////////////////////////////////////
/// @file PortalCommand.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class PortalCommand
/// @brief Classe qui s'occupe de créer une commande de création pour les portails
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class PortalCommand: EditionCommand {
    
    var endUuid: String = ""
    
    var startPosition: [Float] = [Float]()
    var startRotation: Float = 0.0
    var startScale: [Float] = [Float]()
    
    var endPosition: [Float] = [Float]()
    var endRotation: Float = 0.0
    var endScale: [Float] = [Float]()
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, endUuid: String, startPos: [Float], startRotation: Float, startScale: [Float],
         endPos: [Float], endRotation: Float, endScale: [Float]) {
        super.init(objectUuid: objectUuid)
        self.endUuid = endUuid
        self.startPosition = startPos
        self.startRotation = startRotation
        self.startScale = startScale
        self.endPosition = endPos
        self.endRotation = endRotation
        self.endScale = endScale
    }
    
    override func executeCommand() {
        let portail1Pos = GLKVector3.init(v: (self.startPosition[0], self.startPosition[1], self.startPosition[2]))
        let portail1Scale = GLKVector3.init(v: (self.startScale[0], self.startScale[1], self.startScale[2]))
        let portail2Pos = GLKVector3.init(v: (self.endPosition[0], self.endPosition[1], self.endPosition[2]))
        let portail2Scale = GLKVector3.init(v: (self.endScale[0], self.endScale[1], self.endScale[2]))
        NodeCreator.instance.createPortal(startUuid: self.objectUuid, startPos: portail1Pos, startAngle: self.startRotation, startScale: portail1Scale,
                                          endUuid: self.endUuid, endPos: portail2Pos, endAngle: self.endRotation, endScale: portail2Scale)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.PORTAL_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "EndUuid": self.endUuid,
                         "StartPosition": self.startPosition,
                         "StartRotation": self.startRotation,
                         "StartScale": self.startScale,
                         "EndPosition": self.endPosition,
                         "EndRotation": self.endRotation,
                         "EndScale": self.endScale]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
                    JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.endUuid = json["EndUuid"].string!
        self.startPosition = json["StartPosition"].arrayObject as! [Float]
        self.startRotation = json["StartRotation"].float!
        self.startScale = json["StartScale"].arrayObject as! [Float]
        self.endPosition = json["EndPosition"].arrayObject as! [Float]
        self.endRotation = json["EndRotation"].float!
        self.endScale = json["EndScale"].arrayObject as! [Float]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
