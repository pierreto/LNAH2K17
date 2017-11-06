///////////////////////////////////////////////////////////////////////////////
/// @file NodeCreator.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class NodeCreator
/// @brief Classe qui gère la création de noeud
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class NodeCreator {
    
    // Singleton
    static let instance = NodeCreator()
    
    func createBoost(uuid: String, pos: GLKVector3, angle: Float, scale: GLKVector3) {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        let noeud = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_ACCELERATEUR, uuid: uuid) as! NoeudAccelerateur
        noeud.assignerPositionRelative(positionRelative: pos)
        noeud.rotation = SCNVector4(0.0, 1.0, 0.0, angle)
        noeud.scale = SCNVector3FromGLKVector3(scale)
        
        // Ajout du noeud à l'arbre de rendu
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        table.addChildNode(noeud)
    }
    
    func createWall(uuid: String, position: GLKVector3, angle: Float, scale: GLKVector3) {
    
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        let noeud = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_MUR, uuid: uuid) as! NoeudMur
        
        noeud.assignerPositionRelative(positionRelative: position)
        noeud.rotation = SCNVector4(0.0, 1.0, 0.0, angle)
        noeud.scale = SCNVector3FromGLKVector3(scale)
        
        
        // Ajout du noeud à l'arbre de rendu
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        table.addChildNode(noeud)
    }
    
    func createPortal(startUuid: String, startPos: GLKVector3, startAngle: Float, startScale: GLKVector3,
                      endUuid: String, endPos: GLKVector3, endAngle: Float, endScale: GLKVector3) {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        let noeud1 = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_PORTAIL, uuid: startUuid) as! NoeudPortail
        noeud1.assignerPositionRelative(positionRelative: startPos)
        noeud1.rotation = SCNVector4(0.0, 1.0, 0.0, startAngle)
        noeud1.scale = SCNVector3FromGLKVector3(startScale)
        
        let noeud2 = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_PORTAIL, uuid: endUuid) as! NoeudPortail
        noeud2.assignerPositionRelative(positionRelative: endPos)
        noeud2.rotation = SCNVector4(0.0, 1.0, 0.0, endAngle)
        noeud2.scale = SCNVector3FromGLKVector3(endScale)
    
        noeud1.assignerOppose(portail: noeud2)
        noeud2.assignerOppose(portail: noeud1)
        
        // Ajout du noeud à l'arbre de rendu
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        table.addChildNode(noeud1)
        table.addChildNode(noeud2)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
