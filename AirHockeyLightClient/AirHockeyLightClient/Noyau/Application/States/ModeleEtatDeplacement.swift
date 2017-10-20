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
/// @class ModeleEtatDeplacement
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
        
        // Le déplacement des noeuds s'effectue par un gesture pan
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
    }
    
    override func nettoyerEtat() {
        /// Enlève la gesture pan
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func panGesture(sender: ImmediatePanGestureRecognizer) {
        super.panGesture(sender: sender)
        
        // Bouger le noeud
        if sender.state != UIGestureRecognizerState.began {
            let visiteur = VisiteurDeplacement(lastPosition: self.lastPosition, position: self.position)
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
