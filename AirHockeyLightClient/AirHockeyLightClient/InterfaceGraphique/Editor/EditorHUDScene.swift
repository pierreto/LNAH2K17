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
    
    private var cameraControlButton: SKSpriteNode?
    private var selectionButton: SKSpriteNode?
    private var deplacementButton: SKSpriteNode?
    private var pointControlButton: SKSpriteNode?
    private var portalButton: SKSpriteNode?
    private var wallButton: SKSpriteNode?
    
    override func sceneDidLoad() {
        super.sceneDidLoad()
        
        // Chercher les boutons noeud
        self.cameraControlButton = (self.childNode(withName: "//cameraControlButton") as? SKSpriteNode)!
        self.selectionButton = (self.childNode(withName: "//selectionButton") as? SKSpriteNode)!
        self.deplacementButton = (self.childNode(withName: "//deplacementButton") as? SKSpriteNode)!
        self.pointControlButton = (self.childNode(withName: "//pointControlButton") as? SKSpriteNode)!
        self.portalButton = (self.childNode(withName: "//portalButton") as? SKSpriteNode)!
        self.wallButton = (self.childNode(withName: "//wallButton") as? SKSpriteNode)!
        
        // Charger les images
        self.selectionButton?.texture = SKTexture(imageNamed: "Select")
        self.deplacementButton?.texture = SKTexture(imageNamed: "Move")
        self.pointControlButton?.texture = SKTexture(imageNamed: "ControlPoint")
        self.portalButton?.texture = SKTexture(imageNamed: "Portal")
        self.wallButton?.texture = SKTexture(imageNamed: "Wall");
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
        
        if touchedNode?.name == "cameraControlButton"{
            print("CameraControl")
            FacadeModele.instance.changerModeleEtat(etat: .CAMERA_CONTROLE)
        }
        else if touchedNode?.name == "selectionButton"{
            print("Selection")
            FacadeModele.instance.changerModeleEtat(etat: .SELECTION)
        }
        else if touchedNode?.name == "deplacementButton"{
            print("Deplacement")
            FacadeModele.instance.changerModeleEtat(etat: .DEPLACEMENT)
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
        for t in touches { self.touchDown(atPoint: t.location(in: self)) }
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
