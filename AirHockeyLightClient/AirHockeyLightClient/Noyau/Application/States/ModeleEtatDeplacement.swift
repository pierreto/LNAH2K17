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
    
    /// Position au début du pan gesture
    private var beganPosition = CGPoint()
    
    /// Position à la fin du pan gesture
    private var endedPosition = CGPoint()
    
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
        
        if sender.state == UIGestureRecognizerState.began {
            // Sauvegarder la position au début du pan gesture
            self.beganPosition = self.position
            print("Debut deplacement noeud")
            print(self.beganPosition)
        }
        else if sender.state == UIGestureRecognizerState.ended {
            // Sauvegarder la position de fin du pan gesture
            self.endedPosition = self.position
            print("Fin deplacement noeud")
            print(self.endedPosition)
            
            // Déplacer le noeud
            let visiteur = VisiteurDeplacement(lastPosition: self.lastPosition, position: self.position)
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
            
            if !self.noeudsSurLaTable() {
                // Annuler le déplacement
                let visiteurAnnulation = VisiteurDeplacement(lastPosition: self.endedPosition, position: self.beganPosition)
                FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteurAnnulation)
            }
        }
        else {
            // Déplacer le noeud
            let visiteur = VisiteurDeplacement(lastPosition: self.lastPosition, position: self.position)
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
