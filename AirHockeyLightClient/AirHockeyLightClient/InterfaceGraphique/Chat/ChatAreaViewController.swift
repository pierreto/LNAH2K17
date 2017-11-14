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

var channelsToJoin = [ChannelEntity]()
class ChatAreaViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    //Mark : Properties
    @IBOutlet weak var joinChannelConstraint: NSLayoutConstraint!
    @IBOutlet weak var messageField: UITextField!
    @IBOutlet weak var chatTableView: UITableView!
    @IBOutlet weak var addChannelView: UIView!
    @IBOutlet weak var joinChannelView: UIView!
    @IBOutlet weak var joinChannelTableView: UITableView!
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
        chatTableView.register(UITableViewCell.self, forCellReuseIdentifier: "Cell")
        
        joinChannelTableView.dataSource = self
        joinChannelTableView.delegate = self
        joinChannelTableView.register(UITableViewCell.self, forCellReuseIdentifier: "JoinCell")
        
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
        
        let dateString = self.convertDate(dateString: timestamp!)
        
        print("Message: \(String(describing: sender! + " (" + dateString + ") : " + messageValue!))\n")
        let chan = channels.first(where: { $0.name == "Principal" })
        
        chan?.messages.append(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (dateString)))
        
        DispatchQueue.main.async(execute: { () -> Void in
            self.chatTableView.reloadData()
        })
    }
    
    func receiveMessageChannel(message: Dictionary<String, String>, channelName: String) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        
        let dateString = self.convertDate(dateString: timestamp!)
        
        print("Message: \(String(describing: sender! + " (" + dateString + ") : " + messageValue!))\n")
        let chan = channels.first(where: { $0.name == channelName })
        chan?.messages.append(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (dateString)))
        
        DispatchQueue.main.async(execute: { () -> Void in
            self.chatTableView.reloadData()
        })
    }
    
    func channelDeleted(channelName: String) {
        if let index = channelsToJoin.index(where: { $0.name == channelName }) {
            channelsToJoin.remove(at: index)
            joinChannelTableView.reloadData()
        }
    }
    
    func newJoinableChannel(channelName: String) {
        channelsToJoin.append(ChannelEntity(name: channelName))
        joinChannelTableView.reloadData()
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
    
    func joinChannel(channelName: String) {
        clientConnection.getChatHub().joinChannel(channelName: channelName)
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        var count:Int?
        
        if tableView == self.chatTableView {
            count = channel.messages.count
        }
        
        if tableView == self.joinChannelTableView {
            count =  channelsToJoin.count
        }
        
        return count!
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        if tableView == self.joinChannelTableView {
            self.joinChannel(channelName: channelsToJoin[indexPath.row].name)
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
        }
        
        if tableView == self.joinChannelTableView {
            cell = joinChannelTableView.dequeueReusableCell(withIdentifier: "JoinCell", for: indexPath)
            let channelToJoin = channelsToJoin[indexPath.row]
            cell!.textLabel?.text = channelToJoin.name
        }
        return cell!
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
    
    func convertDate(dateString: String) -> String {
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ssZ"
        let date = dateFormatter.date(from: dateString)
        
        dateFormatter.dateFormat = "HH:mm:ss"
        let dateString = dateFormatter.string(from: date!)
        
        return dateString
    }
}

