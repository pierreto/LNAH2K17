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
import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtat
/// @brief Cette classe comprend l'interface de base que doivent
///        implanter tous les états possibles du modèle
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class ModeleEtat {
    
    /// Dernière position
    var lastPosition = CGPoint()
    
    /// Position du toucher
    var position = CGPoint()
    
    func initialiser() {}
    
    /// Annule l'action en cours
    func nettoyerEtat() {}
    
    // Fonctions gérant les entrées de l'utilisateur

    // TAP
    func tapGesture(point: CGPoint) {
        self.position = point
    }
    
    // PAN
    func panGesture(sender: ImmediatePanGestureRecognizer) {
        self.lastPosition = self.position
        self.position = sender.location(in: sender.view)
    }
    
    // PINCH
    func pinchGesture(sender: UIPinchGestureRecognizer) {
    }
    
    /// Fonction pour obtenir la vue rapidement
    func obtenirVue() -> UIViewController {
        return FacadeModele.instance.obtenirVue()
    }
    
    /// Vérifie que les objets sont sur la table.
    func noeudsSurLaTable() -> Bool {
        let visiteur = VisiteurSurTable()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        let sontSurTable = visiteur.obtenirSontSurTable()
        
        // Faire apparaître un message d'erreur
        if !sontSurTable {
            FacadeModele.instance.obtenirVue().editorHUDScene?.showErrorOutOfBoundMessage(activer: true)
        }
        
        return sontSurTable
    }
    
    /// Détermine le déplacement (delta)
    func obtenirDeplacement() -> GLKVector3 {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        
        let start = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView,
                                                   depth: table.position.z, point: self.lastPosition)
        let end = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView,
                                                 depth: table.position.z, point: self.position)
        let delta = GLKVector3Make(end.x - start.x, end.y - start.y, end.z - start.z)
        
        return delta
    }
    
    /// Supprimer les noeuds sélectionnés
    final func supprimerSelection() {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        /// Supprimer les objets sélectionnés
        let visiteur = VisiteurObtenirSelection()
        arbre.accepterVisiteur(visiteur: visiteur)
        let noeuds = visiteur.obtenirNoeuds()
        
        for noeud in noeuds {
            // Suppression du portail opposé qu'il soit sélectionné ou non
            if noeud.obtenirType() == arbre.NOM_PORTAIL {
                let portail = noeud as! NoeudPortail
                portail.obtenirOppose().removeFromParentNode()
            }
            
            noeud.removeFromParentNode()
        }
        
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        table.deselectionnerTout()
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
