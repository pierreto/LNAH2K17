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
    var nameError: String
    var emailError: String
    var passwordError: String
    var confirmPasswordError: String
    
    private let clientConnection = HubManager.sharedConnection
    
    override init() {
        self.usernameError = ""
        self.nameError = ""
        self.emailError = ""
        self.passwordError = ""
        self.confirmPasswordError = ""
        super.init()
    }
    
    func validateFields(username: String, name: String, email: String, password: String, confirmPassword: String) ->Promise<Bool> {
        let validUsername = validateUsername(username: username)
        let validName = validateName(name: name)
        let validEmail = validateEmail(email: email)
        let validPassowrd = validatePassword(password: password)
        let validConfirmPassword = validatePasswordsMatch(password: password, confirmPassword: confirmPassword)
        
        NotificationCenter.default.post(name: Notification.Name(rawValue: SignupNotification.SubmitNotification), object: self)

        if validUsername && validName && validEmail && validPassowrd && validConfirmPassword {
            let parameters: [String: String] = [
                "Username" : username,
                "Name" : name,
                "Email" : email,
                "Password" : password
            ]
            return Promise { fullfil, error in
                Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/signup", method: .post, parameters: parameters, encoding: JSONEncoding.default)
                    .responseJSON { response in
                        if(response.response?.statusCode == 200) {
                            self.clientConnection.setUsername(username: username)
                            if let result = response.result.value {
                                let id = result as! Int
                                self.clientConnection.setId(id: id)
                            }
                            HubManager.sharedConnection.getChatHub().subscribe()
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
    
    private func validateName(name: String) -> Bool {
        let validNameRegex = "^[a-zA-Z_]$"
        let nameMatches = name.range(of: validNameRegex, options: .regularExpression)
        if(name.isEmpty) {
            self.nameError = "Nom requis"
            return false
        } else if (name.characters.count > 32) {
            self.nameError = "Maximum 32 charactères permis"
            return false
        } else if (nameMatches != nil) {
            self.nameError = ""
            return true
        } else {
            self.nameError = "Nom d'usager invalide"
            return false
        }
    }
    
    private func validateEmail(email: String) -> Bool {
        if(email.isEmpty) {
            self.emailError = "Courriel requis"
            return false
        } else if (email.characters.count > 64) {
            self.nameError = "Maximum 64 charactères permis"
            return false
        } else if (email.isEmail) {
            self.nameError = ""
            return true
        } else {
            self.nameError = "Courriel invalide"
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

extension String {
    var isEmail: Bool {
        let emailRegEx = "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,20}"
        let emailTest  = NSPredicate(format:"SELF MATCHES %@", emailRegEx)
        return emailTest.evaluate(with: self)
    }
}
