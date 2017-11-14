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
    @IBAction func disconnectAction(_ sender: Any) {
        let parameters: [String: Any] = [
            "Username" : HubManager.sharedConnection.getUsername()!
        ]
        Alamofire.request("http://" + HubManager.sharedConnection.getIpAddress()! + ":63056/api/logout", method: .post, parameters: parameters, encoding: JSONEncoding.default)
            .responseJSON { response in
                if (!(HubManager.sharedConnection.getIpAddress()?.isEmpty)!) {
                    HubManager.sharedConnection.Logout()
                    self.performSegue(withIdentifier: "disconnectSegway", sender: self)
                }
        }
        
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.setNavigationBarHidden(false, animated: true)
        FacadeModele.instance.changerEditorState(etat: .ONLINE_EDITION)
    }
}
