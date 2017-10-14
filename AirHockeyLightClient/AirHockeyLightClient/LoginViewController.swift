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
    @IBOutlet weak var ipAddressInput: UITextField!
    @IBOutlet weak var usernameInput: UITextField!
    @IBOutlet weak var passwordInput: UITextField!
    
    @IBOutlet weak var ipAddressErrorLabel: UILabel!
    @IBOutlet weak var usernameErrorLabel: UILabel!
    @IBOutlet weak var passwordErrorLabel: UILabel!

    @IBOutlet weak var connectionIndicator: UIActivityIndicatorView!

    // Mark: Actions
    @IBAction func login(_ sender: Any) {
        self.connectionIndicator.startAnimating()
        deactivateInputs()
        viewModel?.login(ipAddress: ipAddressInput.text!, username: usernameInput.text!, password: passwordInput.text!)
            .then {
                data -> Void in
                    if(data) {
                        self.connectionIndicator.stopAnimating()
                        let storyboard = UIStoryboard(name: "Main", bundle: nil)
                        let vc = storyboard.instantiateViewController(withIdentifier: "ChannelViewController")
                        self.present(vc, animated: true, completion: nil)
                    } else {
                        self.connectionError()
                    }
            }.always {
                //Laisse le temps au message d'erreur d'apparaitre avant le shake
                let when = DispatchTime.now() + 0.1
                DispatchQueue.main.asyncAfter(deadline: when) {
                    if(self.ipAddressErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.ipAddressInput)
                    }
                    if(self.usernameErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.usernameInput)
                    }
                    if(self.passwordErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.passwordInput)
                    }
                }
        }


    }
    
    @IBAction func signup(_ sender: Any) {
        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SignupViewController")
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func editOffline(_ sender: Any) {
        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "EditorViewController")
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func editIpAddressInput(_ sender: Any) {
        resetStyle(textField: ipAddressInput)
        self.ipAddressErrorLabel.text = ""
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
        viewModel.ipAddressError.bindAndFire { [unowned self] in self.ipAddressErrorLabel.text = $0 }
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
        self.connectionIndicator.stopAnimating()
        self.ipAddressInput.isEnabled = true
        self.usernameInput.isEnabled = true
        self.passwordInput.isEnabled = true
    }
    
    private func deactivateInputs(){
        self.ipAddressInput.isEnabled = false
        self.usernameInput.isEnabled = false
        self.passwordInput.isEnabled = false
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
}

