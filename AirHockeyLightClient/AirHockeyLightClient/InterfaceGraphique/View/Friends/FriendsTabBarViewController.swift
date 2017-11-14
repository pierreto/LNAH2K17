//
//  CustomUITabBarViewController.swift
//  AirHockeyLightClient
//
//  Created by Mikael Ferland on 2017-11-12.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

class FriendsTabBarViewController: UITabBarController {
    
    override func viewWillLayoutSubviews() {
        super.viewWillLayoutSubviews()
        var tabFrame:CGRect = self.tabBar.frame
        tabFrame.origin.y = self.view.frame.origin.y + 65
        self.tabBar.frame = tabFrame
    }

}
