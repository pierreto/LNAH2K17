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
    
    let localSource = [ImageSource(imageString: "0 (1)")!, ImageSource(imageString: "0 (1)")!, ImageSource(imageString: "0 (1)")!]
    
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
