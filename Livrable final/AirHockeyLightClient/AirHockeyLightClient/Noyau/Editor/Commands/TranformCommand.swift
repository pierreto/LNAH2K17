///////////////////////////////////////////////////////////////////////////////
/// @file TransformCommand.swift
/// @author Pierre To
/// @date 2017-11-07
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class TransformCommand
/// @brief Classe qui s'occupe de créer une commande pour la tranformation des noeuds
///
/// @author Pierre To
/// @date 2017-11-07
///////////////////////////////////////////////////////////////////////////
class TransformCommand: EditionCommand {
    
    var username: String = ""
    var position: [Float] = [Float]()
    var rotation: Float = 0.0
    var scale: [Float] = [Float]()
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, username: String, pos: [Float], rotation: Float, scale: [Float]) {
        super.init(objectUuid: objectUuid)
        self.username = username
        self.position = pos
        self.rotation = rotation
        self.scale = scale
    }
    
    override func executeCommand() {
        FacadeModele.instance.setTransformByUUID(uuid: self.objectUuid, username: self.username,
                                                 position: position, rotation: self.rotation, scale: scale)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.TRANSFORM_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "Username": self.username,
                         "Position": self.position,
                         "Rotation": self.rotation,
                         "Scale": self.scale]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
            JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.username = json["Username"].string!
        self.position = json["Position"].arrayObject as! [Float]
        self.rotation = json["Rotation"].float!
        self.scale = json["Scale"].arrayObject as! [Float]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
