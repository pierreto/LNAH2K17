//
//  LoginViewModel.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import Foundation
import PromiseKit

protocol ILoginViewModel {
    var ipAddressError: Dynamic<String> { get }
    var usernameError: Dynamic<String> { get }
    var passwordError: Dynamic<String> { get }
    
    func login(username: String, password: String) -> Promise<Bool>
}
