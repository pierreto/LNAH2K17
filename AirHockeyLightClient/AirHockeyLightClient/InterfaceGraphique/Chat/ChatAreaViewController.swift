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
    func newUnreadMessage()
}

var channelsToJoin = [ChannelEntity]()
var filteredChannelsToJoin = [ChannelEntity]()
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
    //let searchController = UISearchController(searchResultsController: nil)
    
    var isSearching = false;
    weak var delegate: AddChannelDelegate?
    let clientConnection = HubManager.sharedConnection
    static var sharedChatAreaViewController = ChatAreaViewController()
    
    //Mar : Actions
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
        
        joinChannelTableView.autoresizingMask = UIViewAutoresizing()
        joinChannelTableView.translatesAutoresizingMaskIntoConstraints = false
        
        //Search bar
        searchBar.delegate = self

    }
    
    override func viewWillAppear(_ animated: Bool) {
        extendedLayoutIncludesOpaqueBars = true
    }
    
    /// Enclenché lorsqu'un message est reçu
    func receiveMessage(message: Dictionary<String, String>) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        let dateString = self.convertDate(dateString: timestamp!)
        
        print("Message: \(String(describing: sender! + " (" + dateString + ") : " + messageValue!))\n")
        let chan = channels.first(where: { $0.name == "Principal" })
        if channel.name != chan?.name {
            chan?.hasUnreadMessage = true;
            delegate = MasterViewController.sharedMasterViewController
            delegate?.newUnreadMessage()
        }
        chan?.messages.append(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (dateString)))
//        let indexPath = IndexPath(row: (chan?.messages.count)! - 1, section: 0);
//        self.chatTableView?.scrollToRow(at: indexPath, at: UITableViewScrollPosition.bottom, animated: false)
        DispatchQueue.main.async(execute: { () -> Void in
            self.chatTableView.reloadData()
            let indexPath = IndexPath(row: (chan?.messages.count)! - 1, section: 0);
            self.chatTableView?.scrollToRow(at: indexPath, at: .bottom, animated: true)
        })
    }
    
    func receiveMessageChannel(message: Dictionary<String, String>, channelName: String) {
        let sender = message["Sender"]
        let messageValue = message["MessageValue"]
        let timestamp = message["TimeStamp"]
        
        let dateString = self.convertDate(dateString: timestamp!)
        
        print("Message: \(String(describing: sender! + " (" + dateString + ") : " + messageValue!))\n")
        let chan = channels.first(where: { $0.name == channelName })
        if channel.name != chan?.name {
            chan?.hasUnreadMessage = true;
            delegate = MasterViewController.sharedMasterViewController
            delegate?.newUnreadMessage()
        }
        chan?.messages.append(ChatMessageEntity(sender:sender!, messageValue: messageValue!, timestamp: (dateString)))

        DispatchQueue.main.async(execute: { () -> Void in
            self.chatTableView.reloadData()
            let indexPath = IndexPath(row: (chan?.messages.count)! - 1, section: 0);
            self.chatTableView?.scrollToRow(at: indexPath, at: .bottom, animated: true)
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
            //timestamp.text = channel.messages[indexPath.row].getTimestamp()
            timestamp.text = "temp val"

            if sender.text != HubManager.sharedConnection.getUsername() {
                (cell as! MessageViewCell).leadingConstraint.isActive = true
                (cell as! MessageViewCell).bubbleConstraint.isActive = false
                (cell as! MessageViewCell).messageContainer.backgroundColor = UIColor(red:0.79, green:0.79, blue:0.79, alpha:1.0)
                sender.textColor = UIColor .black
                messageValue.textColor = UIColor .black
                timestamp.textColor = UIColor .black
            } else {
                (cell as! MessageViewCell).leadingConstraint.isActive = false
                (cell as! MessageViewCell).bubbleConstraint.isActive = true
            }
        }
        
        if tableView == self.joinChannelTableView {
            cell = joinChannelTableView.dequeueReusableCell(withIdentifier: "JoinCell", for: indexPath)
            let channelToJoin: ChannelEntity
            if isSearching {
                channelToJoin = filteredChannelsToJoin[indexPath.row]
            } else {
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
    
    private func refreshUI() {
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
        cJoinChannelConstraint = -241
        self.addChannelView.isHidden = !self.addChannelView.isHidden
    }
}

