//
//  LoginViewModel.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import PromiseKit
import Foundation

class LoginViewModel: NSObject, ILoginViewModel {
    
    let loginModel: Login
    
    let ipAddressError: Dynamic<String>
    let usernameError: Dynamic<String>
    let passwordError: Dynamic<String>
    
    func login(ipAddress: String, username: String, password: String) -> Promise<Bool>{
        return loginModel.validateFields(ipAddress: ipAddress, username: username, password: password)
    }
    
    init(loginModel: Login) {
        self.loginModel = loginModel
        self.ipAddressError = Dynamic(loginModel.ipAddressError)
        self.usernameError = Dynamic(loginModel.usernameError)
        self.passwordError = Dynamic(loginModel.passwordError)
        super.init()
        subscribeToNotifications()
    }
    
    deinit {
        unsubscribeFromNotifications()
    }
    
    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(loginPressed(_:)),
                                               name: NSNotification.Name(rawValue: LoginNotification.SubmitNotification),
                                               object: loginModel)
    }
    
    fileprivate func unsubscribeFromNotifications() {
        NotificationCenter.default.removeObserver(self)
    }
    
    @objc fileprivate func loginPressed(_ notification: NSNotification){
        self.ipAddressError.value = loginModel.ipAddressError
        self.usernameError.value = loginModel.usernameError
        self.passwordError.value = loginModel.passwordError
    }
}
