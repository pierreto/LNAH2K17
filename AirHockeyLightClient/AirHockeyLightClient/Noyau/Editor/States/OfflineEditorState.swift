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
    
    override func joinEdition(mapEntity: MapEntity) {}
    override func leaveEdition() {}
    override func currentUserCreatedBoost(uuid: String, pos: SCNVector3) {}
    override func currentUserCreatedPortal(startUuid: String, startPos: SCNVector3,
                                           endUuid: String, endPos: SCNVector3) {}
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
