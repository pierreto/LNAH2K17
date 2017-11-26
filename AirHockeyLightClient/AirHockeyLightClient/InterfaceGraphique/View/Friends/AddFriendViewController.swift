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
    
    @IBOutlet weak var search: SearchTextField!
    
    private var filterStrings = [String]()
    private let friendsService = FriendsService()
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        
        // Initialize Tab Bar Item
        tabBarItem = UITabBarItem()
        tabBarItem.setTitleTextAttributes([NSFontAttributeName: UIFont(name: "FontAwesome", size: 17)!], for: .normal)
        tabBarItem.titlePositionAdjustment = UIOffset(horizontal: 0, vertical: 0)
        tabBarItem.title = "\u{f234}"
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        let friendsService = FriendsService()
        friendsService.getAllUsers() { users, error in
            for user in users! {
                self.filterStrings.append(user.getUsername())
            }

            self.search.filterStrings(self.filterStrings)
            return
        }
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
