//
//  ChannelViewController.swift
//  AirHockeyLightClient
//
//  Created by Mikael Ferland and Pierre To on 17-09-24.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//


import UIKit

struct Channel {
    let name: String!
}

class ChannelViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    
    @IBOutlet weak var channels: UITableView!
    var channelsData = [Channel]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        channels.delegate = self;
        channels.dataSource = self;
        
        self.channelsData.append(Channel(name: "Canal public"))
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.channels.reloadData()
        })

    }
    
    // Table view delegate methods
    /*func numberOfSectionsInTableView(tableView: UITableView) -> Int {
     return 1
     }*/
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return channelsData.count;
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = channels.dequeueReusableCell(withIdentifier: "Channel", for: indexPath);
        
        let button = cell.viewWithTag(1) as! UIButton;
        button.setTitle(channelsData[indexPath.row].name, for: UIControlState.normal)
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
