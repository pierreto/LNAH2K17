///////////////////////////////////////////////////////////////////////////////
/// @file ServerDownViewController.swift
/// @author Pierre To
/// @date 2017-11-24
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class ServerDownViewController
/// @brief Controlleur de la vue de la perte de connexion au serveur
///
/// @author Pierre To
/// @date 2017-11-24
///////////////////////////////////////////////////////////////////////////
class ServerDownViewController: UIViewController {
    
    /// Instance singleton
    static var instance = ServerDownViewController()
    
    private var timer = Timer()
    private var maxReturnHomeTimer = 5
    @IBOutlet weak var timerLabel: UILabel!
    @IBOutlet weak var messageLabel: UILabel!
    @IBOutlet weak var returnHomeButton: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        ServerDownViewController.instance = self
        
        self.view.backgroundColor = UIColor.black.withAlphaComponent(0.8)
        self.showAnimate()
        
        self.maxReturnHomeTimer = 5
        self.timerLabel.text = self.maxReturnHomeTimer.description
        self.timer = Timer.scheduledTimer(timeInterval: 1, target: self, selector: #selector(self.waitToReturnHome), userInfo: nil, repeats: true)
        self.disableUI()
    }
    
    @objc private func waitToReturnHome() {
        self.maxReturnHomeTimer = self.maxReturnHomeTimer - 1
        self.timerLabel.text = self.maxReturnHomeTimer.description
        
        // Return to home page
        if self.maxReturnHomeTimer == 0 {
            self.timer.invalidate()
            self.returnHomeButton.isEnabled = false
            self.messageLabel.alpha = 0.0
            
            UIView.animate(withDuration: 2,
                           animations: {
                            self.messageLabel.textColor = UIColor.red
                            self.messageLabel.alpha = 1.0
            }, completion: {(finished: Bool) in
                if finished {
                    self.removeAnimate()
                    
                    let topViewController = self.navigationController?.topViewController
                    if !(topViewController is MainViewController) {
                        self.performSegue(withIdentifier: "returnHomeSegueFromServerDown", sender: self)
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
                    self.view.removeFromSuperview()
                }
        }
        )
    }
    
    func disableUI() {
        self.navigationController?.isNavigationBarHidden = true
        VerticalSplitViewController.sharedVerticalSplitViewController.chatButton.isEnabled = false
        VerticalSplitViewController.sharedVerticalSplitViewController.friendsButton.isEnabled = false
        VerticalSplitViewController.sharedVerticalSplitViewController.hideAllBottomMenu()
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
