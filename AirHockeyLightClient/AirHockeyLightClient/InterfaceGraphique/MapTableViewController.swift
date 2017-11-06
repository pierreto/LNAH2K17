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
import Alamofire
import SwiftyJSON

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
    
    private let clientConnection = HubManager.sharedConnection
    
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
        // Fetch server maps
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps",
                              method: .get, parameters: nil, encoding: JSONEncoding.default)
                .responseJSON { response in
                    if(response.response?.statusCode == 200) {
                        print("Succeed to fetch maps from server")
                        let maps = JSON(response.result.value!)
                        for map in maps {
                            self.mapsData.append(self.buildMapEntity(json: map.1))
                        }
                        
                        DispatchQueue.main.async(execute: { () -> Void in
                            // Reload tableView
                            self.maps.reloadData()
                        })
                    } else {
                        print("Failed to fetch maps from server")
                    }
            }
        }
        
        self.mapsData = DBManager.instance.recupererCartes()
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.maps.reloadData()
        })
    }
    
    // TODO : bouger ceci dans un service
    func buildMapEntity(json: JSON) -> MapEntity {
        let mapEntity = MapEntity()
        mapEntity.id.value = json["Id"].int
        mapEntity.creator = json["Creator"].string
        mapEntity.mapName = json["MapName"].string
        // TODO : Fuseau horaire différent
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
        let date = dateFormatter.date(from: json["LastBackup"].string!)
        mapEntity.lastBackup = date
        mapEntity.json = json["Json"].string
        mapEntity.privacy.value = json["Private"].bool
        mapEntity.password = json["Password"].string
        mapEntity.currentNumberOfPlayer.value = json["CurrentNumberOfPlayer"].int
        return mapEntity
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.mapsData.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = self.maps.dequeueReusableCell(withIdentifier: "Map", for: indexPath)
        
        let txtLabel = cell.viewWithTag(1) as! UILabel
        txtLabel.text = self.mapsData[indexPath.row].mapName
        
        return cell
    }
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let editor = storyboard?.instantiateViewController(withIdentifier: "EditorViewController") as! EditorViewController
        
        // Fetch map json
        // TODO : bug création de map mode hors-ligne puis aller dans mode en ligne, sélectionner map
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            let mapId = self.mapsData[indexPath.row].id.value
            
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps/get/" + (mapId?.description)!, method: .get, parameters: nil, encoding: JSONEncoding.default)
                .responseJSON { response in
                    if(response.response?.statusCode == 200) {
                        print("Succeed to fetch map with id from server")
                        let maps = JSON(response.result.value!)
                        editor.currentMap = self.buildMapEntity(json: maps)
                        
                        // Rejoindre la salle d'édition
                        // TODO : A enlever quand toutes les commandes du mode en ligne seront faites
                        FacadeModele.instance.changerEditorState(etat: .ONLINE_EDITION)
                        
                        FacadeModele.instance.obtenirEtatEdition().joinEdition(mapEntity: editor.currentMap!)
                        
                        self.navigationController?.pushViewController(editor, animated: true)
                    } else {
                        print("Failed to fetch map with id from server")
                    }
            }
        }
        else {
            editor.currentMap = self.mapsData[indexPath.row]
            FacadeModele.instance.obtenirEtatEdition().joinEdition(mapEntity: editor.currentMap!)
            
            self.navigationController?.pushViewController(editor, animated: true)
        }
    }
    
    /// Supprimer une entrée de la table après un swipe
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        
        if editingStyle == .delete {
            DBManager.instance.effacerCarte(mapName: self.mapsData[indexPath.row].mapName!)
            
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
