///////////////////////////////////////////////////////////////////////////////
/// @file MapDisplayViewController.swift
/// @author Mikael Ferland
/// @date 2017-10-29
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class MapDisplayViewController
/// @brief Contrôleur de la sélection des cartes
///
/// @author Mikael Ferland
/// @date 2017-10-29
///////////////////////////////////////////////////////////////////////////
class MapDisplayViewController: UIViewController {
    
    /// Instance singleton
    static var instance = MapDisplayViewController()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        MapDisplayViewController.instance = self
        
        let addMapBtn = UIBarButtonItem(barButtonSystemItem: .add, target: self, action: #selector(addMapBtnClicked))
        self.navigationItem.setRightBarButtonItems([addMapBtn], animated: true)
    }
    
    // Ouvrir le pop-up pour la création de cartes
    func addMapBtnClicked(sender: AnyObject)
    {
        let createMapVC = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "CreateMapViewController") as! CreateMapViewController
        self.addChildViewController(createMapVC)
        
        createMapVC.view.frame = self.view.frame
        self.view.addSubview(createMapVC.view)
        createMapVC.didMove(toParentViewController: self)
        
        // Désactiver la barre de navigation
        self.enableNavigationBar(activer: false)
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
