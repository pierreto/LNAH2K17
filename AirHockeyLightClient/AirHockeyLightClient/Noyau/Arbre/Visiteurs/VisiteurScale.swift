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
    
    /// Facteur de redimensionnement (grossir : >= 1.0, rapetisser : < 1.0)
    private var scale: Float
    
    /// Constructeur
    init(scale: Float) {
        self.scale = scale
    }
    
    /// Cette fonction permet d'assigner les facteurs de redimensionnement
    func assignerScale(scale: Float) {
        self.scale = scale
    }

    /// Visiter un accélérateur pour le rendre sélectionnable
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        if (noeud.estSelectionne()) {
            var scale = SCNVector3.init(noeud.scale.x * self.scale, noeud.scale.y * self.scale, noeud.scale.z * self.scale)
            
            if (scale.x <= 1.0) {
                scale = SCNVector3.init(1.0, 1.0, 1.0)
            }
            
            noeud.scale = scale
            
            // Envoyer la commande
            if ModeleEtatSelection.instance.isLastGestureRecognizer is UIPinchGestureRecognizer {
                FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                             pos: noeud.position,
                                                                                             rotation: MathHelper.determinerAngleAxeY(rotation: noeud.rotation),
                                                                                             scale: noeud.scale)
            }
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
            var scaleZ = self.scale * noeud.scale.z
            
            if (scaleZ <= 1.0) {
                scaleZ = 1.0
            }
            
            noeud.scale = SCNVector3.init(x: 1.0, y: 1.0, z: scaleZ)
            
            // Envoyer la commande
            if ModeleEtatSelection.instance.isLastGestureRecognizer is UIPinchGestureRecognizer {
                FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                             pos: noeud.position,
                                                                                             rotation: MathHelper.determinerAngleAxeY(rotation: noeud.rotation),
                                                                                             scale: noeud.scale)
            }
        }
    }
    
    /// Visiter un portail pour le rendre sélectionnable
    func visiterPortail(noeud: NoeudPortail) {
        if noeud.estSelectionne() {
            var scale = SCNVector3.init(noeud.scale.x * self.scale, noeud.scale.y * self.scale, noeud.scale.z * self.scale)
            
            if (scale.x <= 1) {
                scale = SCNVector3.init(1.0, 1.0, 1.0)
            }
            
            noeud.scale = scale
            
            // Envoyer la commande
            if ModeleEtatSelection.instance.isLastGestureRecognizer is UIPinchGestureRecognizer {
                FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                             pos: noeud.position,
                                                                                             rotation: MathHelper.determinerAngleAxeY(rotation: noeud.rotation),
                                                                                             scale: noeud.scale)
            }
        }
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
