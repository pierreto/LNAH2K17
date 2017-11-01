///////////////////////////////////////////////////////////////////////////////
/// @file MapTableViewController.swift
/// @author Mikael Ferland
/// @date 2017-10-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class MapTableViewController
/// @brief Controlleur pour visualiser les cartes sauvegardées localement
///        et les cartes sauvegardées sur le serveur (si l'utilisateur est
///        authentifié)
///
/// @author Mikael Ferland
/// @date 2017-09-27
///////////////////////////////////////////////////////////////////////////
class MapTableViewController: UITableViewController {
    
    /// Instance singleton
    static var instance = MapTableViewController()
    
    @IBOutlet weak var maps: UITableView!
    
    private var mapsData = [MapEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        MapTableViewController.instance = self
        self.maps.delegate = self
        self.maps.dataSource = self
        
        self.updateEntries()
    }
    
    func updateEntries() {
        self.mapsData = DBManager.instance.recupererCartes()
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.maps.reloadData()
        })
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.mapsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = self.maps.dequeueReusableCell(withIdentifier: "Map", for: indexPath)
        
        let txtLabel = cell.viewWithTag(1) as! UILabel
        txtLabel.text = self.mapsData[indexPath.row].name
        
        return cell
    }
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let editor = storyboard?.instantiateViewController(withIdentifier: "EditorViewController") as! EditorViewController
        editor.currentMap = self.mapsData[indexPath.row]
        navigationController?.pushViewController(editor, animated: true)
    }
    
    /// Supprimer une entrée de la table après un swipe
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        
        if editingStyle == .delete {
            DBManager.instance.effacerCarte(mapName: self.mapsData[indexPath.row].name!)
            
            // remove the item from the data model
            self.mapsData.remove(at: indexPath.row)
            
            // delete the table view row
            tableView.deleteRows(at: [indexPath], with: .fade)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
