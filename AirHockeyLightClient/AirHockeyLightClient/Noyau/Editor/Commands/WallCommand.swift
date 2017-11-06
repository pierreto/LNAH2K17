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
    
    var position: [Float] = [Float]()
    var rotation: Float = 0.0
    var scale: [Float] = [Float]()
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, pos: [Float], rotation: Float, scale: [Float]) {
        super.init(objectUuid: objectUuid)
        self.position = pos
        self.rotation = rotation
        self.scale = scale
    }
    
    override func executeCommand() {
        let pos = GLKVector3.init(v: (self.position[0], self.position[1], self.position[2]))
        let scale = GLKVector3.init(v: (self.scale[0], self.scale[1], self.scale[2]))
        NodeCreator.instance.createWall(uuid: self.objectUuid, position: pos, angle: self.rotation, scale: scale)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.WALL_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "Position": self.position,
                         "Rotation": self.rotation,
                         "Scale": self.scale]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
            JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.position = json["Position"].arrayObject as! [Float]
        self.rotation = json["Rotation"].float!
        self.scale = json["Scale"].arrayObject as! [Float]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
