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
    
    /// Instance singleton
    static var instance: FriendRequestsViewController?
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        
        FriendRequestsViewController.instance = self
        
        // Initialize Tab Bar Item
        self.tabBarItem = UITabBarItem()
        self.tabBarItem.setTitleTextAttributes([NSFontAttributeName: UIFont(name: "FontAwesome", size: 30)!], for: .normal)
        self.tabBarItem.titlePositionAdjustment = UIOffset(horizontal: 0, vertical: -5)
        self.tabBarItem.title = "\u{f0f3}"
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.view.alpha = 0.2
        UIView.animate(
            withDuration: 0.5,
            animations: {
                self.view.alpha = 1.0
        })
    }
    
    func displayNotification() {
        if !FriendRequestsTableViewController.instance.isOpen {
            self.tabBarItem.badgeValue = " "
        }
    }
    
    func resetNotification() {
        self.tabBarItem.badgeValue = ""
    }

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
