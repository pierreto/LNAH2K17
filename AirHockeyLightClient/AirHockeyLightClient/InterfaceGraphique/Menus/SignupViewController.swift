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
    @IBOutlet weak var usernameInput: UITextField!
    @IBOutlet weak var nameInput: UITextField!
    @IBOutlet weak var emailInput: UITextField!
    @IBOutlet weak var passwordInput: UITextField!
    @IBOutlet weak var confirmPasswordInput: UITextField!
    
    @IBOutlet weak var usernameErrorLabel: UILabel!
    @IBOutlet weak var nameErrorLabel: UILabel!
    @IBOutlet weak var emailErrorLabel: UILabel!
    @IBOutlet weak var passwordErrorLabel: UILabel!
    @IBOutlet weak var confirmPasswordErrorLabel: UILabel!
    
    @IBOutlet weak var createAccountButton: UIButton!
    @IBOutlet weak var loadingSpinner: UIActivityIndicatorView!
    @IBOutlet weak var navigationBar: UINavigationItem!
    @IBOutlet weak var scrollView: UIScrollView!
    @IBOutlet weak var stackView: UIStackView!
    
    // Mark: Actions
    @IBAction func createAccount(_ sender: Any) {
        createAccount()
    }
    
    @IBAction func usernameInputReturn(_ sender: Any) {
        nameInput.becomeFirstResponder()
    }
    
    @IBAction func nameInputReturn(_ sender: Any) {
        emailInput.becomeFirstResponder()
    }
    
    @IBAction func emailInputReturn(_ sender: Any) {
        passwordInput.becomeFirstResponder()
    }
    
    @IBAction func passwordInputReturn(_ sender: Any) {
        confirmPasswordInput.becomeFirstResponder()
    }
    
    @IBAction func confirmPasswordInputReturn(_ sender: Any) {
        createAccount()
    }
    
    
    @IBAction func editUsernameInput(_ sender: Any) {
        self.resetStyle(textField: self.usernameInput)
        self.usernameErrorLabel.text = ""
    }
    
    @IBAction func editNameInput(_ sender: Any) {
        self.resetStyle(textField: self.nameInput)
        self.nameErrorLabel.text = ""
    }
    
    @IBAction func editEmailInput(_ sender: Any) {
        self.resetStyle(textField: self.emailInput)
        self.emailErrorLabel.text = ""
    }
    
    @IBAction func editPasswordInput(_ sender: Any) {
        self.resetStyle(textField: self.passwordInput)
        self.passwordErrorLabel.text = ""
    }
    
    @IBAction func editConfirmPasswordInput(_ sender: Any) {
        self.resetStyle(textField: self.confirmPasswordInput)
        self.confirmPasswordErrorLabel.text = ""
    }
    
    // Mark: Functions
    var viewModel: ISignupViewModel? {
        didSet {
            fillUI()
        }
    }
    
    override func viewDidLoad() {
        let signupModel: Signup = Signup()
        self.navigationController?.setNavigationBarHidden(false, animated: true)
        viewModel = SignupViewModel(signupModel: signupModel)
        super.viewDidLoad()
        styleUI()
        fillUI()
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillChangeFrame, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillHide, object: nil)
    }
    override func viewDidLayoutSubviews() {
        super.viewDidLayoutSubviews()
        self.scrollView.contentSize = self.stackView.frame.size
    }
    func createAccount() {
        disableInputs()
        loading()
        viewModel?.signup(username: usernameInput.text!, name: nameInput.text!, email: emailInput.text!, password: passwordInput.text!, confirmPassword: confirmPasswordInput.text!)
            .then{
                data -> Void in
                if(data) {
                    self.loadingDone()
                    OperationQueue.main.addOperation {
                        self.performSegue(withIdentifier: "signupSuccess", sender: self)
                        NotificationCenter.default.post(name: Notification.Name(rawValue: SignupNotification.SubmitNotification), object: nil)
                        self.enableInputs()
                    }
                } else {
                    self.connectionError()
                    self.enableInputs()
                }
            }.always {
                //Laisse le temps au message d'erreur d'apparaitre avant le shake
                let when = DispatchTime.now() + 0.1
                DispatchQueue.main.asyncAfter(deadline: when) {
                    if(self.usernameErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.usernameInput)
                    }
                    if(self.nameErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.nameInput)
                    }
                    if(self.emailErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.emailInput)
                    }
                    if(self.passwordErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.passwordInput)
                    }
                    if(self.confirmPasswordErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.confirmPasswordInput)
                    }
                }
        }
    }
    
    func adjustForKeyboard(notification: Notification) {
        let userInfo = notification.userInfo!
        
        let keyboardScreenEndFrame = (userInfo[UIKeyboardFrameEndUserInfoKey] as! NSValue).cgRectValue
        let keyboardViewEndFrame = view.convert(keyboardScreenEndFrame, from: view.window)
        
        if notification.name == Notification.Name.UIKeyboardWillHide {
            scrollView.contentInset = UIEdgeInsets.zero
        } else {
            scrollView.contentInset = UIEdgeInsets(top: 0, left: 0, bottom: keyboardViewEndFrame.height + 64, right: 0)
        }
        
        scrollView.scrollIndicatorInsets = scrollView.contentInset
    }
    
    func keyboardWillShow(notification:NSNotification){
        //give room at the bottom of the scroll view, so it doesn't cover up anything the user needs to tap
        var userInfo = notification.userInfo!
        var keyboardFrame:CGRect = (userInfo[UIKeyboardFrameBeginUserInfoKey] as! NSValue).cgRectValue
        keyboardFrame = self.view.convert(keyboardFrame, from: nil)
        
        var contentInset:UIEdgeInsets = self.scrollView.contentInset
        contentInset.bottom = keyboardFrame.size.height
        scrollView.contentInset = contentInset
    }
    
    func keyboardWillHide(notification:NSNotification){
        let contentInset:UIEdgeInsets = UIEdgeInsets.zero
        scrollView.contentInset = contentInset
        scrollView.scrollIndicatorInsets = contentInset
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
        viewModel.nameError.bindAndFire { [unowned self] in self.nameErrorLabel.text = $0 }
        viewModel.emailError.bindAndFire { [unowned self] in self.emailErrorLabel.text = $0 }
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
    
    private func disableInputs() {
        self.usernameInput.isEnabled = false
        self.nameInput.isEnabled = false
        self.emailInput.isEnabled = false
        self.passwordInput.isEnabled = false
        self.confirmPasswordInput.isEnabled = false
        self.createAccountButton.isEnabled = false
    }
    
    private func enableInputs() {
        self.usernameInput.isEnabled = true
        self.nameInput.isEnabled = true
        self.emailInput.isEnabled = true
        self.passwordInput.isEnabled = true
        self.confirmPasswordInput.isEnabled = true
        self.createAccountButton.isEnabled = true
    }
    
    private func connectionError() {
        self.loadingDone()
    }
    
    private func loading() {
        self.loadingSpinner.startAnimating()
        self.view.alpha = 0.7
        self.view.isUserInteractionEnabled = false
        self.navigationBar.hidesBackButton = true
    }
    
    private func loadingDone() {
        self.loadingSpinner.stopAnimating()
        self.view.alpha = 1.0
        self.view.isUserInteractionEnabled = true
        self.navigationBar.hidesBackButton = false
    }

}

