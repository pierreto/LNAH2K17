//
//  ViewController.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-10.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import UIKit

class LoginViewController: UIViewController {

    // Mark: Properties
    @IBOutlet weak var usernameInput: UITextField!
    @IBOutlet weak var passwordInput: UITextField!
    
    @IBOutlet weak var usernameErrorLabel: UILabel!
    @IBOutlet weak var passwordErrorLabel: UILabel!
    
    @IBOutlet weak var loadingSpinner: UIActivityIndicatorView!
    

    // Mark: Actions
    @IBAction func login(_ sender: Any) {
        self.loading()
        deactivateInputs()
        viewModel?.login(username: usernameInput.text!, password: passwordInput.text!)
            .then {
                data -> Void in
                    if(data) {
                        self.loadingDone()
                        OperationQueue.main.addOperation {
                            self.performSegue(withIdentifier: "loginSuccess", sender: self)
                            NotificationCenter.default.post(name: Notification.Name(rawValue: ConnectionNotification.Connection), object: nil)
                        }
                    } else {
                        self.connectionError()
                    }
            }.always {
                //Laisse le temps au message d'erreur d'apparaitre avant le shake
                let when = DispatchTime.now() + 0.1
                DispatchQueue.main.asyncAfter(deadline: when) {
                    if(self.usernameErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.usernameInput)
                    }
                    if(self.passwordErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.passwordInput)
                    }
                }
        }
    }
    
    @IBAction func editUsernameInput(_ sender: Any) {
        resetStyle(textField: usernameInput)
        self.usernameErrorLabel.text = ""
    }
    
    @IBAction func editPasswordInput(_ sender: Any) {
        resetStyle(textField: passwordInput)
        self.passwordErrorLabel.text = ""
    }
    
    // Mark: Functions
    var viewModel: ILoginViewModel? {
        didSet {
            fillUI()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.setNavigationBarHidden(false, animated: true)
        let loginModel: Login = Login()
        viewModel = LoginViewModel(loginModel: loginModel)
        styleUI()
        fillUI()
    }

    fileprivate func styleUI() {
        
    }
    
    fileprivate func fillUI() {
        if !isViewLoaded {
            return
        }
        
        guard let viewModel = viewModel else {
            return
        }
        viewModel.usernameError.bindAndFire { [unowned self] in self.usernameErrorLabel.text = $0 }
        viewModel.passwordError.bindAndFire { [unowned self] in self.passwordErrorLabel.text = $0 }
    }
    
    /// Modifier l'apparence du input en cas d'erreur
    private func notifyErrorInput(textField: UITextField) {
        textField.shake()
        textField.text = ""
        let errorColor : UIColor = UIColor.red
        textField.layer.borderColor = errorColor.cgColor
        textField.layer.borderWidth = 1.0
    }
 
    private func connectionError() {
        self.loadingDone()
        self.usernameInput.isEnabled = true
        self.passwordInput.isEnabled = true
    }
    
    private func deactivateInputs(){
        self.usernameInput.isEnabled = false
        self.passwordInput.isEnabled = false
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
    
    private func loading() {
        self.loadingSpinner.startAnimating()
        self.view.alpha = 0.7
    }
    
    private func loadingDone() {
        self.loadingSpinner.stopAnimating()
        self.view.alpha = 1.0
    }
}

