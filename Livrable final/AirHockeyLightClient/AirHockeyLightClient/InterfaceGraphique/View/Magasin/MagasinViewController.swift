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
    
    @IBOutlet weak var errorView: UIView!
    @IBOutlet weak var errorMessage: UILabel!
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
        self.loadUserStoreItems()
    }
    
    private func loadStoreItems() {
        _ = self.storeService.getStoreItems().then { items -> Void in
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
    
    private func loadUserStoreItems() {
        _ = self.storeService.getUserStoreItems().then { items -> Void in
            // Mettre à jour les items achetés par l'utilisateur
            for item in items {
                self.storeItems.first(where: { $0.getId() == item.getId() })?.setIsBoughtByUser(isBoughtByUser: true)
            }
            
            DispatchQueue.main.async(execute: { () -> Void in
                // Reload table
                self.itemCollectionView.reloadData()
            })
        }
    }

    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return storeItems.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = itemCollectionView.dequeueReusableCell(withReuseIdentifier: "itemCell", for: indexPath) as! ItemCollectionViewCell
        let storeItem = storeItems[indexPath.item]
        
        cell.itemImage.image = UIImage.init(named: storeItem.getImageUrl().replacingOccurrences(of: "\\media\\textures\\", with: "", options: .literal, range: nil))
        cell.itemName.text = storeItem.getName()
        cell.itemPrice.text = storeItem.getPrice().description
        cell.itemDescription.text = storeItem.getDescription()
        cell.isUserInteractionEnabled = !storeItem.getIsBoughtByUser()
        cell.itemAchete.isHidden = !storeItem.getIsBoughtByUser()
        
        let selectedBgItem = UIView()
        selectedBgItem.backgroundColor = UIColor(red: 99.0/255.0, green: 205.0/255.0, blue: 251.0/255.0, alpha: 0.5)
        cell.selectedBackgroundView = selectedBgItem
        
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        print("Select item")
        let storeItem = storeItems[indexPath.item]
        let numberOfItemsSelected = self.itemCollectionView.indexPathsForSelectedItems?.count
        
        self.totalSelected.text = numberOfItemsSelected?.description
        self.totalPoints.text = (Int.init(self.totalPoints.text!)! + storeItem.getPrice()).description
        
        // Check if user can buy
        if Int.init(self.totalPoints.text!)! > Int.init(self.userPointsLabel.text!)! {
            self.activateBuy(activate: false)
            self.showError(show: true)
        }
        else {
            self.activateBuy(activate: numberOfItemsSelected! > 0)
            self.showError(show: false)
        }
        
        // Check if user can clear cart
        self.activateResetCart(activate: (self.itemCollectionView.indexPathsForSelectedItems?.count)! > 0)
    }
    
    func collectionView(_ collectionView: UICollectionView, didDeselectItemAt indexPath: IndexPath) {
        print("Deselect item")
        let storeItem = storeItems[indexPath.item]
        let numberOfItemsSelected = self.itemCollectionView.indexPathsForSelectedItems?.count
        
        self.totalSelected.text = numberOfItemsSelected?.description
        self.totalPoints.text = (Int.init(self.totalPoints.text!)! - storeItem.getPrice()).description
        
        // Check if user can buy
        if Int.init(self.totalPoints.text!)! > Int.init(self.userPointsLabel.text!)! {
            self.activateBuy(activate: false)
        }
        else {
            self.activateBuy(activate: numberOfItemsSelected! > 0)
            self.showError(show: false)
        }
        
        // Check if user can clear cart
        self.activateResetCart(activate: numberOfItemsSelected! > 0)
    }

    @IBAction func buyElements(_ sender: Any) {
        // Buy all selected items
        var selectedItems = [StoreItemEntity]()
        
        for index in self.itemCollectionView.indexPathsForSelectedItems! {
            let storeItem = storeItems[index.item]
            selectedItems.append(storeItem)
            storeItem.setIsBoughtByUser(isBoughtByUser: true)
            self.userPointsLabel.text = (Int.init(self.userPointsLabel.text!)! - storeItem.getPrice()).description
        }
        
        // Envoyer la requête d'achat au serveur
        print("Send buying http request")
        _ = self.storeService.buyElement(items: selectedItems, userId: HubManager.sharedConnection.getId()!)
        
        // Mettre à jour l'UI
        self.resetCart((Any).self)
    }
    
    @IBAction func resetCart(_ sender: Any) {
        // Deselect all items
        for index in self.itemCollectionView.indexPathsForSelectedItems! {
            self.itemCollectionView.deselectItem(at: index, animated: true)
        }
        
        self.totalSelected.text = "0"
        self.totalPoints.text = "0"
        self.showError(show: false)
        self.activateBuy(activate: false)
        self.activateResetCart(activate: false)
        
        DispatchQueue.main.async(execute: { () -> Void in
            // Reload table
            self.itemCollectionView.reloadData()
        })
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
    
    private func showError(show: Bool) {
        if show {
            self.errorView.isHidden = false
            self.errorMessage.text = "Points insuffisants"
            self.errorView.shake()
            self.totalPoints.textColor = UIColor.init(red: 234.0/255.0, green: 74.0/255.0, blue: 76.0/255.0, alpha: 1.0)
        }
        else {
            self.errorView.isHidden = true
            self.errorMessage.text = "Erreur"
            self.totalPoints.textColor = UIColor.white
        }
    }
    
    private func activateBuy(activate: Bool) {
        self.buyElementsButton.isEnabled = activate
    }
    
    private func activateResetCart(activate: Bool) {
        self.resetCartButton.isEnabled = activate
    }
    
    private func disableInputs() {
        self.profileButton.isEnabled = false
        self.resetCartButton.isEnabled = false
        self.buyElementsButton.isEnabled = false
        self.itemCollectionView.isUserInteractionEnabled = false
    }
    
    private func enableInputs() {
        self.profileButton.isEnabled = true
        self.itemCollectionView.isUserInteractionEnabled = true
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
