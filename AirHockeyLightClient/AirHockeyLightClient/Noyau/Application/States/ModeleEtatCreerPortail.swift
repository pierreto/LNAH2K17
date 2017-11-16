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
        
        // Envoyer la commande
        FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: "", isSelected: false, deselectAll: true)
        
        // Activer la reconnaissance de tap pour la création de portails
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
    }
    
    /// Cette fonction nettoie l'état des changements apportes. Elle annule l'action en cours lors d'un changement d'état
    override func nettoyerEtat() {
        self.annulerCreation()
    }
    
    func annulerCreation() {
        /// Ignorer le tap associé au bouton pour la création de portails
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
        
        if (self.premierNoeud != nil) {
            // Supprimer le noeud ajouté
            self.premierNoeud?.removeFromParentNode()
            self.premierNoeud = nil
            FacadeModele.instance.obtenirVue().editorHUDScene?.showCancelButton(activer: false)
        }
        
        // Activer la reconnaissance de tap pour la création de portails
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    
    /// Évènement appelé lorsque l'utilisateur tap sur l'écran
    override func tapGesture(point: CGPoint) {
        super.tapGesture(point: point)
        
        // Création du noeud
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let noeud = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_PORTAIL, uuid: "") as! NoeudPortail
            
        // Déplacement du noeud
        // Transformation du point dans l'espace virtuelle
        let convertedPoint = MathHelper.GetHitTestSceneViewCoordinates(point: self.position)
        
        if convertedPoint != nil {
            let position = GLKVector3.init(v: ((convertedPoint?.x)!, (convertedPoint?.y)!, (convertedPoint?.z)!))
            noeud.assignerPositionRelative(positionRelative: position)
        
            // Ajout du noeud à l'arbre de rendu
            let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
            table.addChildNode(noeud)
            
            // Etat de l'operation
            if (self.premierNoeud == nil) {
                self.premierNoeud = noeud
                self.premierNoeud?.appliquerMaterielSelection(activer: true)
                FacadeModele.instance.obtenirVue().editorHUDScene?.showCancelButton(activer: true)
                
                // Jouer le son
                AudioService.instance.playSound(soundName: EDITION_SOUND.OBJECT1.rawValue)
            }
            else {
                // Verification que les noeuds soient sur la table
                // TODO : implémenter VisiteurSurTable
                if (self.noeudsSurLaTable()) {
                    // Link portal together
                    noeud.assignerOppose(portail: self.premierNoeud!);
                    self.premierNoeud?.assignerOppose(portail: noeud);
                    self.premierNoeud?.appliquerMaterielSelection(activer: false)
                    
                    // Envoyer la commande
                    FacadeModele.instance.obtenirEtatEdition().currentUserCreatedPortal(
                        startUuid: (self.premierNoeud?.obtenirUUID())!,
                        startPos: (self.premierNoeud?.position)!, startRotation: (self.premierNoeud?.rotation.w)!, startScale: (self.premierNoeud?.scale)!,
                        endUuid: noeud.obtenirUUID(),
                        endPos: noeud.position, endRotation: noeud.rotation.w, endScale: noeud.scale)
                    
                    self.premierNoeud = nil;
                    FacadeModele.instance.obtenirVue().editorHUDScene?.showCancelButton(activer: false)
                    
                    // Jouer le son
                    AudioService.instance.playSound(soundName: EDITION_SOUND.OBJECT2.rawValue)
                    
                    FacadeModele.instance.sauvegarderCarte(map: EditorViewController.instance.currentMap!)
                }
                else { // Annulation de la commande
                    self.annulerCreation()
                    noeud.removeFromParentNode()
                }
            }
        }
        else {
            // Afficher un message d'erreur
            FacadeModele.instance.obtenirVue().editorNotificationScene?.showErrorOutOfBoundMessage(activer: true)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
