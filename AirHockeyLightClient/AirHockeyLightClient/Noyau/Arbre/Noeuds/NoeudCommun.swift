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
    
    /// Type du noeud
    private var type: String = ""
    
    /// Vecteur spécifiant les axes bloqués pour le déplacement
    private var axisLock: GLKVector3 = GLKVector3(v: (1.0, 0.0, 1.0))
    
    /// Vrai si le noeud est sélectionnable.
    private var selectionnable: Bool = true
    
    /// Constructeur avec géométrie
    required init(type: String, geometry: SCNGeometry) {
        super.init()
        self.geometry = geometry
        self.type = type
    }
    
    /// Constructeur sans géométrie
    required init(type: String) {
        super.init()
        
        self.type = type
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Obtient le type du noeud
    func obtenirType() -> String {
        return self.type
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
    
    /// Permet de changer les axes de déplacement valide
    func assignerAxisLock(axisLock: GLKVector3) {
        self.axisLock = axisLock
    }
    
    /// Cette fonction permet d'itérer à travers tous les noeuds enfants avec le visiteur
    func accepterVisiteur(visiteur: VisiteurAbstrait) {
        for child in self.childNodes {
            let noeud = child as! NoeudCommun
            noeud.accepterVisiteur(visiteur: visiteur)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
