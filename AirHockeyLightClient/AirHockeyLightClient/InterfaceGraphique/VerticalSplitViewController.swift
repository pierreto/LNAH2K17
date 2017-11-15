//
//  VerticalSplitViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-06.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

class VerticalSplitViewController: UIViewController {
    
    
    @IBOutlet weak var topViewBottomConstraint: NSLayoutConstraint!
    @IBOutlet weak var topView: UIView!
    @IBOutlet weak var chatView: UIView!
    @IBOutlet weak var chatButton: UIButton!
    @IBOutlet weak var chatButtonBottom: NSLayoutConstraint!
    @IBOutlet weak var chatBottom: NSLayoutConstraint!
    @IBOutlet weak var friendsButton: UIButton!
    @IBOutlet weak var friendsButtonBottom: NSLayoutConstraint!
    @IBOutlet weak var friendsBottom: NSLayoutConstraint!
    @IBOutlet weak var topViewBottom: NSLayoutConstraint!
    var chatOpen = false;
    var friendsOpen = false;
    
    @IBAction func toggleChat(_ sender: Any) {
        self.togChat()
    }
    @IBAction func toggleFriends(_ sender: Any) {
        self.togFriends()
    }
    @IBOutlet weak var bottomConstraint: NSLayoutConstraint!
    
    static var sharedVerticalSplitViewController = VerticalSplitViewController()
    override func viewDidLoad() {
        super.viewDidLoad()
        toggleChatButtonVisibility()
        subscribeToNotifications()
        VerticalSplitViewController.sharedVerticalSplitViewController = self;
        // TODO: Check if user is connected, if not hide the chat button
        // Do any additional setup after loading the view.
    }

    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(togChatButtonVisibility(_:)),
                                               name: NSNotification.Name(rawValue: LoginNotification.SubmitNotification),
                                               object: nil)
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(togChatButtonVisibility(_:)),
                                               name: NSNotification.Name(rawValue: LoginNotification.LogoutNotification),
                                               object: nil)
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(togChatButtonVisibility(_:)),
                                               name: NSNotification.Name(rawValue: SignupNotification.SubmitNotification),
                                               object: nil)
    }
    
    func togChat() {
        if(!chatOpen){
            chatBottom.constant = 0
            chatButtonBottom.constant = 330
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        } else {
            chatButtonBottom.constant = 0
            chatBottom.constant = -330
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        }
        chatOpen = !chatOpen
    }
    
    func togFriends() {
        if(!friendsOpen){
            friendsBottom.constant = 0
            friendsButtonBottom.constant = 330
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        } else {
            friendsButtonBottom.constant = 0
            friendsBottom.constant = -330
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        }
        friendsOpen = !friendsOpen
    }
    
    func toggleChatButtonVisibility() {
        if(HubManager.sharedConnection.getUsername() != "" && HubManager.sharedConnection.getUsername() != nil){
            chatButtonBottom.constant = 0
            friendsButtonBottom.constant = 0
            topViewBottom.constant = 0
            chatButton.layer.shadowOpacity = 0.5
            chatButton.layer.shadowRadius = 5
            friendsButton.layer.shadowOpacity = 0.5
            friendsButton.layer.shadowRadius = 5
        } else {
            chatButtonBottom.constant = -44
            friendsButtonBottom.constant = -44
            topViewBottom.constant = -44
            chatButton.layer.shadowOpacity = 0
            friendsButton.layer.shadowOpacity = 0
        }
    }
    
    func togChatButtonVisibility(_ notification: NSNotification) {
        chatOpen = true
        friendsOpen = true
        self.togChat()
        self.togFriends()
        toggleChatButtonVisibility()
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
