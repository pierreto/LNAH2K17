///////////////////////////////////////////////////////////////////////////////
/// @file PortalCommand.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

class PortalCommand: AbstractEditionCommand {
    
    var objectUuid: String = ""
    var endUuid: String = ""
    var startPosition: [Float] = []
    var endPosition: [Float] = []
    
    init(objectUuid: String) {
        self.objectUuid = objectUuid
    }
    
    func executeCommand() {
        FacadeModele.instance.creerNoeuds(type: "", nomType: "")
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
