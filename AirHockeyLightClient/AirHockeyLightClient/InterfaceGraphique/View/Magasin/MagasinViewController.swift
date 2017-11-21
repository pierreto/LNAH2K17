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
    
    private var shopItems = [StoreItemEntity]()
    @IBOutlet weak var itemCollectionView: UICollectionView!
    
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var userPointsLabel: UILabel!
    
    @IBOutlet weak var totalSelected: UILabel!
    @IBOutlet weak var totalPoints: UILabel!
    
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
        
        itemCollectionView.isUserInteractionEnabled = true
        itemCollectionView.allowsMultipleSelection = true
        itemCollectionView.layer.masksToBounds = true
        itemCollectionView.layer.cornerRadius = 20.0
        itemCollectionView.delegate = self
        itemCollectionView.dataSource = self
        
        // Dummy shop item
        for _ in 0..<15 {
            let item = StoreItemEntity()
            item.setName(name: "Maillet rouge")
            item.setPrice(price: 5)
            item.setImageUrl(url: "fire_texture.png")
            item.setDescription(description: "Change la couleur du maillet en rouge")
            shopItems.append(item)
        }
        
        self.usernameLabel.text = HubManager.sharedConnection.getUsername()
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return shopItems.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = itemCollectionView.dequeueReusableCell(withReuseIdentifier: "itemCell", for: indexPath) as! ItemCollectionViewCell
        let shopItem = shopItems[indexPath.item]
        cell.itemImage.image = UIImage.init(named: shopItem.getImageUrl())
        cell.itemName.text = shopItem.getName()
        cell.itemPrice.text = shopItem.getPrice().description
        cell.itemDescription.text = shopItem.getDescription()
        
        let selectedBgItem = UIView()
        // let unavailableBgItem = UIView()
        selectedBgItem.backgroundColor = UIColor(red: 102.0/255.0, green: 178.0/255.0, blue: 255.0/255.0, alpha: 0.5)
        // unavailableBgItem.backgroundColor = UIColor(red: 102.0/255.0, green: 178.0/255.0, blue: 255.0/255.0, alpha: 1)
        cell.selectedBackgroundView = selectedBgItem
        
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        print("Select item")
        let shopItem = shopItems[indexPath.item]
        self.totalSelected.text = (Int.init(self.totalSelected.text!)! + 1).description
        self.totalPoints.text = (Int.init(self.totalPoints.text!)! + shopItem.getPrice()).description
    }
    
    func collectionView(_ collectionView: UICollectionView, didDeselectItemAt indexPath: IndexPath) {
        print("Deselect item")
        let shopItem = shopItems[indexPath.item]
        self.totalSelected.text = (Int.init(self.totalSelected.text!)! - 1).description
        self.totalPoints.text = (Int.init(self.totalPoints.text!)! - shopItem.getPrice()).description
    }

    @IBAction func buyElements(_ sender: Any) {
    }
    
    @IBAction func resetCart(_ sender: Any) {
        for index in self.itemCollectionView.indexPathsForSelectedItems! {
            self.itemCollectionView.deselectItem(at: index, animated: true)
            self.totalSelected.text = "0"
            self.totalPoints.text = "0"
        }
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
