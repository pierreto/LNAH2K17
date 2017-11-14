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

    private let friendsHub = HubManager.sharedConnection.getFriendsHub()
    private var pendingRequestsData = [FriendRequestEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        FriendRequestsTableViewController.instance = self
        
        self.pendingRequests.delegate = self
        self.pendingRequests.dataSource = self
    }
    
    func updatePendingRequestsEntries(pendingRequests: [FriendRequestEntity]) {
        self.pendingRequestsData = pendingRequests
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.pendingRequestsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        return self.pendingRequests.dequeueReusableCell(withIdentifier: "Request", for: indexPath)
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
