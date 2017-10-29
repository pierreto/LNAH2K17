///////////////////////////////////////////////////////////////////////////////
/// @file MapDisplayViewController.swift
/// @author Mikael Ferland
/// @date 2017-10-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class MapDisplayViewController
/// @brief Controlleur pour visualiser les cartes sauvegardées localement
///        et les cartes sauvegardées sur le serveur (si l'utilisateur est
///        authentifié)
///
/// @author Mikael Ferland
/// @date 2017-09-27
///////////////////////////////////////////////////////////////////////////
class MapDisplayViewController: UIViewController, UITableViewDelegate, UITableViewDataSource {
    
    private var mapsData = [MapEntity]()
    
    @IBOutlet weak var maps: UITableView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.maps.delegate = self
        self.maps.dataSource = self
        
        self.mapsData = DBManager.instance.recupererCartes()
        
        let defaultMap = MapEntity()
        defaultMap.name = "Default map #1"
        self.mapsData.append(defaultMap)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.maps.reloadData()
        })
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return mapsData.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = maps.dequeueReusableCell(withIdentifier: "Map", for: indexPath)
        
        let button = cell.viewWithTag(1) as! UIButton
        button.setTitle(mapsData[indexPath.row].name, for: UIControlState.normal)
        button.titleEdgeInsets.left = 20
        
        return cell
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
