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
    static var instance: FriendRequestsTableViewController?
    
    @IBOutlet weak var pendingRequests: UITableView!
    
    public var isLoaded = false
    public var isOpen = false
    private let CHECK_BUTTON_ICON = "\u{f058}"
    private var notificationCount = 0
    private var friendsHub: FriendsHub?
    private var pendingRequestsData = [FriendRequestEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        FriendRequestsTableViewController.instance = self
        
        self.isLoaded = true
        
        self.pendingRequests.delegate = self
        self.pendingRequests.dataSource = self
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.isOpen = true
        FriendRequestsViewController.instance?.resetNotification()
        
        friendsHub = HubManager.sharedConnection.getFriendsHub()
        friendsHub?.getAllPendingRequest()
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        self.isOpen = false
    }
    
    func resetFriendRequests() {
        if self.pendingRequestsData != nil && self.pendingRequests != nil {
            self.pendingRequestsData = [FriendRequestEntity]()
        
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.pendingRequests.reloadData()
            })
        }
    }
    
    func updatePendingRequestsEntries(pendingRequests:  [FriendRequestEntity]) {
        if self.isLoaded {
            self.pendingRequestsData = pendingRequests
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.pendingRequests.reloadData()
            })
        }
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
        if newRequest.getStatus() == RequestStatus.PENDING {
            if self.isOpen == false {
                FriendRequestsViewController.instance?.displayNotification()
            }
            self.pendingRequestsData.append(newRequest)
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.pendingRequests.reloadData()
            })
        }
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.pendingRequestsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = self.pendingRequests.dequeueReusableCell(withIdentifier: "Request", for: indexPath)
        
        cell.backgroundColor = UIColor(red:0.24, green:0.24, blue:0.24, alpha:1.0)
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
        
        let bgColorView = UIView()
        bgColorView.backgroundColor = UIColor(red:0.29, green:0.29, blue:0.29, alpha:1.0)
        cell.selectedBackgroundView = bgColorView
        
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
