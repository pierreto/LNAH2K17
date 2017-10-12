///////////////////////////////////////////////////////////////////////////////
/// @file NoeudCommun.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class NoeudCommun
/// @brief Classe de base du patron composite utilisée pour créer l'arbre de rendu
///        Cette classe comprend l'interface de base que doivent implanter 
///        tous les noeuds pouvant être présent dans l'arbre de rendu
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
///////////////////////////////////////////////////////////////////////////
class NoeudCommun : SCNNode {
    
    /// Vecteur spécifiant les axes bloqués pour le déplacement
    private var axisLock: GLKVector3 = GLKVector3(v: (1.0, 0.0, 1.0))
    
    /// Sélection du noeud.
    private var selectionne: Bool = false
    
    /// Vrai si le noeud est sélectionnable.
    private var selectionnable: Bool = true
    
    /// Constructeur avec géométrie
    required init(type: String, geometry: SCNGeometry) {
        super.init()
        self.geometry = geometry
        self.name = type
    }
    
    /// Constructeur sans géométrie
    required init(type: String) {
        super.init()
        self.name = type
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Cette fonction permet d'itérer à travers tous les noeuds enfants avec le visiteur
    func accepterVisiteur(visiteur: VisiteurAbstrait) {
        for child in self.childNodes {
            let noeud = child as! NoeudCommun
            noeud.accepterVisiteur(visiteur: visiteur)
        }
    }
    
    /// Obtient le type du noeud
    func obtenirType() -> String {
        return self.name!
    }
    
    /// Cette fonction permet d'assigner l'état d'être sélectionné ou non du noeud.
    func assignerSelection(selectionne: Bool)
    {
        // Un objet non sélectionnable n'est jamais sélectionné.
        self.selectionne = (selectionne && self.selectionnable);
    
        if (self.selectionne) {
            self.effetFantome(activer: true);
        }
        else {
            self.effetFantome(activer: false);
        }
    }

    /// Cette fonction retourne l'état d'être sélectionné ou non du noeud.
    func estSelectionne() -> Bool
    {
        // Un objet non sélectionnable n'est jamais sélectionné.
        return (self.selectionne && self.selectionnable);
    }
    
    /// Désélectonne tous les noeuds qui sont sélectitonnés parmi les descendants de ce noeud, lui-même étant inclus
    func deselectionnerTout() {
        self.selectionne = false
        self.effetFantome(activer: false)
        
        for child in self.childNodes {
            let noeud = child as! NoeudCommun
            noeud.deselectionnerTout()
        }
    }
    
    /// Écrit si le noeud peut être sélectionné ou non.
    func assignerEstSelectionnable(selectionnable: Bool) {
        self.selectionnable = selectionnable
    }
    
    /// Vérifie si le noeud est sélectionnable.
    func estSelectionnable() -> Bool {
        return self.selectionnable
    }
    
    /// Obtient la position relative du noeud.
    func obtenirPositionRelative() -> GLKVector3 {
        let vector = GLKMatrix4GetColumn(SCNMatrix4ToGLKMatrix4(self.transform), 3)
        return GLKVector3Make(vector[0], vector[1], vector[2])
    }
    
    /// Assigne la position relative du noeud.
    func assignerPositionRelative(positionRelative: GLKVector3) {
        var inverse = self.axisLock
        
        inverse.x = inverse.x == 0.0 ? 1.0 : 0.0
        inverse.y = inverse.y == 0.0 ? 1.0 : 0.0
        inverse.z = inverse.z == 0.0 ? 1.0 : 0.0
        
        let p = GLKVector3Add(GLKVector3Multiply(positionRelative, self.axisLock), GLKVector3Multiply(self.obtenirPositionRelative(), inverse))
        
        var translateMatrix = GLKMatrix4()
        translateMatrix.m00 = 1.0
        translateMatrix.m11 = 1.0
        translateMatrix.m22 = 1.0
        translateMatrix.m33 = 1.0
        let transform = GLKMatrix4Translate(translateMatrix, p[0], p[1], p[2])
        self.transform = SCNMatrix4FromGLKMatrix4(transform)
    }
    
    /// Cette fonction permet de changer la position relative en conservant
    /// le scaling et la rotation
    func deplacer(position: GLKVector3) {
        self.assignerPositionRelative(positionRelative: position)
        
        //let glkRotation = SCNVector4ToGLKVector4(self.rotation)
        //let glkScale = GLKVector4MakeWithVector3(SCNVector3ToGLKVector3(self.scale), 0.0)
        //var glkTransform = SCNMatrix4ToGLKMatrix4(self.transform)
        //glkTransform = GLKMatrix4MultiplyVector4(glkTransform, GLKVector4Multiply(glkRotation, glkScale))
        
        //self.transform = SCNMatrix4FromGLKMatrix4(glkTransform)
        //transformationRelative_ *= rotation_ * scale_;
        
        //self.position = SCNVector3FromGLKVector3(position)
    }
    
    /// Permet de changer les axes de déplacement valide
    func assignerAxisLock(axisLock: GLKVector3) {
        self.axisLock = axisLock
    }
    
    /// Cette fonction applique une autre couleur sur le noeud
    func useOtherColor(activer: Bool, color: UIColor = UIColor(red: 1.0, green: 1.0, blue: 1.0, alpha: 1.0)) {
        let material = self.geometry?.firstMaterial
        
        if (activer) {
            material?.diffuse.contents = color
        } else {
            let defaultColor = UIColor(red: 1.0, green: 1.0, blue: 1.0, alpha: 1.0)
            material?.diffuse.contents = defaultColor
        }
    }
    
    /// Cette fonction active ou désactive un effet fantome sur le mur
    func effetFantome(activer: Bool) {
        let material = self.geometry?.firstMaterial?.copy() as! SCNMaterial
        
        if (material.diffuse.contents is UIColor) {
            self.geometry?.firstMaterial = material
            self.geometry?.firstMaterial?.lightingModel = SCNMaterial.LightingModel.phong
            self.geometry?.firstMaterial?.transparency = activer ? 0.25 : 1.0
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
