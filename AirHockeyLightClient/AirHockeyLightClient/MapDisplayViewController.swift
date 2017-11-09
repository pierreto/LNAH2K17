///////////////////////////////////////////////////////////////////////////////
/// @file MapDisplayViewController.swift
/// @author Mikael Ferland
/// @date 2017-10-29
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import Alamofire
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class MapDisplayViewController
/// @brief Contrôleur de la sélection des cartes
///
/// @author Mikael Ferland
/// @date 2017-10-29
///////////////////////////////////////////////////////////////////////////
class MapDisplayViewController: UIViewController {
    
    /// Instance singleton
    static var instance = MapDisplayViewController()
    
    private let clientConnection = HubManager.sharedConnection
    
    public var currentMap: MapEntity?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        MapDisplayViewController.instance = self
        
        let addMapBtn = UIBarButtonItem(barButtonSystemItem: .add, target: self, action: #selector(addMapBtnClicked))
        self.navigationItem.setRightBarButtonItems([addMapBtn], animated: true)
    }
    
    // Ouvrir le pop-up pour la création de cartes
    func addMapBtnClicked(sender: AnyObject)
    {
        let createMapVC = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "CreateMapViewController") as! CreateMapViewController
        self.addChildViewController(createMapVC)
        
        createMapVC.view.frame = self.view.frame
        self.view.addSubview(createMapVC.view)
        createMapVC.didMove(toParentViewController: self)
        
        // Désactiver la barre de navigation
        self.enableNavigationBar(activer: false)
    }
    
    func handleTableSelection(map: MapEntity) {
        self.currentMap = map
        
        // Ouvrir le pop-up pour déverrouiller une carte
        if self.currentMap?.privacy.value == true {
            let unlockMapVC = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "UnlockMapViewController") as! UnlockMapViewController
            self.addChildViewController(unlockMapVC)
            
            unlockMapVC.view.frame = self.view.frame
            self.view.addSubview(unlockMapVC.view)
            unlockMapVC.didMove(toParentViewController: self)
            
            // Désactiver la barre de navigation
            self.enableNavigationBar(activer: false)
        } else {
            print("okok")
        }
    }
    
    func openEditor() {
        let editor = storyboard?.instantiateViewController(withIdentifier: "EditorViewController") as! EditorViewController

        // Fetch map json
        // TODO : bug création de map mode hors-ligne puis aller dans mode en ligne, sélectionner map
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            let mapId = self.currentMap?.id.value
            
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
            editor.currentMap = self.currentMap
            FacadeModele.instance.obtenirEtatEdition().joinEdition(mapEntity: editor.currentMap!)
            
            self.navigationController?.pushViewController(editor, animated: true)
        }
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
    
    func enableNavigationBar(activer: Bool) {
        self.navigationItem.hidesBackButton = !activer
        
        for button in self.navigationItem.rightBarButtonItems! {
            button.isEnabled = activer
        }
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
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
