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
    
    public var currentMap: MapEntity?
    
    @IBOutlet weak var editorView: SCNView!
    public var editorScene: SCNScene!
    public var editorNotificationScene: EditorNotificationScene?
    public var cameraNode: SCNNode!
    
    @IBOutlet weak var hudView: SCNView!
    public var hudScene: SCNScene!
    public var editorHUDScene: EditorHUDScene?
    
    @IBOutlet weak var navigationBar: UINavigationItem!
    
    /// Object Properties View
    @IBOutlet weak var objectPropertiesView: UIView!
    @IBOutlet weak var objectProperties: UIView!
    @IBOutlet weak var showButton: UIButton!
    @IBOutlet weak var hideButton: UIButton!
    @IBOutlet weak var resetButton: UIBarButtonItem!
    @IBOutlet weak var applyButton: UIBarButtonItem!
    @IBOutlet weak var positionX: UITextField!
    @IBOutlet weak var positionY: UITextField!
    @IBOutlet weak var positionZ: UITextField!
    @IBOutlet weak var rotationX: UITextField!
    @IBOutlet weak var rotationY: UITextField!
    @IBOutlet weak var rotationZ: UITextField!
    @IBOutlet weak var scaleX: UITextField!
    @IBOutlet weak var scaleY: UITextField!
    @IBOutlet weak var scaleZ: UITextField!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        EditorViewController.instance = self
        
        self.initView()
        self.initScene()
        self.initCamera()
        
        self.initFacadeModele()
        
        // Load the SKScene from 'EditorHUDScene.sks'
        self.editorHUDScene = SKScene(fileNamed: "EditorHUDScene") as? EditorHUDScene
        if self.editorHUDScene != nil {
            // Set the scale mode to scale to fit the window
            self.editorHUDScene?.scaleMode = .aspectFill
            self.hudView.overlaySKScene = self.editorHUDScene
        }
        
        // Load the SKScene from 'EditorNotificationScene.sks'
        self.editorNotificationScene = SKScene(fileNamed: "EditorNotificationScene") as? EditorNotificationScene
        if self.editorNotificationScene != nil {
            // Set the scale mode to scale to fit the window
            self.editorNotificationScene?.scaleMode = .aspectFill
            self.editorView.overlaySKScene = self.editorNotificationScene
        }
        
        FacadeModele.instance.chargerCarte(map: currentMap!)

        // Cacher les propriétés de l'objet par défaut
        self.showObjectPropertiesView(activer: false)
        self.objectProperties.isHidden = true;
        self.hideObjectPropertiesButtons()
    }
    
    func initView() {
        self.editorView.allowsCameraControl = true
        self.editorView.autoenablesDefaultLighting = true
    }
    
    func initScene() {
        self.editorScene = SCNScene()
        self.editorView.scene = editorScene
        self.editorView.isPlaying = true
        
        self.hudScene = SCNScene()
        self.hudView.scene = hudScene
        self.hudView.isPlaying = true
    }
    
    func initCamera() {
        self.cameraNode = SCNNode()
        self.cameraNode.camera = SCNCamera()
    }
    
    func initFacadeModele() {
        let facade = FacadeModele.instance
        facade.initialiser()
    }
    
    /// Affichage de la vue de configuration des propriétés de la zone de jeu
    @IBAction func showGeneralProperties(_ sender: Any) {
        let generalPropertiesVC = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "GeneralPropertiesVCID") as! GeneralPropertiesViewController
        self.addChildViewController(generalPropertiesVC)
        
        generalPropertiesVC.view.frame = self.view.frame
        self.view.addSubview(generalPropertiesVC.view)
        generalPropertiesVC.didMove(toParentViewController: self)
        
        // Désactiver la barre de navigation
        self.enableNavigationBar(activer: false)
    }
    
    func enableNavigationBar(activer: Bool) {
        self.navigationBar.hidesBackButton = !activer
        
        for button in self.navigationBar.rightBarButtonItems! {
            button.isEnabled = activer
        }
    }
    
    /// Afficher/Cacher la vue
    func showObjectPropertiesView(activer: Bool) {
        self.objectPropertiesView.isHidden = !activer
        
        if activer {
            // Charger les informations de l'objet sélectionné
            self.loadObjectProperties()
        }
    }
    
    /// Affiche la vue de configuration des propriétés de l'objet
    @IBAction func showObjectProperties(_ sender: Any) {
        self.showAnimate()
        self.showObjectPropertiesButtons()
    }
    
    /// Fermer la vue de configuration des propriétés de l'objet
    @IBAction func hideObjectProperties(_ sender: Any) {
        self.removeAnimate()
        self.hideObjectPropertiesButtons()
    }
    
    /// Animation à l'ouverture
    func showAnimate() {
        self.objectProperties.isHidden = false
        self.objectProperties.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
        self.objectProperties.alpha = 0.0
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.objectProperties.alpha = 1.0
                self.objectProperties.transform = CGAffineTransform(scaleX: 1.0, y: 1)
            }
        )
    }
    
    /// Animation à la fermeture
    func removeAnimate() {
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.objectProperties.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
                self.objectProperties.alpha = 0.0
            },
            completion: {
                (finished: Bool) in
                if finished {
                    self.objectProperties.isHidden = true
                }
            }
        )
    }
    
    /// Afficher les boutons de configuration de propriétés d'objet
    func showObjectPropertiesButtons() {
        self.showButton.isHidden = true
        self.hideButton.isHidden = false
        self.resetButton.isEnabled = true
        self.applyButton.isEnabled = true
    }
    
    /// Cacher les boutons de configuration de propriétés d'objet
    func hideObjectPropertiesButtons() {
        self.showButton.isHidden = false
        self.hideButton.isHidden = true
        self.resetButton.isEnabled = false
        self.applyButton.isEnabled = false
    }
    
    /// Permet le chargement des propriétés à afficher
    private func loadObjectProperties() {
        var infos: [Float]? = [Float]()
        let success = FacadeModele.instance.selectedNodeInfos(infos: &infos)
        
        if success {
            self.positionX.text = String(format: "%.2f", (infos?[0])!)
            self.positionY.text = String(format: "%.2f", (infos?[1])!)
            self.positionZ.text = String(format: "%.2f", (infos?[2])!)
            
            self.rotationX.text = String(format: "%.2f", (infos?[3])!)
            self.rotationY.text = String(format: "%.2f", (infos?[4])!)
            self.rotationZ.text = String(format: "%.2f", (infos?[5])!)
            
            self.scaleX.text = String(format: "%.2f", (infos?[6])!)
            self.scaleY.text = String(format: "%.2f", (infos?[7])!)
            self.scaleZ.text = String(format: "%.2f", (infos?[8])!)
        }
        else {
            self.showObjectPropertiesView(activer: false)
        }
    }
    
    /// Appliquer les changements à la configuration des propriétés de l'objet
    @IBAction func applyObjectProperties(_ sender: Any) {
        var infos = [Float]()
        
        infos.append(Float.init(self.positionX.text!)!)
        infos.append(Float.init(self.positionY.text!)!)
        infos.append(Float.init(self.positionZ.text!)!)
        
        infos.append(Float.init(self.rotationX.text!)!)
        infos.append(Float.init(self.rotationY.text!)!)
        infos.append(Float.init(self.rotationZ.text!)!)
        
        infos.append(Float.init(self.scaleX.text!)!)
        infos.append(Float.init(self.scaleY.text!)!)
        let scaleZ = MathHelper.clamp(value: Float.init(self.scaleZ.text!)!,
                                      lower: Float(1.0),
                                      upper: Float.greatestFiniteMagnitude)
        self.scaleZ.text = String(format: "%.2f", scaleZ)
        infos.append(scaleZ)
            
        FacadeModele.instance.applyNodeInfos(infos: infos)
    }
    
    /// Réinitialiser les changements à la configuration des propriétés de l'objet
    @IBAction func resetObjectProperties(_ sender: Any) {
        self.loadObjectProperties()
    }
    
    @IBAction func sauvegarderCarte(_ sender: Any) {
        FacadeModele.instance.sauvegarderCarte(map: currentMap!)
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
