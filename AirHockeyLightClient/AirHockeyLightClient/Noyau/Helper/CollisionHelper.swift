///////////////////////////////////////////////////////////////////////////////
/// @file CollisionHelper.swift
/// @author Pierre To
/// @date 2017-10-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation
import SceneKit

class CollisionHelper {
    
    /// Vérifie si un point est à l'intérieur d'un triangle
    static func pointDansTriangle(point: SCNVector3, t1: SCNVector3, t2: SCNVector3, t3: SCNVector3) -> Bool {
        
        let denominateur = (t2.z - t3.z) * (t1.x - t3.x) + (t3.x - t2.x) * (t1.z - t3.z)
        let alpha = ((t2.z - t3.z) * (point.x - t3.x) + (t3.x - t2.x) * (point.z - t3.z)) / denominateur
        let beta = ((t3.z - t1.z)*(point.x - t3.x) + (t1.x - t3.x)*(point.z - t3.z)) / denominateur
        let gamma = 1.0 - alpha - beta
    
        return 0 <= alpha && alpha <= 1 && 0 <= beta && beta <= 1 && 0 <= gamma && gamma <= 1
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
