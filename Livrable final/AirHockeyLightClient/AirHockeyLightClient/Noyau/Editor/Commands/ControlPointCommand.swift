///////////////////////////////////////////////////////////////////////////////
/// @file ControlPointCommand.swift
/// @author Pierre To
/// @date 2017-11-08
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class ControlPointCommand
/// @brief Classe qui s'occupe de créer une commande pour le déplacement de point de contrôle
///
/// @author Pierre To
/// @date 2017-11-08
///////////////////////////////////////////////////////////////////////////
class ControlPointCommand: EditionCommand {
    
    var username: String = ""
    var position: [Float] = [Float]()
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, username: String, pos: [Float]) {
        super.init(objectUuid: objectUuid)
        self.position = pos
        self.username = username
    }
    
    override func executeCommand() {
        FacadeModele.instance.setControlPointPosition(uuid: self.objectUuid, username: self.username, pos: self.position)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.CONTROLPOINT_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "Username": self.username,
                         "Position": self.position]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
            JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.username = json["Username"].string!
        self.position = json["Position"].arrayObject as! [Float]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
