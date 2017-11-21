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
import Alamofire
import SwiftyJSON
import PromiseKit

///////////////////////////////////////////////////////////////////////////
/// @class MagasinViewController
/// @brief Contrôleur du magasin
///
/// @author Pierre To
/// @date 2017-11-19
///////////////////////////////////////////////////////////////////////////
class MagasinViewController: UIViewController, UICollectionViewDelegate, UICollectionViewDataSource, CALayerDelegate, UIImagePickerControllerDelegate, UINavigationControllerDelegate {
    
    private var storeService = StoreService()
    
    private var storeItems = [StoreItemEntity]()
    @IBOutlet weak var itemCollectionView: UICollectionView!
    
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var userPointsLabel: UILabel!
    
    @IBOutlet weak var totalSelected: UILabel!
    @IBOutlet weak var totalPoints: UILabel!
    
    @IBOutlet weak var profileButton: UIButton!
    @IBOutlet weak var resetCartButton: UIButton!
    @IBOutlet weak var buyElementsButton: UIButton!
    
    @IBOutlet weak var loadingSpinner: UIActivityIndicatorView!
    
    let gradient = CAGradientLayer()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        itemCollectionView.delegate = self
        itemCollectionView.dataSource = self
        
        // Makes the scroll view fade at the sides to indicate it is scrollable
        gradient.frame = itemCollectionView.bounds
        gradient.colors = [UIColor.clear.cgColor, UIColor.black.cgColor, UIColor.black.cgColor, UIColor.clear.cgColor]
        gradient.locations = [0.0, 0.1, 0.9, 1.0]
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
        
        // Définir les items dans le magasin
        self.loadStoreItems()

        // Réinitialiser toutes les informations
        self.resetUserInfo()
        
        // Définir les informations de l'usager courant
        self.loadUserInfo()
    }
    
    private func loadStoreItems() {
        self.storeService.getStoreItems().then { items -> Void in
            self.storeItems = items
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload table
                self.itemCollectionView.reloadData()
            })
        }
    }

    private func resetUserInfo() {
        self.usernameLabel.text = ""
        self.userPointsLabel.text = ""
    }
    
    private func loadUserInfo() {
        self.loading()
        Alamofire.request("http://" + HubManager.sharedConnection.getIpAddress()! + ":63056/api/profile/" + ((HubManager.sharedConnection.getId())?.description)!)
            .responseJSON { response in
                if let jsonValue = response.result.value {
                    let json = JSON(jsonValue)
                    //print("JSON: \(jsonValue)")
                    // Username
                    let username = json["UserEntity"]["Username"].string
                    self.usernameLabel.text = username
                    
                    // Points
                    let stats = json["StatsEntity"]
                    if stats["Points"].description != "" && stats["Points"].description != "null" {
                        let points = json["StatsEntity"]["Points"].description
                        self.userPointsLabel.text = points
                    } else {
                        self.userPointsLabel.text = "0"
                    }
                }
                self.loadingDone()
        }
    }

    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return storeItems.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = itemCollectionView.dequeueReusableCell(withReuseIdentifier: "itemCell", for: indexPath) as! ItemCollectionViewCell
        let storeItem = storeItems[indexPath.item]
        cell.itemImage.image = UIImage.init(named: storeItem.getTextureName())
        cell.itemName.text = storeItem.getName()
        cell.itemPrice.text = storeItem.getPrice().description
        cell.itemDescription.text = storeItem.getDescription()
        
        let selectedBgItem = UIView()
        // let unavailableBgItem = UIView()
        selectedBgItem.backgroundColor = UIColor(red: 102.0/255.0, green: 178.0/255.0, blue: 255.0/255.0, alpha: 0.5)
        // unavailableBgItem.backgroundColor = UIColor(red: 102.0/255.0, green: 178.0/255.0, blue: 255.0/255.0, alpha: 1)
        cell.selectedBackgroundView = selectedBgItem
        
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        print("Select item")
        let storeItem = storeItems[indexPath.item]
        self.totalSelected.text = (Int.init(self.totalSelected.text!)! + 1).description
        self.totalPoints.text = (Int.init(self.totalPoints.text!)! + storeItem.getPrice()).description
    }
    
    func collectionView(_ collectionView: UICollectionView, didDeselectItemAt indexPath: IndexPath) {
        print("Deselect item")
        let storeItem = storeItems[indexPath.item]
        self.totalSelected.text = (Int.init(self.totalSelected.text!)! - 1).description
        self.totalPoints.text = (Int.init(self.totalPoints.text!)! - storeItem.getPrice()).description
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
        self.profileButton.isEnabled = false
        self.resetCartButton.isEnabled = false
        self.buyElementsButton.isEnabled = false
    }
    
    private func enableInputs() {
        self.profileButton.isEnabled = true
        self.resetCartButton.isEnabled = true
        self.buyElementsButton.isEnabled = true
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
