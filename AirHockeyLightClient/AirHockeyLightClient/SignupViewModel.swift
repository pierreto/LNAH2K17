//
//  SignupViewModel.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import PromiseKit
import Foundation

class SignupViewModel: NSObject, ISignupViewModel {
    let signupModel : Signup
    
    let usernameError: Dynamic<String>
    let passwordError: Dynamic<String>
    let confirmPasswordError: Dynamic<String>
    
    func signup(username: String, password: String, confirmPassword: String) -> Promise<Bool>{
        return signupModel.validateFields(username: username, password: password, confirmPassword: confirmPassword)
    }
    
    init(signupModel: Signup) {
        self.signupModel = signupModel
        self.usernameError = Dynamic(signupModel.usernameError)
        self.passwordError = Dynamic(signupModel.passwordError)
        self.confirmPasswordError = Dynamic(signupModel.confirmPasswordError)
        super.init()
        subscribeToNotifications()
    }
    
    deinit {
        unsubscribeFromNotifications()
    }
    
    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(signupPressed(_:)),
                                               name: NSNotification.Name(rawValue: SignupNotification.SubmitNotification),
                                               object: signupModel)
    }
    
    fileprivate func unsubscribeFromNotifications() {
        NotificationCenter.default.removeObserver(self)
    }
    
    @objc fileprivate func signupPressed(_ notification: NSNotification){
        self.usernameError.value = signupModel.usernameError
        self.passwordError.value = signupModel.passwordError
        self.confirmPasswordError.value = signupModel.confirmPasswordError

    }
}
