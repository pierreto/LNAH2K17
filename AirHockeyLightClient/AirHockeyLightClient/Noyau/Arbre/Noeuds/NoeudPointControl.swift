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
    
    /// Les points de la table
    var points = [SCNVector3]()
    
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
        
        // TODO : À implémenter
        // Update de la position des points openGl
        /*if (voisins.count) {
            ajusterPoints();
            voisins[0]->ajusterPoints();
            voisins[1]->ajusterPoints();
        }*/
    }
    
    /// TODO : à mettre dans Utilitaire
    func clamp<T: Comparable>(value: T, lower: T, upper: T) -> T {
        return min(max(value, lower), upper)
    }
    
    // TODO : A implementer
    /// Ajuste la position des points reliés au point de contrôle
    /*void NoeudPointControl::ajusterPoints()
    {
        // Ajustment des points triviaux
        glm::vec3 position = obtenirPositionRelative();
        points_[0]->x = position.x; points_[0]->z = position.z;
        points_[1]->x = position.x; points_[1]->z = position.z;
    
        glm::vec3 v1 = glm::normalize(voisins_[0]->obtenirPositionRelative() - position);
        glm::vec3 v2 = glm::normalize(voisins_[1]->obtenirPositionRelative() - position);
    
        // Calcul de la bisectrice
        glm::vec3 bisect;
        if ( glm::length(v1 + v2) < 0.001f) { // Droite perpendiculaire
            bisect = glm::normalize(glm::vec3(-v1.z, 0, v1.x));
        }
        else {
            bisect = glm::normalize(v1 + v2);
        }
    
        // Inverse bisect if needed
        float dotProduct = glm::clamp(glm::dot(bisect, glm::normalize(position)),-1.0f,1.0f);
        if (glm::acos(dotProduct) > utilitaire::PI/2)
            bisect = -bisect;
    
        // Calcul de la diagonale de la bordure
        float angle = utilitaire::PI/2 - glm::acos(glm::clamp(glm::dot(v1, v2),-1.0f, 1.0f))/2;
        float h =  TABLE_BORDER_WIDTH / glm::cos(angle);
    
        // Ajustement des points de la bordure interne
        glm::vec3 p0 = position + (bisect * h);
        points_[2]->x = p0.x; points_[2]->z = p0.z;
        points_[3]->x = p0.x; points_[3]->z = p0.z;
    
        glm::vec3 p4 = position + -bisect * 10.0f;
        points_[4]->x = p4.x; points_[4]->z = p4.z;
    
        // Update de la position du but
        if (but_) {
            but_->ajusterPoints();
        }
    
        // Update de la position du booster
        if (booster_ != nullptr)
        {
            glm::vec3 p5 = p4 - bisect * 5.0f;
            booster_->deplacer(p5);
        }
    }*/
    
    /// Assigne les points de la table au noeud
    func assignerPoints(points: [SCNVector3]) {
        self.points = points
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
