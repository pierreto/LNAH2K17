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
    private let GO_TO_PROFILE_BUTTON_ICON = "\u{f2bd}"
    
    @IBOutlet weak var friends: UITableView!

    private var friendsData = [UserEntity]()
    
    @IBAction func goToProfileOf(_ sender: Any) {
        let buttonPosition:CGPoint = (sender as AnyObject).convert(CGPoint.init(x: 5.0, y: 5.0), to:self.tableView)
        let indexPath = self.tableView.indexPathForRow(at: buttonPosition)
        print("Go to profile of : ", friendsData[(indexPath?.row)!].getUsername())
        HubManager.sharedConnection.searchId = friendsData[(indexPath?.row)!].getId()
        NotificationCenter.default.post(name: Notification.Name(rawValue: "GoToProfile"), object: self)
    }
    
    @IBAction func startPrivateChannel(_ sender: Any) {
        let buttonPosition:CGPoint = (sender as AnyObject).convert(CGPoint.init(x: 5.0, y: 5.0), to:self.tableView)
        let indexPath = self.tableView.indexPathForRow(at: buttonPosition)
        let friend: UserEntity = friendsData[(indexPath?.row)!]
        print("send message to : ", friend.getUsername())
        MasterViewController.sharedMasterViewController.addPrivateChannel(othersName: friend.getUsername(), othersId: friend.getId(), othersProfile: friend.getProfile())
    }
    
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
    
    func addFriend(newFriend: UserEntity) {
        self.friendsData.append(newFriend)
        
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
                self.friendsData.remove(at: index)
                break
            }
        }
        
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
        
        cell.backgroundColor = UIColor(red:0.24, green:0.24, blue:0.24, alpha:1.0)
        
        let imageView = cell.viewWithTag(1) as! UIImageView
        let profile = self.friendsData[indexPath.row].getProfile()
        var image: UIImage?
        if profile != "" {
            let imageData = NSData(base64Encoded: profile)
            image = UIImage(data: imageData! as Data)
        } else {
            image = UIImage(named: "default_profile_picture.png")
        }
        imageView.image = image
        
        let usernameLabel = cell.viewWithTag(2) as! UILabel
        usernameLabel.text = self.friendsData[indexPath.row].getUsername()
        
        let chatButton = cell.viewWithTag(3) as! UIButton
        chatButton.setTitle(CHAT_BUTTON_ICON, for: .normal)
        chatButton.isHidden = true
        
        let profileButton = cell.viewWithTag(4) as! UIButton
        profileButton.setTitle(GO_TO_PROFILE_BUTTON_ICON, for: .normal)
        profileButton.isHidden = true
        
        let bgColorView = UIView()
        bgColorView.backgroundColor = UIColor(red:0.29, green:0.29, blue:0.29, alpha:1.0)
        cell.selectedBackgroundView = bgColorView
        
        return cell
    }
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
    
        for cell in self.tableView.visibleCells {
            let chatButton = cell.viewWithTag(3) as! UIButton
            chatButton.isHidden = true
            let profileButton = cell.viewWithTag(4) as! UIButton
            profileButton.isHidden = true
        }
        
        let currentCell = tableView.cellForRow(at: indexPath)!
        
        let chatButton = currentCell.viewWithTag(3) as! UIButton
        chatButton.isHidden = false
        let profileButton = currentCell.viewWithTag(4) as! UIButton
        profileButton.isHidden = false
        
        let circle = currentCell.viewWithTag(5) as! UIView
        circle.backgroundColor = UIColor(red: 0.235, green: 0.8, blue: 0.208, alpha: 1.0)
        
        let innerCircle = currentCell.viewWithTag(6) as! UIView
        innerCircle.backgroundColor = .white
    }
    
    override func tableView(_ tableView: UITableView, titleForDeleteConfirmationButtonForRowAt indexPath: IndexPath) -> String? {
        return "Supprimer"
    }
    
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        if editingStyle == .delete {
            let exFriend = self.friendsData[indexPath.row]
            
            self.friendsData.remove(at: indexPath.row) // remove the item from the data model
            tableView.deleteRows(at: [indexPath], with: .fade) // delete the table view row
            
            HubManager.sharedConnection.getFriendsHub().removeFriend(exFriend: exFriend)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
