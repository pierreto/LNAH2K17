///////////////////////////////////////////////////////////////////////////////
/// @file EditorNotificationScene.swift
/// @author Pierre To
/// @date 2017-10-26
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SpriteKit
import GameplayKit

///////////////////////////////////////////////////////////////////////////
/// @class EditorNotificationScene
/// @brief Cette classe contrôle la zone de notification de l'éditeur
///
/// @author Pierre To
/// @date 2017-10-26
///////////////////////////////////////////////////////////////////////////
class EditorNotificationScene: SKScene {
    
    /// Messages
    private var errorOutOfBound: SKShapeNode?

    override func sceneDidLoad() {
        if #available(iOS 10.0, *) {
            super.sceneDidLoad()
        } else {
            // Fallback on earlier versions
        }
        
        self.errorOutOfBound = (self.childNode(withName: "//errorOutOfBound") as? SKShapeNode)!
        self.errorOutOfBound?.isHidden = true
    }
    
    /// Affiche le message d'erreur
    func showErrorOutOfBoundMessage(activer: Bool) {
        self.errorOutOfBound?.isHidden = !activer
        
        if activer {
            self.errorOutOfBound?.alpha = 0.0
            self.errorOutOfBound?.run(SKAction.fadeIn(withDuration: 0.5))
            self.errorOutOfBound?.run(SKAction.sequence([SKAction.wait(forDuration: 1.5),
                                                         SKAction.fadeOut(withDuration: 0.5)]))
            
            SCNNodeHelper.shakeNode(node: FacadeModele.instance.obtenirVue().editorView.pointOfView!, duration: 0.2)
            
            // Jouer le son
            AudioService.instance.playSound(soundName: EDITION_SOUND.ERROR.rawValue)
        }
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
