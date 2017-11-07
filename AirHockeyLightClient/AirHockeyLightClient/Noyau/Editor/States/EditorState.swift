///////////////////////////////////////////////////////////////////////////////
/// @file AbstractEditorState.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit
import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class AbstractEditorState
/// @brief Cette classe comprend l'interface de base que doivent
///        implanter tous les états possibles du mode d'édition
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class EditorState {
    
    func joinEdition(mapEntity: MapEntity) {}
    func leaveEdition() {}
    func currentUserCreatedBoost(uuid: String, pos: SCNVector3, rotation: Float, scale: SCNVector3) {}
    func currentUserCreatedWall(uuid: String, pos: SCNVector3, rotation: Float, scale: SCNVector3) {}
    func currentUserCreatedPortal(startUuid: String, startPos: SCNVector3, startRotation: Float, startScale: SCNVector3,
                                  endUuid: String, endPos: SCNVector3, endRotation: Float, endScale: SCNVector3) {}
    func currentUserSelectedObject(uuidSelected: String, isSelected: Bool, deselectAll: Bool) {}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
