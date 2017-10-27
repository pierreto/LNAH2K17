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
    
    @IBOutlet weak var coefficientFrictionText: UITextField!
    @IBOutlet weak var coefficientFrictionStepper: UIStepper!
    @IBOutlet weak var coefficientFrictionError: UILabel!
    
    // Mark: Functions
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
    
    private func fillUI() {
        if !isViewLoaded {
            return
        }
        
        guard let viewModel = viewModel else {
            return
        }
        viewModel.coefficientFrictionError.bindAndFire { [unowned self] in self.coefficientFrictionError.text = $0 }
    }
    
    private func styleUI() {
        self.activateInputs()
        self.resetStyle(textField: self.coefficientFrictionText)
        self.resetErrorMessage()
    }
    
    /// Permet le chargement des propriétés à afficher
    private func loadGeneralProperties() {
        let coefficients = FacadeModele.instance.obtenirGeneralProperties().getCoefficientValues()
        
        self.coefficientFrictionText.text = coefficients[0].description
        //this.Input_CoefRebound.Text = coefficientRebond.ToString();
        //this.Input_CoefAcceleration.Text = coefficientAcceleration.ToString();
    }
    
    /// Permet la sauvegarde des inputs losque le bouton sauvegarder est utilisé.
    @IBAction func saveGeneralProperties(_ sender: Any) {
        self.deactivateInputs()
        
        let coefficientFriction = Float.init(self.coefficientFrictionText.text!)
        
        if (self.viewModel?.save(coefficientFriction: coefficientFriction!))! {
            /// Fermer la fenêtre
            self.removeAnimate()
        }
        else {
            //Laisse le temps au message d'erreur d'apparaitre avant le shake
            let when = DispatchTime.now() + 0.1
            DispatchQueue.main.asyncAfter(deadline: when) {
                if(self.coefficientFrictionError.text != ""){
                    self.notifyErrorInput(textField: self.coefficientFrictionText)
                }
            }
            
            self.activateInputs()
        }
    }
    
    @IBAction func editCoefficientFriction(_ sender: Any) {
        resetStyle(textField: self.coefficientFrictionText)
        self.coefficientFrictionError.text = ""
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
        self.coefficientFrictionText.isEnabled = true
    }
    
    private func deactivateInputs(){
        self.coefficientFrictionText.isEnabled = false
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
    
    private func resetErrorMessage() {
        self.coefficientFrictionError.text = ""
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
