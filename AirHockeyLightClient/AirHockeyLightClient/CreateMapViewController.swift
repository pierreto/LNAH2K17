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
    @IBOutlet weak var isLocalMap: UISwitch!
    @IBOutlet weak var isPrivateMap: UISwitch!
    
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
        self.deactivateInputs()
        
        if (self.viewModel?.save(mapName: self.mapName.text!, isLocalMap: self.isLocalMap.isOn, isPrivateMap: self.isPrivateMap.isOn))! {
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
            }
            
            self.activateInputs()
        }
    }
    
    private func linkErrorMessagesToViewModel() {
        if !isViewLoaded {
            return
        }
        
        guard let viewModel = viewModel else {
            return
        }
        
        viewModel.mapNameError.bindAndFire { [unowned self] in self.mapNameError.text = $0 }
    }
    
    private func resetUI() {
        self.activateInputs()
        self.resetStyle(textField: self.mapName)
        self.resetErrorMessage()
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
    
    private func resetErrorMessage() {
        self.mapNameError.text = ""
    }
    
    private func activateInputs() {
        self.mapName.isEnabled = true
        self.isLocalMap.isEnabled = true
        self.isPrivateMap.isEnabled = true
    }
    
    private func deactivateInputs() {
        self.mapName.isEnabled = false
        self.isLocalMap.isEnabled = false
        self.isPrivateMap.isEnabled = false
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
