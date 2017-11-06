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
    var endPosition: [Float] = [Float]()
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, endUuid: String, startPos: [Float], endPos: [Float]) {
        super.init(objectUuid: objectUuid)
        self.endUuid = endUuid
        self.startPosition = startPos
        self.endPosition = endPos
    }
    
    override func executeCommand() {
        let portail1Pos = GLKVector3.init(v: (self.startPosition[0], self.startPosition[1], self.startPosition[2]))
        let portail2Pos = GLKVector3.init(v: (self.endPosition[0], self.endPosition[1], self.endPosition[2]))
        NodeCreator.instance.createPortal(startUuid: self.objectUuid,
                                          portal1Pos: portail1Pos,
                                          endUuid: self.endUuid,
                                          portal2Pos: portail2Pos)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.PORTAL_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "EndUuid": self.endUuid,
                         "StartPosition": self.startPosition,
                         "EndPosition": self.endPosition]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
                    JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.endUuid = json["EndUuid"].string!
        self.startPosition = json["StartPosition"].arrayObject as! [Float]
        self.endPosition = json["EndPosition"].arrayObject as! [Float]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
