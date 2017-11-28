///////////////////////////////////////////////////////////////////////////////
/// @file FriendsViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class FriendsViewController
/// @brief Contrôleur de la liste d'amis d'un utilisateur
///
/// @author Mikael Ferland
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class FriendsViewController: UIViewController {
    
    override func viewDidLoad() {
        super.viewDidLoad()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.view.alpha = 0.7
        UIView.animate(
            withDuration: 0.4,
            animations: {
                self.view.alpha = 1.0
        })
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        
        // Initialize Tab Bar Item
        tabBarItem = UITabBarItem()
        tabBarItem.setTitleTextAttributes([NSFontAttributeName: UIFont(name: "FontAwesome", size: 17)!], for: .normal)
        tabBarItem.titlePositionAdjustment = UIOffset(horizontal: 0, vertical: -12)
        tabBarItem.title = "\u{f0c0}"
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
