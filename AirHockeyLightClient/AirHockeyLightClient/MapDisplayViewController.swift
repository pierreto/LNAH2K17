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
    
    @IBOutlet weak var loadingSpinner: UIActivityIndicatorView!
    
    public var currentMap: MapEntity?
    
    private let clientConnection = HubManager.sharedConnection
    private var isDeleting = false
    private var addMapBtn: UIBarButtonItem?
    private var deleteMapBtn: UIBarButtonItem?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        MapDisplayViewController.instance = self
        
        self.addMapBtn = UIBarButtonItem(barButtonSystemItem: .add, target: self, action: #selector(addMapBtnClicked))
        self.deleteMapBtn = UIBarButtonItem(barButtonSystemItem: .trash, target: self, action: #selector(deleteMapBtnClicked))
        self.deleteMapBtn?.isEnabled = false
        self.navigationItem.setRightBarButtonItems([addMapBtn!, deleteMapBtn!], animated: true)
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
    
    func updateEntries() {
        MapCarouselViewController.instance.updateEntries()
        
        if MapCarouselViewController.instance.numberOfItems() > 0 {
            self.deleteMapBtn?.isEnabled = true
        }
    }
    
    func deleteMapBtnClicked(sender: AnyObject)
    {
        self.currentMap = MapCarouselViewController.instance.getCurrentMap()
        self.isDeleting = true
            
        // Ouvrir le pop-up pour déverrouiller une carte
        if self.currentMap?.privacy.value == true {
            self.openUnlockMapVC()
        } else {
            MapCarouselViewController.instance.deleteCurrentMap()
            self.isDeleting = false
        }
        
        if MapCarouselViewController.instance.numberOfItems() == 0 {
            self.deleteMapBtn?.isEnabled = false
        }
    }
    
    func handleTableSelection() {
        if !self.loadingSpinner.isAnimating {
            self.currentMap = MapCarouselViewController.instance.getCurrentMap()
        
            // Ouvrir le pop-up pour déverrouiller une carte
            if self.currentMap?.privacy.value == true {
                self.openUnlockMapVC()
            } else {
                self.openEditor()
            }
        }
    }
    
    func handleCarouselDidEndScrolling() {
        self.currentMap = MapCarouselViewController.instance.getCurrentMap()
        
        var username: String?
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            username = self.clientConnection.getUsername()
        } else {
            username = "N/A"
        }
        
        self.deleteMapBtn?.isEnabled = self.currentMap?.creator == username
    }
    
    func openEditor() {
        self.loading()
        
        let editor = storyboard?.instantiateViewController(withIdentifier: "EditorViewController") as! EditorViewController

        // Fetch map json
        // TODO : bug création de map mode hors-ligne puis aller dans mode en ligne, sélectionner map
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            let mapId = self.currentMap?.id
            
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps/get/" + mapId!, method: .get, parameters: nil, encoding: JSONEncoding.default)
                .responseJSON { response in
                    if(response.response?.statusCode == 200) {
                        print("Succeed to fetch map with id from server")
                        if (response.result.value != nil) {
                            let maps = JSON(response.result.value!)
                            let mapService = MapService()
                            editor.currentMap = mapService.buildMapEntity(json: maps)
                            
                            // Rejoindre la salle d'édition
                            // TODO : A enlever quand toutes les commandes du mode en ligne seront faites
                            FacadeModele.instance.changerEditorState(etat: .ONLINE_EDITION)
                            
                            FacadeModele.instance.obtenirEtatEdition().joinEdition(mapEntity: editor.currentMap!)
                            
                            self.navigationController?.pushViewController(editor, animated: true)
                            self.loadingDone()
                        }
                    } else {
                        print("Failed to fetch map with id from server")
                    }
            }
        }
        else {
            editor.currentMap = self.currentMap
            FacadeModele.instance.obtenirEtatEdition().joinEdition(mapEntity: editor.currentMap!)
            
            self.navigationController?.pushViewController(editor, animated: true)
            self.loadingDone()
        }
    }
    
    private func openUnlockMapVC() {
        let unlockMapVC = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "UnlockMapViewController") as! UnlockMapViewController
        self.addChildViewController(unlockMapVC)
        
        unlockMapVC.view.frame = self.view.frame
        self.view.addSubview(unlockMapVC.view)
        unlockMapVC.didMove(toParentViewController: self)
        
        // Désactiver la barre de navigation
        self.enableNavigationBar(activer: false)
    }
    
    func closeUnlockMapVC() {
        if isDeleting {
            MapCarouselViewController.instance.deleteCurrentMap()
            self.isDeleting = false
            
            if MapCarouselViewController.instance.numberOfItems() == 0 {
                self.deleteMapBtn?.isEnabled = false
            }
        } else {
            self.openEditor()
        }
    }
    
    private func loading() {
        self.loadingSpinner.startAnimating()
        self.view.alpha = 0.7
        self.disableInputs()
    }
    
    private func loadingDone() {
        self.loadingSpinner.stopAnimating()
        self.view.alpha = 1.0
        self.enableInputs()
    }
    
    private func disableInputs() {
        self.addMapBtn?.isEnabled = false
        self.deleteMapBtn?.isEnabled = false
        self.enableNavigationBar(activer: false)
    }
    
    private func enableInputs() {
        self.addMapBtn?.isEnabled = true
        self.deleteMapBtn?.isEnabled = true
        self.enableNavigationBar(activer: true)
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
