//
//  ConnectServerViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-10-18.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit
class ConnectServerViewController: UIViewController {
    // Mark: Properties
    @IBOutlet weak var ipAddressInput: UITextField!
    @IBOutlet weak var ipAddressErrorLabel: UILabel!
    @IBOutlet weak var loadingSpinner: UIActivityIndicatorView!
    
    @IBOutlet weak var scrollView: UIScrollView!
    private let clientConnection = HubManager.sharedConnection
    
    // Mark: Actions
    @IBAction func connectServer(_ sender: Any) {
        self.loading()
        disableInputs()
        viewModel?.connect(ipAddress: ipAddressInput.text!).then {
            data -> Void in
            if (data) {
                self.loadingDone()
                //TODO: If still connected no need to return to ConnectServerView
                self.clientConnection.connected = true
                OperationQueue.main.addOperation {
                    self.performSegue(withIdentifier: "connectionSuccess", sender: self)
                    self.enableInputs()
                }
            } else {
                //ERROR
                self.loadingDone()
                self.enableInputs()
            }
            }.always {
                //Laisse le temps au message d'erreur d'apparaitre avant le shake
                let when = DispatchTime.now() + 0.1
                DispatchQueue.main.asyncAfter(deadline: when) {
                    if(self.ipAddressErrorLabel.text != ""){
                        self.notifyErrorInput(textField: self.ipAddressInput)
                    }
                }
        }
    }
    @IBAction func editIpAddress(_ sender: Any) {
        resetStyle(textField: ipAddressInput)
        self.ipAddressErrorLabel.text = ""
    }
    
    // Mark: Functions
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.setNavigationBarHidden(false, animated: true)
        let connectServerModel: ConnectServer = ConnectServer()
        viewModel = ConnectServerViewModel(connectServerModel: connectServerModel)
        styleUI()
        fillUI()
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillChangeFrame, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillHide, object: nil)
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
    
    var viewModel: IConnectServerViewModel? {
        didSet {
            fillUI()
        }
    }
    
    fileprivate func fillUI() {
        if !isViewLoaded {
            return
        }
        
        guard let viewModel = viewModel else {
            return
        }
        viewModel.ipAddressError.bindAndFire { [unowned self] in self.ipAddressErrorLabel.text = $0 }
    }
    
    private func styleUI() {
        // Style
    }
    
    private func resetStyle(textField: UITextField) {
        textField.layer.borderWidth = 0.0
    }
    
    private func disableInputs() {
        self.ipAddressInput.isEnabled = false
    }
    
    private func enableInputs() {
        self.ipAddressInput.isEnabled = true
    }
    
    private func notifyErrorInput(textField: UITextField) {
        textField.shake()
        textField.text = ""
        let errorColor : UIColor = UIColor.red
        textField.layer.borderColor = errorColor.cgColor
        textField.layer.borderWidth = 1.0
    }
    
    private func loading() {
        self.loadingSpinner.startAnimating()
        self.view.alpha = 0.7
    }
    
    private func loadingDone() {
        self.loadingSpinner.stopAnimating()
        self.view.alpha = 1.0
    }
    
    override func viewWillAppear(_ animated: Bool){
        ipAddressInput.text = "";
        HubManager.sharedConnection.setIpAddress(ipAddress: ipAddressInput.text!)
        HubManager.sharedConnection.connected = false;
    }
}
