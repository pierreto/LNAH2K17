///////////////////////////////////////////////////////////////////////////////
/// @file UnlockMapViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-08
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

extension String {
    func sha1() -> String {
        let data = self.data(using: String.Encoding.utf8)!
        var digest = [UInt8](repeating: 0, count:Int(CC_SHA1_DIGEST_LENGTH))
        data.withUnsafeBytes {
            _ = CC_SHA1($0, CC_LONG(data.count), &digest)
        }
        return Data(bytes: digest).base64EncodedString()
    }
}

///////////////////////////////////////////////////////////////////////////
/// @class UnlockMapViewController
/// @brief Contrôleur de la vue pour déverrouiller une carte privée
///
/// @author Mikael Ferland
/// @date 2017-11-08
///////////////////////////////////////////////////////////////////////////
class UnlockMapViewController: UIViewController {
    
    /// Instance singleton
    static var instance = UnlockMapViewController()
    
    @IBOutlet weak var unlockPassword: UITextField!
    @IBOutlet weak var unlockPasswordError: UILabel!
    
    var isUnlocked = false
    
    var viewModel: IMapViewModel? {
        didSet {
            self.linkErrorMessagesToViewModel()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        UnlockMapViewController.instance = self
        
        self.view.backgroundColor = UIColor.black.withAlphaComponent(0.8)
        self.showAnimate()
        
        self.viewModel = MapViewModel(map: Map())
        self.linkErrorMessagesToViewModel()
        self.resetUI()
    }
    
    @IBAction func closeView(_ sender: Any) {
        self.removeAnimate()
    }
    
    /// Animation à l'ouverture
    func showAnimate() {
        self.view.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
        self.view.alpha = 0.0
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.view.alpha = 1.0
                self.view.transform = CGAffineTransform(scaleX: 1.0, y: 1)
            }
        )
    }
    
    /// Animation à la fermeture
    func removeAnimate() {
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.view.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
                self.view.alpha = 0.0
            },
            completion: {
                (finished: Bool) in
                if finished {
                    self.view.removeFromSuperview()
                }
            }
        )
        
        // Réactiver la barre de navigation
        MapDisplayViewController.instance.enableNavigationBar(activer: true)
    }
    
    @IBAction func unlockMap(_ sender: Any) {
        self.deactivateInput()
        isUnlocked = (self.viewModel?.unlock(map: MapDisplayViewController.instance.currentMap!, unlockPassword: self.unlockPassword.text!.sha1()))!
        
        if isUnlocked {
            /// Fermer la fenêtre
            self.removeAnimate()
            
            let parent = self.parent as! MapDisplayViewController
            parent.closeUnlockMapVC()
        }
        else {
            // Laisse le temps au message d'erreur d'apparaître avant le shake
            let when = DispatchTime.now() + 0.1
            
            DispatchQueue.main.asyncAfter(deadline: when) {
                if (self.unlockPasswordError.text != "") {
                    self.notifyError(textField: self.unlockPassword)
                }
            }
            
            self.activateInput()
        }
    }
    
    @IBAction func unlockPasswordEditingBegan(_ sender: Any) {
        self.resetUI()
    }
    
    private func linkErrorMessagesToViewModel() {
        if !isViewLoaded {
            return
        }
        
        guard let viewModel = viewModel else {
            return
        }
        
        viewModel.unlockPasswordError.bindAndFire { [unowned self] in self.unlockPasswordError.text = $0 }
    }
    
    private func resetUI() {
        self.activateInput()
        self.resetStyle(textField: self.unlockPassword)
        self.resetErrorMessage()
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
    
    private func resetErrorMessage() {
        self.unlockPasswordError.text = ""
    }
    
    private func activateInput() {
        self.unlockPassword.isEnabled = true
    }
    
    private func deactivateInput() {
        self.unlockPassword.isEnabled = false
    }
    
    /// Modifier l'apparence du input en cas d'erreur
    private func notifyError(textField: UITextField) {
        textField.shake()
        textField.text = ""
        let errorColor : UIColor = UIColor.red
        textField.layer.borderColor = errorColor.cgColor
        textField.layer.borderWidth = 1.0
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
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
