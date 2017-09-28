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
    let sender: String!
    let messageValue: String!
    let timestamp: String!
}

class ChatViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {

    @IBOutlet weak var chatBodyView: UIView!
    @IBOutlet weak var chatInput: UITextField!
    @IBOutlet weak var messages: UITableView!
    var messagesData = [Message]()
    let clientConnection = ClientConnection.sharedConnection
    
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
        
        messages.delegate = self
        messages.dataSource = self
        messages.transform = CGAffineTransform(rotationAngle: -(CGFloat)(Double.pi))
        
        //Adding notifies on keyboard appearing
        NotificationCenter.default.addObserver(self, selector: #selector(keyboardWillShow), name: NSNotification.Name.UIKeyboardWillShow, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(keyboardWillHide), name: NSNotification.Name.UIKeyboardWillHide, object: nil)
        
        clientConnection.EstablishConnection()
        
        // When a message is received from the server
        clientConnection.getChatHub().on("ChatMessageReceived") { args in
            let message = args![0] as! Dictionary<String, String>
            let sender = message["Sender"]
            let messageValue = message["MessageValue"]
            let timestamp = message["TimeStamp"]
            print("Message: \(String(describing: sender! + " (" + timestamp! + ") : " + messageValue!))\n")
            
            self.messagesData.insert(Message(sender:sender, messageValue: messageValue, timestamp: timestamp), at: 0)
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.messages.reloadData()
            })
        }
        
        messages.rowHeight = UITableViewAutomaticDimension
        messages.estimatedRowHeight = 140
    }
    
    deinit {
        //Removing notifies on keyboard appearing
        NotificationCenter.default.removeObserver(self, name: NSNotification.Name.UIKeyboardWillShow, object: nil)
        NotificationCenter.default.removeObserver(self, name: NSNotification.Name.UIKeyboardWillHide, object: nil)
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
            jsonObject.setValue(clientConnection.getUsername(), forKey: "Sender")
            jsonObject.setValue(chatInput.text, forKey: "MessageValue")
            jsonObject.setValue(Date().description, forKey: "TimeStamp")
            
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
            //let chatMessage = ChatMessageEntity(sender: clientConnection.getUsername(), messageValue: self.chatInput.text!, timeStamp: Date())
            
            let message = [
                "Sender": clientConnection.getUsername(),
                "MessageValue": self.chatInput.text!,
                "TimeStamp": Date().description
            ]
            clientConnection.SendBroadcast(message : message)

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
            //self.messagesData.append(Message(bodyText: responseString))
            
            /*DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.messages.reloadData()
            })*/
        }
        
        task.resume()
    }
    
    func keyboardWillShow(notification: NSNotification) {
        if let keyboardSize = (notification.userInfo?[UIKeyboardFrameBeginUserInfoKey] as? NSValue)?.cgRectValue {
            if self.chatBodyView.frame.origin.y == 76 {
                self.chatBodyView.frame.origin.y -= keyboardSize.height
            }
        }
    }
    
    func keyboardWillHide(notification: NSNotification) {
        if let keyboardSize = (notification.userInfo?[UIKeyboardFrameBeginUserInfoKey] as? NSValue)?.cgRectValue {
            if self.chatBodyView.frame.origin.y != 76 {
                self.chatBodyView.frame.origin.y += keyboardSize.height
            }
        }
    }
    
    // Table view delegate methods
    /*func numberOfSectionsInTableView(tableView: UITableView) -> Int {
        return 1
    }*/
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return messagesData.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = messages.dequeueReusableCell(withIdentifier: "Message", for: indexPath) as! MessageViewCell
        
        let sender = cell.viewWithTag(1) as! UILabel
        sender.text = messagesData[indexPath.row].sender
        
        let messageValue = cell.viewWithTag(2) as! PaddingLabel
        messageValue.text = messagesData[indexPath.row].messageValue
        
        let timestamp = cell.viewWithTag(3) as! UILabel
        timestamp.text = messagesData[indexPath.row].timestamp
        
        cell.transform = CGAffineTransform(rotationAngle: CGFloat(Double.pi));
        
        return cell
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
