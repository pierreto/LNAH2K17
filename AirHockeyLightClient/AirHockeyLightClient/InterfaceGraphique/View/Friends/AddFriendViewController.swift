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
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        let friendsService = FriendsService()
        friendsService.getAllUsers() { users, error in
            return
        }
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
