///////////////////////////////////////////////////////////////////////////////
/// @file UIViewShake.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-01
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

extension UIView {
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn shake()
    ///
    /// Produire un effet de brassage sur un élément de type UIView (ex. UITextField)
    ///
    /// @param[in] keyPath : Le nom de la propriété à animer
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    func shake() {
        let animation = CABasicAnimation(keyPath: "position")
        animation.duration = 0.07
        animation.repeatCount = 4
        animation.autoreverses = true
        animation.fromValue = NSValue(cgPoint: CGPoint(x: self.center.x - 10, y: self.center.y))
        animation.toValue = NSValue(cgPoint: CGPoint(x: self.center.x + 10, y: self.center.y))
        self.layer.add(animation, forKey: "position")
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
