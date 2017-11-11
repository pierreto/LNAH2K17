//
//  ISignupViewModel.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import PromiseKit
import Foundation

protocol ISignupViewModel {
    var usernameError: Dynamic<String> { get }
    var nameError: Dynamic<String> { get }
    var emailError: Dynamic<String> { get }
    var passwordError: Dynamic<String> { get }
    var confirmPasswordError: Dynamic<String> { get }
    
    func signup(username: String, name: String, email: String, password: String, confirmPassword: String) -> Promise<Bool>
}
