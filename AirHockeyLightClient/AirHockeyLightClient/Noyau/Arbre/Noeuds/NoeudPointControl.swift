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
    
    /// Zone de déplacement (-x, x, -z, z)
    private var possedeZoneDeplacement: Bool = false
    private var zoneDeplacement: GLKVector4 = GLKVector4()
    
    /// Voisins du point de contrôle
    private var voisins = [NoeudPointControl]()
    
    /// Les points de la table (index des sommets du parent NoeudTable)
    private var pointIndexes = [Int]()
    
    /// Point de contrôle opposé sur la table
    private var noeudOppose: NoeudCommun?
    
    /// But associé au point de contrôle
    private var but: NoeudBut?
    
    /// Vecteur applicant la symétrie au déplacement du noeud opposé
    private var symetrie = GLKVector3()
    
    /// Rayon du modèle 3D
    private var rayonModele3D = -1.0
    
    /// Constructeur
    required init(type: String, geometry: SCNGeometry, uuid: String) {
        super.init(type: type, geometry: geometry, uuid: uuid)
        self.symetrie = GLKVector3(v: (1.0, 1.0, 1.0))
        
        // Un point de contrôle n'est pas sélectionnable par défaut
        self.assignerEstSelectionnable(selectionnable: false)
        
        // Couleur du point de contrôle par défaut
        self.assignerDefaultColor(color: self.geometry?.firstMaterial?.diffuse.contents as! UIColor)
        
        // Couleur du point de contrôle lorsqu'il est sélectionnable
        let selectionnableColor = UIColor(red: 1.0, green: 85.0/255.0, blue: 82.0/255.0, alpha: 1.0)
        self.assignerSelectionnableColor(color: selectionnableColor)
    }
    
    /// Le point de contrôle a un modèle obj
    required init(type: String, uuid: String) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
    override func accepterVisiteur(visiteur: VisiteurAbstrait) {
        // Envoie le visiteur aux enfants
        super.accepterVisiteur(visiteur: visiteur)
        
        visiteur.visiterPointControl(noeud: self)
    }
    
    /// Surchage de la fonction assignerPositionRelative
    override func assignerPositionRelative(positionRelative: GLKVector3) {
        //let anciennePosition = obtenirPositionRelative()
        var positionCorrigee = positionRelative
        
        // Limiter les points selon leur zone de deplacement
        if (self.possedeZoneDeplacement) {
            positionCorrigee.x = MathHelper.clamp(value: positionCorrigee.x,
                                                  lower: self.zoneDeplacement[0],
                                                  upper: self.zoneDeplacement[1])
            positionCorrigee.z = MathHelper.clamp(value: positionCorrigee.z,
                                                  lower: self.zoneDeplacement[2],
                                                  upper: self.zoneDeplacement[3])
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
        let dotProduct = MathHelper.clamp(value: GLKVector3DotProduct(bisect, GLKVector3Normalize(position)), lower: -1.0, upper: 1.0);
        if (acos(dotProduct) > Float.pi/2) {
            bisect = GLKVector3Negate(bisect);
        }
    
        // Calcul de la diagonale de la bordure
        let angle = Float.pi/2 - acos(MathHelper.clamp(value: GLKVector3DotProduct(v1, v2), lower: -1.0, upper: 1.0)) / 2;
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
        if (self.but != nil) {
            self.but?.ajusterPoints();
        }
    
        // Update de la position du booster
        /*if (booster_ != nullptr)
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
    
    /// Cette fonction renvoie la symmetrie
    func obtenirSymmetrie() -> GLKVector3
    {
        return self.symetrie;
    }

    /// Cette fonction retourne un pointeur vers le noeud oppose
    func obtenirNoeudOppose() -> NoeudPointControl
    {
        return self.noeudOppose as! NoeudPointControl;
    }
    
    /// Assigne des voisins au point de contrôle
    func assignerVoisins(voisins: [NoeudPointControl]) {
        self.voisins = voisins
    }

    /// Retourne les voisins du point de contrôle
    func obtenirVoisins() -> [NoeudPointControl]
    {
        return self.voisins;
    }
    
    /// Assigne une zone dans lequel un noeud peut se déplacer
    func assignerZoneDeplacement(zone: GLKVector4) {
        self.possedeZoneDeplacement = true
        self.zoneDeplacement = zone
    }
    
    /// Cette fonction assigne un but au point de controle
    func assignerBut(noeud: NoeudBut) {
        self.but = noeud
        noeud.assignerPointControl(pointControl: self)
    }
    
    /// Cette fonction retourne le but auquel le noeud est associe
    func obtenirBut() -> NoeudBut {
        return but!
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
