//
//  MasterViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-05.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

protocol ChannelSelectionDelegate: class {
    var sChannelNameErrMsg: String {get set}
    var sChannelName: String { get set}
    var cJoinChannelConstraint: Float { get set}
    func channelSelected(newChannel: ChannelEntity)
    func toggleAddChannelView()
}

class MasterViewController: UITableViewController {
    let clientConnection = HubManager.sharedConnection
    static var sharedMasterViewController = MasterViewController()
    var channels = [ChannelEntity]()
    
    weak var delegate: ChannelSelectionDelegate?
    @IBOutlet weak var toggleChannelButton: UIButton!
    @IBOutlet weak var addChannelButton: UIButton!
    @IBOutlet var channelTableView: UITableView!
    @IBAction func toggleJoinChannelMenu(_ sender: Any) {
        if delegate?.cJoinChannelConstraint == 1 {
            self.toggleChannelButton.setTitle("\u{f061}", for: .normal)
            delegate?.cJoinChannelConstraint = -241
        } else {
            self.toggleChannelButton.setTitle("\u{f060}", for: .normal)
            delegate?.cJoinChannelConstraint = 1
        }
        
        //Hide the add channel popup if we open the join channel view
        if !ChatAreaViewController.sharedChatAreaViewController.addChannelView.isHidden {
            ChatAreaViewController.sharedChatAreaViewController.addChannelView.isHidden = true
        }
    }

    @IBAction func addChannel(_ sender: Any) {
        self.toggleChannelButton.setTitle("\u{f061}", for: .normal)
        self.delegate?.toggleAddChannelView()
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        channels.append(ChannelEntity(name: "Principal"))
        MasterViewController.sharedMasterViewController = self
        ChatAreaViewController.sharedChatAreaViewController.channel = channels.first
        delegate = ChatAreaViewController.sharedChatAreaViewController
        channelTableView.delegate = self
        channelTableView.dataSource = self
        channelTableView.register(UITableViewCell.self, forCellReuseIdentifier: "Cell")
        
        let indexPath = IndexPath(row: 0, section: 0);
        self.channelTableView.selectRow(at: indexPath, animated: false, scrollPosition: .bottom)
        self.channelTableView.delegate?.tableView!(self.channelTableView, didSelectRowAt: indexPath)
        
        self.toggleChannelButton.setTitle("\u{f061}", for: .normal)
        self.addChannelButton.setTitle("\u{f067}", for: .normal)
        // Uncomment the following line to preserve selection between presentations
        // self.clearsSelectionOnViewWillAppear = false

        // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
        // self.navigationItem.rightBarButtonItem = self.editButtonItem
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    // MARK: - Table view data source

    override func numberOfSections(in tableView: UITableView) -> Int {
        // #warning Incomplete implementation, return the number of sections
        return 1
    }

    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        // #warning Incomplete implementation, return the number of rows
        return channels.count
    }

    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "Cell", for: indexPath)

        // Configure the cell...
        let channel = channels[indexPath.row]
        cell.backgroundColor = UIColor(red:0.24, green:0.24, blue:0.24, alpha:1.0)
        cell.textLabel?.text = channel.name
        if channel.hasUnreadMessage {
            cell.textLabel?.textColor = UIColor .red
        } else {
            cell.textLabel?.textColor = UIColor .white
        }
        
        let bgColorView = UIView()
        bgColorView.backgroundColor = UIColor(red:0.29, green:0.29, blue:0.29, alpha:1.0)
        cell.selectedBackgroundView = bgColorView
        
        return cell
    }

    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let selectedChannel = channels[indexPath.row]
        self.delegate?.channelSelected(newChannel: selectedChannel)
        selectedChannel.hasUnreadMessage = false
        let currentCell = channelTableView.cellForRow(at: indexPath)!
        currentCell.textLabel?.textColor = UIColor .white
    }

    // Override to support conditional editing of the table view.
    override func tableView(_ tableView: UITableView, canEditRowAt indexPath: IndexPath) -> Bool {
        // Return false if you do not want the specified item to be editable.
        //Cant delete private channels nor main channel
        if(indexPath.row == 0 || channels[indexPath.row].isPrivate) { return false }
        return true
    }
    
    override func tableView(_ tableView: UITableView, titleForDeleteConfirmationButtonForRowAt indexPath: IndexPath) -> String? {
        return "Quitter"
    }
    
    // Override to support editing the table view.
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        if editingStyle == .delete {
            // Delete the row from the data source
            HubManager.sharedConnection.getChatHub().leaveRoom(roomName: channels[indexPath.row].getName())
            channels.remove(at: indexPath.row)
            tableView.deleteRows(at: [indexPath], with: .fade)
        } else if editingStyle == .insert {
            // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view
        }    
    }

    override func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
//        if 0 == indexPath.row {
//            print(indexPath)
//            tableView.selectRow(at: indexPath, animated: false, scrollPosition: UITableViewScrollPosition.none)
//        }
    }
    /*
    // Override to support rearranging the table view.
    override func tableView(_ tableView: UITableView, moveRowAt fromIndexPath: IndexPath, to: IndexPath) {

    }
    */

    /*
    // Override to support conditional rearranging of the table view.
    override func tableView(_ tableView: UITableView, canMoveRowAt indexPath: IndexPath) -> Bool {
        // Return false if you do not want the item to be re-orderable.
        return true
    }
    */

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

    deinit {
        print("de-init MasterViewController")
        reset()
    }
    
    func reset() {
        if( channelTableView != nil) {
            self.channels = [ChannelEntity]()
            self.channels.append(ChannelEntity(name: "Principal"))
            self.channels[0].messages = [ChatMessageEntity]()
            let indexPath = IndexPath(row: 0, section: 0);
            self.channelTableView.selectRow(at: indexPath, animated: false, scrollPosition: .bottom)
            self.channelTableView.reloadData()

        }
    }
}

extension MasterViewController: AddChannelDelegate {
    func addChannel(channelName: String) {
        if(channelName == "" || channelName == nil) {
            self.delegate?.sChannelNameErrMsg = "Nom invalide"
        } else {
            let chatHub = clientConnection.getChatHub()
            chatHub.CreateChannel(channelName: channelName) { res in
            if res == "" {
                self.channels.insert(ChannelEntity(name: channelName), at:1)
                self.delegate?.sChannelNameErrMsg = ""
                self.delegate?.sChannelName = ""
                self.delegate?.toggleAddChannelView()
                self.channelTableView.reloadData()
                let indexPath = IndexPath(row: 1, section: 0);
                self.channelTableView.selectRow(at: indexPath, animated: false, scrollPosition: .bottom)
                self.channelTableView.delegate?.tableView!(self.channelTableView, didSelectRowAt: indexPath)
            } else {
                self.delegate?.sChannelNameErrMsg = "Canal déjà créé"
                print("Canal existe deja")
            }
        }
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.channelTableView.reloadData()
        })
        }

    }
    
    func joinChannel(channelName: String) {
        self.channels.insert(ChannelEntity(name: channelName), at:1)
        self.channelTableView.beginUpdates()
        self.channelTableView.insertRows(at: [IndexPath.init(row: 1, section: 0)], with: .automatic)
        self.channelTableView.endUpdates()
        print("Num channels: ", channels.count)
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.channelTableView.reloadData()
            self.channelTableView.selectRow(at: IndexPath.init(row: 1, section: 0), animated: false, scrollPosition: .bottom)
            self.channelTableView.delegate?.tableView!(self.channelTableView, didSelectRowAt: IndexPath.init(row: 1, section: 0))
        })
    }
    
    func addPrivateChannel(othersName: String, othersId: Int, othersProfile: String) {
        if !channels.contains(where: { (cE: ChannelEntity) -> Bool in
            (cE.name == othersName && (cE.isPrivate == true))
        }) {
            let chatHub = clientConnection.getChatHub()
            chatHub.CreatePrivateChannel(othersName: HubManager.sharedConnection.getUsername()!, myId: HubManager.sharedConnection.getId()!, othersId: othersId, othersProfile: HubManager.sharedConnection.getUser().getProfile()) { res in
                if res == true {
                    self.channels.insert(ChannelEntity(name: othersName, isPrivate: true, privateUserId: othersId, profile: othersProfile), at: 1)
                    self.channelTableView.reloadData()
                    let indexPath = IndexPath(row: 1, section: 0);
                    self.channelTableView.selectRow(at: indexPath, animated: false, scrollPosition: .bottom)
                    self.channelTableView.delegate?.tableView!(self.channelTableView, didSelectRowAt: indexPath)
                } else {
                    self.delegate?.sChannelNameErrMsg = "Canal déjà créé"
                    print("Canal existe deja")
                }
            }
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.channelTableView.reloadData()
            })
        }
    }
    
    
    func newUnreadMessage() {
        self.channelTableView.reloadData()
    }
}
