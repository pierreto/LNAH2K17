///////////////////////////////////////////////////////////////////////////////
/// @file EditorViewController.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-01
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import SpriteKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class EditorViewController
/// @brief Controlleur de la vue de l'éditeur de cartes
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-01
///////////////////////////////////////////////////////////////////////////
class EditorViewController: UIViewController {
    
    var editorView: SCNView!
    var editorScene: SCNScene!
    var cameraNode: SCNNode!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.initView()
        self.initScene()
        self.initCamera()
        self.initObjects()
    }
    
    func initView() {
        editorView = self.view as! SCNView
        editorView.allowsCameraControl = true
        editorView.autoenablesDefaultLighting = true
    }
    
    func initScene() {
        editorScene = SCNScene()
        editorView.scene = editorScene
        
        editorView.isPlaying = true
    }
    
    func initCamera() {
        cameraNode = SCNNode()
        cameraNode.camera = SCNCamera()
        cameraNode.position = SCNVector3(x: 0, y: 5, z: 10)
    }
    
    func initObjects() {
        self.editorScene.rootNode.addChildNode(NoeudTable.init())
    }
    
    /*func addButToScene() {
        let but = SCNScene(named: "but.dae")
        let butNode = but?.rootNode.childNode(withName: "Cube", recursively: true)
        self.editorScene.rootNode.addChildNode(butNode!)
    }*/
    
    override var shouldAutorotate: Bool {
        return true
    }
    
    override var supportedInterfaceOrientations: UIInterfaceOrientationMask {
        if UIDevice.current.userInterfaceIdiom == .phone {
            return .allButUpsideDown
        } else {
            return .all
        }
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Release any cached data, images, etc that aren't in use.
    }
    
    override var prefersStatusBarHidden: Bool {
        return true
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
