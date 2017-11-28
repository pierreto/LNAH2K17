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
    
    private var filterStrings = [String]()
    private let friendsService = FriendsService()
    
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
            for user in users! {
                if user.getUsername() != activeUsername {
                    self.filterStrings.append(user.getUsername())
                }
            }
            
            self.search.filterStrings(self.filterStrings)
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
    
    func addNewAddableUser(username : String) {
        self.filterStrings.append(username)
        self.search.filterStrings(self.filterStrings)
    }
    
    @IBAction func addFriend(_ sender: Any) {
        self.friendsService.sendFriendRequest(friendUsername: self.search.text!)
        self.search.text = ""
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
