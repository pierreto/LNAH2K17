///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatDeplacement.swift
/// @author Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtat
/// @brief Cette classe représente l'état de déplacement. Implémente aussi le
///        patron Singleton.
///
/// @author Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class ModeleEtatDeplacement: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatDeplacement()
    
    /// Déplacement total appliqué
    private var deplacementTotal = GLKVector3()
    
    /// Fonction qui initialise l'état de déplacement
    override func initialiser() {
        self.deplacementTotal = GLKVector3(v: (0.0, 0.0, 0.0))
        
        // TODO : à supprimer une fois que le touch drag est implémenter
        self.touchDrag()
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    
    /// Évènement appelé lorsque le bouton gauche de la souris est descendu
    
    // TODO : à remplacer par un vrai touch drag
    func touchDrag() {
        let delta = GLKVector3(v: (-20.0, -20.0, -20.0))
        let visiteur = VisiteurDeplacement(delta: delta)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
