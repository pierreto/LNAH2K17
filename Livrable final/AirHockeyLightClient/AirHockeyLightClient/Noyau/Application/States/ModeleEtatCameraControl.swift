///////////////////////////////////////////////////////////////////////////////
/// @file ModeleCameraControl.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCameraControl
/// @brief Cette classe permet le contrôle de la caméra
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-12
///////////////////////////////////////////////////////////////////////////
class ModeleEtatCameraControl: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatCameraControl()
    
    /// Annule la gesture pan (pour pouvoir contrôler la caméra)
    override func initialiser() {
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
