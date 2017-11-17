///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatSelection.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatSelection
/// @brief Cette classe représente l'état de sélection. Implémente aussi le
///        patron Singleton. Cette classe effectue les transformations
///        (déplacement, rotation, mise à l'échelle) sur les noeuds sélectionnés.
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class ModeleEtatSelection: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatSelection()
    
    /// Déplacement total appliqué
    private var deplacementTotal = GLKVector3()
    
    /// Centre de rotation
    private var centreRotation = GLKVector3()
    
    /// Rotation totale
    private var angleTotale: Float = 0.0
    
    /// Noeuds sélectionnés
    private var noeuds = [NoeudCommun]()
    
    /// Cette fonction initialise l'état. Elle décide quels objects sont
    /// sélectionnable
    override func initialiser() {
        // Rendre tous les objets non selectionnables
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_MUR, selectionnable: true));
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_PORTAIL, selectionnable: true));
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_ACCELERATEUR, selectionnable: true));
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_POINT_CONTROL, selectionnable: false));
        
        /// Afficher la configuration des propriétés de l'objet
        FacadeModele.instance.obtenirVue().showObjectPropertiesView(activer: true)
        
        // Le déplacement des noeuds s'effectue par un gesture pan
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.normalPanGestureRecognizer!)
        
        // La rotation des noeuds s'effectue par la gesture rotate
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.rotateGestureRecognizer!)
        
        // La mise à l'échelle des noeuds s'effectue par la gesture pinch
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.pinchGestureRecognizer!)
        
        // Désactiver le contrôle de la caméra
        FacadeModele.instance.obtenirVue().editorView.allowsCameraControl = false
    }
    
    override func nettoyerEtat() {
        /// Cacher la configuration des propriétés de l'objet
        FacadeModele.instance.obtenirVue().showObjectPropertiesView(activer: false)
        
        /// Enlève la gesture pan
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.normalPanGestureRecognizer!)
        
        /// Enlève la gesture rotate
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.rotateGestureRecognizer!)
        
        /// Enlève la gesture pinch
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.pinchGestureRecognizer!)
        
        // Réactiver le contrôle de la caméra
        FacadeModele.instance.obtenirVue().editorView.allowsCameraControl = true
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    
    // TAP
    override func tapGesture(point: CGPoint) {
        super.tapGesture(point: point)
        
        print("Tap gesture")
        
        let visiteur = VisiteurSelection(point: self.position)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
    }
    
    // NORMAL PAN
    override func normalPanGesture(sender: UIPanGestureRecognizer) {
        super.normalPanGesture(sender: sender)
        
        if sender.state == UIGestureRecognizerState.began {
            print("Debut deplacement noeud")
        }
        else if sender.state == UIGestureRecognizerState.ended {
            print("Fin deplacement noeud")
            
            // Jouer le son
            AudioService.instance.playSound(soundName: EDITION_SOUND.TRANSFORM.rawValue)
            
            // Dernier déplacement
            self.deplacer()
            
            if !self.noeudsSurLaTable() {
                // Annuler le déplacement
                self.annulerDeplacement()
            }
            
            self.reactiverButtons()
            
            // Réinitialiser le déplacement total
            self.deplacementTotal = GLKVector3.init(v: (0.0, 0.0, 0.0))
        }
        else {
            // Déplacer le noeud
            self.deplacer()
            
            self.showButtonsNoeudSurTable()
        }
    }
    
    // ROTATE
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
            
            // Jouer le son
            AudioService.instance.playSound(soundName: EDITION_SOUND.TRANSFORM.rawValue)
            
            // Dernier rotation
            self.appliquerRotation(rotation: Float(-sender.rotation))
            
            // Set the rotation to 0 to avoid compounding the rotations
            sender.rotation = 0.0
            
            if !self.noeudsSurLaTable() {
                // Annuler la rotation
                self.annulerRotation()
            }
            
            // Clean state
            self.reactiverButtons()
            self.noeuds.removeAll()
            self.centreRotation = GLKVector3()
            self.angleTotale = 0.0
        }
        else {
            self.appliquerRotation(rotation: Float(-sender.rotation))
            
            // Set the rotation to 0 to avoid compounding the rotations
            sender.rotation = 0.0
            
            self.showButtonsNoeudSurTable()
        }
    }
    
    // PINCH
    override func pinchGesture(sender: UIPinchGestureRecognizer) {
        super.pinchGesture(sender: sender)
        
        if sender.state == UIGestureRecognizerState.began {
            print("Pinch began")
            
            // Sauvegarder la valeur du scale des noeuds
            self.saveScale()
        }
        else if sender.state == UIGestureRecognizerState.ended {
            print("Pinch ended")
            
            // Jouer le son
            AudioService.instance.playSound(soundName: EDITION_SOUND.TRANSFORM.rawValue)
            
            // Dernier rotation
            self.appliquerScaling(scale: Float(sender.scale))
            
            // Set the scale factor to 1.0 to avoid exponential growth
            sender.scale = 1.0
            
            if !self.noeudsSurLaTable() {
                // Annuler la rotation
                self.revertScale()
            }
            
            self.reactiverButtons()
        }
        else {
            self.appliquerScaling(scale: Float(sender.scale))
            
            // Set the scale factor to 1.0 to avoid exponential growth
            sender.scale = 1.0
            
            self.showButtonsNoeudSurTable()
        }
    }
    
    // Déplacer des noeuds
    private func deplacer() {
        let deplacement = super.obtenirPanDeplacement()
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
    }
    
    // Effectue la rotation des noeuds
    private func appliquerRotation(rotation: Float) {
        let angle = rotation
        
        // Update de l'angle totale
        self.angleTotale += Float(angle)
        
        for noeud in self.noeuds {
            if noeud.estSelectionne() {
                // Effectue une translation du point central vers l'origine
                noeud.appliquerDeplacement(deplacement: GLKVector3Negate(self.centreRotation))

                // Effectuer la rotation
                noeud.appliquerRotation(angle: Float(angle), axes: GLKVector3.init(v: (x: 0.0, y: 1.0, z: 0.0)))

                // Remettre le point central à sa position initiale
                noeud.appliquerDeplacement(deplacement: self.centreRotation)

                // Envoyer la commande
                FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                             pos: noeud.position,
                                                                                             rotation: MathHelper.determinerAngleAxeY(rotation: noeud.rotation),
                                                                                             scale: noeud.scale)
            }
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
                                                                                         pos: noeud.position,
                                                                                         rotation: MathHelper.determinerAngleAxeY(rotation: noeud.rotation),
                                                                                         scale: noeud.scale)
        }
    }
    
    /// Applique le scaling
    private func appliquerScaling(scale: Float) {
        // Calcul du nouveau scale
        // Pinch out (grossir)  : >= 1.0. Pinch in (rapetisser) : < 1.0
        let scale = Float(scale)
        
        // Apply nouveau scale
        let visiteur = VisiteurScale(scale: scale)
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
    private func revertScale() {
        // Revert scale on matrice
        let visiteur = VisiteurObtenirSelection()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        let noeuds = visiteur.obtenirNoeuds()
        
        for noeud in noeuds {
            noeud.revertScale()
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                         pos: noeud.position,
                                                                                         rotation: MathHelper.determinerAngleAxeY(rotation: noeud.rotation),
                                                                                         scale: noeud.scale)
        }
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
