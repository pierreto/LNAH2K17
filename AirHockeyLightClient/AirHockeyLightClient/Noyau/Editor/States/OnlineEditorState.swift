///////////////////////////////////////////////////////////////////////////////
/// @file OnlineEditorState.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class OnlineEditorState
/// @brief Cette classe implémente l'état du mode d'édition en ligne.
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class OnlineEditorState: EditorState {
    
    /// Instance singleton
    static let instance = OnlineEditorState()
    
    private let clientConnection = HubManager.sharedConnection
    
    override func joinEdition(mapEntity: MapEntity) {
        FacadeModele.instance.obtenirUserManager()?.clearUsers()
        
        // TODO : changer username: "test.." pour username: clientConnection.getUsername() quand online mode done
        self.clientConnection.getEditionHub().joinPublicRoom(username: "testOnlineUser",
                                                             mapEntity: mapEntity)
    }
    
    override func leaveEdition() {
        self.clientConnection.getEditionHub().leaveRoom()
    }
    
    override func currentUserCreatedPortal(startUuid: String, startPos: SCNVector3,
                                           endUuid: String, endPos: SCNVector3) {
        let startPosition = [startPos.x, startPos.y, startPos.z]
        let endPosition = [endPos.x, endPos.y, endPos.z]
        let portalCommand = PortalCommand(objectUuid: startUuid, endUuid: endUuid,
                                          startPos: startPosition, endPos: endPosition)
        self.clientConnection.getEditionHub().sendEditionCommand(command: portalCommand)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
