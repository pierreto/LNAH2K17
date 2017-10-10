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
    
    func initialiser() {}
    
    /// Annule l'action en cours
    func nettoyerEtat() {}
    
    // Fonctions gérant les entrées de l'utilisateur

    /// Évènement appelé lorsque le bouton gauche de la souris est descendu
    
    /// Fonction pour obtenir la vue rapidement
    func obtenirVue() -> UIViewController {
        return FacadeModele.instance.obtenirVue()
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
