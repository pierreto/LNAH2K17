//
//  VerticalSplitViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-06.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

class VerticalSplitViewController: UIViewController {
    @IBOutlet weak var chatButtonBottomConstraint: NSLayoutConstraint!
    
    @IBOutlet weak var topView: UIView!
    @IBOutlet weak var chatView: UIView!
    @IBOutlet weak var chatButton: UIButton!
    var chatOpen = false;
    
    @IBAction func toggleChat(_ sender: Any) {
        if(!chatOpen){
            bottomConstraint.constant = 0
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        } else {
            bottomConstraint.constant = -330
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        }
        chatOpen = !chatOpen
        
    }
    @IBOutlet weak var bottomConstraint: NSLayoutConstraint!
    @IBOutlet weak var topViewBottomConstraint: NSLayoutConstraint!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        toggleChatVisibility()
        subscribeToNotifications()
        // TODO: Check if user is connected, if not hide the chat button
        // Do any additional setup after loading the view.
    }

    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(togChatVisibility(_:)),
                                               name: NSNotification.Name(rawValue: LoginNotification.SubmitNotification),
                                               object: nil)
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(togChatVisibility(_:)),
                                               name: NSNotification.Name(rawValue: LoginNotification.SubmitNotification),
                                               object: nil)
    }
    
    func toggleChatVisibility() {
        if(HubManager.sharedConnection.connected!){
            chatButtonBottomConstraint.constant = 0
            chatButton.layer.shadowOpacity = 0.5
            chatButton.layer.shadowRadius = 5
        } else {
            chatButtonBottomConstraint.constant = -44
            chatButton.layer.shadowOpacity = 0
        }
    }
    
    func togChatVisibility(_ notification: NSNotification) {
        if(HubManager.sharedConnection.connected!){
            chatButtonBottomConstraint.constant = 0
            chatButton.layer.shadowOpacity = 0.5
            chatButton.layer.shadowRadius = 5
        } else {
            chatButtonBottomConstraint.constant = -44
            chatButton.layer.shadowOpacity = 0
        }
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    

    
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    

}
