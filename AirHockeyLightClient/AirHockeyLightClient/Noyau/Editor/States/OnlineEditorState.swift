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
        self.clientConnection.getEditionHub().joinPublicRoom(username: clientConnection.getUsername()!, mapEntity: mapEntity)
    }
    
    override func leaveEdition() {
        self.clientConnection.getEditionHub().leaveRoom()
    }
    
    override func currentUserCreatedBoost(uuid: String, pos: SCNVector3, rotation: Float, scale: SCNVector3) {
        let position = [pos.x, pos.y, pos.z]
        let scale = [scale.x, scale.y, scale.z]
        let command = BoostCommand(objectUuid: uuid, pos: position, rotation: rotation, scale: scale)
        self.clientConnection.getEditionHub().sendEditionCommand(command: command)
    }
    
    override func currentUserCreatedWall(uuid: String, pos: SCNVector3, rotation: Float, scale: SCNVector3) {
        let position = [pos.x, pos.y, pos.z]
        let scale = [scale.x, scale.y, scale.z]
        let command = WallCommand(objectUuid: uuid, pos: position, rotation: rotation, scale: scale)
        self.clientConnection.getEditionHub().sendEditionCommand(command: command)
    }
    
    override func currentUserCreatedPortal(startUuid: String, startPos: SCNVector3, startRotation: Float, startScale: SCNVector3,
                                           endUuid: String, endPos: SCNVector3, endRotation: Float, endScale: SCNVector3) {
        let startPosition = [startPos.x, startPos.y, startPos.z]
        let startScale = [startScale.x, startScale.y, startScale.z]
        let endPosition = [endPos.x, endPos.y, endPos.z]
        let endScale = [endScale.x, endScale.y, endScale.z]
        let command = PortalCommand(objectUuid: startUuid, endUuid: endUuid,
                                    startPos: startPosition, startRotation: startRotation, startScale: startScale,
                                    endPos: endPosition, endRotation: endRotation, endScale: endScale)
        self.clientConnection.getEditionHub().sendEditionCommand(command: command)
    }
    
    override func currentUserSelectedObject(uuidSelected: String, isSelected: Bool, deselectAll: Bool) {
        let command = SelectionCommand(objectUuid: uuidSelected, username: HubManager.sharedConnection.getUsername()!,
                                       isSelected: isSelected, deselectAll: deselectAll)
        self.clientConnection.getEditionHub().sendEditionCommand(command: command)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
