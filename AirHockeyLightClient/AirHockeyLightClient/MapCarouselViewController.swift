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
        var label: UILabel
        var itemView: UIImageView
        
        //reuse view if available, otherwise create a new view
        if let view = view as? UIImageView {
            itemView = view
            //get a reference to the label in the recycled view
            label = itemView.viewWithTag(1) as! UILabel
        } else {
            //don't do anything specific to the index within
            //this `if ... else` statement because the view will be
            //recycled and used with other index values later
            itemView = UIImageView(frame: CGRect(x: 0, y: 0, width: 200, height: 200))
            itemView.image = UIImage()
            itemView.contentMode = .center
            
            label = UILabel(frame: itemView.bounds)
            label.backgroundColor = .clear
            label.textAlignment = .center
            label.font = label.font.withSize(50)
            label.tag = 1
            itemView.addSubview(label)
        }
        
        //set item label
        //remember to always set any properties of your carousel item
        //views outside of the `if (view == nil) {...}` check otherwise
        //you'll get weird issues with carousel item content appearing
        //in the wrong place in the carousel
        label.text = "\(items[index])"
        
        return itemView
    }
    
    func carousel(_ carousel: iCarousel, valueFor option: iCarouselOption, withDefault value: CGFloat) -> CGFloat {
        if (option == .spacing) {
            return value * 1.1
        }
        return value
    }
    
    /*
    private var maps = [MapEntity]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        let map1 = MapEntity()
        map1.id = "300"
        let map2 = MapEntity()
        map2.id = "400"
        
        self.maps.append(map1)
        self.maps.append(map2)
        
        DispatchQueue.main.async(execute: { () -> Void in
            self.carouselView.reloadData()
        })
    }
    
    func numberOfItems(in carousel: iCarousel) -> Int {
        return self.maps.count
    }
    
    func carousel(_ carousel: iCarousel, viewForItemAt index: Int, reusing view: UIView?) -> UIView {
        var label: UILabel
        var itemView: UIImageView
        
        itemView = UIImageView(frame: CGRect(x: 0, y: 0, width: 200, height: 200))
        itemView.image = UIImage(named: "default_profile_picture.png")
        itemView.contentMode = .center
            
        label = UILabel(frame: itemView.bounds)
        label.backgroundColor = .clear
        label.textAlignment = .center
        label.font = label.font.withSize(50)
        label.tag = 1
        itemView.addSubview(label)
        
        label.text = "test"
        
        return itemView
    }
    
    func carousel(_ carousel: iCarousel, valueFor option: iCarouselOption, withDefault value: CGFloat) -> CGFloat {
        if (option == .spacing) {
            return value * 1.2
        }
        return value
    }
    */
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
