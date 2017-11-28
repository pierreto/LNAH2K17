///////////////////////////////////////////////////////////////////////////////
/// @file AddFriendViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class AddFriendViewController
/// @brief Contrôleur pour l'ajout d'amis
///
/// @author Mikael Ferland
/// @date 2017-11-19
///////////////////////////////////////////////////////////////////////////
class AddFriendViewController: UIViewController {
    
    /// Instance singleton
    static var instance: AddFriendViewController?
    
    @IBOutlet weak var search: SearchTextField!
    
    @IBOutlet weak var goToProfileButton: UIButton!
    @IBAction func goToProfileAction(_ sender: Any) {
        let user = self.friendsToAdd.first(where: {$0.getUsername() == search.text})
        if user != nil {
            HubManager.sharedConnection.searchId = user?.getId()
            NotificationCenter.default.post(name: Notification.Name(rawValue: "GoToProfile"), object: self)
        }
    }
    
    
    private var filterStrings = [String]()
    private let friendsService = FriendsService()
    private var friendsToAdd = [UserEntity]()
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        
        AddFriendViewController.instance = self
        
        // Initialize Tab Bar Item
        tabBarItem = UITabBarItem()
        tabBarItem.setTitleTextAttributes([NSFontAttributeName: UIFont(name: "FontAwesome", size: 17)!], for: .normal)
        tabBarItem.titlePositionAdjustment = UIOffset(horizontal: 0, vertical: 0)
        tabBarItem.title = "\u{f234}"
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Retrieve all users
        let friendsService = FriendsService()
        friendsService.getAllUsers() { users, error in
            let activeUsername = HubManager.sharedConnection.getUsername()
            self.friendsToAdd = users!
            for user in users! {
                if user.getUsername() != activeUsername {
                    self.filterStrings.append(user.getUsername())
                }
            }
            self.search.filterStrings(self.filterStrings)
            
            // Update users which can be added as friends depending on existing friends and pending requests
            HubManager.sharedConnection.getFriendsHub().getAllPendingRequest()
            HubManager.sharedConnection.getFriendsHub().getAllFriends()

            return
        }
    }

    override func viewWillAppear(_ animated: Bool) {
        self.view.alpha = 0.7
        UIView.animate(
            withDuration: 0.4,
            animations: {
                self.view.alpha = 1.0
        })
    }
    
    func filterUserEntries(pendingRequests: [FriendRequestEntity]) {
        if self.isViewLoaded {
            let activeUsername = HubManager.sharedConnection.getUsername()
            for req in pendingRequests {
                if req.getRequestor().getUsername() == activeUsername {
                    self.filterStrings.filter{$0 != req.getFriend().getUsername()}
                }
            }
            self.search.filterStrings(self.filterStrings)
        }
    }
    
    func filterUserEntries(friends: [UserEntity]) {
        if self.isViewLoaded {
            for friend in friends {
                self.filterStrings.filter{$0 != friend.getUsername()}
            }
            
            self.search.filterStrings(self.filterStrings)
        }
    }
    
    func addNewAddableUser(username : String) {
        self.filterStrings.append(username)
        
        if self.search != nil {
            self.search.filterStrings(self.filterStrings)
        }
    }
    
    @IBAction func addFriend(_ sender: Any) {
        self.friendsService.sendFriendRequest(friendUsername: self.search.text!)
        
        var newFilterStrings = [String]()
        for user in self.filterStrings {
            if user != self.search.text! {
                newFilterStrings.append(user)
            }
        }
        
        
        self.filterStrings = newFilterStrings
        self.search.filterStrings(self.filterStrings)
    }
    
    override var shouldAutorotate: Bool {
        return true
    }
    
    override var supportedInterfaceOrientations: UIInterfaceOrientationMask {
        if UIDevice.current.userInterfaceIdiom == .phone {
            return .allButUpsideDown
        } else {
            return .all
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
