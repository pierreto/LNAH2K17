///////////////////////////////////////////////////////////////////////////////
/// @file MapCarouselViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-20
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class MapCarouselViewController
/// @brief Contrôleur permettant d'afficher dynamiquement les cartes
///
/// @author Mikael Ferland
/// @date 2017-11-20
///////////////////////////////////////////////////////////////////////////
class MapCarouselViewController: UIViewController, iCarouselDataSource, iCarouselDelegate {
    
    /// Instance singleton
    static var instance = MapCarouselViewController()
    
    @IBOutlet var carouselView: iCarousel!
    
    private let LOCK_BUTTON_ICON = "\u{f023}"
    private var maps = [MapEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        MapCarouselViewController.instance = self
        self.carouselView.type = .coverFlow
        self.updateEntries()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.carouselView.reloadData()
        })
    }
    
    func getCurrentMap() ->  MapEntity {
        return self.maps[self.carouselView.currentItemIndex]
    }
    
    func updateEntries() {
        // Fetch server maps
        let mapService = MapService()
        mapService.getMaps() { maps, error in
            if maps != nil {
                for map in maps! {
                    self.maps.append(mapService.buildMapEntity(json: map.1))
                }
            }
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload tableView
                self.carouselView.reloadData()
            })
            
            return
        }
        
        self.maps = DBManager.instance.recupererCartes()
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.carouselView.reloadData()
        })
    }
    
    func numberOfItems() -> Int {
        return maps.count
    }
    
    func numberOfItems(in carousel: iCarousel) -> Int {
        return maps.count
    }
    
    func carousel(_ carousel: iCarousel, viewForItemAt index: Int, reusing view: UIView?) -> UIView {
        let map = self.maps[index]
        
        let tempView = UIView(frame: CGRect(x: 0, y: 0, width: 400, height: 200))
        tempView.backgroundColor = UIColor.init(red: 232, green: 232, blue: 232, alpha: 1)
        
        let mapInfo = UIView(frame: CGRect(x: 200, y: 0, width: 200, height: 200))
        
        let mapNameLabel = UILabel()
        mapNameLabel.text = map.mapName
        mapNameLabel.font = UIFont.boldSystemFont(ofSize: 30)
        mapNameLabel.textAlignment = NSTextAlignment.right
        mapNameLabel.numberOfLines = 1
        mapNameLabel.textColor = UIColor.black
        mapNameLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let numberOfPlayersLabel = UILabel()
        numberOfPlayersLabel.text = (map.currentNumberOfPlayer.value?.description)! + "/4"
        numberOfPlayersLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 15.0)
        numberOfPlayersLabel.textAlignment = NSTextAlignment.right
        numberOfPlayersLabel.numberOfLines = 1
        numberOfPlayersLabel.textColor = UIColor.black
        numberOfPlayersLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let creatorLabel = UILabel()
        creatorLabel.text = map.creator
        creatorLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 15.0)
        creatorLabel.textAlignment = NSTextAlignment.right
        creatorLabel.numberOfLines = 1
        creatorLabel.textColor = UIColor.black
        creatorLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let idLabel = UILabel()
        if map.id == nil {
            idLabel.text = "(N/A)"
        } else {
            idLabel.text = "(" + map.id! + ")"
        }
        idLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 10.0)
        idLabel.textAlignment = NSTextAlignment.right
        idLabel.numberOfLines = 1
        idLabel.textColor = UIColor.black
        idLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let button = UIButton(frame: CGRect(x: 0, y: 0, width: 400, height: 200))
        button.backgroundColor = UIColor.lightGray
        button.addTarget(self, action:#selector(handleRegister(sender:)), for: .touchUpInside)
        
        tempView.addSubview(mapInfo)
        tempView.addSubview(button)
        tempView.addSubview(mapNameLabel)
        tempView.addSubview(numberOfPlayersLabel)
        tempView.addSubview(creatorLabel)
        tempView.addSubview(idLabel)
 
        mapNameLabel.centerXAnchor.constraint(equalTo: mapInfo.centerXAnchor).isActive = true
        mapNameLabel.centerYAnchor.constraint(equalTo: mapInfo.centerYAnchor).isActive = true
        
        numberOfPlayersLabel.bottomAnchor.constraint(equalTo: mapInfo.bottomAnchor, constant: -15).isActive = true
        numberOfPlayersLabel.leftAnchor.constraint(equalTo: mapInfo.leftAnchor, constant: 15).isActive = true
        
        creatorLabel.topAnchor.constraint(equalTo: mapInfo.topAnchor, constant: 15).isActive = true
        creatorLabel.rightAnchor.constraint(equalTo: mapInfo.rightAnchor, constant: -15).isActive = true
        
        idLabel.centerXAnchor.constraint(equalTo: mapInfo.centerXAnchor).isActive = true
        idLabel.topAnchor.constraint(equalTo: mapInfo.topAnchor, constant: 115).isActive = true
        
        if map.privacy.value == true {
            let isPublicLabel = UILabel()
            isPublicLabel.text = self.LOCK_BUTTON_ICON
            isPublicLabel.font = UIFont(name:"FontAwesome", size: 15.0)
            isPublicLabel.textAlignment = NSTextAlignment.right
            isPublicLabel.numberOfLines = 1
            isPublicLabel.textColor = UIColor.black
            isPublicLabel.translatesAutoresizingMaskIntoConstraints = false
            
            tempView.addSubview(isPublicLabel)
            isPublicLabel.bottomAnchor.constraint(equalTo: mapInfo.bottomAnchor, constant: -15).isActive = true
            isPublicLabel.rightAnchor.constraint(equalTo: mapInfo.rightAnchor, constant: -15).isActive = true
        } else {
            let mapImageView = UIImageView(frame: CGRect(x: 10, y: 10, width: 180, height: 180))
            mapImageView.image = (map.icon != nil && map.icon != "") ? ImageService.convertStrBase64ToImage(strBase64: map.icon!) : UIImage(named: "map.png")
            tempView.addSubview(mapImageView)
        }
        
        return tempView
    }
    
    func handleRegister(sender: UIButton) {
        let parent = self.parent as! MapDisplayViewController
        parent.handleTableSelection()
    }
    
    func carouselDidEndScrollingAnimation(_ carousel: iCarousel) {
        let parent = self.parent as! MapDisplayViewController
        parent.handleCarouselDidEndScrolling()
    }
    
    func carousel(_ carousel: iCarousel, valueFor option: iCarouselOption, withDefault value: CGFloat) -> CGFloat {
        if (option == .spacing) {
            return value * 1.1
        }
        return value
    }
    
    func deleteCurrentMap() {
        let mapService = MapService()
        mapService.deleteMap(map: self.maps[self.carouselView.currentItemIndex], completionHandler: { success, error in
            if success! {
                self.maps.remove(at: self.carouselView.currentItemIndex) // remove the item from the data model
                    
                DispatchQueue.main.async(execute: { () -> Void in
                    // Reload tableView
                    self.carouselView.reloadData()
                })
            }
                
            return
        })
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
