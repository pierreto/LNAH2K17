///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerPortail.swift
/// @author Pierre To
/// @date 2017-10-18
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerPortail
/// @brief Cette classe représente l'état de création des portails.
///        Implémente aussi le patron Singleton.
///
/// @author Pierre To
/// @date 2017-10-18
///////////////////////////////////////////////////////////////////////////
class ModeleEtatCreerPortail: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatCreerPortail()
    
    /// Premier portail
    private var premierNoeud: NoeudPortail?
    
    /// Fonction qui initialise l'état de création des portails
    override func initialiser() {
        /// Ignorer le tap associé au bouton pour la création de portails
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
        
        // Déselectionner tous les noeuds
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudCommun
        table.deselectionnerTout()
        
        // Activer la reconnaissance de tap pour la création de portails
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
    }
    
    /// Cette fonction nettoie l'état des changements apportes. Elle annule l'action en cours lors d'un changement d'état
    override func nettoyerEtat() {
        self.annulerCreation()
    }
    
    func annulerCreation() {
        if (self.premierNoeud != nil) {
            // Supprimer le noeud ajouté
            //FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->effacer(premierNoeud_);
            self.premierNoeud?.removeFromParentNode()
            self.premierNoeud?.effetFantome(activer: false)
            self.premierNoeud = nil
        }
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    
    /// Évènement appelé lorsque l'utilisateur tap sur l'écran
    override func tapGesture(point: CGPoint) {
        // TODO : Vérifier que c'est au-dessus de la table tapOverTable()
        //if (mouseDownL_ && isAClick() && mouseOverTable()) {
        
        super.tapGesture(point: point)
        
        // Création du noeud
        let noeud = FacadeModele.instance.obtenirArbreRendu().creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_PORTAIL) as! NoeudPortail
            
        // Déplacement du noeud
        // Transformation du point dans l'espace virtuelle
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        let convertedPoint = MathHelper.GetHitTestSceneViewCoordinates(point: self.position)
        let position = GLKVector3.init(v: (convertedPoint.x, convertedPoint.y, convertedPoint.z))
        noeud.assignerPositionRelative(positionRelative: position)
        
        // Ajout du noeud à l'arbre de rendu
        arbre.addChildNode(noeud)
            
        // Etat de l'operation
        if (self.premierNoeud == nil) {
            self.premierNoeud = noeud
            self.premierNoeud?.effetFantome(activer: true)
        }
        else {
            // Verification que les noeuds soient sur la table
            // TODO : implémenter VisiteurSurTable
            //if (noeudsSurLaTable()) {
                // Link portal together
                noeud.assignerOppose(portail: self.premierNoeud!);
                self.premierNoeud?.assignerOppose(portail: noeud);
                self.premierNoeud?.effetFantome(activer: false);
                self.premierNoeud = nil;
            //}
            //else { // Annulation de la commande
            //    self.annulerCreation()
            //    noeud.removeFromParentNode()
            //}
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
