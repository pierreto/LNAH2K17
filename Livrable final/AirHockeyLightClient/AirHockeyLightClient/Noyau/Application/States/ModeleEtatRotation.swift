///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatRotation.swift
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
/// @class ModeleEtatRotation
/// @brief Cette classe représente l'état de rotation. Implémente aussi le
///        patron Singleton.
///
/// @author Pierre To
/// @date 2017-10-22
///////////////////////////////////////////////////////////////////////////
class ModeleEtatRotation: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatRotation()
    
    /// Centre de rotation
    private var centreRotation = GLKVector3()
    
    /// Rotation totale
    private var angleTotale: Float = 0.0
    
    /// Noeuds sélectionnés
    private var noeuds = [NoeudCommun]()
    
    /// Fonction qui initialise l'état de déplacement
    override func initialiser() {
        // La rotation des noeuds s'effectue par la gesture rotate
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.rotateGestureRecognizer!)
    }
    
    override func nettoyerEtat() {
        /// Enlève la gesture rotate
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.rotateGestureRecognizer!)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func rotateGesture(sender: UIRotationGestureRecognizer) {
        super.rotateGesture(sender: sender)
        
        if sender.state == UIGestureRecognizerState.began {
            print("Rotate began")
            
            // Creation du visiteur
            let visiteur = VisiteurObtenirSelection()
            // Appel du visiteur
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
            // Recuperation des noeuds
            self.noeuds = visiteur.obtenirNoeuds()
            // Récupérer le centre de rotation
            self.centreRotation = visiteur.obtenirCentreSelection()
        }
        else if sender.state == UIGestureRecognizerState.ended {
            print("Fin rotation noeud")
            
            // Dernier rotation
            self.appliquerRotation(rotation: Float(-sender.rotation))
            
            // Set the rotation to 0 to avoid compounding the rotations
            sender.rotation = 0.0
            
            if !self.noeudsSurLaTable() {
                // Annuler la rotation
                self.annulerRotation()
            }
            
            // Clean state
            self.noeuds.removeAll()
            self.centreRotation = GLKVector3()
            self.angleTotale = 0.0
        }
        else {
            self.appliquerRotation(rotation: Float(-sender.rotation))
            
            // Set the rotation to 0 to avoid compounding the rotations
            sender.rotation = 0.0
        }
    }
    
    // Effectue la rotation des noeuds
    private func appliquerRotation(rotation: Float) {
        let angle = rotation
        
        // Update de l'angle totale
        self.angleTotale += Float(angle)
        
        for noeud in self.noeuds {
            // Effectue une translation du point central vers l'origine
            noeud.appliquerDeplacement(deplacement: GLKVector3Negate(self.centreRotation))
            
            // Effectuer la rotation
            noeud.appliquerRotation(angle: Float(angle), axes: GLKVector3.init(v: (x: 0.0, y: 1.0, z: 0.0)))
            
            // Remettre le point central à sa position initiale
            noeud.appliquerDeplacement(deplacement: self.centreRotation)
            
            //print("rotation envoyée")
            //print (GLKMathRadiansToDegrees(noeud.rotation.w))
            //print (GLKMathRadiansToDegrees(noeud.eulerAngles.y))
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                         pos: noeud.position, rotation: noeud.rotation.w, scale: noeud.scale)
        }
    }
    
    // Annuler la rotation
    private func annulerRotation() {
        for noeud in self.noeuds {
            // Effectue une translation du point central vers l'origine
            noeud.appliquerDeplacement(deplacement: GLKVector3Negate(self.centreRotation))
            
            // Effectuer la rotation
            noeud.appliquerRotation(angle: Float(-self.angleTotale),
                                    axes: GLKVector3.init(v: (x: 0.0, y: 1.0, z: 0.0)))
            
            // Remettre le point central à sa position initiale
            noeud.appliquerDeplacement(deplacement: self.centreRotation)
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                         pos: noeud.position, rotation: noeud.rotation.w, scale: noeud.scale)
        }
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
