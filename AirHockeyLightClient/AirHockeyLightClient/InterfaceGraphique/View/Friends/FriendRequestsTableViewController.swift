///////////////////////////////////////////////////////////////////////////////
/// @file FriendRequestsTableViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-13
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class FriendRequestsTableViewController
/// @brief Contrôleur pour visualiser les demandes d'amitié d'un utilisateur
///
/// @author Mikael Ferland
/// @date 2017-11-13
///////////////////////////////////////////////////////////////////////////
class FriendRequestsTableViewController: UITableViewController {
    
    /// Instance singleton
    static var instance = FriendRequestsTableViewController()
    
    @IBOutlet weak var pendingRequests: UITableView!
    
    private var friendsHub: FriendsHub?
    private var pendingRequestsData = [FriendRequestEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        FriendRequestsTableViewController.instance = self
        
        self.pendingRequests.delegate = self
        self.pendingRequests.dataSource = self
        
        friendsHub = HubManager.sharedConnection.getFriendsHub()
        friendsHub?.getAllPendingRequest()
    }
    
    func updatePendingRequestsEntries(pendingRequests:  [FriendRequestEntity]) {
        self.pendingRequestsData = pendingRequests
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.pendingRequests.reloadData()
        })
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.pendingRequestsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = self.pendingRequests.dequeueReusableCell(withIdentifier: "Request", for: indexPath)
        
        let nameLabel = cell.viewWithTag(1) as! UILabel
        nameLabel.text = self.pendingRequestsData[indexPath.row].getRequestor().getName()
        
        return cell
    }
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        // TODO: handle table selection
    }
    
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        
        if editingStyle == .delete {
            // remove the item from the data model
            self.pendingRequestsData.remove(at: indexPath.row)
            
            // delete the table view row
            tableView.deleteRows(at: [indexPath], with: .fade)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
