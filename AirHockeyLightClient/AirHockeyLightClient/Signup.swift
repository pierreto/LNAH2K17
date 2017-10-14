//
//  Signup.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//

import PromiseKit
import Alamofire
import SwiftyJSON
import Foundation

enum SignupNotification {
    static let SubmitNotification = "SubmitNotification"
}
class Signup: NSObject {
    
    // Mark: Properties
    var usernameError: String
    var passwordError: String
    var confirmPasswordError: String
    
    override init() {
        self.usernameError = ""
        self.passwordError = ""
        self.confirmPasswordError = ""
        super.init()
    }
    
    func validateFields(username: String, password: String, confirmPassword: String) ->Promise<Bool> {
        let validUsername = validateUsername(username: username)
        let validPassowrd = validatePassword(password: password)
        let validConfirmPassword = validatePasswordsMatch(password: password, confirmPassword: confirmPassword)
        
        NotificationCenter.default.post(name: Notification.Name(rawValue: SignupNotification.SubmitNotification), object: self)

        if validUsername && validPassowrd && validConfirmPassword {
            let parameters: [String: String] = [
                "Username" : username,
                "Password" : password
            ]
            return Promise { fullfil, error in
                Alamofire.request("http://" + "192.168.1.20" + ":63056/api/signup", method: .post, parameters: parameters, encoding: JSONEncoding.default)
                    .responseJSON { response in
                        if(response.response?.statusCode == 200) {
                            fullfil(true)
                        } else {
                            if let data = response.data {
                                let responseJSON = JSON(data: data)
                                
                                if let message: String = responseJSON["Message"].stringValue {
                                    if !message.isEmpty {
                                        self.usernameError = message
                                    }
                                }
                            }
                            NotificationCenter.default.post(name: Notification.Name(rawValue: SignupNotification.SubmitNotification), object: self)
                            fullfil(false)
                        }
                }
            }
        } else {
            return Promise(value: false)
        }
    }
    /// Vérifier la syntaxe du nom de l'usager
    fileprivate func validateUsername(username: String) -> Bool {
        // Usernames contain only alphanumeric characters and underscore
        let validUsernameRegex = "^[a-zA-Z0-9_]{2,16}$"
        let userNameMatches = username.range(of: validUsernameRegex, options: .regularExpression)
        // Usernames must have 16 characters at most
        if (username.isEmpty) {
            self.usernameError = "Nom d'usager requis"
            return false
        } else if (username.characters.count > 16) {
            self.usernameError = "Maximum 16 charactères permis"
            return false
        } else if (username.characters.count < 2) {
            self.usernameError = "Minimum 2 charactères requis"
            return false
        } else if (userNameMatches != nil) {
            self.usernameError = ""
            return true
        } else {
            self.usernameError = "Nom d'usager invalide"
            return false
        }
    }
    
    fileprivate func validatePassword(password: String) -> Bool {
        let validPasswordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,16}$"
        let passwordMatches = password.range(of: validPasswordRegex, options: .regularExpression)
        if(password.isEmpty) {
            self.passwordError = "Mot de passe requis"
            return false;
        }
        if(passwordMatches != nil) {
            self.passwordError = ""
            return true;
        } else {
            self.passwordError = "Le mot de passe doit contenir une lettre majuscule, une lettre minuscule et un chiffre"
            return false;
        }
    }
    
    fileprivate func validatePasswordsMatch(password: String, confirmPassword: String) -> Bool {
        if(confirmPassword.isEmpty) {
            self.confirmPasswordError = "Confirmation du mot de passe requise"
            return false;
        } else if(confirmPassword != password) {
            self.confirmPasswordError = "Les mots de passe ne sont pas identiques"
            return false;
        } else {
            self.confirmPasswordError = ""
            return true;
        }
    }
    
}
