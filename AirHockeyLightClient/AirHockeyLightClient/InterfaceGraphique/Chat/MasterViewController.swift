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

var channels = [ChannelEntity]()

class MasterViewController: UITableViewController {
    let clientConnection = HubManager.sharedConnection
    static var sharedMasterViewController = MasterViewController()
    
    weak var delegate: ChannelSelectionDelegate?
    @IBOutlet var channelTableView: UITableView!
    @IBAction func toggleJoinChannelMenu(_ sender: Any) {
        if delegate?.cJoinChannelConstraint == 1 {
            delegate?.cJoinChannelConstraint = -241
        } else {
            delegate?.cJoinChannelConstraint = 1
        }
    }

    @IBAction func addChannel(_ sender: Any) {
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
        self.channelTableView.selectRow(at: indexPath, animated: true, scrollPosition: .bottom)
        self.channelTableView.delegate?.tableView!(self.channelTableView, didSelectRowAt: indexPath)
        
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
        selectedChannel.hasUnreadMessage = false
        self.delegate?.channelSelected(newChannel: selectedChannel)
    }

    // Override to support conditional editing of the table view.
    override func tableView(_ tableView: UITableView, canEditRowAt indexPath: IndexPath) -> Bool {
        // Return false if you do not want the specified item to be editable.
        if(indexPath.row == 0) { return false }
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

}

extension MasterViewController: AddChannelDelegate {
    func addChannel(channelName: String) {
        let chatHub = clientConnection.getChatHub()
        chatHub.CreateChannel(channelName: channelName) { res in
            if res == "" {
                channels.append(ChannelEntity(name: channelName))
                self.delegate?.sChannelNameErrMsg = ""
                self.delegate?.sChannelName = ""
                self.delegate?.toggleAddChannelView()
                self.channelTableView.reloadData()
                let indexPath = IndexPath(row: channels.count - 1, section: 0);
                self.channelTableView.selectRow(at: indexPath, animated: true, scrollPosition: .bottom)
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
    
    func newUnreadMessage() {
        self.channelTableView.reloadData()
    }
}
