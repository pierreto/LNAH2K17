///////////////////////////////////////////////////////////////////////////////
/// @file TutorialViewController.swift
/// @author Mikael Ferland
/// @date 2017-11-25
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import ImageSlideshow

///////////////////////////////////////////////////////////////////////////
/// @class TutorialViewController
/// @brief Contrôleur permettant d'afficher le tutoriel pour l'édition
///
/// @author Mikael Ferland
/// @date 2017-11-25
///////////////////////////////////////////////////////////////////////////
class TutorialViewController: UIViewController {
    
    /// Instance singleton
    static var instance = TutorialViewController()
    
    @IBOutlet weak var slideshow: ImageSlideshow!
    
    let localSource = [ImageSource(imageString: "editor_light_1")!,
                       ImageSource(imageString: "editor_light_2")!,
                       ImageSource(imageString: "editor_light_3")!,
                       ImageSource(imageString: "editor_light_4")!,
                       ImageSource(imageString: "editor_light_5")!,
                       ImageSource(imageString: "editor_light_6")!,
                       ImageSource(imageString: "editor_light_7")!,
                       ImageSource(imageString: "editor_light_8")!,
                       ImageSource(imageString: "editor_light_9")!,
                       ImageSource(imageString: "editor_light_10")!,
                       ImageSource(imageString: "editor_light_11")!,
                       ImageSource(imageString: "editor_light_12")!,
                       ImageSource(imageString: "editor_light_13")!,
                       ImageSource(imageString: "editor_light_14")!,
                       ImageSource(imageString: "editor_light_15")!,
                       ImageSource(imageString: "editor_light_16")!]
    
    override func viewDidLoad() {
        super.viewDidLoad()
        TutorialViewController.instance = self
        
        self.view.backgroundColor = UIColor.black.withAlphaComponent(0.8)
        self.showAnimate()
        
        slideshow.pageControlPosition = PageControlPosition.underScrollView
        slideshow.pageControl.currentPageIndicatorTintColor = UIColor.lightGray
        slideshow.pageControl.pageIndicatorTintColor = UIColor.black
        slideshow.contentScaleMode = UIViewContentMode.scaleAspectFit
        slideshow.setImageInputs(localSource)
    }
    
    @IBAction func closeView(_ sender: Any) {
        self.removeAnimate()
    }
    
    /// Animation à l'ouverture
    func showAnimate() {
        self.view.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
        self.view.alpha = 0.0
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.view.alpha = 1.0
                self.view.transform = CGAffineTransform(scaleX: 1.0, y: 1)
        }
        )
    }
    
    /// Animation à la fermeture
    func removeAnimate() {
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.view.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
                self.view.alpha = 0.0
        },
            completion: {
                (finished: Bool) in
                if finished {
                    if self.parent is OnlineMenuViewController {
                        (self.parent as! OnlineMenuViewController).navigationController?.isNavigationBarHidden = false
                    } else if self.parent is OfflineMenuViewController {
                        (self.parent as! OfflineMenuViewController).navigationController?.isNavigationBarHidden = false
                    } else if self.parent is EditorViewController {
                        (self.parent as! EditorViewController).enableNavigationBar(activer: true)
                        
                    }
                    
                    self.view.removeFromSuperview()
                }
        }
        )
        
        // Réactiver la barre de navigation
        //EditorViewController.instance.enableNavigationBar(activer: true)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
