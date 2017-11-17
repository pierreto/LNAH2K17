//
//  VerticalSplitViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-06.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

class VerticalSplitViewController: UIViewController {
    
    // Mark: Properties
    @IBOutlet weak var topViewBottomConstraint: NSLayoutConstraint!
    @IBOutlet weak var topView: UIView!
    @IBOutlet weak var chatView: UIView!
    @IBOutlet weak var chatButton: UIButton!
    @IBOutlet weak var chatButtonBottom: NSLayoutConstraint!
    @IBOutlet weak var chatBottom: NSLayoutConstraint!
    @IBOutlet weak var friendsView: UIView!
    @IBOutlet weak var friendsButton: UIButton!
    @IBOutlet weak var friendsButtonBottom: NSLayoutConstraint!
    @IBOutlet weak var friendsBottom: NSLayoutConstraint!
    @IBOutlet weak var topViewBottom: NSLayoutConstraint!
    @IBOutlet weak var bottomConstraint: NSLayoutConstraint!
    
    var chatOpen = false
    var friendsOpen = false
    var screenHeight = CGFloat(0)

    static var sharedVerticalSplitViewController = VerticalSplitViewController()
    
    //Mark: Actions
    @IBAction func toggleChat(_ sender: Any) {
        self.togChat()
    }
    @IBAction func toggleFriends(_ sender: Any) {
        self.togFriends()
    }
    
    
    //Mark : functions
    override func viewDidLoad() {
        super.viewDidLoad()
        toggleChatButtonVisibility()
        subscribeToNotifications()
        VerticalSplitViewController.sharedVerticalSplitViewController = self;
        let screenSize = UIScreen.main.bounds
        self.screenHeight = screenSize.height
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
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillHide, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(adjustForKeyboard), name:NSNotification.Name.UIKeyboardWillShow, object: nil)
    }
    
    func adjustForKeyboard(notification: Notification) {
        //Check if online mode
        if(HubManager.sharedConnection.getUsername() != "" && HubManager.sharedConnection.getUsername() != nil) {
            let userInfo = notification.userInfo!
            
            let keyboardScreenEndFrame = (userInfo[UIKeyboardFrameEndUserInfoKey] as! NSValue).cgRectValue
            let keyboardViewEndFrame = view.convert(keyboardScreenEndFrame, from: view.window)
            
            //Move the view up or down depending on if keyboard is already open or not or if it should close
            if(chatOpen){
                if notification.name == Notification.Name.UIKeyboardWillHide {
                    if (chatView.frame.minY + chatView.frame.height != screenHeight) {
                        chatView.frame.origin.y += keyboardViewEndFrame.height
                        chatButton.frame.origin.y += keyboardViewEndFrame.height
                    }
                } else if notification.name == Notification.Name.UIKeyboardWillShow {
                    if (chatView.frame.minY + chatView.frame.height == screenHeight) {
                        chatView.frame.origin.y -= keyboardViewEndFrame.height
                        chatButton.frame.origin.y -= keyboardViewEndFrame.height
                    }
                }
            }
            if(friendsOpen){
                if notification.name == Notification.Name.UIKeyboardWillHide {
                    if (friendsView.frame.minY + friendsView.frame.height != screenHeight) {
                        friendsView.frame.origin.y += keyboardViewEndFrame.height
                        friendsButton.frame.origin.y += keyboardViewEndFrame.height
                    }
                } else if notification.name == Notification.Name.UIKeyboardWillShow {
                    if (friendsView.frame.minY + friendsView.frame.height == screenHeight) {
                        friendsView.frame.origin.y -= keyboardViewEndFrame.height
                        friendsButton.frame.origin.y -= keyboardViewEndFrame.height
                    }
                }
            }
        }
    }
    
    func togChat() {
        if(!chatOpen){
            chatBottom.constant = 0
            chatButtonBottom.constant = 300
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        } else {
            chatView.endEditing(true)
            chatButtonBottom.constant = 0
            chatBottom.constant = -300
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        }
        chatOpen = !chatOpen
    }
    
    func togFriends() {
        if(!friendsOpen){
            friendsBottom.constant = 0
            friendsButtonBottom.constant = 300
            UIView.animate(withDuration: 0.5, animations: {self.view.layoutIfNeeded()})
        } else {
            friendsView.endEditing(true)
            friendsButtonBottom.constant = 0
            friendsBottom.constant = -300
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

    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    

}
