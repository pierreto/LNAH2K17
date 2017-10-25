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
    private var estCopie: Bool = false
    
    /// Fonction qui initialise l'état de création de murets
    override func initialiser() {
        super.initialiser();
        
        // La duplication des noeuds s'effectue via un gesture pan
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
        
        self.estCopie = false
    }
    
    /// Cette fonction nettoie l'état des changements apportes
    override func nettoyerEtat() {
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func panGesture(sender: ImmediatePanGestureRecognizer) {
        super.panGesture(sender: sender)
        
        if sender.state == UIGestureRecognizerState.began {
            print("Debut duplication noeud")
            // Faire apparaitre le tampon
            self.dupliquerNoeud()
        }
        else if sender.state == UIGestureRecognizerState.ended {
            print("Fin duplication noeud")
            // Dupliquer les noeuds
            
            // Dernier déplacement
            //let deplacement = super.obtenirDeplacement()
            //self.deplacerNoeud(deplacement: deplacement)
            
            if (noeudsSurLaTable()) {
                // Redupliquer les objets sont dans la zone de jeu
                //let duplication = VisiteurDuplication();
                //FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: duplication)
            }
            else {
                // Annuler la duplication
                //let visiteur = VisiteurObtenirSelection()
                //FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
                //let noeuds = visiteur.obtenirNoeuds()
                
                //for noeud in noeuds {
                //    noeud.removeFromParentNode()
                //}
            }
            
            //self.reset()
        }
        else {
            // Déplacer le noeud fantôme
            let deplacement = super.obtenirDeplacement()
            self.deplacerNoeud(deplacement: deplacement)
        }
    }
    
    /// Déplace un fantome de la sélection
    func dupliquerNoeud() {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        if !self.estCopie {
            let duplication = VisiteurDuplication()
            arbre.accepterVisiteur(visiteur: duplication)
            let centreDuplication = duplication.obtenirCentreDuplication()
            
            let deplacement = obtenirDeplacementDuplication(centreDuplication: centreDuplication)
            self.deplacerNoeud(deplacement: deplacement)
    
            self.estCopie = true
        }
    }
    
    /// Détermine le déplacement pour la duplication (delta)
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
    
    /// Supprime noeud fantôme et remet à l'état initial avant duplication
    func reset() {
        //VisiteurSuppression suppression = VisiteurSuppression();
        //FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&suppression);
        //suppression.deleteAllSelectedNode();
        
        self.estCopie = false
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
