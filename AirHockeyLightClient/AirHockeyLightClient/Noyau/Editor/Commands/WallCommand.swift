///////////////////////////////////////////////////////////////////////////////
/// @file WallCommand.swift
/// @author Pierre To
/// @date 2017-11-05
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class WallCommand
/// @brief Classe qui s'occupe de créer une commande de création pour des murets
///
/// @author Pierre To
/// @date 2017-11-05
///////////////////////////////////////////////////////////////////////////
class WallCommand: EditionCommand {
    
    var startPosition: [Float] = [Float]()
    var endPosition: [Float] = [Float]()
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, startPos: [Float], endPos: [Float]) {
        super.init(objectUuid: objectUuid)
        self.startPosition = startPos
        self.endPosition = endPos
    }
    
    override func executeCommand() {
        let startPos = GLKVector3.init(v: (self.startPosition[0], self.startPosition[1], self.startPosition[2]))
        let endPos = GLKVector3.init(v: (self.endPosition[0], self.endPosition[1], self.endPosition[2]))
        NodeCreator.instance.createWall(uuid: self.objectUuid, startPos: startPos, endPos: endPos)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.WALL_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "StartPosition": self.startPosition,
                         "EndPosition": self.endPosition]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
            JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.startPosition = json["StartPosition"].arrayObject as! [Float]
        self.endPosition = json["EndPosition"].arrayObject as! [Float]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
