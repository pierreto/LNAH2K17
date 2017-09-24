//
//  ChatViewController.swift
//  AirHockeyLightClient
//
//  Created by Pierre To and Mikael Ferland on 17-09-10.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit
import SpriteKit
import GameplayKit

import SwiftR

struct Message {
    let bodyText: String!
}

class ChatViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {

    @IBOutlet weak var chatInput: UITextField!
    @IBOutlet weak var messages: UITableView!
    var messagesData = [Message]()
    var clientConnection = ClientConnection()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        /*if let view = self.view as! SKView? {
            // Load the SKScene from 'GameScene.sks'
            if let scene = SKScene(fileNamed: "GameScene") {
                // Set the scale mode to scale to fit the window
                scene.scaleMode = .aspectFill
                
                // Present the scene
                view.presentScene(scene)
            }
            
            view.ignoresSiblingOrder = true
            
            view.showsFPS = true
            view.showsNodeCount = true
        }*/
        
        messages.delegate = self;
        messages.dataSource = self;
        
        clientConnection.EstablishConnection()
        
        // When a message is received from the server
        clientConnection.getChatHub().on("ChatMessageReceived") { args in
            let message = args![0] as! String
            print("Message: \(message)\n")
            
            self.messagesData.append(Message(bodyText: message))
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.messages.reloadData()
            })
        }
    }
    
    // Send message to server on button click
    @IBAction func sendMessage(_ sender: UIButton) {
        if chatInput.text != "" {
            // Create POST request
            //var request = URLRequest(url: URL(string: "http://localhost:8080/api/chat/")!)
            //request.httpMethod = "POST"
            
            // Headers
            //request.allHTTPHeaderFields?["Content-Type"] = "application/json"
            
            // Body
            /*let jsonObject: NSMutableDictionary = NSMutableDictionary()
            jsonObject.setValue("Server", forKey: "Recipient")
            jsonObject.setValue("Light-client", forKey: "Sender")
            jsonObject.setValue(chatInput.text, forKey: "MessageValue")
            
            let date = Date()
            jsonObject.setValue(date.description, forKey:"TimeStamp")
            
            let jsonString = convertToJsonString(jsonObject: jsonObject)*/
            //request.httpBody = requestBody.data(using: .utf8)
            
            // Send request
            //sendHttpRequest(request: request)
            
            // Convert json string to byte array
            /*var str = jsonString
            var byteArray = [UInt8]()
            
            for char in str.utf8 {
                byteArray += [char]
            }*/
            
            // Send message to server
            clientConnection.SendBroadcast(username: "Username", message: self.chatInput.text!)

            // Clear chat box
            self.chatInput.text = ""
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
            
            if let httpStatus = response as? HTTPURLResponse, httpStatus.statusCode != 200 {
                // check for http errors
                print("statusCode should be 200, but is \(httpStatus.statusCode)")
                print("response = \(String(describing: response))")
            }
            
            responseString = String(data: data, encoding: .utf8)
            print("responseString = \(String(describing: responseString))")
            
            // Fake chat
            self.messagesData.append(Message(bodyText: responseString))
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.messages.reloadData()
            })
        }
        
        task.resume()
    }
    
    
    // Table view delegate methods
    /*func numberOfSectionsInTableView(tableView: UITableView) -> Int {
        return 1
    }*/
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return messagesData.count;
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = messages.dequeueReusableCell(withIdentifier: "Message", for: indexPath);
        
        let textView = cell.viewWithTag(1) as! UITextView;
        textView.text = messagesData[indexPath.row].bodyText;
        
        return cell;
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
