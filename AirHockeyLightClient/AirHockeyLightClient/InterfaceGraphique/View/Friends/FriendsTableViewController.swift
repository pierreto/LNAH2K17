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
    
    private let CHAT_BUTTON_ICON = "\u{f075}"
    
    @IBOutlet weak var friends: UITableView!

    private var friendsData = [UserEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        FriendsTableViewController.instance = self
        
        self.friends.delegate = self
        self.friends.dataSource = self
    }
    
    func updateAllFriends(friends: [UserEntity]) {
        self.friendsData = friends
    
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.friends.reloadData()
        })
    }
    
    func removeFriend(exFriend: UserEntity) {
        var index = -1
        for friend in self.friendsData {
            index = index + 1
            if (friend.getId() == exFriend.getId()) {
                break
            }
        }
        
        self.friendsData.remove(at: index)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.friends.reloadData()
        })
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.friendsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = self.friends.dequeueReusableCell(withIdentifier: "Friend", for: indexPath)
        
        let usernameLabel = cell.viewWithTag(2) as! UILabel
        usernameLabel.text = self.friendsData[indexPath.row].getUsername()
        
        let chatButton = cell.viewWithTag(3) as! UIButton
        chatButton.setTitle(CHAT_BUTTON_ICON, for: .normal)
        chatButton.isHidden = true
        
        return cell
    }
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        for cell in self.tableView.visibleCells {
            let chatButton = cell.viewWithTag(3) as! UIButton
            chatButton.isHidden = true
        }
        
        let currentCell = tableView.cellForRow(at: indexPath)!
        let chatButton = currentCell.viewWithTag(3) as! UIButton
        chatButton.setTitle(CHAT_BUTTON_ICON, for: .normal)
        chatButton.isHidden = false
    }
    
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        if editingStyle == .delete {
            let exFriend = self.friendsData[indexPath.row]
            
            // remove the item from the data model
            self.friendsData.remove(at: indexPath.row)
            
            // delete the table view row
            tableView.deleteRows(at: [indexPath], with: .fade)
            
            HubManager.sharedConnection.getFriendsHub().removeFriend(exFriend: exFriend)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
