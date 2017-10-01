///////////////////////////////////////////////////////////////////////////////
/// @file ChatViewController.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import SwiftR

///////////////////////////////////////////////////////////////////////////
/// @class ChatViewController
/// @brief Controlleur de la vue de la messagerie
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-10
///////////////////////////////////////////////////////////////////////////
class ChatViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    
    static var sharedChatViewController = ChatViewController()
    
    var messagesData = [ChatMessageEntity]()
    let clientConnection = ClientConnection.sharedConnection
    
    @IBOutlet weak var chatBodyView: UIView!
    @IBOutlet weak var chatInput: UITextField!
    @IBOutlet weak var sendButton: UIButton!
    @IBOutlet weak var messages: UITableView!
    
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
        messages.rowHeight = UITableViewAutomaticDimension
        messages.estimatedRowHeight = 140
        
        /// Ajouter les notifications pour ajuster l'affichage lorsque le clavier virtuel apparait/disparait
        NotificationCenter.default.addObserver(self, selector: #selector(keyboardWillShow), name: NSNotification.Name.UIKeyboardWillShow, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(keyboardWillHide), name: NSNotification.Name.UIKeyboardWillHide, object: nil)
    
        ChatViewController.sharedChatViewController = self
    }
    
    deinit {
        /// Retirer les notifications pour ajuster l'affichage lorsque le clavier virtuel apparait/disparait
        NotificationCenter.default.removeObserver(self, name: NSNotification.Name.UIKeyboardWillShow, object: nil)
        NotificationCenter.default.removeObserver(self, name: NSNotification.Name.UIKeyboardWillHide, object: nil)
    }
    
    /// Enclenché lorsqu'un message est reçu
    func receiveMessage(message: Dictionary<String, String>) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        print("Message: \(String(describing: sender! + " (" + timestamp! + ") : " + messageValue!))\n")
            
        self.messagesData.insert(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (timestamp?.description)!), at: 0)
            
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.messages.reloadData()
        })
    }
    
    /// Enclenché lorsque l'utilisateur pèse sur la touche Enter du clavier
    @IBAction func triggerSendMessage(_ sender: Any) {
        self.sendMessage(nil)
    }
    
    /// Enclenché lorsque l'utilisateur envoie un message
    @IBAction func sendMessage(_ sender: UIButton?) {
        if chatInput.text != "" {
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
    
    /// Enclenché lorsque le clavier virtuel apparait
    func keyboardWillShow(notification: NSNotification) {
        if let keyboardSize = (notification.userInfo?[UIKeyboardFrameBeginUserInfoKey] as? NSValue)?.cgRectValue {
            if self.chatBodyView.frame.origin.y == 76 {
                self.chatBodyView.frame.origin.y -= keyboardSize.height
            }
        }
    }
    
    /// Enclenché lorsque le clavier virtuel disparait
    func keyboardWillHide(notification: NSNotification) {
        if let keyboardSize = (notification.userInfo?[UIKeyboardFrameBeginUserInfoKey] as? NSValue)?.cgRectValue {
            if self.chatBodyView.frame.origin.y != 76 {
                self.chatBodyView.frame.origin.y += keyboardSize.height
            }
        }
    }
    
    // Table view delegate methods
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return messagesData.count
    }
    
    /// Afficher un message dans le tableau
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = messages.dequeueReusableCell(withIdentifier: "Message", for: indexPath) as! MessageViewCell
        
        let sender = cell.viewWithTag(1) as! UILabel
        sender.text = messagesData[indexPath.row].getSender()
        
        let messageValue = cell.viewWithTag(2) as! PaddingLabel
        messageValue.text = messagesData[indexPath.row].getMessageValue()
        
        let timestamp = cell.viewWithTag(3) as! UILabel
        timestamp.text = messagesData[indexPath.row].getTimestamp()
        
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

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
