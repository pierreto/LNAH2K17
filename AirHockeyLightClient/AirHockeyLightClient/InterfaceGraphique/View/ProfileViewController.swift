//
//  ProfileViewController.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-18.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit
import Alamofire
import SwiftyJSON

class ProfileViewController: UIViewController, UICollectionViewDelegate, UICollectionViewDataSource, CALayerDelegate, UIImagePickerControllerDelegate, UINavigationControllerDelegate {
    
    @IBOutlet weak var achievementCollectionView: UICollectionView!
    
    @IBOutlet weak var profileImage: UIImageView!
    let imagePicker = UIImagePickerController()
    
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var emailLabel: UILabel!
    @IBOutlet weak var dateJoinedLabel: UILabel!
    
    @IBOutlet weak var pointsLabel: UILabel!
    
    @IBOutlet weak var gamesWonLabel: UILabel!
    @IBOutlet weak var gamesPlayedLabel: UILabel!
    
    @IBOutlet weak var tournamentsWonLabel: UILabel!
    @IBOutlet weak var tournamentsPlayedLabel: UILabel!
    
    let gradient = CAGradientLayer()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        achievementCollectionView.delegate = self
        achievementCollectionView.dataSource = self
        // Do any additional setup after loading the view.
        profileImage.image = UIImage(named: "default_profile_picture.png")
        
        // Makes the scroll view fade at the sides to indicate it is scrollable
        gradient.frame = achievementCollectionView.bounds
        gradient.colors = [UIColor.clear.cgColor, UIColor.black.cgColor, UIColor.black.cgColor, UIColor.clear.cgColor]
        gradient.locations = [0.0, 0.2, 0.8, 1.0]
        gradient.startPoint = CGPoint(x: 0.0, y: 0.5)
        gradient.endPoint = CGPoint(x: 1.0, y: 0.5)
        achievementCollectionView.layer.mask = gradient
        gradient.delegate = self
        
        // create tap gesture recognizer
        let tapGesture = UITapGestureRecognizer(target: self, action: #selector(imageTapped(gesture:)))
        
        // add it to the image view;
        profileImage.addGestureRecognizer(tapGesture)
        // make sure imageView can be interacted with by user
        profileImage.isUserInteractionEnabled = true
        profileImage.layer.masksToBounds = true
        profileImage.layer.cornerRadius = 20.0
        imagePicker.delegate = self
        
        loadUserProfile()
    }
    
    func loadUserProfile() {
        Alamofire.request("http://" + HubManager.sharedConnection.getIpAddress()! + ":63056/api/profile/" + ((HubManager.sharedConnection.getId())?.description)!)
            .responseJSON { response in
                if let jsonValue = response.result.value {
                    let json = JSON(jsonValue)
                    
                    // Start String to image
                    let profile = json["UserEntity"]["Profile"].string
                    let imageData = NSData(base64Encoded: profile!)
                    let image = UIImage(data: imageData! as Data)
                    self.profileImage.image = image
                    // End String to image
                    
                    var id = json["UserEntity"]["Id"]
                    print("id: ", id)
                    
                }
        }
    }
    
    func imageTapped(gesture: UIGestureRecognizer) {
        // if the tapped view is a UIImageView then set it to imageview
        if (gesture.view as? UIImageView) != nil {
            print("Image Tapped")
            //Here you can initiate your new ViewController
            imagePicker.allowsEditing = false
            imagePicker.sourceType = .photoLibrary
            
            present(imagePicker, animated: true, completion: nil)
        }
    }
    
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [String : Any]) {
        if let pickedImage = info[UIImagePickerControllerOriginalImage] as? UIImage {
            profileImage.contentMode = .scaleToFill
            
            let imageData : Data = UIImagePNGRepresentation(pickedImage)! as Data
            let strBase64 = imageData.base64EncodedString(options: Data.Base64EncodingOptions.init(rawValue: 0))
            if strBase64.characters.count > 65535 {
                // error dialog
                profileImage.image = pickedImage

                //print(strBase64)
            } else {
                // update
                profileImage.image = pickedImage
                //print(strBase64)
            }
        }
        dismiss(animated: true, completion: nil)
    }
    
    private func imagePickerControllerDidCancel(picker: UIImagePickerController) {
        dismiss(animated: true, completion: nil)
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return 15
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = achievementCollectionView.dequeueReusableCell(withReuseIdentifier: "achievementCell", for: indexPath) as! AchievementCollectionViewCell
        cell.imageView.image = UIImage(named: "coin_5_enabled.png")
        cell.imageLabel.text = "testing1212 sfdsdffs sdfsd"
//        cell.alpha = 0
//
//        UIView.animate(withDuration: 0.1, animations: { cell.alpha = 1 })
        return cell
    }
    
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
        gradient.frame = achievementCollectionView.bounds
    }

    func action(for layer: CALayer, forKey event: String) -> CAAction? {
        return NSNull()
    }
    
    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

}
