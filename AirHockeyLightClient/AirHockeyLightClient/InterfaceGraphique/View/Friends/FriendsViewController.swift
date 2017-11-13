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
        
        // let addMapBtn = UIBarButtonItem(barButtonSystemItem: .add, target: self, action: #selector(addMapBtnClicked))
        // self.navigationItem.setRightBarButtonItems([addMapBtn], animated: true)
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
