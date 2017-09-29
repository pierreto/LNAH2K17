//
//  AuthentificationViewController.swift
//  AirHockeyLightClient
//
//  Created by Mikael Ferland and Pierre To on 17-09-23.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit
import SpriteKit
import GameplayKit

extension UIView {
    func shake() {
        let animation = CABasicAnimation(keyPath: "position")
        animation.duration = 0.07
        animation.repeatCount = 4
        animation.autoreverses = true
        animation.fromValue = NSValue(cgPoint: CGPoint(x: self.center.x - 10, y: self.center.y))
        animation.toValue = NSValue(cgPoint: CGPoint(x: self.center.x + 10, y: self.center.y))
        self.layer.add(animation, forKey: "position")
    }
}

class AuthentificationViewController: UIViewController {
    
    let clientConnection = ClientConnection.sharedConnection
    @IBOutlet weak var usernameInput: UITextField!
    @IBOutlet weak var ipAddessInput: UITextField!
    
    @IBOutlet weak var usernameInvalidErrorMessage: UILabel!
    @IBOutlet weak var usernameNotUniqueErrorMessage: UILabel!
    @IBOutlet weak var ipAddressInvalidErrorMessage: UILabel!
    @IBOutlet weak var ipAddressNotConnectedErrorMessage: UILabel!
    
    override func viewDidLoad() {
        super.viewDidLoad()
    }
    
    @IBAction func triggerUserLogin(_ sender: Any) {
        userLogin(nil)
    }
    
    // Set username
    @IBAction func userLogin(_ sender: UIButton?) {
        self.setIpAddressInputToDefaultUI()
        self.setUsernameInputToDefaultUI()
        
        let ipAddress = ipAddessInput.text!
        let username = usernameInput.text!
        
        let validSyntaxIP = self.validateIPAdress(ipAddress: ipAddress)
        let validSyntaxUsername = self.validateUsername(username: username)
        
        if validSyntaxIP {
            self.setIpAddressInputToDefaultUI()
        } else {
            self.notifyErrorInput(textField: self.ipAddessInput)
            self.ipAddressInvalidErrorMessage.isHidden = false
        }
        
        if validSyntaxUsername {
            self.setUsernameInputToDefaultUI()
        } else {
            self.notifyErrorInput(textField: self.usernameInput)
            self.usernameInvalidErrorMessage.isHidden = false
        }
        
        // Try to cone
        if (validSyntaxIP && validSyntaxUsername) {
            connectToServer(ipAddress: ipAddress, username: username)
        }
    }
    
    func validateIPAdress(ipAddress: String) -> Bool {
        print("IP address: " + ipAddress)
        
        let validIpAddressRegex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"
        let ipMatches = ipAddress.range(of: validIpAddressRegex, options: .regularExpression)
        
        if ipAddress.isEmpty {
            print("no input!")
            return false
        } else if (ipMatches != nil) {
            print("\(String(describing: ipAddress)) is a valid IP address")
            return true
        } else if (ipAddress == "localhost") {
            print("\(ipAddress) is a valid hostname")
            return true
        } else {
            print("\(ipAddress) is not valid")
            return false
        }
    }
    
    func validateUsername(username: String) -> Bool {
        print("Username: " + username)
        
        // Usernames must have 15 characters at most
        if (username.characters.count > 15) {
            return false
        }
        
        // Usernames contain only alphanumeric characters and underscore
        let validUsernameRegex = "^[a-zA-Z0-9_]*$"
        let userNameMatches = username.range(of: validUsernameRegex, options: .regularExpression)
        
        if username.isEmpty {
            print("no input!")
            return false
        } else if (userNameMatches != nil) {
            print("\(String(describing: username)) is a valid username")
            return true
        } else {
            print("\(username) is not valid")
            return false
        }
    }
    
    func notifyErrorInput(textField: UITextField) {
        textField.shake()
        
        textField.text = ""
        
        let errorColor : UIColor = UIColor.red
        textField.layer.borderColor = errorColor.cgColor
        textField.layer.borderWidth = 1.0
    }
    
    func setIpAddressInputToDefaultUI() {
        self.ipAddessInput.layer.borderWidth = 0.0
        self.ipAddressInvalidErrorMessage.isHidden = true
        self.ipAddressNotConnectedErrorMessage.isHidden = true
    }
    
    func setUsernameInputToDefaultUI() {
        self.usernameInput.layer.borderWidth = 0.0
        self.usernameInvalidErrorMessage.isHidden = true
        self.usernameNotUniqueErrorMessage.isHidden = true
    }
    
    func connectToServer(ipAddress: String, username: String) {
        // Connect to server via SignalR
        clientConnection.EstablishConnection(ipAddress: ipAddress, hubName: "ChatHub")
        
        clientConnection.getConnection().error = { error in
            print("Error: (error)")
            self.notifyErrorInput(textField: self.ipAddessInput)
            self.ipAddressNotConnectedErrorMessage.isHidden = false
        }
        
        clientConnection.getConnection().connectionFailed = { error in
            print("Connection failed")
            self.notifyErrorInput(textField: self.ipAddessInput)
            self.ipAddressNotConnectedErrorMessage.isHidden = false
        }
        
        // When a message is received from the server
        clientConnection.getChatHub().on("ChatMessageReceived") { args in
            ChatViewController.sharedChatViewController.receiveMessage(message: args?[0] as! Dictionary<String, String>)
        }
        
        clientConnection.getConnection().starting = { print("started") }
        clientConnection.getConnection().connectionSlow = { print("connectionSlow") }
        clientConnection.getConnection().reconnecting = { print("reconnecting") }
        clientConnection.getConnection().reconnected = { print("reconnected") }
        clientConnection.getConnection().disconnected = { print("disconnected") }
        
        clientConnection.getConnection().connected = {
            print("Connected with ip: " + ipAddress)
            self.clientConnection.setIpAddress(ipAddress: ipAddress)
            self.registerUsername(ipAddress: ipAddress, username: username)
        }
    }
    
    func registerUsername(ipAddress: String, username: String) {
        // Create POST request
        var request = URLRequest(url: URL(string: "http://" + ipAddress + ":63056/api/login")!)
        request.httpMethod = "POST"
        
        // Headers
        request.allHTTPHeaderFields?["Content-Type"] = "application/json"
        
        // Body
        let jsonObject: NSMutableDictionary = NSMutableDictionary()
        jsonObject.setValue(self.usernameInput.text, forKey: "username")
        
        let jsonString = convertToJsonString(jsonObject: jsonObject)
        request.httpBody = jsonString.data(using: .utf8)
        
        // Send request
        sendHttpRequest(request: request)
    }
    
    func sendHttpRequest(request: URLRequest) {
        var responseString: String?
        
        let task = URLSession.shared.dataTask(with: request) { data, response, error in
            guard let data = data, error == nil else {
                // check for fundamental networking error
                print("error=\(String(describing: error))")
                return
            }
            
            let httpStatus = response as? HTTPURLResponse
            if httpStatus?.statusCode != 200 {
                // check for http errors
                print("statusCode should be 200, but is \(String(describing: httpStatus?.statusCode))")
                print("response = \(String(describing: response))")
            
                DispatchQueue.main.async {
                    self.notifyErrorInput(textField: self.usernameInput)
                    self.usernameNotUniqueErrorMessage.isHidden = false
                }
            }
            
            responseString = String(data: data, encoding: .utf8)
            print("responseString = \(String(describing: responseString))")
            
            if httpStatus?.statusCode == 200 {
                DispatchQueue.main.async {
                    self.clientConnection.setUsername(username: self.usernameInput.text!)
                    self.setUsernameInputToDefaultUI()
                
                    let storyboard = UIStoryboard(name: "Main", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "ChannelViewController")
                    self.present(vc, animated: true, completion: nil)
                }
            }
        }
        
        task.resume()
    }
    
    func convertToJsonString(jsonObject: NSMutableDictionary) -> String {
        let jsonData: NSData
        var jsonString: String?
        
        do {
            jsonData = try JSONSerialization.data(withJSONObject: jsonObject, options: JSONSerialization.WritingOptions()) as NSData
            jsonString = NSString(data: jsonData as Data, encoding: String.Encoding.utf8.rawValue)! as String
            print("json string = \(String(describing: jsonString))")
        } catch _ {
            print ("JSON Failure")
        }
        
        return jsonString!
    }
    
    override var shouldAutorotate: Bool {
        return true
    }
    
    override var supportedInterfaceOrientations: UIInterfaceOrientationMask {
        if UIDevice.current.userInterfaceIdiom == .phone {
            return .allButUpsideDown
        } else {
            return .all
        }
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Release any cached data, images, etc that aren't in use.
    }
    
    override var prefersStatusBarHidden: Bool {
        return true
    }
}
