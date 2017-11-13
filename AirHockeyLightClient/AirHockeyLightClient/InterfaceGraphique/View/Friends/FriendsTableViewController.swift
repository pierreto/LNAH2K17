///////////////////////////////////////////////////////////////////////////////
/// @file FriendsTableViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class FriendsTableViewController
/// @brief Contrôleur pour visualiser les amis d'un utilisateur
///
/// @author Mikael Ferland
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class FriendsTableViewController: UITableViewController {
    
    /// Instance singleton
    static var instance = FriendsTableViewController()
    
    @IBOutlet weak var friends: UITableView!
    
    private let friendsHub = HubManager.sharedConnection.getFriendsHub()
    private var friendsData = [UserEntity]()
    private var pendingRequestsData = [FriendRequestEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        FriendsTableViewController.instance = self
        
        self.friends.delegate = self
        self.friends.dataSource = self
        
        self.friendsHub.initialize()
        self.friendsHub.getAllFriends()
        self.friendsHub.getAllPendingRequest()
    }
    
    func updateFriendsEntries(friends: [UserEntity]) {
        self.friendsData = friends
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.friends.reloadData()
        })
    }
    
    func updatePendingRequestsEntries(pendingRequests: [FriendRequestEntity]) {
        self.pendingRequestsData = pendingRequests
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.friendsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        return self.friends.dequeueReusableCell(withIdentifier: "Friend", for: indexPath)
    }
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        // TODO: handle table selection
    }
    
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        
        if editingStyle == .delete {
            // remove the item from the data model
            self.friendsData.remove(at: indexPath.row)
            
            // delete the table view row
            tableView.deleteRows(at: [indexPath], with: .fade)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
