///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurScale.swift
/// @author Pierre To
/// @date 2017-10-22
/// @version 0.1
///
/// @addtogroup log3900 LOG3900
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit
import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurScale
/// @brief Permet de scaler les noeuds
///
///
/// @author Pierre To
/// @date 2017-10-22
///////////////////////////////////////////////////////////////////////////
class VisiteurScale: VisiteurAbstrait {
    
    /// Vecteur trois dimensions pour le redimensionnement
    private var scale: GLKVector3
    
    /// Bool d'ajustement pour le redimensionnement
    private var ajusterScaling: Bool
    
    /// Constructeur
    init(scale: GLKVector3, ajusterScaling: Bool) {
        self.scale = scale
        self.ajusterScaling = ajusterScaling
    }
    
    /// Cette fonction permet d'assigner les facteurs de redimensionnement
    func assignerScale(scale: GLKVector3) {
        self.scale = scale
    }

    /// Visiter un accélérateur pour le rendre sélectionnable
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        if (noeud.estSelectionne()) {
            var scale = GLKVector3Add(
                            SCNVector3ToGLKVector3(noeud.scale),
                            (self.ajusterScaling ? GLKVector3DivideScalar(self.scale, 6.0) : self.scale))
            
            if (scale.x <= 1.0) {
                scale = GLKVector3.init(v: (1.0, 1.0, 1.0))
            }
            
            noeud.scale = SCNVector3FromGLKVector3(scale)
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                         pos: noeud.position, rotation: noeud.rotation.w, scale: noeud.scale)
        }
    }
    
    /// Visiter un maillet pour le rendre sélectionnable
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour le rendre sélectionnable
    func visiterTable(noeud: NoeudTable) {
        // Ne rien faire
    }
    
    /// Visiter un point de contrôle pour le rendre sélectionnable
    func visiterPointControl(noeud: NoeudPointControl) {
        // Ne rien faire
    }
    
    /// Visiter un mur pour le rendre sélectionnable
    func visiterMur(noeud: NoeudMur) {
        if noeud.estSelectionne() {
            var scale = GLKVector3Add(SCNVector3ToGLKVector3(noeud.scale), self.scale)
            
            if (scale.z <= 1.0) {
                scale.z = 1.0
            }
            
            noeud.scale = SCNVector3.init(x: 1.0, y: 1.0, z: scale.z)
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                         pos: noeud.position, rotation: noeud.rotation.w, scale: noeud.scale)
        }
    }
    
    /// Visiter un portail pour le rendre sélectionnable
    func visiterPortail(noeud: NoeudPortail) {
        if noeud.estSelectionne() {
            var scale = GLKVector3Add(
                SCNVector3ToGLKVector3(noeud.scale),
                (self.ajusterScaling ? GLKVector3DivideScalar(self.scale, 12.0) : self.scale))
            
            if (scale.x <= 1) {
                scale = GLKVector3.init(v: (1.0, 1.0, 1.0))
            }
            
            noeud.scale = SCNVector3FromGLKVector3(scale)
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                         pos: noeud.position, rotation: noeud.rotation.w, scale: noeud.scale)
        }
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
