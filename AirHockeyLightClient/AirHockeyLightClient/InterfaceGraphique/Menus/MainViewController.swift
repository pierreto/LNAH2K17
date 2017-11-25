//
//  MainViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-10-18.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit
import Reachability

class MainViewController: UIViewController {
    // Mark: Properties
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var onlineButton: UIButton!
    @IBOutlet weak var offlineButton: UIButton!
    @IBOutlet weak var lostConnectionLabel: UILabel!
    
    // Reachability écoute les modifications de connexion du iPad
    private var reachability = Reachability()!
    
    // Mark: Functions
    override func viewDidLoad() {
        super.viewDidLoad()
        styleUI()
        
        DBManager.instance.effacerToutesCartes()
        
        reachability = Reachability()!
        
        reachability.whenReachable = { reachability in
            if reachability.connection == .wifi {
                // print("Reachable via WiFi")
                self.onlineButton.isEnabled = true
                self.lostConnectionLabel.isHidden = true
                
                let topViewController = self.navigationController?.topViewController
                if self == topViewController {
                    // Sur la page d'accueil, on ne devrait jamais être connecté
                    _ = HubManager.sharedConnection.DisconnectUser().then(execute: { response -> Void in
                        _ = HubManager.sharedConnection.StopConnection().then(execute: { response -> Void in
                            // print("déconnexion de la page d'accueil")
                            VerticalSplitViewController.sharedVerticalSplitViewController.toggleChatButtonVisibility()
                        })
                    })
                }
            }
        }
        
        reachability.whenUnreachable = { _ in
            // print("Wifi not reachable")
            self.onlineButton.isEnabled = false
            self.lostConnectionLabel.isHidden = false
            
            let topViewController = self.navigationController?.topViewController
            if self != topViewController {
                let lostConnectionVC = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "LostConnectionViewControllerID") as! LostConnectionViewController
                
                topViewController?.addChildViewController(lostConnectionVC)
                
                lostConnectionVC.view.frame = (topViewController?.view.frame)!
                topViewController?.view.addSubview(lostConnectionVC.view)
                lostConnectionVC.didMove(toParentViewController: topViewController)
            }
        }
        
        do {
            try reachability.startNotifier()
        } catch {
            print("Unable to start notifier")
        }
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.navigationController?.setNavigationBarHidden(true, animated: false)
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        self.navigationController?.setNavigationBarHidden(false, animated: true)
        VerticalSplitViewController.sharedVerticalSplitViewController.chatButton.isEnabled = true
        VerticalSplitViewController.sharedVerticalSplitViewController.friendsButton.isEnabled = true
    }
    
    fileprivate func styleUI() {
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
