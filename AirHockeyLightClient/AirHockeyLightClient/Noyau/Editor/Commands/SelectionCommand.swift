///////////////////////////////////////////////////////////////////////////////
/// @file SelectionCommand.swift
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
/// @class SelectionCommand
/// @brief Classe qui s'occupe de créer une commande de sélection de noeuds
///
/// @author Pierre To
/// @date 2017-11-05
///////////////////////////////////////////////////////////////////////////
class SelectionCommand: EditionCommand {
    
    var username: String = ""
    var isSelected: Bool = false
    var deselectAll: Bool = false
    
    override init(objectUuid: String) {
        super.init(objectUuid: objectUuid)
    }
    
    init(objectUuid: String, username: String, isSelected: Bool, deselectAll: Bool) {
        super.init(objectUuid: objectUuid)
        self.username = username
        self.isSelected = isSelected
        self.deselectAll = deselectAll
    }
    
    override func executeCommand() {
        FacadeModele.instance.selectNode(username: self.username, uuid: self.objectUuid, isSelected: self.isSelected, deselectAll: self.deselectAll)
    }
    
    override func toJSON() -> JSON? {
        let type = JSON(["$type": EDITION_COMMAND.SELECTION_COMMAND.rawValue]).rawString()
        let data = JSON(["ObjectUuid": self.objectUuid,
                         "Username": self.username,
                         "IsSelected": self.isSelected,
                         "DeselectAll": self.deselectAll]).rawString()
        
        // L'ordre est important "$type" puis données
        return JSON(JsonHelper.removeLastChar(jsonString: type!) + "," +
            JsonHelper.removeFirstChar(jsonString: data!))
    }
    
    override func fromJSON(json: JSON) {
        self.username = json["Username"].string!
        self.isSelected = json["IsSelected"].bool!
        self.deselectAll = json["DeselectAll"].bool!
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
