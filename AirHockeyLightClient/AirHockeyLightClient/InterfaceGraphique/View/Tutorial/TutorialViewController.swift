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
        
        slideshow.pageControlPosition = PageControlPosition.underScrollView
        slideshow.pageControl.currentPageIndicatorTintColor = UIColor.lightGray
        slideshow.pageControl.pageIndicatorTintColor = UIColor.black
        slideshow.contentScaleMode = UIViewContentMode.scaleAspectFit
        slideshow.setImageInputs(localSource)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
