///////////////////////////////////////////////////////////////////////////////
/// @file LostConnectionViewController.swift
/// @author Pierre To
/// @date 2017-11-24
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import Reachability

///////////////////////////////////////////////////////////////////////////
/// @class LostConnectionViewController
/// @brief Controlleur de la vue de la perte de connexion
///
/// @author Pierre To
/// @date 2017-11-24
///////////////////////////////////////////////////////////////////////////
class LostConnectionViewController: UIViewController {
    
    /// Instance singleton
    static var instance = LostConnectionViewController()
    
    // Reachability écoute les modifications de connexion du iPad
    private var reachability = Reachability()!
    
    private var timer = Timer()
    private var maxReconnectionTime = 7
    @IBOutlet weak var timerLabel: UILabel!
    @IBOutlet weak var reconnectLabel: UILabel!
    @IBOutlet weak var returnHomeButton: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        LostConnectionViewController.instance = self
        
        self.view.backgroundColor = UIColor.black.withAlphaComponent(0.8)
        self.showAnimate()
        
        self.maxReconnectionTime = 7
        self.timerLabel.text = self.maxReconnectionTime.description
        self.timer = Timer.scheduledTimer(timeInterval: 1, target: self, selector: #selector(self.tryReconnecting), userInfo: nil, repeats: true)
        self.disableUI()
        
        reachability.whenReachable = { reachability in
            if reachability.connection == .wifi {
                self.returnHomeButton.isEnabled = false
                self.reconnectLabel.text = "Reconnexion réussie"
                self.timer.invalidate()
                self.timerLabel.isHidden = true
                
                self.reconnectLabel.alpha = 0.0
                UIView.animate(withDuration: 2,
                               animations: {
                                    self.reconnectLabel.textColor = UIColor.green
                                    self.reconnectLabel.alpha = 1.0
                }, completion: {(finished: Bool) in
                    if finished {
                        self.enableUI()
                        self.removeAnimate()
                    }
                })
            }
        }
        
        do {
            try reachability.startNotifier()
        } catch {
            print("Unable to start notifier")
        }
    }
    
    @objc private func tryReconnecting() {
        self.maxReconnectionTime = self.maxReconnectionTime - 1
        self.timerLabel.text = self.maxReconnectionTime.description
        
        if self.maxReconnectionTime == 0 {
            self.timer.invalidate()
            self.returnHomeButton.isEnabled = false
            self.reconnectLabel.text = "Échec de reconnexion"
            self.reconnectLabel.alpha = 0.0
            UIView.animate(withDuration: 2,
                           animations: {
                            self.reconnectLabel.textColor = UIColor.red
                            self.reconnectLabel.alpha = 1.0
            }, completion: {(finished: Bool) in
                if finished {
                    self.removeAnimate()
                    
                    let topViewController = self.navigationController?.topViewController
                    if !(topViewController is MainViewController) {
                        self.performSegue(withIdentifier: "returnHomeSegue", sender: self)
                    }
                }
            })

        }
    }
    
    @IBAction func returnToHome(_ sender: Any) {
        self.timer.invalidate()
        self.removeAnimate()
    }
    
    /// Animation à l'ouverture
    func showAnimate() {
        self.view.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
        self.view.alpha = 0.0
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.view.alpha = 1.0
                self.view.transform = CGAffineTransform(scaleX: 1.0, y: 1)
        }
        )
    }
    
    /// Animation à la fermeture
    func removeAnimate() {
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.view.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
                self.view.alpha = 0.0
        },
            completion: {
                (finished: Bool) in
                if finished {
                    self.reachability.stopNotifier()
                    self.view.removeFromSuperview()
                }
        }
        )
    }
    
    func disableUI() {
        self.navigationController?.isNavigationBarHidden = true
        VerticalSplitViewController.sharedVerticalSplitViewController.chatButton.isEnabled = false
        VerticalSplitViewController.sharedVerticalSplitViewController.friendsButton.isEnabled = false
    }
    
    func enableUI() {
        self.navigationController?.isNavigationBarHidden = false
        VerticalSplitViewController.sharedVerticalSplitViewController.chatButton.isEnabled = true
        VerticalSplitViewController.sharedVerticalSplitViewController.friendsButton.isEnabled = true
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
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Release any cached data, images, etc that aren't in use.
    }
    
    override var prefersStatusBarHidden: Bool {
        return true
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
