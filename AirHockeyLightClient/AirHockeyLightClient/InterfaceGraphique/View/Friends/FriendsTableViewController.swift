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
    
    @IBOutlet weak var friends: UITableView!
    
    private var friendsData = [UserEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        HubManager.sharedConnection.getFriendsHub().initialize()
        
        self.friends.delegate = self
        self.friends.dataSource = self
        
        self.updateEntries()
    }
    
    func updateEntries() {
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.friends.reloadData()
        })
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
