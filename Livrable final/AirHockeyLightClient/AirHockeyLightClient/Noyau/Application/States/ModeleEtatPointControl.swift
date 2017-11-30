///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatPointControl.swift
/// @author Pierre To
/// @date 2017-10-10
/// @version 1.0
///
/// @addtogroup log3900 LOG2990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatPointControl
/// @brief Classe concrète du patron State pour l'état de gestion des points 
///        de controle. Implémente aussi le patron Singleton.
///
/// @author Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class ModeleEtatPointControl : ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatPointControl()
    
    private var playEndSound: Bool = false
    
    /// Cette fonction initialise l'etat. Elle ajuste la couleur des points
    /// de controle et rend ces derniers selectionnables
    override func initialiser() {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        
        // Désactiver les boutons associés à la sélection
        FacadeModele.instance.obtenirVue().editorHUDScene?.disableSelectContextButtons()
        
        // Déselectionner tous les objets
        table.deselectionnerTout()
        
        // Envoyer la commande
        FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: "", isSelected: false, deselectAll: true)
        
        // Rendre seulement les points de contrôle selectionnable
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_MUR, selectionnable: false))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_ACCELERATEUR, selectionnable: false))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_PORTAIL, selectionnable: false))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_POINT_CONTROL, selectionnable: true))
        
        // Le déplacement des points de contrôle s'effectue par un gesture pan
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
    }

    /// Cette fonction nettoie l'état des changements apportes
    override func nettoyerEtat() {
        // Retablir les objets selectionnables
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_MUR, selectionnable: true))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_ACCELERATEUR, selectionnable: true))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_PORTAIL, selectionnable: true))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_POINT_CONTROL, selectionnable: false))
        
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
        
        // Réactiver les boutons associés à la sélection
        FacadeModele.instance.obtenirVue().editorHUDScene?.enableSelectContextButtons()
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func tapGesture(point: CGPoint) {
        super.tapGesture(point: point)
        
        let visiteur = VisiteurSelection(point: self.position)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func panGesture(sender: ImmediatePanGestureRecognizer) {
        super.panGesture(sender: sender)
        
        // Sélectionner le point de contrôle
        if sender.state == UIGestureRecognizerState.began {
            print("Debut deplacement point contrôle")
            
            let visiteurSelection = VisiteurSelection(point: self.position)
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteurSelection)
            
            // Sauver la position du noeud selectionne
            savePosition()
        }
        // Déselectionner le point de contrôle
        else if sender.state == UIGestureRecognizerState.ended {
            print("Fin deplacement point contrôle")
            let arbre = FacadeModele.instance.obtenirArbreRendu()
            let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
            
            let visiteur = VisiteurDeplacement(delta: super.obtenirDeplacement())
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
            
            if !self.noeudsSurLaTable() {
                // Annuler le déplacement
                revertPosition()
                
                table.updateGeometry()
            }
            
            self.reactiverButtons()
            table.deselectionnerTout()
            
            if self.playEndSound {
                // Jouer le son
                AudioService.instance.playSound(soundName: EDITION_SOUND.SELECTION2.rawValue)
                self.playEndSound = false
                
                FacadeModele.instance.sauvegarderCarte(map: EditorViewController.instance.currentMap!)
            }
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: "", isSelected: false, deselectAll: true)
        }
        // Bouger le point de contrôle
        else {
            let visiteur = VisiteurDeplacement(delta: super.obtenirDeplacement())
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
            
            self.showButtonsNoeudSurTable()
        }
    }
    
    /// Cette fonction sauve la position du noeud selectionne
    private func savePosition() {
        // Save position of matrice
        let visiteur = VisiteurObtenirSelection()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        let noeuds = visiteur.obtenirNoeuds()
    
        for noeud in noeuds {
            noeud.savePosition()
            (noeud as! NoeudPointControl).obtenirNoeudOppose().savePosition()
            
            self.playEndSound = true;
        }
    }
    
    /// Cette fonction applique la position sauvée au noeud sélectionné
    private func revertPosition() {
        // Revert position
        let visiteur = VisiteurObtenirSelection()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        let noeuds = visiteur.obtenirNoeuds()
    
        for noeud in noeuds {
            noeud.revertPosition()
            (noeud as! NoeudPointControl).obtenirNoeudOppose().revertPosition()
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserControlPointChanged(uuid: noeud.obtenirUUID(), pos: noeud.position)
            FacadeModele.instance.obtenirEtatEdition().currentUserControlPointChanged(
                uuid: (noeud as! NoeudPointControl).obtenirNoeudOppose().obtenirUUID(),
                pos: (noeud as! NoeudPointControl).obtenirNoeudOppose().position)
        }
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


