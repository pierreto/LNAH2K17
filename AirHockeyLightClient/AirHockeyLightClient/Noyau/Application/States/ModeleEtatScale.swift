///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatScale.swift
/// @author Pierre To
/// @date 2017-10-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatScale.swift
/// @brief Cette classe représente l'état de scaling. Implémente aussi le
///        patron Singleton.
///
/// @author Pierre To
/// @date 2017-10-22
///////////////////////////////////////////////////////////////////////////
class ModeleEtatScale: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatScale()
    
    /// Fonction qui initialise l'état de déplacement
    override func initialiser() {
        // La mise à l'échelle des noeuds s'effectue par la gesture pinch
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.pinchGestureRecognizer!)
    }
    
    override func nettoyerEtat() {
        /// Enlève la gesture pinch
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.pinchGestureRecognizer!)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func pinchGesture(sender: UIPinchGestureRecognizer) {
        super.pinchGesture(sender: sender)
        
        if sender.state == UIGestureRecognizerState.began {
            print("Pinch began")
            
            // Sauvegarder la valeur du scale des noeuds
            self.saveScale()
        }
        else if sender.state == UIGestureRecognizerState.ended {
            print("Pinch ended")
            
            // Dernier rotation
            self.appliquerScaling(scale: Float(sender.scale))
            
            if !self.noeudsSurLaTable() {
                // Annuler la rotation
                self.revertScale()
            }
        }
        else {
            self.appliquerScaling(scale: Float(sender.scale))
        }
    }
    
    /// Applique le scaling
    private func appliquerScaling(scale: Float) {
        // Calcul du nouveau scale
        // Pinch in
        let ajoutScale: Float
        if (scale > 1.0) {
            ajoutScale = Float(scale) * 2.0
        }
        // Pinch out
        else {
            ajoutScale = -Float(scale) * 6.0
        }
        
        // Apply nouveau scale
        let visiteur = VisiteurScale(scale: GLKVector3.init(v: (ajoutScale, ajoutScale, ajoutScale)),
                                     ajusterScaling: true)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
    }
    
    /// Cette fonction sauvegarde le scaling de chacun des noeuds
    private func saveScale() {
        // Save scale on matrice
        let visiteur = VisiteurObtenirSelection()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        let noeuds = visiteur.obtenirNoeuds()
    
        for noeud in noeuds {
            noeud.saveScale()
        }
    }
    
    /// Cette fonction revert le scaling de chacun des noeuds
    func revertScale() {
        // Revert scale on matrice
        let visiteur = VisiteurObtenirSelection()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        let noeuds = visiteur.obtenirNoeuds()
    
        for noeud in noeuds {
            noeud.revertScale()
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                         pos: noeud.position, rotation: noeud.rotation.w, scale: noeud.scale)
        }
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
