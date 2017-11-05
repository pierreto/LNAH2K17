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
import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class OnlineMenuViewController
/// @brief Controlleur de la vue du menu principale en ligne
///
/// @author Pierre To
/// @date 2017-11-05
///////////////////////////////////////////////////////////////////////////
class OnlineMenuViewController: UIViewController {
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.setNavigationBarHidden(false, animated: true)
        FacadeModele.instance.changerEditorState(etat: .ONLINE_EDITION)
    }
}
