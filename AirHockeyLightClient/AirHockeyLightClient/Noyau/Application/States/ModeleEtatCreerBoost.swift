///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerBoost.swift
/// @author Pierre To
/// @date 2017-10-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerBoost
/// @brief Cette classe représente l'état de création des portails.
///        Implémente aussi le patron Singleton.
///
/// @author Pierre To
/// @date 2017-10-19
///////////////////////////////////////////////////////////////////////////
class ModeleEtatCreerBoost: ModeleEtat {

    /// Instance singleton
    static let instance = ModeleEtatCreerBoost()
    
    /// Fonction qui initialise l'état de création d'accélérateurs
    override func initialiser() {
        /// Ignorer le tap associé au bouton pour la création d'accélérateurs
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
        
        // Déselectionner tous les noeuds
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudCommun
        table.deselectionnerTout()
        
        // Activer la reconnaissance de tap pour la création d'accélérateurs
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
    }
    
    /// Évènement appelé lorsque l'utilisateur tap sur l'écran
    override func tapGesture(point: CGPoint) {
        // TODO : Vérifier que c'est au-dessus de la table tapOverTable()
        //if (mouseDownL_ && isAClick() && mouseOverTable()) {
        
        super.tapGesture(point: point)
        
        // Création du noeud
        let noeud = FacadeModele.instance.obtenirArbreRendu().creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_ACCELERATEUR) as! NoeudAccelerateur
        
        // Déplacement du noeud
        // Transformation du point dans l'espace virtuelle
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let convertedPoint = MathHelper.GetHitTestSceneViewCoordinates(point: self.position)
        
        if convertedPoint != nil {
            let position = GLKVector3.init(v: ((convertedPoint?.x)!, (convertedPoint?.y)!, (convertedPoint?.z)!))
            noeud.assignerPositionRelative(positionRelative: position)
            
            // Ajout du noeud à l'arbre de rendu
            arbre.addChildNode(noeud)
        }
        
        // Verification que les noeuds soient sur la table
        if (!self.noeudsSurLaTable()) {
            // Annulation de la commande
            noeud.removeFromParentNode()
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

