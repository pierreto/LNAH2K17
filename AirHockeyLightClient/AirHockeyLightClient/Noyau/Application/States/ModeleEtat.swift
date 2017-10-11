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
    
    /// Position du tap
    var positionTap = CGPoint()
    
    func initialiser() {}
    
    /// Annule l'action en cours
    func nettoyerEtat() {}
    
    // Fonctions gérant les entrées de l'utilisateur

    /// Évènement appelé lorsque l'utilisateur tap sur l'écran
    func tapGesture(point: CGPoint) {
        self.positionTap = point
    }
    
    /// Fonction pour obtenir la vue rapidement
    func obtenirVue() -> UIViewController {
        return FacadeModele.instance.obtenirVue()
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
