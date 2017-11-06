///////////////////////////////////////////////////////////////////////////////
/// @file EditionCommand.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftyJSON

/// Les différentes commandes du mode d'édition
enum EDITION_COMMAND : String {
    case PORTAL_COMMAND = "InterfaceGraphique.Entities.EditonCommand.PortalCommand, InterfaceGraphique"
}

class EditionCommand {
    
    var objectUuid: String
    
    init(objectUuid: String) {
        self.objectUuid = objectUuid
    }
    
    func executeCommand() {}
    
    func toJSON() -> JSON? {
        return nil
    }
    
    func fromJSON(json: JSON) {}
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
