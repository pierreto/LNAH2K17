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
class MapDisplayViewController: UITableViewController {
    
    private var mapsData = [MapEntity]()
    
    //@IBOutlet weak var maps: UITableView!
    @IBOutlet weak var maps: UITableView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.maps.delegate = self
        self.maps.dataSource = self
        
        let defaultMap = MapEntity()
        defaultMap.name = "Default map #1"
        self.mapsData.append(defaultMap)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.maps.reloadData()
        })
        
        let addMapBtn = UIBarButtonItem(barButtonSystemItem: .add, target: self, action: #selector(addMapBtnClicked))
        self.navigationItem.setRightBarButtonItems([addMapBtn], animated: true)
    }
    
    func addMapBtnClicked(sender: AnyObject)
    {
        print("create map")
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return mapsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = maps.dequeueReusableCell(withIdentifier: "Map", for: indexPath)
        
        let txtLabel = cell.viewWithTag(1) as! UILabel
        txtLabel.text = mapsData[indexPath.row].name
        
        return cell
    }
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let editor = storyboard?.instantiateViewController(withIdentifier: "EditorViewController") as! EditorViewController
        editor.currentMap = self.mapsData[indexPath.row]
        navigationController?.pushViewController(editor, animated: true)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
