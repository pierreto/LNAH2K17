///////////////////////////////////////////////////////////////////////////////
/// @file SCNNodeHelper.swift
/// @author Pierre To
/// @date 2017-10-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

class SCNNodeHelper {
    
    struct BoiteEnglobante {
        var coinMin: GLKVector3?
        var coinMax: GLKVector3?
    }
    
    /// Cette fonction retourne les points d'un noeud de l'arbre de rendu
    static func obtenirSommets(noeud: NoeudCommun) -> [SCNVector3] {
        let vertexSources = noeud.geometry?.getGeometrySources(for: SCNGeometrySource.Semantic.vertex)
        
        if let vertexSource = vertexSources?.first {
            let count = vertexSource.data.count / MemoryLayout<SCNVector3>.size
            return vertexSource.data.withUnsafeBytes {
                [SCNVector3](UnsafeBufferPointer<SCNVector3>(start: $0, count: count))
            }
        }
        
        return []
    }
    
    /// Cette fonction calcule la boite englobante d'un modèle 3D en espace
    /// local.
    static func calculerBoiteEnglobante(noeud: NoeudCommun) -> BoiteEnglobante {
        let dmin = Float.leastNormalMagnitude
        let dmax = Float.greatestFiniteMagnitude
        
        var xMin = SCNVector3.init(x: dmax, y: dmax, z: dmax)
        var xMax = SCNVector3.init(x: dmin, y: dmin, z: dmin)
        var yMin = SCNVector3.init(x: dmax, y: dmax, z: dmax)
        var yMax = SCNVector3.init(x: dmin, y: dmin, z: dmin)
        var zMin = SCNVector3.init(x: dmax, y: dmax, z: dmax)
        var zMax = SCNVector3.init(x: dmin, y: dmin, z: dmin)
        
        /// Récupérer les points extrêmes du modèle 3D
        SCNNodeHelper.obtenirPointsExtremes(noeud: noeud,
                                            xMin: &xMin, xMax: &xMax,
                                            yMin: &yMin, yMax: &yMax,
                                            zMin: &zMin, zMax: &zMax)
        
        // Le coin min est simplement les coordonnées minimales et le coin max est
        // simplement les coordonnées maximales.
        let coinMin = GLKVector3.init(v: (x: xMin.x, y: yMin.y, z: zMin.z))
        let coinMax = GLKVector3.init(v: (x: xMax.x, y: yMax.y, z: zMax.z))
        
        return self.BoiteEnglobante(coinMin: coinMin, coinMax: coinMax)
    }
    
    /// Cette fonction calcule les points extrêmes d'un noeud, récursivement.
    static func obtenirPointsExtremes(noeud: NoeudCommun,
                               xMin: inout SCNVector3, xMax: inout SCNVector3,
                               yMin: inout SCNVector3, yMax: inout SCNVector3,
                               zMin: inout SCNVector3, zMax: inout SCNVector3) {
        
        // On vérifie les coordonnées...
        let sommets = self.obtenirSommets(noeud: noeud)
        
        for sommet in sommets {
            if (sommet.x < xMin.x) {
                xMin = sommet;
            }
            else if (sommet.x > xMax.x){
                xMax = sommet;
            }
        
            if (sommet.y < yMin.y){
                yMin = sommet;
            }
            else if (sommet.y > yMax.y){
                yMax = sommet;
            }
        
            if (sommet.z < zMin.z){
                zMin = sommet;
            }
            else if (sommet.z > zMax.z) {
                zMax = sommet;
            }
        }
        
        // Calcul récursif des points extrêmes
        for noeud in noeud.childNodes {
            self.obtenirPointsExtremes(noeud: noeud as! NoeudCommun,
                                       xMin: &xMin, xMax: &xMax,
                                       yMin: &yMin, yMax: &yMax,
                                       zMin: &zMin, zMax: &zMax)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
