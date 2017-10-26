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

// TODO : Déplacer dans utilitaire
struct BoiteEnglobante {
    var coinMin: GLKVector3?
    var coinMax: GLKVector3?
}

///////////////////////////////////////////////////////////////////////////
/// @class NoeudMur
/// @brief Classe pour afficher un point de contrôle
///
/// @author Mikael Ferland
/// @date 2017-10-18
///////////////////////////////////////////////////////////////////////////
class NoeudMur : NoeudCommun {

    private var collider: BoiteEnglobante?
    
    /// Constructeur
    required init(type: String, geometry: SCNGeometry) {
        super.init(type: type, geometry: geometry)
        self.collider = BoiteEnglobante(coinMin: GLKVector3.init(v: (0, 0, 0)), coinMax: GLKVector3.init(v : (0,0,0)));
    }
    
    /// Le point de contrôle a un modèle obj
    required init(type: String) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    func obtenirCollider() -> BoiteEnglobante {
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
