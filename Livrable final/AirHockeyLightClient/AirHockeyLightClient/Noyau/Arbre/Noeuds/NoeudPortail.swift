///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPortail.swift
/// @author Pierre To
/// @date 2017-10-18
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class NoeudPortail
/// @brief Classe pour afficher un portail
///
/// @author Pierre To
/// @date 2017-10-18
///////////////////////////////////////////////////////////////////////////
class NoeudPortail : NoeudCommun {
    
    /// Angle selon l'axe des Y
    private var angleY: Float?
    
    /// Le portail opposé
    private var portailOppose: NoeudPortail?
    
    /// Retient si le portail est actif
    private var desactiver: Bool = false
    
    /// Constructeur
    required init(type: String, geometry: SCNGeometry, uuid: String) {
        super.init(type: type, geometry: geometry, uuid: uuid)
        
        // Faire une animation de rotation
        let spin = CABasicAnimation(keyPath: "rotation")
        spin.fromValue = SCNVector4(x: 0, y: 1, z: 0, w: 0)
        spin.toValue = SCNVector4(x: 0, y: 1, z: 0, w: Float(2 * Float.pi))
        spin.duration = 5
        spin.repeatCount = .infinity
        self.addAnimation(spin, forKey: "spin around")
        
        // Couleur par défaut et lorsque sélectionnable
        let defaultColor = UIColor(red: 255/255.0, green: 166/255.0, blue: 0/255.0, alpha: 1.0)
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
    
    /// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
    override func accepterVisiteur(visiteur: VisiteurAbstrait) {
        // Envoie le visiteur aux enfants
        super.accepterVisiteur(visiteur: visiteur)
        
        visiteur.visiterPortail(noeud: self)
    }
    
    /// Cette fonction assigne un noeud opposé à un portail
    func assignerOppose(portail: NoeudPortail?) {
        self.portailOppose = portail
    }
    
    /// Cette fonction permet d'obtenir le portail opposé
    func obtenirOppose() -> NoeudPortail? {
        return self.portailOppose
    }
    
    /// Activer/Désactiver le noeud
    func assignerDesactiver(desactiver: Bool) {
        self.desactiver = desactiver
    }
    
    /// Savoir si le noeud est désactivé
    func estDesactiver() -> Bool {
        return self.desactiver;
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
