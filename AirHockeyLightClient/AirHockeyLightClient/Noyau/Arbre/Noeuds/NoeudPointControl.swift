///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPointControl.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-03
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class NoeudPointControl
/// @brief Classe pour afficher un point de contrôle
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-03
///////////////////////////////////////////////////////////////////////////
class NoeudPointControl : NoeudCommun {
    
    // TODO : Ajouter la symétrie et le rayon du modèle
    
    /// Zone de déplacement (-x, x, -z, z)
    var possedeZoneDeplacement: Bool = false
    var zoneDeplacement: GLKVector4 = GLKVector4()
    
    /// Voisins du point de contrôle
    var voisins = [NoeudPointControl]()
    
    /// Les points de la table (index des sommets du parent NoeudTable)
    var pointIndexes = [Int]()
    
    var noeudOppose: NoeudCommun?
    
    /// Vecteur applicant la symétrie au déplacement du noeud opposé
    var symetrie = GLKVector3()
    
    required init(type: String, geometry: SCNGeometry) {
        super.init(type: type, geometry: geometry)
        //self.addMaterial()
    }
    
    required init(type: String) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    override func assignerPositionRelative(positionRelative: GLKVector3) {
        //let anciennePosition = obtenirPositionRelative()
        var positionCorrigee = positionRelative
        
        // Limiter les points selon leur zone de deplacement
        if (self.possedeZoneDeplacement) {
            positionCorrigee.x = self.clamp(value: positionCorrigee.x, lower: self.zoneDeplacement[0], upper: self.zoneDeplacement[1])
            positionCorrigee.z = self.clamp(value: positionCorrigee.z, lower: self.zoneDeplacement[2], upper: self.zoneDeplacement[3])
        }
        
        // Appel a l'implementation de la classe de base
        super.assignerPositionRelative(positionRelative: positionCorrigee)
        
        // Update de la position des points openGl
        if (!voisins.isEmpty) {
            ajusterPoints();
            voisins[0].ajusterPoints();
            voisins[1].ajusterPoints();
        }
    }
    
    /// TODO : à mettre dans Utilitaire
    func clamp<T: Comparable>(value: T, lower: T, upper: T) -> T {
        return min(max(value, lower), upper)
    }
    
    /// Ajuste la position des points reliés au point de contrôle
    func ajusterPoints() {
        // Trouver le noeud parent
        let noeudTable = self.parent as! NoeudTable
        
        // Ajustment des points triviaux
        let position = super.obtenirPositionRelative();
        noeudTable.sommets[self.pointIndexes[0]].x = position.x
        noeudTable.sommets[self.pointIndexes[0]].z = position.z;
        noeudTable.sommets[self.pointIndexes[1]].x = position.x;
        noeudTable.sommets[self.pointIndexes[1]].z = position.z;
    
        let v1 = GLKVector3Normalize(GLKVector3Subtract(self.voisins[0].obtenirPositionRelative(), position));
        let v2 = GLKVector3Normalize(GLKVector3Subtract(self.voisins[1].obtenirPositionRelative(), position));
    
        // Calcul de la bisectrice
        var bisect: GLKVector3;
        if (GLKVector3Length(GLKVector3Add(v1, v2)) < 0.001) { // Droite perpendiculaire
            bisect = GLKVector3Normalize(GLKVector3Make(-v1.z, 0, v1.x));
        }
        else {
            bisect = GLKVector3Normalize(GLKVector3Add(v1, v2));
        }
    
        // Inverse bisect if needed
        let dotProduct = self.clamp(value: GLKVector3DotProduct(bisect, GLKVector3Normalize(position)), lower: -1.0, upper: 1.0);
        if (acos(dotProduct) > Float.pi/2) {
            bisect = GLKVector3Negate(bisect);
        }
    
        // Calcul de la diagonale de la bordure
        let angle = Float.pi/2 - acos(self.clamp(value: GLKVector3DotProduct(v1, v2), lower: -1.0, upper: 1.0)) / 2;
        let h =  Float(Table.TABLE_BORDER_WIDTH) / cos(angle);
    
        // Ajustement des points de la bordure interne
        let p0 = GLKVector3Add(position, GLKVector3MultiplyScalar(bisect, h));
        noeudTable.sommets[self.pointIndexes[2]].x = p0.x;
        noeudTable.sommets[self.pointIndexes[2]].z = p0.z;
        noeudTable.sommets[self.pointIndexes[3]].x = p0.x;
        noeudTable.sommets[self.pointIndexes[3]].z = p0.z;
    
        let p4 = GLKVector3Add(position, GLKVector3MultiplyScalar(GLKVector3Negate(bisect), 10));
        noeudTable.sommets[self.pointIndexes[4]].x = p4.x;
        noeudTable.sommets[self.pointIndexes[4]].z = p4.z;
    
        // Update de la position du but
        /*if (but_) {
            but_->ajusterPoints();
        }
    
        // Update de la position du booster
        if (booster_ != nullptr)
        {
            glm::vec3 p5 = p4 - bisect * 5.0f;
            booster_->deplacer(p5);
        }*/
    }
    
    /// Assigne les index des sommets du NoeudTable
    func assignerPointIndexes(pointIndexes: [Int]) {
        self.pointIndexes = pointIndexes
    }
    
    /// Assigne un noeud opposé au noeud présent
    func assignerNoeudOppose(noeud: NoeudCommun, symetrie: GLKVector3) {
        self.noeudOppose = noeud
        self.symetrie = symetrie
    }
    
    /// Assigne des voisins au point de contrôle
    func assignerVoisins(voisins: [NoeudPointControl]) {
        self.voisins = voisins
    }
    
    /// Assigne une zone dans lequel un noeud peut se déplacer
    func assignerZoneDeplacement(zone: GLKVector4) {
        self.possedeZoneDeplacement = true
        self.zoneDeplacement = zone
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
