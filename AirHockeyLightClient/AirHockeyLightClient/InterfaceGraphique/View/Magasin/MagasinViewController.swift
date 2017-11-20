///////////////////////////////////////////////////////////////////////////////
/// @file MagasinViewController.swift
/// @author Pierre To
/// @date 2017-11-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class MagasinViewController
/// @brief Contrôleur du magasin
///
/// @author Pierre To
/// @date 2017-11-19
///////////////////////////////////////////////////////////////////////////
class MagasinViewController: UIViewController, UICollectionViewDelegate, UICollectionViewDataSource, CALayerDelegate, UIImagePickerControllerDelegate, UINavigationControllerDelegate {
    
    @IBOutlet weak var itemCollectionView: UICollectionView!
    
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var userPointsLabel: UILabel!
    
    let gradient = CAGradientLayer()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        itemCollectionView.delegate = self
        itemCollectionView.dataSource = self
        
        // Makes the scroll view fade at the sides to indicate it is scrollable
        gradient.frame = itemCollectionView.bounds
        gradient.colors = [UIColor.clear.cgColor, UIColor.black.cgColor, UIColor.black.cgColor, UIColor.clear.cgColor]
        gradient.locations = [0.0, 0.2, 0.8, 1.0]
        gradient.startPoint = CGPoint(x: 0.0, y: 0.5)
        gradient.endPoint = CGPoint(x: 1.0, y: 0.5)
        itemCollectionView.layer.mask = gradient
        gradient.delegate = self
        
        // create tap gesture recognizer
        let tapGesture = UITapGestureRecognizer(target: self, action: #selector(itemTapped(gesture:)))
        
        // add it to the item view;
        itemCollectionView.addGestureRecognizer(tapGesture)
        // make sure itemCollectionView can be interacted with by user
        itemCollectionView.isUserInteractionEnabled = true
        itemCollectionView.layer.masksToBounds = true
        itemCollectionView.layer.cornerRadius = 20.0
        itemCollectionView.delegate = self
    }
    
    func itemTapped(gesture: UIGestureRecognizer) {
        // if the tapped view is a UIImageView then set it to imageview
        if (gesture.view as? UIImageView) != nil {
            print("Item Tapped")
            // TODO : Sélection d'item
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return 15
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = itemCollectionView.dequeueReusableCell(withReuseIdentifier: "itemCell", for: indexPath) as! ItemCollectionViewCell
        cell.itemImage.image = UIImage(named: "fire_texture.png")
        cell.itemName.text = "Maillet rouge"
        cell.itemPrice.text = "5"
        cell.itemDescription.text = "Change la couleur du maillet en rouge"
        //        cell.alpha = 0
        //        UIView.animate(withDuration: 0.1, animations: { cell.alpha = 1 })
        return cell
    }
    
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
        gradient.frame = itemCollectionView.bounds
    }
    
    func action(for layer: CALayer, forKey event: String) -> CAAction? {
        return NSNull()
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
