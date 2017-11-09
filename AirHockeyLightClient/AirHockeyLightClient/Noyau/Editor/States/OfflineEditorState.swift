///////////////////////////////////////////////////////////////////////////////
/// @file OfflineEditorState.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class OfflineEditorState
/// @brief Cette classe implémente l'état du mode d'édition hors ligne.
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class OfflineEditorState: EditorState {
    
    /// Instance singleton
    static let instance = OfflineEditorState()
    
    override func joinEdition(mapEntity: MapEntity) {
        FacadeModele.instance.setCurrentUserDefaultColor()
    }
    
    override func leaveEdition() {}
    override func currentUserCreatedBoost(uuid: String, pos: SCNVector3, rotation: Float, scale: SCNVector3) {}
    override func currentUserCreatedWall(uuid: String, pos: SCNVector3, rotation: Float, scale: SCNVector3) {}
    override func currentUserCreatedPortal(startUuid: String, startPos: SCNVector3, startRotation: Float, startScale: SCNVector3,
                                           endUuid: String, endPos: SCNVector3, endRotation: Float, endScale: SCNVector3) {}
    override func currentUserSelectedObject(uuidSelected: String, isSelected: Bool, deselectAll: Bool) {}
    override func currentUserObjectTransformChanged(uuid: String, pos: SCNVector3, rotation: Float, scale: SCNVector3) {}
    override func currentUserControlPointChanged(uuid: String, pos: SCNVector3) {}
    override func currentUserDeletedNode(uuid: String) {}
    override func currentUserChangedCoefficient(coefficientFriction: Float, coefficientRebond: Float, coefficientAcceleration: Float) {}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
