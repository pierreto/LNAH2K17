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
    static func GetHitTestSceneViewCoordinates(point: CGPoint) -> SCNVector3? {
        let hits = FacadeModele.instance.obtenirVue().editorView.hitTest(point, options: nil)

        if (hits.count > 0) {
            let result: AnyObject = hits[0]
            if  result is SCNHitTestResult {
                let hitResult = result as! SCNHitTestResult
                
                // localCoordinates corresponds to the table coordinates
                return hitResult.localCoordinates
            }
        }
        
        return nil
    }
    
    /// Cette fonction trouve si un segment et un cercle se croisent
    static func segmentCercleIntersect(s1: GLKVector3, s2: GLKVector3, centreCercle: GLKVector3, rayon: Float) -> Bool {
        let v1 = GLKVector3Subtract(s2, s1)
        let v2 = GLKVector3Subtract(s1, centreCercle)
    
        let a = GLKVector3DotProduct(v1, v1)
        let b = 2 * GLKVector3DotProduct(v2, v1)
        let c = GLKVector3DotProduct(v2, v2) - rayon * rayon;
    
        let disc = sqrt((b * b) - (4 * a * c));
    
        // Intersect 1 point
        if (disc < 0) {
            return false;
        }
    
        // Les racines
        let r1 = (-b - disc) / (2 * a);
        let r2 = (-b + disc) / (2 * a);
    
        if (0 <= r1 && r1 <= 1) {
            return true;
        }
    
        if (0 <= r2 && r2 <= 1) {
            return true;
        }
    
        return false;
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

