///////////////////////////////////////////////////////////////////////////////
/// @file ChannelViewController.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-24
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class MessageViewCell
/// @brief Label personnalisé pour afficher les messages
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-27
///////////////////////////////////////////////////////////////////////////
class ChannelViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    
    private var channelsData = [ChannelEntity]()
    
    @IBOutlet weak var channels: UITableView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        channels.delegate = self;
        channels.dataSource = self;
        
        self.channelsData.append(ChannelEntity(name: "Canal public"))
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.channels.reloadData()
        })
    }
    
    /// Enclenché lorsque l'utilisateur se déconnecte
    @IBAction func deregisterClient(_ sender: Any) {
        let appDelegate = UIApplication.shared.delegate as! AppDelegate
        appDelegate.deregisterUsername(ipAddress: HubManager.sharedConnection.getIpAddress(), username: HubManager.sharedConnection.getUsername())
    }
    
    // Table view delegate methods
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return channelsData.count;
    }
    
    /// Afficher un canal dans le tableau
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = channels.dequeueReusableCell(withIdentifier: "Channel", for: indexPath);
        
        let button = cell.viewWithTag(1) as! UIButton;
        button.setTitle(channelsData[indexPath.row].getName(), for: UIControlState.normal)
        button.titleEdgeInsets.left = 20;
        
        return cell;
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
