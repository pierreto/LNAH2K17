//
//  ChatAreaViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-05.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit
import SwiftyRSA

protocol AddChannelDelegate: class {
    func addChannel(channelName: String)
    func newUnreadMessage()
}

class ChatAreaViewController: UIViewController, UITableViewDelegate, UITableViewDataSource, UISearchBarDelegate, UISearchControllerDelegate {
    //Mark : Properties
    @IBOutlet weak var joinChannelConstraint: NSLayoutConstraint!
    @IBOutlet weak var messageField: UITextField!
    @IBOutlet weak var chatTableView: UITableView!
    @IBOutlet weak var addChannelView: UIView!
    @IBOutlet weak var joinChannelView: UIView!
    @IBOutlet weak var joinChannelTableView: UITableView!
    @IBOutlet weak var channelNameField: UITextField!
    @IBOutlet weak var channelNameErrMsg: UILabel!
    @IBOutlet weak var searchBar: UISearchBar!

    private var publicKey: PublicKey?
    private var privateKey: PrivateKey?

    var isSearching = false
    let joinChannelHiddenX = Float(-241)
    weak var delegate: AddChannelDelegate?
    
    let clientConnection = HubManager.sharedConnection
    static var sharedChatAreaViewController = ChatAreaViewController()

    var channelsToJoin = [ChannelEntity]()
    var filteredChannelsToJoin = [ChannelEntity]()
    
    //Mark : Actions
    //Envoi le message lorsqu'on pese sur envoyer
    @IBAction func sendButton(_ sender: Any) {
        sendMessage()
    }
    //Utilisateur pese sur return lorsqu'il tappe un message
    @IBAction func messageInputReturn(_ sender: Any) {
        sendMessage()
        self.messageField.resignFirstResponder()
    }
    
    //Changement dans le nom du canal pour effacer le message d'erreur
    @IBAction func editChannelName(_ sender: Any) {
        self.channelNameErrMsg.text = ""
    }
    
    //Boutton + pour creer le canal
    @IBAction func createChannel(_ sender: Any) {
        delegate = MasterViewController.sharedMasterViewController
        delegate?.addChannel(channelName: channelNameField.text!)
    }
    
    //Ferme la vue de creation de canal
    @IBAction func cancelCreateChannel(_ sender: Any) {
        channelNameField.text = ""
        toggleAddChannelView()
    }
    
    public var channel: ChannelEntity! {
        didSet (newChannel) {
        }
    }
    
    // Mark: functions
    override func viewDidLoad() {
        super.viewDidLoad()
        
        channelNameErrMsg.text = ""
        ChatAreaViewController.sharedChatAreaViewController = self;
        
        //Les messages du chat
        chatTableView.delegate = self
        chatTableView.dataSource = self
        chatTableView.register(UITableViewCell.self, forCellReuseIdentifier: "Cell")
        chatTableView.estimatedRowHeight = 90
        
        //Les canals joignables (initialement plus a gauche -- hidden --)
        joinChannelConstraint.constant = -241
        joinChannelTableView.dataSource = self
        joinChannelTableView.delegate = self
        joinChannelTableView.register(UITableViewCell.self, forCellReuseIdentifier: "JoinCell")
        
        //On cache le popup creation de canaux
        addChannelView.isHidden = true
        delegate = MasterViewController.sharedMasterViewController
        
        //Search bar
        searchBar.delegate = self
        
        do {
            self.publicKey = try PublicKey(pemNamed: "public")
            self.privateKey = try PrivateKey(pemNamed: "private")
        } catch {
            print("Error loading public and/or private key.")
        }
    }

    /// Enclenché lorsqu'un message est reçu dans le canal principal
    func receiveMessage(message: Dictionary<String, String>) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "HH:mm:ss"
        let dateString = dateFormatter.string(from: Date())
        // let dateString = self.convertDate(dateString: timestamp!)
        
        print("Message: \(String(describing: sender! + " (" + dateString + ") : " + messageValue!))\n")
        //Make sure the channel to which messages are added is the main channel
        let chan = MasterViewController.sharedMasterViewController.channels.first(where: { $0.name == "Principal" })
        //If currently active channel is not the main channel, receive an unread message notification
        if channel.name != chan?.name {
            chan?.hasUnreadMessage = true;
            delegate = MasterViewController.sharedMasterViewController
            delegate?.newUnreadMessage()
        }
        //Add messages to the channel
        chan?.messages.append(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (dateString)))
        DispatchQueue.main.async(execute: { () -> Void in
            //Reload table to see new message
            self.chatTableView.reloadData()
            //Scroll to the last message on insert
            print("NUM MSG: ", (chan?.messages.count)!)
            if(chan?.messages.count)! > 1 {
                let indexPath = IndexPath(row: (chan?.messages.count)! - 1, section: 0);
                self.chatTableView?.scrollToRow(at: indexPath, at: .bottom, animated: true)
            }
        })
    }
    
    /// Enclenché lorsqu'un message est reçu dans un autre canal
    func receiveMessageChannel(message: Dictionary<String, String>, channelName: String) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "HH:mm:ss"
        let dateString = dateFormatter.string(from: Date())
        
        //let dateString = self.convertDate(dateString: timestamp!)
        
        print("Message: \(String(describing: sender! + " (" + dateString + ") : " + messageValue!))\n")
        let chan = MasterViewController.sharedMasterViewController.channels.first(where: { $0.name == channelName })
        if channel.name != chan?.name {
            chan?.hasUnreadMessage = true;
            delegate = MasterViewController.sharedMasterViewController
            delegate?.newUnreadMessage()
        }
        chan?.messages.append(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (dateString)))

        DispatchQueue.main.async(execute: { () -> Void in
            self.chatTableView.reloadData()
            print("NUM MSG: ", (chan?.messages.count)!)
            if(chan?.messages.count)! > 1 {
                let indexPath = IndexPath(row: (chan?.messages.count)! - 1, section: 0);
                self.chatTableView?.scrollToRow(at: indexPath, at: .bottom, animated: true)
            }
        })
    }
    
    //Message envoye par le chatHub lorsqu'il ne reste plus personne dans un canal
    //Cela indique qu'il devrait etre retire des canaux joignables
    func channelDeleted(channelName: String) {
        if let index = channelsToJoin.index(where: { $0.name == channelName }) {
            channelsToJoin.remove(at: index)
            joinChannelTableView.reloadData()
        }
    }
    
    //Message envoye par le chatHub lorsqu'un nouveau canal joignable est cree
    func newJoinableChannel(channelName: String) {
        if !channelsToJoin.contains(where: { (cE: ChannelEntity) -> Bool in
            cE.name == channelName
        }) {
            channelsToJoin.append(ChannelEntity(name: channelName))
            joinChannelTableView.reloadData()
        }
    }
    
    //Envoi le message dans le canal
    func sendMessage() {
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "HH:mm:ss"
        let dateString = dateFormatter.string(from: Date())
        
        if messageField.text != "" {
            let message = [
                "Sender": clientConnection.getUsername()!,
                "MessageValue": self.messageField.text!,
                "TimeStamp": dateString
                ] as [String : Any]
            
            let chatHub = clientConnection.getChatHub()
            if (channel.name == "Principal") {
                chatHub.SendBroadcast(message : message)
            } else {
                chatHub.SendChannel(channelName: channel.name, message : message)
            }
            
            // Clear chat box
            self.messageField.text = ""
        }
    }
    
    private func encryptMessage(msg: String) -> String {
        do {
            let clear = try ClearMessage(string: msg, using: .utf8)
            let encrypted = try clear.encrypted(with: self.publicKey!, padding: .PKCS1)
            return encrypted.base64String
        } catch {
            print("Error encrypting message.")
        }
        
        return ""
    }
    
    private func decryptMessage(msg: String) -> String {
        do {
            let encrypted = try EncryptedMessage(base64Encoded: msg)
            let clear = try encrypted.decrypted(with: self.privateKey!, padding: .PKCS1)
            return try clear.string(encoding: .utf8)
        } catch {
            print("Error decrypting message.")
        }
        
        return ""
    }
    
    func joinChannel(channelName: String) {
        clientConnection.getChatHub().joinChannel(channelName: channelName)
    }
    
    // Mark: table functions
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        var count:Int?
        
        if tableView == self.chatTableView {
            count = channel.messages.count
        }
        
        if tableView == self.joinChannelTableView {
            if isSearching {
                return filteredChannelsToJoin.count
            } else {
                count = channelsToJoin.count
            }
        }
        
        return count!
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        if tableView == self.joinChannelTableView {
            self.joinChannel(channelName: channelsToJoin[indexPath.row].name)
            // Hide join table view a channel is joined
            cJoinChannelConstraint = -241
        }
    }
    
    /// Afficher un message dans le tableau
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        
        var cell:UITableViewCell?
        if tableView == self.chatTableView {
            cell = chatTableView.dequeueReusableCell(withIdentifier: "Message", for: indexPath) as! MessageViewCell
            
            let sender = cell!.viewWithTag(1) as! UILabel
            sender.text = channel.messages[indexPath.row].getSender()
            
            let messageValue = cell!.viewWithTag(2) as! UILabel
            messageValue.text = channel.messages[indexPath.row].getMessageValue()
            
            let timestamp = cell!.viewWithTag(3) as! UILabel
            timestamp.text = channel.messages[indexPath.row].getTimestamp()

            //Display text differently if it is from you or someone else
            //Someone else
            if sender.text != HubManager.sharedConnection.getUsername() {
                //Align bubble to the left
                (cell as! MessageViewCell).leadingConstraint.isActive = true
                (cell as! MessageViewCell).bubbleConstraint.isActive = false
                (cell as! MessageViewCell).messageContainer.backgroundColor = UIColor(red:0.79, green:0.79, blue:0.79, alpha:1.0)
                sender.textColor = UIColor .black
                messageValue.textColor = UIColor .black
                timestamp.textColor = UIColor .black
            //You
            } else {
                //Align bubble to the right
                (cell as! MessageViewCell).leadingConstraint.isActive = false
                (cell as! MessageViewCell).bubbleConstraint.isActive = true
                (cell as! MessageViewCell).messageContainer.backgroundColor = UIColor(red:0.0, green:0.0, blue:0.0, alpha:1.0)

                sender.textColor = UIColor .white
                messageValue.textColor = UIColor .white
                timestamp.textColor = UIColor .white
            }
        }
        
        if tableView == self.joinChannelTableView {
            cell = joinChannelTableView.dequeueReusableCell(withIdentifier: "JoinCell", for: indexPath)
            let channelToJoin: ChannelEntity
            if isSearching {
                // Display filtered channels
                channelToJoin = filteredChannelsToJoin[indexPath.row]
            } else {
                // Display unfiltered channels
                channelToJoin = channelsToJoin[indexPath.row]
            }
            cell?.backgroundColor = UIColor(red:0.24, green:0.24, blue:0.24, alpha:1.0)
            cell?.textLabel?.textColor = UIColor .white
            
            let bgColorView = UIView()
            bgColorView.backgroundColor = UIColor(red:0.29, green:0.29, blue:0.29, alpha:1.0)
            cell?.selectedBackgroundView = bgColorView
            cell!.textLabel?.text = channelToJoin.name
        }

        return cell!
    }
    
    func convertDate(dateString: String) -> String {
        /*let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
        let date = dateFormatter.date(from: dateString)
        
        dateFormatter.dateFormat = "HH:mm:ss"
        let dateString = dateFormatter.string(from: date!)*/
        
        return dateString
    }
    
    func searchBar(_ searchBar: UISearchBar, textDidChange searchText: String) {
        if searchBar.text == nil || searchBar.text == "" {
            isSearching = false
            view.endEditing(true)
            joinChannelTableView.reloadData()
        } else {
            isSearching = true
            filteredChannelsToJoin = channelsToJoin.filter({ (ce: ChannelEntity) -> Bool in
                ce.getName().lowercased().contains((searchBar.text as String!).lowercased())
            })
            joinChannelTableView.reloadData()
        }
    }
    
}

extension ChatAreaViewController: ChannelSelectionDelegate {
    var cJoinChannelConstraint: Float {
        get {
            return Float(self.joinChannelConstraint.constant)
        }
        set(value) {
            self.joinChannelConstraint.constant = CGFloat(value)
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        }
    }
    
    var sChannelName: String {
        get {
            return self.channelNameField.text!
        }
        set(value) {
            self.channelNameField.text = value
        }
    }
    
    var sChannelNameErrMsg: String {
        get {
            return self.channelNameErrMsg.text!
        }
        set(value) {
            self.channelNameErrMsg.text = value
        }
    }
    
    func channelSelected(newChannel: ChannelEntity) {
        channel = newChannel
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.chatTableView.reloadData()
        })
    }
    
    func toggleAddChannelView() {
        cJoinChannelConstraint = -241
        self.addChannelView.isHidden = !self.addChannelView.isHidden
    }
}

