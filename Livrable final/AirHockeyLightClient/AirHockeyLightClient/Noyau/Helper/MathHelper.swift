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
        var hits: [SCNHitTestResult]
        
        if #available(iOS 10.0, *) {
            hits = FacadeModele.instance.obtenirVue().editorView.hitTest(point, options: [SCNHitTestOption.categoryBitMask : NoeudTable.CATEGORY_BIT_MASK])
        } else {
            hits = FacadeModele.instance.obtenirVue().editorView.hitTest(point, options: nil)
        }
        
        if (hits.count > 0) {
            let result: AnyObject = hits[0]
            if  result is SCNHitTestResult {
                let hitResult = result as! SCNHitTestResult
                return hitResult.localCoordinates
            }
        }
        
        return nil
    }
    
    /// Cette fonction trouve si deux segments se croisent
    static func segmentsIntersect(s11: GLKVector3, s12: GLKVector3, s21: GLKVector3, s22: GLKVector3) -> Bool {
        let u1 = s11;
        let v1 = GLKVector3Subtract(s12, s11)
        let u2 = s21;
        let v2 = GLKVector3Subtract(s22, s21)
    
        let d = (-v2.x * v1.z + v1.x * v2.z);
    
        if (d == 0) {
            return false;
        }
    
        let s = (-v1.z * (u1.x - u2.x) + v1.x * (u1.z - u2.z)) / d;
        let t = (v2.x * (u1.z - u2.z) - v2.z * (u1.x - u2.x)) / d;
    
        return 0.0 <= s && s <= 1.0 && 0.0 <= t && t <= 1.0;
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
    
    /// Convertit une chaîne hexadécimale en couleur RGB
    static func hexToUIColor(hex: String) -> UIColor {
        var str = hex
        
        if hex[hex.startIndex] == "#" {
            str.remove(at: str.startIndex)
        }
        
        var startIndex = str.index(str.startIndex, offsetBy: 0)
        var endIndex = str.index(str.startIndex, offsetBy: 1)
        let red = CGFloat.init(UInt8(str[startIndex...endIndex], radix: 16)!)
        
        startIndex = str.index(str.startIndex, offsetBy: 2)
        endIndex = str.index(str.startIndex, offsetBy: 3)
        let green = CGFloat.init(UInt8(str[startIndex...endIndex], radix: 16)!)
        
        startIndex = str.index(str.startIndex, offsetBy: 4)
        endIndex = str.index(str.startIndex, offsetBy: 5)
        let blue = CGFloat.init(UInt8(str[startIndex...endIndex], radix: 16)!)
        
        let color = UIColor.init(red: red/255.0, green: green/255.0, blue: blue/255.0, alpha: 0.50)
        
        return color
    }
    
    /// Retourne l'angle de rotation en radian autour de l'axe positif des y
    static func determinerAngleAxeY(rotation: SCNVector4) -> Float {
        // Rotation autour de l'axe positif des y
        if rotation.y > 0 {
            return rotation.w
        }
            // Rotation autour de l'axe négatif des y
        else {
            return ( 2 * Float.pi ) - rotation.w
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

