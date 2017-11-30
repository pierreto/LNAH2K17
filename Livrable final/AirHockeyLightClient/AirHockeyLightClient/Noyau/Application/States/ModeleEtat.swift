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
    
    /// Position du dernier toucher associé au pan
    var lastPanPosition = CGPoint()
    
    /// Position du toucher associé au pan
    var panPosition = CGPoint()
    
    /// Pour l'affichage d'un message d'erreur et pour le son
    private var sendError: Bool = true
    private var timer: Timer?
    
    func initialiser() {}
    
    /// Annule l'action en cours
    func nettoyerEtat() {}
    
    // Fonctions gérant les entrées de l'utilisateur

    // TAP
    func tapGesture(point: CGPoint) {
        self.position = point
    }
    
    // IMMEDIATE PAN
    func panGesture(sender: ImmediatePanGestureRecognizer) {
        self.lastPosition = self.position
        self.position = sender.location(in: sender.view)
    }
    
    // NORMAL PAN
    func normalPanGesture(sender: UIPanGestureRecognizer) {
        self.lastPanPosition = self.panPosition
        self.panPosition = sender.translation(in: sender.view)
    }
    
    // PINCH
    func pinchGesture(sender: UIPinchGestureRecognizer) {
    }
    
    // ROTATE
    func rotateGesture(sender: UIRotationGestureRecognizer) {
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
            if self.sendError {
                self.sendError = false
                timer = Timer.scheduledTimer(timeInterval: 5, target: self, selector: #selector(self.resetSendError), userInfo: nil, repeats: false)
            
                // Message d'erreur
                FacadeModele.instance.obtenirVue().editorNotificationScene?.showErrorOutOfBoundMessage(activer: true)
            }
        }
        else {
            self.sendError = true
        }
        
        return sontSurTable
    }
    
    /// Réactive l'envoi de signaux d'erreur
    @objc private func resetSendError() {
        self.sendError = true
    }
    
    /// Détermine le déplacement d'un immediate pan gesture (delta)
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

    /// Détermine le déplacement d'un pan gesture normal (delta)
    func obtenirPanDeplacement() -> GLKVector3 {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        
        let start = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView,
                                                   depth: table.position.z, point: self.lastPanPosition)
        let end = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView,
                                                 depth: table.position.z, point: self.panPosition)
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
                portail.obtenirOppose()?.removeFromParentNode()
                
                // Envoyer la commande
                FacadeModele.instance.obtenirEtatEdition().currentUserDeletedNode(uuid: (portail.obtenirOppose()?.obtenirUUID())!)
            }
            
            noeud.removeFromParentNode()
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserDeletedNode(uuid: noeud.obtenirUUID())
        }
        
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        table.deselectionnerTout()
        
        // Envoyer la commande
        FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: "", isSelected: false, deselectAll: true)
    }
    
    // Affiche/Cache les bouttons du HUD et la barre de navigation si les noeuds sont sur la table
    func showButtonsNoeudSurTable() {
        let noeudsSurTable = self.noeudsSurLaTable()
        FacadeModele.instance.obtenirVue().editorHUDScene?.showButtonsNoeudSurTable(activer: noeudsSurTable)
        FacadeModele.instance.obtenirVue().enableNavigationBar(activer: noeudsSurTable)
    }
    
    // Affiche/Cache la barre de navigation si les noeuds sont sur la table
    func enableNavigationBar() {
        let noeudsSurTable = self.noeudsSurLaTable()
        FacadeModele.instance.obtenirVue().enableNavigationBar(activer: noeudsSurTable)
    }
    
    // Réactive tous les buttons du HUD et de la barre de navigation
    func reactiverButtons() {
        FacadeModele.instance.obtenirVue().editorHUDScene?.showButtonsNoeudSurTable(activer: true)
        FacadeModele.instance.obtenirVue().enableNavigationBar(activer: true)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
