///////////////////////////////////////////////////////////////////////////////
/// @file NoeudMur.swift
/// @author Mikael Ferland
/// @date 2017-10-18
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class NoeudMur
/// @brief Classe pour afficher un point de contrôle
///
/// @author Mikael Ferland
/// @date 2017-10-18
///////////////////////////////////////////////////////////////////////////
class NoeudMur : NoeudCommun {

    private var collider: SCNNodeHelper.BoiteEnglobante?
    
    /// Constructeur
    required init(type: String, geometry: SCNGeometry, uuid: String) {
        super.init(type: type, geometry: geometry, uuid: uuid)
        self.collider = SCNNodeHelper.BoiteEnglobante(coinMin: GLKVector3.init(v: (0, 0, 0)),
                                                      coinMax: GLKVector3.init(v: (0, 0, 0)));
        
        // Couleur par défaut et lorsque sélectionnable
        let defaultColor = UIColor(red: 222.0/255.0, green: 187.0/255.0, blue: 141.0/255.0, alpha: 1.0)
        self.assignerDefaultColor(color: defaultColor)
        self.assignerSelectionnableColor(color: defaultColor)
        self.geometry?.firstMaterial?.diffuse.contents = defaultColor
    }
    
    /// Le point de contrôle a un modèle obj
    required init(type: String, uuid: String) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    func obtenirCollider() -> SCNNodeHelper.BoiteEnglobante {
        return self.collider!;
    }
    
    // TODO : implémenter collider
    func calculerCollider() { }
    
    /// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
    override func accepterVisiteur(visiteur: VisiteurAbstrait) {
        // Envoie le visiteur aux enfants
        super.accepterVisiteur(visiteur: visiteur)
        
        visiteur.visiterMur(noeud: self)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
