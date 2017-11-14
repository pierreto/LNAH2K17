///////////////////////////////////////////////////////////////////////////////
/// @file EditorHUDScene.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SpriteKit
import GameplayKit

///////////////////////////////////////////////////////////////////////////
/// @class EditorHUDScene
/// @brief Cette classe contrôle le HUD de l'éditeur
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-12
///////////////////////////////////////////////////////////////////////////
class EditorHUDScene: SKScene {
    
    /// Boutons d'édition
    private var currentButton: SKSpriteNode?
    private var cameraControlButton: SKSpriteNode?
    private var selectionButton: SKSpriteNode?
    private var duplicateButton: SKSpriteNode?
    private var cancelButton: SKSpriteNode?
    private var deselectAllButton: SKSpriteNode?
    private var deleteButton: SKSpriteNode?
    private var pointControlButton: SKSpriteNode?
    private var portalButton: SKSpriteNode?
    private var wallButton: SKSpriteNode?
    private var boosterButton: SKSpriteNode?
    
    private var selectContextButtonsEnable: Bool = true

    private let SELECTED_COLOR = UIColor(red: 153.0/255.0, green: 204.0/255.0, blue: 1.0, alpha: 1.0)
    
    override func sceneDidLoad() {
        if #available(iOS 10.0, *) {
            super.sceneDidLoad()
        } else {
            // Fallback on earlier versions
        }
        
        // Chercher les boutons noeud
        self.cameraControlButton = (self.childNode(withName: "//cameraControlButton") as? SKSpriteNode)!
        self.selectionButton = (self.childNode(withName: "//selectionButton") as? SKSpriteNode)!
        self.duplicateButton = (self.childNode(withName: "//duplicateButton") as? SKSpriteNode)!
        self.cancelButton = (self.childNode(withName: "//cancelButton") as? SKSpriteNode)!
        self.deselectAllButton = (self.childNode(withName: "//deselectAllButton") as? SKSpriteNode)!
        self.deleteButton = (self.childNode(withName: "//deleteButton") as? SKSpriteNode)!
        self.pointControlButton = (self.childNode(withName: "//pointControlButton") as? SKSpriteNode)!
        self.portalButton = (self.childNode(withName: "//portalButton") as? SKSpriteNode)!
        self.wallButton = (self.childNode(withName: "//wallButton") as? SKSpriteNode)!
        self.boosterButton = (self.childNode(withName: "//boosterButton") as? SKSpriteNode)!
        
        /// Cacher le bouton cancel
        self.cancelButton?.isHidden = true
        /// Cacher le bouton déselection
        self.deselectAllButton?.isHidden = true
        /// Cacher le bouton delete
        self.deleteButton?.isHidden = true
        
        // Charger les images
        self.cameraControlButton?.texture = SKTexture(imageNamed: "Camera");
        self.selectionButton?.texture = SKTexture(imageNamed: "Tap")
        self.duplicateButton?.texture = SKTexture(imageNamed: "Duplicate")
        self.cancelButton?.texture = SKTexture(imageNamed: "Cancel");
        self.deselectAllButton?.texture = SKTexture(imageNamed: "Cancel");
        self.deleteButton?.texture = SKTexture(imageNamed: "Delete");
        self.pointControlButton?.texture = SKTexture(imageNamed: "ControlPoint")
        self.portalButton?.texture = SKTexture(imageNamed: "Portal")
        self.wallButton?.texture = SKTexture(imageNamed: "Wall");
        self.boosterButton?.texture = SKTexture(imageNamed: "Booster");
        
        // Bouton courant par défaut
        if self.currentButton == nil {
            self.colorCurrentButton(button: self.cameraControlButton!)
            self.currentButton = self.cameraControlButton
        }
    }
    
    /// Affiche/Cache les boutons liés à la sélection
    func showSelectContextButtons(activer: Bool) {
        self.deleteButton?.isHidden = !activer
        self.deselectAllButton?.isHidden = !activer
        
        if !self.selectContextButtonsEnable {
            self.deleteButton?.isHidden = true
            self.deselectAllButton?.isHidden = true
        }
    }
    
    /// Désactive les boutons liés à la sélection
    func enableSelectContextButtons() {
        self.selectContextButtonsEnable = true
    }
    
    /// Active les boutons liés à la sélection
    func disableSelectContextButtons() {
        self.selectContextButtonsEnable = false
    }
    
    /// Affiche/Cache les boutons liés au fait si les noeuds sont sur la table
    func showButtonsNoeudSurTable(activer: Bool) {
        self.cameraControlButton?.isHidden = !activer;
        self.selectionButton?.isHidden = !activer;
        self.duplicateButton?.isHidden = !activer;
        self.deselectAllButton?.isHidden = !activer;
        self.pointControlButton?.isHidden = !activer;
        self.portalButton?.isHidden = !activer;
        self.wallButton?.isHidden = !activer;
        self.boosterButton?.isHidden = !activer;
        
        if !self.selectContextButtonsEnable {
            self.deleteButton?.isHidden = true
            self.deselectAllButton?.isHidden = true
        }
    }
    
    /// Affiche/Cache le bouton d'annulation
    func showCancelButton(activer: Bool) {
        self.cancelButton?.isHidden = !activer
    }
    
    override func didMove(to view: SKView) {
    }
    
    func touchDown(atPoint pos : CGPoint) {
    }
    
    func touchMoved(toPoint pos : CGPoint) {
    }
    
    func touchUp(atPoint pos : CGPoint) {
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        let touch: UITouch = touches.first!
        let positionInScene = touch.location(in: self)
        let touchedNode = self.nodes(at: positionInScene).first
        
        if touchedNode is SKSpriteNode {
            if touchedNode?.name != "deleteButton" {
                // Jouer le son sauf pour le button delete
                AudioService.instance.playSound(soundName: EDITION_SOUND.HUD.rawValue)
            }
            
            if self.buttonSelectionnable(button: touchedNode as! SKSpriteNode) {
                if touchedNode == self.currentButton {
                    if touchedNode != self.cameraControlButton {
                        // Untoggle current button
                        self.colorCurrentButton(button: self.cameraControlButton!)
                        
                        // Mettre l'état par défaut
                        self.currentButton = self.cameraControlButton!
                        FacadeModele.instance.changerModeleEtat(etat: .CAMERA_CONTROLE)
                        
                        return
                    }
                }
                else {
                    // Color and change current button
                    self.colorCurrentButton(button: touchedNode as! SKSpriteNode)
                    self.currentButton = touchedNode as? SKSpriteNode
                }
            }
            
            // Réaction au bouton
            if touchedNode?.name == "cameraControlButton"{
                print("CameraControl")
                FacadeModele.instance.changerModeleEtat(etat: .CAMERA_CONTROLE)
            }
            else if touchedNode?.name == "selectionButton"{
                print("Selection")
                FacadeModele.instance.changerModeleEtat(etat: .SELECTION)
            }
            else if touchedNode?.name == "duplicateButton"{
                print("Duplication")
                FacadeModele.instance.changerModeleEtat(etat: .DUPLIQUER)
            }
            else if touchedNode?.name == "pointControlButton"{
                print("PointControl")
                FacadeModele.instance.changerModeleEtat(etat: .POINTS_CONTROLE)
            }
            else if touchedNode?.name == "portalButton"{
                print("Portal")
                FacadeModele.instance.changerModeleEtat(etat: .CREATION_PORTAIL)
            }
            else if touchedNode?.name == "wallButton"{
                print("Wall")
                FacadeModele.instance.changerModeleEtat(etat: .CREATION_MURET)
            }
            else if touchedNode?.name == "boosterButton"{
                print("Booster")
                FacadeModele.instance.changerModeleEtat(etat: .CREATION_ACCELERATEUR)
            }
            else if touchedNode?.name == "deleteButton"{
                print("Delete")
                FacadeModele.instance.obtenirEtat().supprimerSelection()
                
                // Jouer le son
                AudioService.instance.playSound(soundName: EDITION_SOUND.DELETE.rawValue)
            }
            else if touchedNode?.name == "cancelButton"{
                print("Annuler")
                FacadeModele.instance.obtenirEtat().nettoyerEtat()
            }
            else if touchedNode?.name == "deselectAllButton"{
                print("Désélectionner")
                let arbre = FacadeModele.instance.obtenirArbreRendu()
                let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
                table.deselectionnerTout()
                
                // Envoyer la commande
                FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: "", isSelected: false, deselectAll: true)
            }
        }
        
        for t in touches { self.touchDown(atPoint: t.location(in: self)) }
    }
    
    private func colorCurrentButton(button: SKSpriteNode) {
        // Enlever la couleur de l'ancien bouton
        let colorizeWhite = SKAction.colorize(with: .white, colorBlendFactor: 1, duration: 0.10)
        self.currentButton?.run(colorizeWhite)
        
        // Ajouter la couleur au nouveau bouton
        let colorizeColor = SKAction.colorize(with: self.SELECTED_COLOR, colorBlendFactor: 1, duration: 0.35)
        button.run(colorizeColor)
    }
    
    private func buttonSelectionnable(button: SKSpriteNode) -> Bool {
        return (button != self.cancelButton &&
                button != self.deselectAllButton &&
                button != self.deleteButton)
    }
    
    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        for t in touches { self.touchMoved(toPoint: t.location(in: self)) }
    }
    
    override func touchesEnded(_ touches: Set<UITouch>, with event: UIEvent?) {
        for t in touches { self.touchUp(atPoint: t.location(in: self)) }
    }
    
    override func touchesCancelled(_ touches: Set<UITouch>, with event: UIEvent?) {
        for t in touches { self.touchUp(atPoint: t.location(in: self)) }
    }
    
    
    override func update(_ currentTime: TimeInterval) {
        // Called before each frame is rendered
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
