//
//  ViewController.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-10.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import UIKit
import Reachability

class LoginViewController: UIViewController {

    // Mark: Properties
    @IBOutlet weak var usernameInput: UITextField!
    @IBOutlet weak var passwordInput: UITextField!
    
    @IBOutlet weak var usernameErrorLabel: UILabel!
    @IBOutlet weak var passwordErrorLabel: UILabel!
    
    @IBOutlet weak var loginButton: UIButton!
    @IBOutlet weak var signupButton: UIButton!
    @IBOutlet weak var loadingSpinner: UIActivityIndicatorView!
    @IBOutlet weak var navigationBar: UINavigationItem!
    
    @IBOutlet weak var scrollView: UIScrollView!
    
    // Reachability écoute les modifications de connexion du iPad
    private var reachability = Reachability()!
    private var wifiEnable = true
    private var serverDownTimer = Timer()
    
    // Mark: Actions
    @IBAction func login(_ sender: Any) {
        login()
    }
    
    @IBAction func usernameInputReturn(_ sender: Any) {
        passwordInput.becomeFirstResponder()
    }
    
    
    @IBAction func loginReturn(_ sender: Any) {
        login()
    }
    
    func login() {
        self.loading()
        disableInputs()
        viewModel?.login(username: usernameInput.text!, password: passwordInput.text!)
            .then {
                data -> Void in
                if(data) {
                    self.loadingDone()
                    OperationQueue.main.addOperation {
                        self.performSegue(withIdentifier: "loginSuccess", sender: self)
                        NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.SubmitNotification), object: nil)
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
                    if(self.passwordErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.passwordInput)
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
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillChangeFrame, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillHide, object: nil)
        
        // Perte de connexion avec le serveur, tentative de reconnexion
        HubManager.sharedConnection.getConnection()!.reconnecting = {
            print("server may be down")
            
            if self.wifiEnable {
                // Start a timer, just in case it reconnects quickly
                self.serverDownTimer = Timer.scheduledTimer(timeInterval: 5, target: self, selector: #selector(self.serverDown), userInfo: nil, repeats: false)
            }
        }
        
        // Reconnxion au serveur
        HubManager.sharedConnection.getConnection()!.reconnected = {
            self.serverDownTimer.invalidate()
            self.serverDownTimer = Timer()
        }
        
        reachability = Reachability()!
        
        reachability.whenReachable = { reachability in
            if reachability.connection == .wifi {
                self.wifiEnable = true
            }
        }
        
        reachability.whenUnreachable = { _ in
            self.wifiEnable = false
        }
        
        do {
            try reachability.startNotifier()
        } catch {
            print("Unable to start notifier")
        }
    }
    
    // Le serveur est en arrêt depuis 5 secondes
    @objc private func serverDown() {
        self.serverDownTimer.invalidate()
        self.serverDownTimer = Timer()
        reachability.stopNotifier()
        
        // Afficher le pop up de retour vers la page d'accueil
        let topViewController = self.navigationController?.topViewController
        let serverDownVC = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "ServerDownViewControllerID") as! ServerDownViewController
        
        topViewController?.addChildViewController(serverDownVC)
        
        serverDownVC.view.frame = (topViewController?.view.frame)!
        topViewController?.view.addSubview(serverDownVC.view)
        serverDownVC.didMove(toParentViewController: topViewController)
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
    }
    
    private func disableInputs() {
        self.usernameInput.isEnabled = false
        self.passwordInput.isEnabled = false
        self.loginButton.isEnabled = false
        self.signupButton.isEnabled = false
    }
    
    private func enableInputs() {
        self.usernameInput.isEnabled = true
        self.passwordInput.isEnabled = true
        self.loginButton.isEnabled = true
        self.signupButton.isEnabled = true
    }

    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
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
    
    override func viewWillDisappear(_ animated : Bool) {
        super.viewWillDisappear(animated)
        // Quand on revient a cette vue
        if self.isMovingFromParentViewController {

        }
        self.navigationController?.interactivePopGestureRecognizer?.isEnabled = true
    }
    
    override func viewWillAppear(_ animated: Bool){
        usernameInput.text = ""
        passwordInput.text = ""
        HubManager.sharedConnection.setUsername(username: "")
        NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.LogoutNotification), object: nil)
        self.navigationController?.interactivePopGestureRecognizer?.isEnabled = false
        
    }
}

