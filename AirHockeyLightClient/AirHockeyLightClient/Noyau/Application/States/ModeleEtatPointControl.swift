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
    
    /// Cette fonction initialise l'etat. Elle ajuste la couleur des points
    /// de controle et rend ces derniers selectionnables
    override func initialiser() {
        // Rendre seulement les points de contrôle selectionnable
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_MUR, selectionnable: false))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_ACCELERATEUR, selectionnable: false))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_PORTAIL, selectionnable: false))
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_POINT_CONTROL, selectionnable: true))
        
        // Afficher les points en rose saumon
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        for child in (table.childNodes) {
            if (child.name == arbre.NOM_POINT_CONTROL) {
                let color = UIColor(red: 1.0, green: 85.0/255.0, blue: 82.0/255.0, alpha: 1.0)
                let pointControl = child as! NoeudPointControl
                pointControl.useOtherColor(activer: true, color: color)
            }
        }
        
        // Déselectionner tous les objets
        table.deselectionnerTout()
        
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
        
        // Annuler la couleur rose saumon
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true)
        for child in (table?.childNodes)! {
            if (child.name == arbre.NOM_POINT_CONTROL) {
                let pointControl = child as! NoeudPointControl
                pointControl.useOtherColor(activer: false)
            }
        }
        
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
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
                
                // TODO : À vérifier si on change la géométrie de la table ici ou non
                table.updateGeometry()
            }
            
            table.deselectionnerTout()
        }
        // Bouger le point de contrôle
        else {
            let visiteur = VisiteurDeplacement(delta: super.obtenirDeplacement())
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
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
        }
    }
    
    /// Cette fonction applique la position sauvée au noeud sélectionné
    func revertPosition() {
        // Revert position
        let visiteur = VisiteurObtenirSelection()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        let noeuds = visiteur.obtenirNoeuds()
    
        for noeud in noeuds {
            noeud.revertPosition()
            (noeud as! NoeudPointControl).obtenirNoeudOppose().revertPosition()
        }
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


