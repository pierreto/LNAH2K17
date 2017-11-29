//
//  OfflineMenuViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-10-18.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import Foundation
import UIKit

class OfflineMenuViewController: UIViewController {
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.setNavigationBarHidden(false, animated: true)
        FacadeModele.instance.changerEditorState(etat: .OFFLINE_EDITION)
    }
    
    @IBAction func openEdition(_ sender: Any) {
        if FacadeModele.instance.getAlreadySeenOfflineTutorial() {
            self.performSegue(withIdentifier: "openOfflineMaps", sender: self)
        } else {
            FacadeModele.instance.setAlreadySeenOfflineTutorial(alreadySeenOfflineTutorial: true)
            self.showTutorialPopUp()
        }
    }
    
    @IBAction func showTutorial(_ sender: Any) {
        FacadeModele.instance.setAlreadySeenOfflineTutorial(alreadySeenOfflineTutorial: true)
        self.showTutorialPopUp()
    }
    
    func showTutorialPopUp() {
        let viewController = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "TutorialViewControllerID") as! TutorialViewController
        self.addChildViewController(viewController)
        
        viewController.view.frame = self.view.frame
        self.view.addSubview(viewController.view)
        viewController.didMove(toParentViewController: self)
        
        // Désactiver la barre de navigation
        self.navigationController?.isNavigationBarHidden = true
    }
    
}
