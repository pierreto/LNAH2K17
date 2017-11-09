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
    
//    required init(coder aDecoder: NSCoder) {
//        super.init(coder: aDecoder)!
//        MasterViewController.sharedMasterViewController = self
//        self.channels.append(ChannelEntity(name: "Principal"))
//    }

    @IBAction func addChannel(_ sender: Any) {
        self.delegate?.toggleAddChannelView()
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        channels.append(ChannelEntity(name: "Principal"))
        MasterViewController.sharedMasterViewController = self
        ChatAreaViewController.sharedChatAreaViewController.channel = channels.first
        delegate = ChatAreaViewController.sharedChatAreaViewController
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
        cell.textLabel?.text = channel.name
        return cell
    }

    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let selectedChannel = channels[indexPath.row]
        self.delegate?.channelSelected(newChannel: selectedChannel)
    }

    // Override to support conditional editing of the table view.
    override func tableView(_ tableView: UITableView, canEditRowAt indexPath: IndexPath) -> Bool {
        // Return false if you do not want the specified item to be editable.
        if(indexPath.row == 0) { return false }
        return true
    }
    
    override func tableView(_ tableView: UITableView, titleForDeleteConfirmationButtonForRowAt indexPath: IndexPath) -> String? {
        return "Supprimer"
    }
    
    // Override to support editing the table view.
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        if editingStyle == .delete {
            // Delete the row from the data source
            channels.remove(at: indexPath.row)
            tableView.deleteRows(at: [indexPath], with: .fade)
        } else if editingStyle == .insert {
            // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view
        }    
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
