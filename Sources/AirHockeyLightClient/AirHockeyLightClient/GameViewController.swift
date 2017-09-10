//
//  GameViewController.swift
//  AirHockeyLightClient
//
//  Created by Pierre To and Mikael Ferland on 17-09-10.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit
import SpriteKit
import GameplayKit

struct Post {
    let bodyText: String!
}

class GameViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {

    @IBOutlet weak var chatInput: UITextField!
    @IBOutlet weak var messages: UITableView!
    var messagesData = [Post]()
    
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
    }
    
    // Add message to messagesData on button click
    @IBAction func sendMessage(_ sender: UIButton) {
        if chatInput.text != "" {
            self.messagesData.append(Post(bodyText: chatInput.text))
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.messages.reloadData()
            })
            
            chatInput.text = ""
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
        let cell = messages.dequeueReusableCell(withIdentifier: "Message", for: indexPath)
        
        let bodyText = cell.viewWithTag(1) as! UITextView
        bodyText.text = messagesData[indexPath.row].bodyText
        
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
