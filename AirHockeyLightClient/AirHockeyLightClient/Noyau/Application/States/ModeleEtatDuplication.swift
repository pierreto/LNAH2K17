///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatDuplication.swift
/// @author Pierre To
/// @date 2017-10-24
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatDuplication
/// @brief Cette classe représente l'état de duplication. Implémente aussi le
///        patron Singleton.
///
/// @author Pierre To
/// @date 2017-10-24
///////////////////////////////////////////////////////////////////////////
class ModeleEtatDuplication: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatDuplication()
    
    /// Booléen pour savoir si on est en copie
    private var copieEnCours: Bool = false
    
    /// Fonction qui initialise l'état de création de murets
    override func initialiser() {
        super.initialiser();
        
        // La duplication des noeuds s'effectue via un gesture pan
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
        
        // Désactiver le bouton de suppression
        FacadeModele.instance.obtenirVue().editorHUDScene?.disableDeleteButton()
        
        self.copieEnCours = false
    }
    
    /// Cette fonction nettoie l'état des changements apportes
    override func nettoyerEtat() {
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
        
        /// Enlever tous les noeuds hors de la table
        /// En théorie, ces noeuds devraient être encore sélectionnés
        if !noeudsSurLaTable() {
            let visiteur = VisiteurObtenirSelection()
            FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
            let noeuds = visiteur.obtenirNoeuds()
        
            for noeud in noeuds {
                noeud.removeFromParentNode()
            }
        }
        
        // Réactiver le bouton de suppression
        FacadeModele.instance.obtenirVue().editorHUDScene?.enableDeleteButton()
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func panGesture(sender: ImmediatePanGestureRecognizer) {
        super.panGesture(sender: sender)
        
        if sender.state == .began {
            print("Debut duplication noeud")
            
            if !copieEnCours {
                // Faire apparaitre le tampon
                self.dupliquerNoeud()
            }
            else {
                // Déplacer les noeuds dupliqués où l'utilisateur touche l'écran
                let visiteur = VisiteurObtenirSelection()
                FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
                let centreDuplication = visiteur.obtenirCentreSelection()
                
                let deplacement = obtenirDeplacementDuplication(centreDuplication: centreDuplication)
                self.deplacerNoeud(deplacement: deplacement)
            }
        }
        else if sender.state == .changed {
            // Déplacer les noeuds dupliqués
            let deplacement = super.obtenirDeplacement()
            self.deplacerNoeud(deplacement: deplacement)
        }
        else if sender.state == .ended {
            print("Fin duplication noeud")
            
            // Dernier déplacement
            let deplacement = super.obtenirDeplacement()
            self.deplacerNoeud(deplacement: deplacement)
            
            if (noeudsSurLaTable()) {
                let visiteur = VisiteurObtenirSelection()
                FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
                let noeuds = visiteur.obtenirNoeuds()
                
                // Valider la position des noeuds dupliqués (en enlevant l'effet sélection)
                // Ceux-ci restent sélectionnés pour des duplications successives
                for noeud in noeuds {
                    noeud.appliquerMaterielSelection(activer: false)
                }
                
                self.copieEnCours = false
            }
        }
    }
    
    /// Dupliquer les noeuds sélectionnés
    func dupliquerNoeud() {
        let duplication = VisiteurDuplication()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: duplication)
        
        // Déplacer les noeuds dupliqués où l'utilisateur touche l'écran
        let centreDuplication = duplication.obtenirCentreDuplication()
        let deplacement = obtenirDeplacementDuplication(centreDuplication: centreDuplication)
        self.deplacerNoeud(deplacement: deplacement)
    
        self.copieEnCours = true
    }
    
    /// Déterminer le déplacement des noeuds dupliqués au toucher de l'utilisateur
    func obtenirDeplacementDuplication(centreDuplication: GLKVector3) -> GLKVector3 {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        
        let start = centreDuplication
        let end = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView,
                                                 depth: table.position.z, point: self.position)
        let delta = GLKVector3Make(end.x - start.x, end.y - start.y, end.z - start.z)
        
        return delta
    }
    
    /// Déplacer des noeuds
    private func deplacerNoeud(deplacement: GLKVector3) {
        let visiteur = VisiteurDeplacement(delta: deplacement)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
