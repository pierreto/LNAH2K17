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
    case BOOST_COMMAND = "InterfaceGraphique.Entities.EditonCommand.BoostCommand, InterfaceGraphique"
    case WALL_COMMAND = "InterfaceGraphique.Entities.EditonCommand.WallCommand, InterfaceGraphique"
    case PORTAL_COMMAND = "InterfaceGraphique.Entities.EditonCommand.PortalCommand, InterfaceGraphique"
    case SELECTION_COMMAND = "InterfaceGraphique.Entities.EditonCommand.SelectionCommand, InterfaceGraphique"
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
