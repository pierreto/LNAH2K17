///////////////////////////////////////////////////////////////////////////////
/// @file DeleteCommand.swift
/// @author Pierre To
/// @date 2017-11-08
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class DeleteCommand
/// @brief Classe qui s'occupe de créer une commande pour la suppression de noeuds
///
/// @author Pierre To
/// @date 2017-11-08
///////////////////////////////////////////////////////////////////////////
class DeleteCommand: EditionCommand {
    
    var username: String = ""
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, username: String) {
        super.init(objectUuid: objectUuid)
        self.username = username
    }
    
    override func executeCommand() {
        FacadeModele.instance.deleteNode(uuid: self.objectUuid, username: self.username)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.DELETE_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "Username": self.username]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
            JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.username = json["Username"].string!
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
