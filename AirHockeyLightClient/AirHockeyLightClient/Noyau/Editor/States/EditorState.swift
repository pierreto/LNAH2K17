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
    func currentUserCreatedPortal(startUuid: String, startPos: SCNVector3, endUuid: String, endPos: SCNVector3) {}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////