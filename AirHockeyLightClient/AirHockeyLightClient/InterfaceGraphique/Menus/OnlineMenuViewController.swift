///////////////////////////////////////////////////////////////////////////////
/// @file OnlineMenuViewController.swift
/// @author Pierre To
/// @date 2017-09-23
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation
import Alamofire
import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class OnlineMenuViewController
/// @brief Controlleur de la vue du menu principale en ligne
///
/// @author Pierre To
/// @date 2017-11-05
///////////////////////////////////////////////////////////////////////////
class OnlineMenuViewController: UIViewController {
    
    @IBOutlet weak var username: UILabel!
    @IBOutlet weak var loadingSpinner: UIActivityIndicatorView!
    @IBOutlet weak var navigationBar: UINavigationItem!
    
    @IBAction func disconnectAction(_ sender: Any) {
        self.loading()
        
        _ = HubManager.sharedConnection.DisconnectUser().then(execute: { response -> Void in
            _ = HubManager.sharedConnection.StopConnection().then(execute: { response -> Void in
                self.loadingDone()
                self.performSegue(withIdentifier: "disconnectSegue", sender: self)
            })
        })
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
       
       self.username.text = HubManager.sharedConnection.getUsername()
       FacadeModele.instance.changerEditorState(etat: .ONLINE_EDITION)
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(goToProfile(_:)),
                                               name: NSNotification.Name(rawValue: "GoToProfile"),
                                               object: nil)
        
    }
    
    @objc fileprivate func goToProfile(_ notification: NSNotification){
        self.performSegue(withIdentifier: "goToProfile", sender: self)
    }
    
    private func loading() {
        self.loadingSpinner.startAnimating()
        self.view.alpha = 0.7
        self.view.isUserInteractionEnabled = false
        self.navigationBar.hidesBackButton = true
    }
    
    private func loadingDone() {
        self.loadingSpinner.stopAnimating()
        self.view.alpha = 1.0
        self.view.isUserInteractionEnabled = true
        self.navigationBar.hidesBackButton = false
    }
}
