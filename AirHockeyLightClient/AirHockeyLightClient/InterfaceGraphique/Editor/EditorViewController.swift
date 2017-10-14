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
    
    /// Instance singleton
    static var instance = EditorViewController()
    
    //public var editorView: SCNView!
    @IBOutlet weak var editorView: SCNView!
    public var editorScene: SCNScene!
    public var cameraNode: SCNNode!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        EditorViewController.instance = self
        
        self.initView()
        self.initScene()
        self.initCamera()
        
        self.initFacadeModele()
        
        // Load the SKScene from 'EditorHUDScene.sks'
        if let scene = SKScene(fileNamed: "EditorHUDScene") {
            // Set the scale mode to scale to fit the window
            scene.scaleMode = .aspectFill
            self.editorView.overlaySKScene = scene
        }
    }
    
    func initView() {
        self.editorView.allowsCameraControl = true
        self.editorView.autoenablesDefaultLighting = true
    }
    
    func initScene() {
        self.editorScene = SCNScene()
        self.editorView.scene = editorScene
        
        self.editorView.isPlaying = true
    }
    
    func initCamera() {
        self.cameraNode = SCNNode()
        self.cameraNode.camera = SCNCamera()
    }
    
    func initFacadeModele() {
        let facade = FacadeModele.instance
        facade.initialiser()
    }
    
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
