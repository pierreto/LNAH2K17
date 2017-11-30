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
        self.deplacementTotal = GLKVector3.init(v: (0.0, 0.0, 0.0))
        
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
        
        if sender.state == UIGestureRecognizerState.began {
            print("Debut deplacement noeud")
        }
        else if sender.state == UIGestureRecognizerState.ended {
            print("Fin deplacement noeud")
            
            // Dernier déplacement
            self.deplacer()
            
            if !self.noeudsSurLaTable() {
                // Annuler le déplacement
                self.annulerDeplacement()
            }
        }
        else {
            // Déplacer le noeud
            self.deplacer()
        }
    }
    
    // Déplacer des noeuds
    private func deplacer() {
        let deplacement = super.obtenirDeplacement()
        let visiteur = VisiteurDeplacement(delta: deplacement)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        
        // Sauvegarder le déplacement total
        self.deplacementTotal = GLKVector3Add(self.deplacementTotal, deplacement)
    }
    
    // Annule le déplacement des noeuds
    private func annulerDeplacement() {
        let deplacement = GLKVector3Negate(self.deplacementTotal)
        let visiteur = VisiteurDeplacement(delta: deplacement)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        
        // Réinitialiser le déplacement total
        self.deplacementTotal = GLKVector3.init(v: (0.0, 0.0, 0.0))
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
