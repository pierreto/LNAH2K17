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
    
    @IBOutlet weak var itemCollectionView: UICollectionView!
    
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
    
    let gradientAchievements = CAGradientLayer()
    let gradientItems = CAGradientLayer()
    
    var itemsUrl = [String]()
    var itemsLabel = [String]()
    var itemsEnable = [Bool]()
    var achievementUrls = [String]()
    var achievementLabels = [String]()
    var achievementEnabled = [Bool]()

    override func viewDidLoad() {
        super.viewDidLoad()
        achievementCollectionView.delegate = self
        achievementCollectionView.dataSource = self
        // Do any additional setup after loading the view.
        profileImage.image = UIImage(named: "default_profile_picture.png")
        
        // Makes the scroll view fade at the sides to indicate it is scrollable
        gradientAchievements.frame = achievementCollectionView.bounds
        gradientAchievements.colors = [UIColor.clear.cgColor, UIColor.black.cgColor, UIColor.black.cgColor, UIColor.clear.cgColor]
        gradientAchievements.locations = [0.0, 0.2, 0.8, 1.0]
        gradientAchievements.startPoint = CGPoint(x: 0.0, y: 0.5)
        gradientAchievements.endPoint = CGPoint(x: 1.0, y: 0.5)
        achievementCollectionView.layer.mask = gradientAchievements
        gradientAchievements.delegate = self
        
                // Makes the scroll view fade at the sides to indicate it is scrollable
        gradientItems.frame = itemCollectionView.bounds
        gradientItems.colors = [UIColor.clear.cgColor, UIColor.black.cgColor, UIColor.black.cgColor, UIColor.clear.cgColor]
        gradientItems.locations = [0.0, 0.2, 0.8, 1.0]
        gradientItems.startPoint = CGPoint(x: 0.0, y: 0.5)
        gradientItems.endPoint = CGPoint(x: 1.0, y: 0.5)
        itemCollectionView.layer.mask = gradientItems
        gradientItems.delegate = self
        
        // create tap gesture recognizer
        let tapGesture = UITapGestureRecognizer(target: self, action: #selector(imageTapped(gesture:)))
        
        // add it to the image view;
        profileImage.addGestureRecognizer(tapGesture)
        // make sure imageView can be interacted with by user
        profileImage.isUserInteractionEnabled = true
        profileImage.layer.masksToBounds = true
        profileImage.layer.cornerRadius = 20.0
        imagePicker.delegate = self
        
        self.usernameLabel.text = "..."
        self.nameLabel.text = "..."
        self.emailLabel.text = "..."
        self.dateJoinedLabel.text = "..."
        self.pointsLabel.text = "0"
        self.gamesWonLabel.text = "0"
        self.gamesPlayedLabel.text = "0"
        self.tournamentsWonLabel.text = "0"
        self.tournamentsPlayedLabel.text = "0"
        
        loadUserProfile()
    }
    
    func loadUserProfile() {
        self.loading()
        Alamofire.request("http://" + HubManager.sharedConnection.getIpAddress()! + ":63056/api/profile/" + ((HubManager.sharedConnection.getId())?.description)!)
            .responseJSON { response in
                if let jsonValue = response.result.value {
                    let json = JSON(jsonValue)
                    //print("JSON: \(jsonValue)")
                    // Image
                    let profile = json["UserEntity"]["Profile"].string
                    if profile != "" && profile != nil {
                        let imageData = NSData(base64Encoded: profile!)
                        let image = UIImage(data: imageData! as Data)
                        self.profileImage.image = image
                    }

                    // Username
                    let username = json["UserEntity"]["Username"].string
                    self.usernameLabel.text = username
                    
                    // Name
                    let name = json["UserEntity"]["Name"].string
                    if name != "" && name != nil {
                       self.nameLabel.text = name
                    }
                    
                    // Email
                    let email = json["UserEntity"]["Email"].string
                    if email != "" && email != nil {
                        self.emailLabel.text = email
                    }
                    
                    // Created
                    let created = json["UserEntity"]["Created"].string
                    self.dateJoinedLabel.text = self.convertDate(dateString: created!)
                    
                    let stats = json["StatsEntity"]
                    
                    // Points
                    if stats["Points"].description != "" && stats["Points"].description != "null" {
                        let points = json["StatsEntity"]["Points"].description
                        self.pointsLabel.text = points
                    }
                    
                    // GamesWon
                    if stats["GamesWon"].description != "" && stats["GamesWon"].description != "null" {
                        let gamesWon = json["StatsEntity"]["GamesWon"].description
                        self.gamesWonLabel.text = gamesWon
                    }
                    
                    // GamesPlayed
                    if json["GamesPlayed"].description != "" && json["GamesPlayed"].description != "null"{
                        let gamesPlayed = json["GamesPlayed"].description
                        self.gamesPlayedLabel.text = gamesPlayed
                    }
                    
                    // TournamentsWon
                    if stats["TournamentsWon"].description != "" && stats["TournamentsWon"].description != "null" {
                        let tournamentsWon = json["StatsEntity"]["TournamentsWon"].description
                        self.tournamentsWonLabel.text = tournamentsWon
                    }
                    
                    // TournamentsPlayed
                    if json["TournamentsPlayed"].description != "" && json["TournamentsPlayed"].description != "null"{
                        let tournamentsPlayed = json["TournamentsPlayed"].description
                        self.tournamentsPlayedLabel.text = tournamentsPlayed
                    }
                    
                    //Achievements
                    let achievements = json["AchievementEntities"].array
                    for var achievement in achievements! {
                        if achievement["IsEnabled"].description == "true" {
                            self.achievementEnabled.append(true)
                            self.achievementUrls.append(achievement["EnabledImageUrl"].description.replacingOccurrences(of: "\\media\\image\\", with: "", options: .literal, range: nil))
                            print(self.achievementUrls[self.achievementUrls.count - 1])
                        } else {
                            self.achievementEnabled.append(false)
                            self.achievementUrls.append(achievement["DisabledImageUrl"].description.replacingOccurrences(of: "\\media\\image\\", with: "", options: .literal, range: nil))
                        }
                        self.achievementLabels.append(achievement["Name"].description)
                    }
                    self.achievementCollectionView.reloadData()
                    let indexPath = IndexPath(row: Int(INT_MAX)/200, section: 0);
                    self.achievementCollectionView.scrollToItem(at: indexPath, at: .centeredHorizontally , animated: false)
                }
                self.loadingDone()
        }
    }
    
    func convertDate(dateString: String) -> String {
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd HH:mm:ss a"
        let date = dateFormatter.date(from: dateString)
        
        dateFormatter.dateFormat = "d MMM yyyy"
        if date != nil {
            return dateFormatter.string(from: date!)
        }
        else {
            return dateString
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
        var count:Int?
        if collectionView == self.achievementCollectionView {
            count = achievementUrls.count > 0 ? Int(INT_MAX)/100 : 0
        }
        if collectionView == self.itemCollectionView {
            count = itemsUrl.count > 0 ? Int(INT_MAX)/100 : 0
        }
        return count!

    }
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        var cell:AchievementCollectionViewCell?
        if collectionView == self.achievementCollectionView {
            cell = achievementCollectionView.dequeueReusableCell(withReuseIdentifier: "achievementCell", for: indexPath) as? AchievementCollectionViewCell
            cell?.imageView.image = UIImage(named: achievementUrls[indexPath.row % achievementUrls.count])
            cell?.imageLabel.text = achievementLabels[indexPath.row % achievementUrls.count]
            if self.achievementEnabled[indexPath.row % achievementUrls.count] {
                cell?.imageLabel.textColor = UIColor.green
            } else {
                cell?.imageLabel.textColor = UIColor.white
            }
        } else if collectionView == self.itemCollectionView {
            cell = itemCollectionView.dequeueReusableCell(withReuseIdentifier: "itemCell", for: indexPath) as? AchievementCollectionViewCell
            cell?.imageView.image = UIImage(named: itemsUrl[indexPath.row % itemsUrl.count])
            cell?.imageLabel.text = itemsLabel[indexPath.row % itemsUrl.count]
            if self.achievementEnabled[indexPath.row % itemsUrl.count] {
                cell?.imageLabel.textColor = UIColor.green
            } else {
                cell?.imageLabel.textColor = UIColor.white
            }
            }
            return cell!
      }
    
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
        gradientItems.frame = itemCollectionView.bounds
        gradientAchievements.frame = achievementCollectionView.bounds
    }

    func action(for layer: CALayer, forKey event: String) -> CAAction? {
        return NSNull()
    }
    
    func loading() {
        
    }
    
    func loadingDone() {
        
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
