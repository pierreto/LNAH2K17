///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSurTable.swift
/// @author Pierre To
/// @date 2017-10-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSurTable
/// @brief Permet de savoir si les noeuds sont sur la table
///
/// @author Pierre To
/// @date 2017-10-19
///////////////////////////////////////////////////////////////////////////
class VisiteurSurTable: VisiteurAbstrait {
    
    /// Table
    private var table: NoeudTable?
    
    /// Indique si les noeuds sont sur la table
    private var sontSurTable: Bool = true
    
    /// Constructeur
    init() {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        self.table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as? NoeudTable
    }
    
    /// Retourne si les noeuds sont sur la table
    func obtenirSontSurTable() -> Bool
    {
        return self.sontSurTable;
    }
    
    /// Visiter un accélérateur pour la vérification sur la table
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        if table != nil {
            self.testPoints(noeud: noeud)
            self.intersectCercleTable(noeud: noeud, rayon: 3.75)
        }
    }
    
    /// Visiter un maillet pour la vérification sur la table
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour la vérification sur la table
    func visiterTable(noeud: NoeudTable) {
    }
    
    /// Visiter un point de contrôle pour la vérification sur la table
    func visiterPointControl(noeud: NoeudPointControl) {
    }
    
    /// Visiter un mur pour la vérification sur la table
    func visiterMur(noeud: NoeudMur) {
    }
    
    /// Visiter un portail pour la vérification sur la table
    func visiterPortail(noeud: NoeudPortail) {
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
    /// Cette fonction test si les points d'un noeud sont tous sur la table
    private func testPoints(noeud: NoeudCommun) {
        // Ramasser les sommets du noeuds
        var sommets: [SCNVector3] = self.obtenirSommets(noeud: noeud)
    
        // Appliquer les transformations sur les points
        for i in 0..<sommets.count {
            var sommetTransforme = GLKVector4.init(v: (sommets[i].x, sommets[i].y, sommets[i].z, 1.0))
            sommetTransforme = GLKMatrix4MultiplyVector4(SCNMatrix4ToGLKMatrix4(noeud.transform), sommetTransforme)
            sommets[i] = SCNVector3.init(sommetTransforme.x, sommetTransforme.y, sommetTransforme.z)
        }
    
        // Ajouter le centre du noeud
        sommets.append(SCNVector3FromGLKVector3(noeud.obtenirPositionRelative()))
    
        // Enleve les doublons
        sommets = self.enleverDoublons(vec: sommets);
    
        // Verifier que tous les noeuds sont sur la table
        for sommet in sommets {
            if (!pointSurTable(sommet: sommet)) {
                self.sontSurTable = false;
                return;
            }
        }
    }
    
    /// Cette fonction retourne les points d'un noeud de l'arbre de rendu
    private func obtenirSommets(noeud: NoeudCommun) -> [SCNVector3] {
        let vertexSources = noeud.geometry?.getGeometrySources(for: SCNGeometrySource.Semantic.vertex)
        
        if let vertexSource = vertexSources?.first {
            let count = vertexSource.data.count / MemoryLayout<SCNVector3>.size
            return vertexSource.data.withUnsafeBytes {
                [SCNVector3](UnsafeBufferPointer<SCNVector3>(start: $0, count: count))
            }
        }
        
        return []
    }
    
    /// Cette fonction enleve les doublons pour un vecteur de sommets
    func enleverDoublons(vec: [SCNVector3]) -> [SCNVector3] {
        var uniqueVector = [SCNVector3]()
        
        uniqueVector = vec.sorted(by: {
            if $0.x < $1.x {
                return true
            } else if ($0.x == $1.x) {
                if ($0.y < $1.y) {
                    return true
                }
                else if ($0.y == $1.y) {
                    if ($0.z < $1.z) {
                        return true
                    }
                }
            }
            
            return false
        })
        
        for i in 0..<(vec.count - 1) {
            if uniqueVector[i].x == uniqueVector[i+1].x
            && uniqueVector[i].y == uniqueVector[i+1].y
            && uniqueVector[i].z == uniqueVector[i+1].z {
                uniqueVector[i] = SCNVector3Zero
            }
        }
        
        uniqueVector = uniqueVector.filter({
            return $0.x == 0 && $0.y == 0 && $0.z == 0
        })
        
        return uniqueVector
    }
    
    /// Cette fonction trouve si un point est situé sur la table
    func pointSurTable(sommet: SCNVector3) -> Bool {
        let sommetsTable = self.table?.obtenirSommetsPatinoire()
        let max = (sommetsTable?.count)! - 1
        
        // Si le point est dans l'un des triangles formant la table, on retourne vrai
        for i in 1...max {
            if CollisionHelper.pointDansTriangle(
                point: sommet,
                t1: sommetsTable![0],
                t2: sommetsTable![i],
                t3: sommetsTable![(i % (max)) + 1]) {
                    return true;
            }
        }
    
        return false;
    }
    
    /// Cette fonction trouve si un accélérateur croise les côtés de la table
    func intersectCercleTable(noeud: NoeudCommun, rayon: Float) {
        // Les sommets de la table
        let sommetsTable = self.table?.obtenirSommetsPatinoire()
    
        // Obtenir le scale maximal
        let scale = noeud.scale
        let scaleValues: [Float] = [scale.x, scale.y, scale.z]
        let max_scale = scaleValues.max()
    
        // Rayon et centre du cercle
        let rayonCercle = rayon * max_scale!;
        let centre = noeud.obtenirPositionRelative();
    
        // Effectuer le test
        let max = (sommetsTable?.count)! - 1
        for i in 1...max {
            if (MathHelper.segmentCercleIntersect(
                s1: SCNVector3ToGLKVector3(sommetsTable![i]),
                s2: SCNVector3ToGLKVector3(sommetsTable![(i % max) + 1]),
                centreCercle: centre,
                rayon: rayonCercle)) {
                self.sontSurTable = false;
            }
        }
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
