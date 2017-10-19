///////////////////////////////////////////////////////////////////////////////
/// @file MathHelper.swift
/// @author Pierre To
/// @date 2017-10-08
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation
import SceneKit

class MathHelper {
    
    static func clamp<T: Comparable>(value: T, lower: T, upper: T) -> T {
        return min(max(value, lower), upper)
    }
    
    static func mod(_ a: Int, _ n: Int) -> Int {
        precondition(n > 0, "modulus must be positive")
        let r = a % n
        return r >= 0 ? r : r + n
    }
    
    /// Transformer un point (données en coordonnées d'affichage) en coordonnées virtuelles
    static func CGPointToSCNVector3(view: SCNView, depth: Float, point: CGPoint) -> SCNVector3 {
        let projectedOrigin = view.projectPoint(SCNVector3Make(0, 0, depth))
        let locationWithz   = SCNVector3Make(Float(point.x), Float(point.y), projectedOrigin.z)
        return view.unprojectPoint(locationWithz)
    }
    
    /// Transformer un point (données en coordonnées d'affichage d'après un TAP) en coordonnées virtuelles    
    static func GetHitTestSceneViewCoordinates(point: CGPoint) -> SCNVector3 {
        let hits = FacadeModele.instance.obtenirVue().editorView.hitTest(point, options: nil)

        if (hits.count > 0) {
            let result: AnyObject = hits[0]
            if  result is SCNHitTestResult {
                let hitResult = result as! SCNHitTestResult
                
                // localCoordinates corresponds to the table coordinates
                return hitResult.localCoordinates
            }
        }
        
        return SCNVector3()
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

