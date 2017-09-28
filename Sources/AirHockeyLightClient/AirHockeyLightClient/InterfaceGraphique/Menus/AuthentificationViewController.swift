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

class AuthentificationViewController: UIViewController  {
    
    let clientConnection = ClientConnection.sharedConnection
    @IBOutlet weak var usernameInput: UITextField!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Connect to server via SignalR
        //clientConnection.EstablishConnection(hubName: "ChatHub")
    }
    
    // Set username
    @IBAction func userLogin(_ sender: UIButton) {
        if usernameInput.text != "" {
            // Create POST request
            var request = URLRequest(url: URL(string: "http://132.207.247.240:63056/api/login")!)
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
                
                //self.usernameInput.text = ""
                //self.usernameInput.placeholder = "Le nom d'utilisateur est déjà pris"
            }
            
            responseString = String(data: data, encoding: .utf8)
            print("responseString = \(String(describing: responseString))")
            
            if httpStatus?.statusCode == 200 {
                self.clientConnection.setUsername(username: self.usernameInput.text!)
            }
        }
        
        task.resume()
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
