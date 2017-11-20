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
    
    private let CHECK_BUTTON_ICON = "\u{f058}"
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
    
    @IBAction func acceptRequest(_ sender: Any) {
        let buttonPosition:CGPoint = (sender as AnyObject).convert(CGPoint.init(x: 5.0, y: 5.0), to:self.tableView)
        let indexPath = self.tableView.indexPathForRow(at: buttonPosition)
        let request = self.pendingRequestsData[(indexPath?.row)!]

        self.pendingRequestsData.remove(at: (indexPath?.row)!) // remove the item from the data model
        tableView.deleteRows(at: [indexPath!], with: .fade) // delete the table view row
        
        HubManager.sharedConnection.getFriendsHub().acceptFriendRequest(request: request)
    }
    
    func addRequest(newRequest: FriendRequestEntity) {
        self.pendingRequestsData.append(newRequest)
        
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
        
        let imageView = cell.viewWithTag(1) as! UIImageView
        let profile = self.pendingRequestsData[indexPath.row].getRequestor().getProfile()
        var image: UIImage?
        if profile != "" {
            let imageData = NSData(base64Encoded: profile)
            image = UIImage(data: imageData! as Data)
        } else {
            image = UIImage(named: "default_profile_picture.png")
        }
        imageView.image = image
        
        let usernameLabel = cell.viewWithTag(2) as! UILabel
        usernameLabel.text = self.pendingRequestsData[indexPath.row].getRequestor().getUsername()
        
        let checkButton = cell.viewWithTag(3) as! UIButton
        checkButton.setTitle(CHECK_BUTTON_ICON, for: .normal)
        
        return cell
    }
    
    override func tableView(_ tableView: UITableView, titleForDeleteConfirmationButtonForRowAt indexPath: IndexPath) -> String? {
        return "Refuser"
    }
    
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        if editingStyle == .delete {
            let request = self.pendingRequestsData[indexPath.row]
            
            self.pendingRequestsData.remove(at: indexPath.row) // remove the item from the data model
            tableView.deleteRows(at: [indexPath], with: .fade) // delete the table view row
            
            HubManager.sharedConnection.getFriendsHub().refuseFriendRequest(request: request)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
