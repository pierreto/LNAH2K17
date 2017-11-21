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
    
    @IBOutlet var carouselView: iCarousel!
    
    var items: [Int] = []
    
    override func awakeFromNib() {
        super.awakeFromNib()
        for i in 0 ... 5 {
            items.append(i)
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        carouselView.type = .cylinder
    }
    
    func numberOfItems(in carousel: iCarousel) -> Int {
        return items.count
    }
    
    func carousel(_ carousel: iCarousel, viewForItemAt index: Int, reusing view: UIView?) -> UIView {
        let tempView = UIImageView(frame: CGRect(x: 0, y: 0, width: 400, height: 200))

        let isPublicLabel = UILabel()
        isPublicLabel.text = "isPublicLabel"
        isPublicLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 15.0)
        isPublicLabel.textAlignment = NSTextAlignment.right
        isPublicLabel.numberOfLines = 1
        isPublicLabel.textColor = UIColor.black
        isPublicLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let mapNameLabel = UILabel()
        mapNameLabel.text = "mapNameLabel"
        mapNameLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 15.0)
        mapNameLabel.textAlignment = NSTextAlignment.right
        mapNameLabel.numberOfLines = 1
        mapNameLabel.textColor = UIColor.black
        mapNameLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let numberOfPlayersLabel = UILabel()
        numberOfPlayersLabel.text = "numberOfPlayersLabel"
        numberOfPlayersLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 15.0)
        numberOfPlayersLabel.textAlignment = NSTextAlignment.right
        numberOfPlayersLabel.numberOfLines = 1
        numberOfPlayersLabel.textColor = UIColor.black
        numberOfPlayersLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let creatorLabel = UILabel()
        creatorLabel.text = "creatorLabel"
        creatorLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 15.0)
        creatorLabel.textAlignment = NSTextAlignment.right
        creatorLabel.numberOfLines = 1
        creatorLabel.textColor = UIColor.black
        creatorLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let idLabel = UILabel()
        idLabel.text = "idLabel"
        idLabel.font = UIFont(name:"HelveticaNeue-Bold", size: 15.0)
        idLabel.textAlignment = NSTextAlignment.right
        idLabel.numberOfLines = 1
        idLabel.textColor = UIColor.black
        idLabel.translatesAutoresizingMaskIntoConstraints = false
        
        let button = UIButton(frame: CGRect(x: 0, y: 0, width: 400, height: 200))
        button.backgroundColor = UIColor.lightGray
        button.addTarget(self, action:#selector(handleRegister(sender:)), for: .touchUpInside)
        
        tempView.addSubview(button)
        tempView.addSubview(isPublicLabel)
        tempView.addSubview(mapNameLabel)
        tempView.addSubview(numberOfPlayersLabel)
        tempView.addSubview(creatorLabel)
        tempView.addSubview(idLabel)
        
        isPublicLabel.centerXAnchor.constraint(equalTo: tempView.centerXAnchor).isActive = true
        isPublicLabel.topAnchor.constraint(equalTo: tempView.topAnchor, constant: 15).isActive = true
        
        mapNameLabel.centerXAnchor.constraint(equalTo: tempView.centerXAnchor).isActive = true
        mapNameLabel.topAnchor.constraint(equalTo: tempView.topAnchor, constant: 50).isActive = true
        
        numberOfPlayersLabel.centerXAnchor.constraint(equalTo: tempView.centerXAnchor).isActive = true
        numberOfPlayersLabel.topAnchor.constraint(equalTo: tempView.topAnchor, constant: 90).isActive = true
        
        creatorLabel.centerXAnchor.constraint(equalTo: tempView.centerXAnchor).isActive = true
        creatorLabel.topAnchor.constraint(equalTo: tempView.topAnchor, constant: 130).isActive = true
        
        idLabel.centerXAnchor.constraint(equalTo: tempView.centerXAnchor).isActive = true
        idLabel.topAnchor.constraint(equalTo: tempView.topAnchor, constant: 170).isActive = true
        
        tempView.isUserInteractionEnabled = true
        return tempView
    }
    
    func handleRegister(sender: UIButton){
        print("open map")
    }
    
    func carousel(_ carousel: iCarousel, valueFor option: iCarouselOption, withDefault value: CGFloat) -> CGFloat {
        if (option == .spacing) {
            return value * 1.1
        }
        return value
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
