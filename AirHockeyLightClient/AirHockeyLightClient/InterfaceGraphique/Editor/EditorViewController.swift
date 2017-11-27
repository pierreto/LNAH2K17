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
class EditorViewController: UIViewController, UIGestureRecognizerDelegate, UICollectionViewDelegate, UICollectionViewDataSource {

    /// Instance singleton
    static var instance = EditorViewController()
    
    @IBOutlet weak var editorView: SCNView!
    public var editorScene: SCNScene!
    public var editorNotificationScene: EditorNotificationScene?
    public var cameraNode: SCNNode!
    public var cameraOrbit: SCNNode!
    
    @IBOutlet weak var hudView: SCNView!
    public var hudScene: SCNScene!
    public var editorHUDScene: EditorHUDScene?
    
    @IBOutlet weak var navigationBar: UINavigationItem!
    @IBOutlet weak var objectPropertiesView: ObjectPropertiesView!
    
    public var currentMap: MapEntity?
    var lastWidthRatio: Float = 0
    var lastHeightRatio: Float = 0
    
    /// Online Users Information
    @IBOutlet weak var usersCollectionView: UICollectionView!
    private var users = [OnlineUser]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Affichage des utilisateurs en ligne
        self.usersCollectionView.delegate = self
        self.usersCollectionView.dataSource = self
        if FacadeModele.instance.obtenirEtatEdition() is OnlineEditorState {
            self.usersCollectionView.isHidden = false
        }
        
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
        self.objectPropertiesView.objectProperties.isHidden = true;
        self.objectPropertiesView.hideObjectPropertiesButtons()
    }
    
    override func viewWillDisappear(_ animated : Bool) {
        super.viewWillDisappear(animated)
        
        if self.isMovingFromParentViewController {
            FacadeModele.instance.sauvegarderIconCarte(map: self.currentMap!)
            FacadeModele.instance.obtenirEtat().nettoyerEtat()
            FacadeModele.instance.obtenirEtatEdition().leaveEdition()
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
        self.editorScene.background.contents = ["rightImage.png", "leftImage.png",
                                                "upImage.png", "downImage.png",
                                                "backImage.png", "frontImage.png"]
        
        self.hudScene = SCNScene()
        self.hudView.scene = hudScene
        self.hudView.isPlaying = true
    }
    
    func initCamera() {
        // Setup camera node
        self.cameraNode = SCNNode()
        self.cameraNode.camera = SCNCamera()
        self.cameraNode.camera?.zNear = 0.1
        self.cameraNode.camera?.zFar = 1000

        // Setup camera orbit
        self.cameraOrbit = SCNNode()
        self.cameraOrbit.addChildNode(self.cameraNode)
        self.cameraOrbit.position = SCNVector3Make(20, 300, 0)
        self.cameraOrbit.eulerAngles = SCNVector3Make((-Float.pi/2), (-Float.pi/2), 0)
        
        self.editorScene.rootNode.addChildNode(cameraOrbit)
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
        
        // TODO : Si c'est désactivé, il faudrait interrompre la sauvegarde automatique. Lorque c'est reactivé, reprendre la sauvegarde auto.
    }
    
    /// Afficher/Cacher la vue
    func showObjectPropertiesView(activer: Bool) {
        self.objectPropertiesView.isHidden = !activer
        
        if activer {
            // Charger les informations de l'objet sélectionné
            self.objectPropertiesView.loadObjectProperties()
        }
    }
    
    @IBAction func sauvegarderCarteManuellement(_ sender: Any) {
        FacadeModele.instance.sauvegarderCarte(map: self.currentMap!)
        
        // Jouer le son
        AudioService.instance.playSound(soundName: EDITION_SOUND.SAVE.rawValue)
    }
    
    @IBAction func openTutorial(_ sender: Any) {
        self.performSegue(withIdentifier: "tutorialSegue", sender: self)
    }
    
    func takeMapSnapshot() -> UIImage {
        return self.editorView.snapshot()
    }
    
    /// Permettre la reconnaissance simultanée de plusieurs gestures
    func gestureRecognizer(_ gestureRecognizer: UIGestureRecognizer, shouldRecognizeSimultaneouslyWith otherGestureRecognizer: UIGestureRecognizer) -> Bool {
        if (gestureRecognizer is UIPinchGestureRecognizer || gestureRecognizer is UIRotationGestureRecognizer
            || gestureRecognizer is UIPanGestureRecognizer || gestureRecognizer is UITapGestureRecognizer) {
            return true
        } else {
            return false
        }
    }
    
    func addUser(user: OnlineUser) {
        self.users.append(user)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload table
            self.usersCollectionView.reloadData()
        })
    }
    
    func removeUser(username: String) {
        self.users.remove(at: self.users.index(where: { $0.getUsername() == username })!)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload table
            self.usersCollectionView.reloadData()
        })
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.users.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = self.usersCollectionView.dequeueReusableCell(withReuseIdentifier: "userCell", for: indexPath) as! UserCollectionViewCell
        let user = self.users[indexPath.item]
        
        // Profile picture
        let profilePicture = user.getProfilePicture()
        if profilePicture != "" {
            let imageData = NSData(base64Encoded: profilePicture)
            let image = UIImage(data: imageData! as Data)
            cell.userProfilePicture.image = image
        }
        else {
            cell.userProfilePicture.image = UIImage(named: "default_profile_picture.png")
        }
        
        // Username
        cell.username.text = user.getUsername()
        
        // Color
        cell.username.textColor = user.getHexColor()
        
        return cell
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
