//
//  ChatAreaViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-05.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

protocol AddChannelDelegate: class {
    func addChannel(channelName: String)
}

class ChatAreaViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    //Mark : Properties
    @IBOutlet weak var joinChannelConstraint: NSLayoutConstraint!
    @IBOutlet weak var messageField: UITextField!
    @IBOutlet weak var chatTableView: UITableView!
    @IBOutlet weak var addChannelView: UIView!
    @IBOutlet weak var joinChannelView: UIView!
    @IBAction func editChannelName(_ sender: Any) {
        self.channelNameErrMsg.text = ""
    }
    @IBAction func createChannel(_ sender: Any) {
        delegate = MasterViewController.sharedMasterViewController
        delegate?.addChannel(channelName: channelNameField.text!)
    }
    
    @IBAction func cancelCreateChannel(_ sender: Any) {
        channelNameField.text = ""
        toggleAddChannelView()
    }
    
    @IBOutlet weak var channelNameField: UITextField!
    @IBOutlet weak var channelNameErrMsg: UILabel!
    
    weak var delegate: AddChannelDelegate?
    let clientConnection = HubManager.sharedConnection
    static var sharedChatAreaViewController = ChatAreaViewController()
    
    public var channel: ChannelEntity! {
        didSet (newChannel) {
            self.refreshUI()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        joinChannelConstraint.constant = -241
        channelNameErrMsg.text = ""
        ChatAreaViewController.sharedChatAreaViewController = self;
        chatTableView.delegate = self
        chatTableView.dataSource = self
        refreshUI()
        addChannelView.isHidden = true
        delegate = MasterViewController.sharedMasterViewController
        // Do any additional setup after loading the view.
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    /// Enclenché lorsqu'un message est reçu
    func receiveMessage(message: Dictionary<String, String>) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        print("Message: \(String(describing: sender! + " (" + timestamp! + ") : " + messageValue!))\n")
    
        self.channel.messages.insert(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (timestamp?.description)!), at: 0)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.chatTableView.reloadData()
        })
    }
    
    func receiveMessageChannel(message: Dictionary<String, String>, channelName: String) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        print("Message: \(String(describing: sender! + " (" + timestamp! + ") : " + messageValue!))\n")
        
        channel.messages.insert(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (timestamp?.description)!), at: 0)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.chatTableView.reloadData()
        })
    }

    
    //Mark: Actions
    @IBAction func sendButton(_ sender: Any) {
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
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return channel.messages.count
    }
    
    /// Afficher un message dans le tableau
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = chatTableView.dequeueReusableCell(withIdentifier: "Message", for: indexPath) as! MessageViewCell
        
        let sender = cell.viewWithTag(1) as! UILabel
        sender.text = channel.messages[indexPath.row].getSender()
        
        let messageValue = cell.viewWithTag(2) as! UILabel
        messageValue.text = channel.messages[indexPath.row].getMessageValue()
        
        let timestamp = cell.viewWithTag(3) as! UILabel
        timestamp.text = channel.messages[indexPath.row].getTimestamp()
                
        return cell
    }
    
    private func refreshUI() {
    }
    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */
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
        self.addChannelView.isHidden = !self.addChannelView.isHidden
    }
}
