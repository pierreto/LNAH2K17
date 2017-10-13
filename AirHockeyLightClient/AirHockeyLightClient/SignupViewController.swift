//
//  SignupViewController.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import UIKit

class SignupViewController: UIViewController {
    // Mark : Properties
    @IBOutlet weak var titleLabel: UILabel!
    
    @IBOutlet weak var usernameInput: UITextField!
    @IBOutlet weak var passwordInput: UITextField!
    @IBOutlet weak var confirmPasswordInput: UITextField!
    
    @IBOutlet weak var usernameErrorLabel: UILabel!
    @IBOutlet weak var passwordErrorLabel: UILabel!
    @IBOutlet weak var confirmPasswordErrorLabel: UILabel!
    
    // Mark: Actions
    @IBAction func createAccount(_ sender: Any) {
        viewModel?.signup(username: usernameInput.text!, password: passwordInput.text!, confirmPassword: confirmPasswordInput.text!)
        //Laisse le temps au message d'erreur d'apparaitre avant le shake
        let when = DispatchTime.now() + 0.1
        DispatchQueue.main.asyncAfter(deadline: when) {
            if(self.usernameErrorLabel.text != ""){
                self.notifyErrorInput(textField: self.usernameInput)
            }
            if(self.passwordErrorLabel.text != ""){
                self.notifyErrorInput(textField: self.passwordInput)
            }
            if(self.confirmPasswordErrorLabel.text != ""){
                self.notifyErrorInput(textField: self.confirmPasswordInput)
            }
        }
    }

    @IBAction func editUsernameInput(_ sender: Any) {
        self.resetStyle(textField: self.usernameInput)
    }
    
    @IBAction func editPasswordInput(_ sender: Any) {
        self.resetStyle(textField: self.passwordInput)
    }
    
    @IBAction func editConfirmPasswordInput(_ sender: Any) {
        self.resetStyle(textField: self.confirmPasswordInput)
    }
    
    // Mark: Functions
    var viewModel: ISignupViewModel? {
        didSet {
            fillUI()
        }
    }
    
    override func viewDidLoad() {
        let signupModel: Signup = Signup()
        viewModel = SignupViewModel(signupModel: signupModel)
        super.viewDidLoad()
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
        viewModel.confirmPasswordError.bindAndFire { [unowned self] in self.confirmPasswordErrorLabel.text = $0 }
    }
    
    /// Modifier l'apparence du input en cas d'erreur
    private func notifyErrorInput(textField: UITextField) {
        textField.shake()
        textField.text = ""
        
        let errorColor : UIColor = UIColor.red
        textField.layer.borderColor = errorColor.cgColor
        textField.layer.borderWidth = 1.0
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
}
