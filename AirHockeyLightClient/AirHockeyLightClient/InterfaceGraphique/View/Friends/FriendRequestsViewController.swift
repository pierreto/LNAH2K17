///////////////////////////////////////////////////////////////////////////////
/// @file FriendRequestsViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-13
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class FriendRequestsViewController
/// @brief Contrôleur pour gérer les demandes d'amitié d'un utilisateur
///
/// @author Mikael Ferland
/// @date 2017-11-13
///////////////////////////////////////////////////////////////////////////
class FriendRequestsViewController: UIViewController {

    func enableNavigationBar(activer: Bool) {
        self.navigationItem.hidesBackButton = !activer
        
        for button in self.navigationItem.rightBarButtonItems! {
            button.isEnabled = activer
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
