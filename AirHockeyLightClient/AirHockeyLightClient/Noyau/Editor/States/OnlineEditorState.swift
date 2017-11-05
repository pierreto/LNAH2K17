///////////////////////////////////////////////////////////////////////////////
/// @file OnlineEditorState.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import GLKit

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
        self.clientConnection.getEditionHub().joinPublicRoom(username: self.clientConnection.getUsername()!,
                                                             mapEntity: mapEntity)
    }
    
    override func leaveEdition() {
        self.clientConnection.getEditionHub().leaveRoom()
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
