///////////////////////////////////////////////////////////////////////////////
/// @file GeneralPropertiesViewController.swift
/// @author Pierre To
/// @date 2017-10-26
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class GeneralPropertiesViewController
/// @brief Controlleur de la vue de la configuration de la zone de jeu
///
/// @author Pierre To
/// @date 2017-10-26
///////////////////////////////////////////////////////////////////////////
class GeneralPropertiesViewController: UIViewController {
    
    /// Instance singleton
    static var instance = GeneralPropertiesViewController()
    
    @IBOutlet weak var saveButton: UIBarButtonItem!
    
    @IBOutlet weak var coefficientFrictionField: UITextField!
    @IBOutlet weak var coefficientFrictionStepper: UIStepper!
    @IBOutlet weak var coefficientFrictionError: UILabel!
    
    @IBOutlet weak var coefficientRebondField: UITextField!
    @IBOutlet weak var coefficientRebondStepper: UIStepper!
    @IBOutlet weak var coefficientRebondError: UILabel!
    
    @IBOutlet weak var coefficientAccelerationField: UITextField!
    @IBOutlet weak var coefficientAccelerationStepper: UIStepper!
    @IBOutlet weak var coefficientAccelerationError: UILabel!
    
    var viewModel: IGeneralPropertiesViewModel? {
        didSet {
            fillUI()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        GeneralPropertiesViewController.instance = self
        
        self.view.backgroundColor = UIColor.black.withAlphaComponent(0.8)
        self.showAnimate()
        
        // Charger les informations de la zone de jeu
        self.loadGeneralProperties()
        
        viewModel = GeneralPropertiesViewModel(generalProperties: FacadeModele.instance.obtenirGeneralProperties())
        fillUI()
        styleUI()
    }
    
    /// Fermer la vue de configuration des propriétés de la zone de jeu
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
        EditorViewController.instance.enableNavigationBar(activer: true)
    }
    
    /// Crée le lien entre les messages d'erreur et le view model
    private func fillUI() {
        if !isViewLoaded {
            return
        }
        
        guard let viewModel = viewModel else {
            return
        }
        viewModel.coefficientFrictionError.bindAndFire { [unowned self] in self.coefficientFrictionError.text = $0 }
        viewModel.coefficientRebondError.bindAndFire { [unowned self] in self.coefficientRebondError.text = $0 }
        viewModel.coefficientAccelerationError.bindAndFire { [unowned self] in self.coefficientAccelerationError.text = $0 }
    }
    
    /// Ajuste le visuel
    private func styleUI() {
        self.activateInputs()
        self.resetStyle(textField: self.coefficientFrictionField)
        self.resetStyle(textField: self.coefficientRebondField)
        self.resetStyle(textField: self.coefficientAccelerationField)
        self.resetErrorMessage()
    }
    
    /// Permet le chargement des propriétés à afficher
    private func loadGeneralProperties() {
        let coefficients = FacadeModele.instance.obtenirGeneralProperties().getCoefficientValues()
        
        self.coefficientFrictionField.text = coefficients[0].description
        self.coefficientFrictionStepper.value = Double(Float.init(self.coefficientFrictionField.text!)!)
        
        self.coefficientRebondField.text = coefficients[1].description
        self.coefficientRebondStepper.value = Double.init(self.coefficientRebondField.text!)!
        
        self.coefficientAccelerationField.text = coefficients[2].description
        self.coefficientAccelerationStepper.value = Double.init(self.coefficientAccelerationField.text!)!
    }
    
    /// Réinitialise toutes les valeurs par défaut
    @IBAction func resetCoefficients(_ sender: Any) {
        self.coefficientFrictionField.text = String.init(1.0)
        self.coefficientFrictionStepper.value = 1.0
        self.coefficientRebondField.text = String.init(0.0)
        self.coefficientRebondStepper.value = 0.0
        self.coefficientAccelerationField.text = String.init(40.0)
        self.coefficientAccelerationStepper.value = 40.0
        self.styleUI()
    }
    
    /// Permet la sauvegarde des inputs losque le bouton sauvegarder est utilisé.
    @IBAction func saveGeneralProperties(_ sender: Any) {
        self.deactivateInputs()
        
        if (self.viewModel?.save(
            coefficientFriction: self.coefficientFrictionField.text!,
            coefficientRebond: self.coefficientRebondField.text!,
            coefficientAcceleration: self.coefficientAccelerationField.text!))! {
            /// Fermer la fenêtre
            self.removeAnimate()
        }
        else {
            //Laisse le temps au message d'erreur d'apparaitre avant le shake
            let when = DispatchTime.now() + 0.1
            DispatchQueue.main.asyncAfter(deadline: when) {
                if (self.coefficientFrictionError.text != "") {
                    self.notifyErrorInput(textField: self.coefficientFrictionField)
                }
                if (self.coefficientRebondError.text != "") {
                    self.notifyErrorInput(textField: self.coefficientRebondField)
                }
                if (self.coefficientAccelerationError.text != "") {
                    self.notifyErrorInput(textField: self.coefficientAccelerationField)
                }
            }
            
            self.activateInputs()
        }
    }
    
    @IBAction func editCoefficientFriction(_ sender: Any) {
        let coefficientValue = Float.init(self.coefficientFrictionField.text!)
        
        if coefficientValue != nil {
            self.coefficientFrictionStepper.value = Double(coefficientValue!)
        }
        
        resetStyle(textField: self.coefficientFrictionField)
        self.coefficientFrictionError.text = ""
    }
    
    @IBAction func editStepperCoefficientFriction(_ sender: Any) {
        self.coefficientFrictionField.text = String.init(self.coefficientFrictionStepper.value)
        
        resetStyle(textField: self.coefficientFrictionField)
        self.coefficientFrictionError.text = ""
    }
    
    @IBAction func editCoefficientRebond(_ sender: Any) {
        let coefficientValue = Double.init(self.coefficientRebondField.text!)
        
        if coefficientValue != nil {
            self.coefficientRebondStepper.value = coefficientValue!
        }
        
        resetStyle(textField: self.coefficientRebondField)
        self.coefficientRebondError.text = ""
    }
    
    @IBAction func editStepperCoefficientRebond(_ sender: Any) {
        self.coefficientRebondField.text = String.init(self.coefficientRebondStepper.value)
        
        resetStyle(textField: self.coefficientRebondField)
        self.coefficientRebondError.text = ""
    }
    
    @IBAction func editCoefficientAcceleration(_ sender: Any) {
        let coefficientValue = Double.init(self.coefficientAccelerationField.text!)
        
        if coefficientValue != nil {
            self.coefficientAccelerationStepper.value = coefficientValue!
        }
        
        resetStyle(textField: self.coefficientAccelerationField)
        self.coefficientAccelerationError.text = ""
    }
    
    @IBAction func editStepperCoefficientAcceleration(_ sender: Any) {
        self.coefficientAccelerationField.text = String.init(self.coefficientAccelerationStepper.value)
        
        resetStyle(textField: self.coefficientAccelerationField)
        self.coefficientAccelerationError.text = ""
    }
    
    /// Modifier l'apparence du input en cas d'erreur
    private func notifyErrorInput(textField: UITextField) {
        textField.shake()
        textField.text = ""
        let errorColor : UIColor = UIColor.red
        textField.layer.borderColor = errorColor.cgColor
        textField.layer.borderWidth = 1.0
    }
    
    private func activateInputs() {
        self.coefficientFrictionField.isEnabled = true
        self.coefficientRebondField.isEnabled = true
        self.coefficientAccelerationField.isEnabled = true
    }
    
    private func deactivateInputs(){
        self.coefficientFrictionField.isEnabled = false
        self.coefficientRebondField.isEnabled = false
        self.coefficientAccelerationField.isEnabled = false
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
    
    private func resetErrorMessage() {
        self.coefficientFrictionError.text = ""
        self.coefficientRebondError.text = ""
        self.coefficientAccelerationError.text = ""
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
