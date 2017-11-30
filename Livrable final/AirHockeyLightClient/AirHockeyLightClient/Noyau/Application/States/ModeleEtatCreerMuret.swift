///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerMuret.swift
/// @author Mikael Ferland
/// @date 2017-10-18
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerMuret
/// @brief Classe concrète du patron State pour l'état de création des murets
///        
///        Cette classe représente l'état de création des murets. Implémente aussi le
///        patron Singleton.
///
/// @author Mikael Ferland
/// @date 2017-10-18
///////////////////////////////////////////////////////////////////////////
class ModeleEtatCreerMuret: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatCreerMuret()
    
    /// Noeud en création
    private var noeud : NoeudCommun?
    
    /// Point initial pour la création du muret
    private var pointInitial = GLKVector3()
    
    /// Joue le son de la fin
    private var playEndSound : Bool = false
    
    /// Fonction qui initialise l'état de création de murets
    override func initialiser() {
        super.initialiser();
        
        /// Ignorer le tap associé au bouton pour la création des murets
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
        
        // Déselectionner tous les noeuds
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudCommun
        table.deselectionnerTout()
        
        // Envoyer la commande
        FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: "", isSelected: false, deselectAll: true)
        
        // Activer la reconnaissance de tap pour la création des murets
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.tapGestureRecognizer!)
        
        // La création des murets s'effectue via un gesture pan
        FacadeModele.instance.obtenirVue().editorView.addGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
    }
    
    /// Cette fonction nettoie l'état des changements apportes
    override func nettoyerEtat() {
        FacadeModele.instance.obtenirVue().editorView.removeGestureRecognizer(FacadeModele.instance.panGestureRecognizer!)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    
    override func panGesture(sender: ImmediatePanGestureRecognizer) {
        super.panGesture(sender: sender)
        
            if sender.state == UIGestureRecognizerState.began && self.noeud == nil {
                print("BEGIN")
                
                // Création du noeud
                let arbre = FacadeModele.instance.obtenirArbreRendu()
                self.noeud = arbre.creerNoeud(typeNouveauNoeud: arbre.NOM_MUR, uuid: "") as! NoeudMur
                
                // Déplacement du noeud (avec transformation du point dans l'espace virtuelle)
                let point = MathHelper.GetHitTestSceneViewCoordinates(point: self.position)
                
                if (point != nil) {
                    self.noeud?.assignerAxisLock(axisLock: GLKVector3.init(v: (1, 1, 1)))
                    self.noeud?.assignerPositionRelative(positionRelative: GLKVector3.init(v: (Float((point?.x)!), Float((point?.y)! + 0.01), Float((point?.z)!))))
                    self.noeud?.assignerAxisLock(axisLock: GLKVector3.init(v: (1, 0, 1)))
                
                    self.pointInitial = GLKVector3.init(v: ((point?.x)!, (point?.y)!, (point?.z)!))
                
                    // Activer effet sélection
                    self.noeud?.appliquerMaterielSelection(activer: true)
                
                    // Ajout du noeud à l'arbre de rendu
                    let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
                    table.addChildNode(self.noeud!)
                    
                    // Jouer le son
                    AudioService.instance.playSound(soundName: EDITION_SOUND.OBJECT1.rawValue)
                    self.playEndSound = true
                }
                else {
                    // Afficher un message d'erreur
                    FacadeModele.instance.obtenirVue().editorNotificationScene?.showErrorOutOfBoundMessage(activer: true)
                }
            }
            else if sender.state == UIGestureRecognizerState.ended {
                print("END")
                
                if (self.noeudsSurLaTable()) {
                    // Desactiver effet sélection
                    self.noeud?.appliquerMaterielSelection(activer: false)

                    // Envoyer la commande
                    FacadeModele.instance.obtenirEtatEdition().currentUserCreatedWall(uuid: (noeud?.obtenirUUID())!,
                                                                                      pos: (noeud?.position)!,
                                                                                      rotation: (noeud?.rotation.w)!,
                                                                                      scale: (noeud?.scale)!)
                    
                    self.noeud = nil
                    
                    if playEndSound {
                        // Jouer le son
                        AudioService.instance.playSound(soundName: EDITION_SOUND.OBJECT2.rawValue)
                        self.playEndSound = false
                        
                        FacadeModele.instance.sauvegarderCarte(map: EditorViewController.instance.currentMap!)
                    }
                }
                else {
                    if (self.noeud != nil) {
                        self.noeud?.removeFromParentNode()
                        self.noeud = nil
                    }
                }
                
            }
            else if self.noeud != nil {
                // Transformation du point dans l'espace virtuelle
                let convertedPoint = MathHelper.GetHitTestSceneViewCoordinates(point: self.position)
                
                if convertedPoint != nil {
                    let point = GLKVector3.init(v: ((convertedPoint?.x)!, (convertedPoint?.y)!, (convertedPoint?.z)!))

                    // Calculer la distance pour le scaling
                    let distance = GLKVector3Distance(point, self.pointInitial)
                    self.noeud?.scale = SCNVector3(1, 1, distance)
                
                    // Calculer l'angle pour la rotation
                    let node2mouse = GLKVector3Subtract(point, self.pointInitial)
        
                    let x = GLKVector3.init(v: (0, 0, 1))
                    let y = GLKVector3Normalize(node2mouse)
                    let dotProduct = GLKVector3DotProduct(x, y)
                    let clampedDot = MathHelper.clamp(value: dotProduct, lower: -1, upper: 1)
                    let angle = acos(clampedDot)
                
                    let ref = GLKVector3.init(v: (0, 1, 0))
                    let crossProduct = GLKVector3CrossProduct(x, y)
                    let dotCrossProduct = GLKVector3DotProduct(ref, crossProduct)
                    let finalAngle = dotCrossProduct >= 0 ? angle : -angle
                    self.noeud?.rotation = SCNVector4(0, 1, 0, finalAngle)
                    
                    // Calculer le déplacement
                    var position = GLKVector3.init(v: (self.pointInitial.x, self.pointInitial.y, self.pointInitial.z));
                    position.x += Float(sin(finalAngle) * distance / 2)
                    position.z += Float(cos(finalAngle) * distance / 2)
                    self.noeud?.position = SCNVector3FromGLKVector3(position)
                }
            }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
