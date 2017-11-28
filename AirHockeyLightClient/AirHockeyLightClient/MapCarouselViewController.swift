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
    private var timer: Timer?
    private var maps = [MapEntity]()
    
    private let BORDER_COLORS = [
        UIColor(red: 84.0/255.0, green: 195.0/255.0, blue: 255.0/255.0, alpha: 0.9),
        UIColor(red: 60.0/255.0, green: 204.0/255.0, blue: 113.0/255.0, alpha: 0.9),
        UIColor(red: 255.0/255.0, green: 190.0/255.0, blue: 105.0/255.0, alpha: 0.9),
        UIColor(red: 234.0/255.0, green: 74.0/255.0, blue: 76.0/255.0, alpha: 0.9)
    ]
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        MapCarouselViewController.instance = self
        self.carouselView.type = .coverFlow
        self.updateEntries()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.activateAutomaticMapRefresh()
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.carouselView.reloadData()
        })
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        self.deactivateAutomaticMapRefresh()
    }
    
    func getCurrentMap() ->  MapEntity {
        return self.maps[self.carouselView.currentItemIndex]
    }
    
    func updateEntries() {
        // Fetch server maps
        let mapService = MapService()
        mapService.getMaps() { maps, error in
            if maps != nil {
                self.maps = [MapEntity]()
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
        
        let mapImageView = UIImageView(frame: CGRect(x: 10, y: 10, width: 180, height: 180))
        mapImageView.image = (map.icon != nil && map.icon != "") ? ImageService.convertStrBase64ToImage(strBase64: map.icon!) : UIImage(named: "map.png")
        
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
        tempView.addSubview(mapImageView)
 
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
        }
        
        tempView.layer.borderWidth = 5
        tempView.layer.borderColor = self.BORDER_COLORS[index % self.BORDER_COLORS.count].cgColor
        tempView.transform = CGAffineTransform(scaleX: 1.8, y: 1.8)
        
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
    
    func activateAutomaticMapRefresh() {
        self.timer = Timer.scheduledTimer(timeInterval: 10, target: self, selector: #selector(self.refreshMaps), userInfo: nil, repeats: true)
    }
    
    func deactivateAutomaticMapRefresh() {
        self.timer?.invalidate()
    }
    
    @objc private func refreshMaps() {
        self.maps = DBManager.instance.recupererCartes()
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload tableView
            self.carouselView.reloadData()
        })
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
