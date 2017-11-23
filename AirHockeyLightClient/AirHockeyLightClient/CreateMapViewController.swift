///////////////////////////////////////////////////////////////////////////////
/// @file CreateMapViewController.swift
/// @author Mikael Ferland
/// @date 2017-10-29
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class CreateMapViewController
/// @brief Contrôleur de la vue pour la création d'une carte
///
/// @author Mikael Ferland
/// @date 2017-10-29
///////////////////////////////////////////////////////////////////////////
class CreateMapViewController: UIViewController {
    
    @IBOutlet weak var mapName: UITextField!
    @IBOutlet weak var mapNameError: UILabel!
    @IBOutlet weak var isPrivateMap: UISwitch!
    @IBOutlet weak var passwordLabel: UILabel!
    @IBOutlet weak var password: UITextField!
    @IBOutlet weak var passwordError: UILabel!
    @IBOutlet weak var passwordConfirmationLabel: UILabel!
    @IBOutlet weak var passwordConfirmation: UITextField!
    @IBOutlet weak var passwordConfirmationError: UILabel!
    
    /// Instance singleton
    static var instance = CreateMapViewController()
    
    var viewModel: IMapViewModel? {
        didSet {
            self.linkErrorMessagesToViewModel()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        CreateMapViewController.instance = self
        
        self.view.backgroundColor = UIColor.black.withAlphaComponent(0.8)
        self.showAnimate()
        
        self.viewModel = MapViewModel(map: Map())
        self.linkErrorMessagesToViewModel()
        self.resetUI()
        
        self.isPrivateMap.isOn = false
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
    
    @IBAction func createMap(_ sender: Any) {
        self.resetUI()
        self.deactivateInputs()
        
        if (self.viewModel?.save(
            name: self.mapName.text!,
            isPrivate: self.isPrivateMap.isOn,
            password: self.password.text!,
            passwordConfirmation: self.passwordConfirmation.text!)  )! {
            
            MapCarouselViewController.instance.updateEntries()
            /// Fermer la fenêtre
            self.removeAnimate()
        }
        else {
            // Laisse le temps au message d'erreur d'apparaître avant le shake
            let when = DispatchTime.now() + 0.1
            
            DispatchQueue.main.asyncAfter(deadline: when) {
                if (self.mapNameError.text != "") {
                    self.notifyError(textField: self.mapName)
                }
                if (self.passwordError.text != "") {
                    self.notifyError(textField: self.password)
                }
                if (self.passwordConfirmationError.text != "") {
                    self.notifyError(textField: self.passwordConfirmation)
                }
            }
            
            self.activateInputs()
        }
    }
    
    @IBAction func changeMapPrivacy(_ sender: Any) {
        self.passwordLabel.isHidden = !self.isPrivateMap.isOn
        self.password.isHidden = !self.isPrivateMap.isOn
        self.passwordError.isHidden = !self.isPrivateMap.isOn
        self.passwordConfirmationLabel.isHidden = !self.isPrivateMap.isOn
        self.passwordConfirmation.isHidden = !self.isPrivateMap.isOn
        self.passwordConfirmationError.isHidden = !self.isPrivateMap.isOn
    }
    
    @IBAction func mapNameEditingBegan(_ sender: Any) {
        self.mapName.layer.borderWidth = 0.0
        self.mapNameError.text = ""
    }
    
    @IBAction func passwordEditingBegan(_ sender: Any) {
        self.password.layer.borderWidth = 0.0
        self.passwordError.text = ""
    }
    
    @IBAction func passwordConfirmationBegan(_ sender: Any) {
        self.passwordConfirmation.layer.borderWidth = 0.0
        self.passwordConfirmationError.text = ""
    }
    
    private func linkErrorMessagesToViewModel() {
        if !isViewLoaded {
            return
        }
        
        guard let viewModel = viewModel else {
            return
        }
        
        viewModel.mapNameError.bindAndFire { [unowned self] in self.mapNameError.text = $0 }
        viewModel.passwordError.bindAndFire { [unowned self] in self.passwordError.text = $0 }
        viewModel.passwordConfirmationError.bindAndFire { [unowned self] in self.passwordConfirmationError.text = $0 }
    }
    
    private func resetUI() {
        self.mapName.layer.borderWidth = 0.0
        self.password.layer.borderWidth = 0.0
        self.passwordConfirmation.layer.borderWidth = 0.0
        
        self.mapNameError.text = ""
        self.passwordError.text = ""
        self.passwordConfirmationError.text = ""
    }
    
    private func activateInputs() {
        self.mapName.isEnabled = true
        self.isPrivateMap.isEnabled = true
        self.password.isEnabled = true
        self.passwordConfirmation.isEnabled = true
    }
    
    private func deactivateInputs() {
        self.mapName.isEnabled = false
        self.isPrivateMap.isEnabled = false
        self.password.isEnabled = false
        self.passwordConfirmation.isEnabled = false
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
