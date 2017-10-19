///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtat.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtat
/// @brief Cette classe comprend l'interface de base que doivent
///        implanter tous les états possibles du modèle
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class ModeleEtat {
    
    /// Dernière position
    var lastPosition = CGPoint()
    
    /// Position du toucher
    var position = CGPoint()
    
    func initialiser() {}
    
    /// Annule l'action en cours
    func nettoyerEtat() {}
    
    // Fonctions gérant les entrées de l'utilisateur

    // TAP
    func tapGesture(point: CGPoint) {
        self.position = point
    }
    
    // PAN
    func panGesture(sender: ImmediatePanGestureRecognizer) {
        self.lastPosition = self.position
        self.position = sender.location(in: sender.view)
    }
    
    /// Fonction pour obtenir la vue rapidement
    func obtenirVue() -> UIViewController {
        return FacadeModele.instance.obtenirVue()
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
