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
    
    func createBoost(uuid: String, pos: GLKVector3) {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        let noeud = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_ACCELERATEUR, uuid: uuid) as! NoeudAccelerateur
        noeud.assignerPositionRelative(positionRelative: pos)
        
        // Ajout du noeud à l'arbre de rendu
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        table.addChildNode(noeud)
    }
    
    func createWall(uuid: String, startPos: GLKVector3, endPos: GLKVector3) {
    
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        let noeud = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_MUR, uuid: uuid) as! NoeudMur
        
        // Déplacement du noeud
        // Transformation du point dans l'espace virtuelle
        noeud.assignerAxisLock(axisLock: GLKVector3.init(v: (1.0, 1.0, 1.0)))
        noeud.assignerPositionRelative(positionRelative: GLKVector3.init(v: (startPos.x, startPos.y + 0.01, startPos.z)))
        noeud.assignerAxisLock(axisLock: GLKVector3.init(v: (1.0, 0.0, 1.0)))
        
        // Calculer la distance pour le scaling
        let distance = GLKVector3Distance(endPos, startPos)
        noeud.scale = SCNVector3(1.0, 1.0, distance)
        
        // Calculer l'angle pour la rotation
        let node2mouse = GLKVector3Subtract(endPos, startPos)
        let x = GLKVector3.init(v: (0, 0, 1))
        let y = GLKVector3Normalize(node2mouse)
        let dotProduct = GLKVector3DotProduct(x, y)
        let clampedDot = MathHelper.clamp(value: dotProduct, lower: -1, upper: 1)
        let angle = acos(clampedDot)
        
        let ref = GLKVector3.init(v: (0, 1, 0))
        let crossProduct = GLKVector3CrossProduct(x, y)
        let dotCrossProduct = GLKVector3DotProduct(ref, crossProduct)
        let finalAngle = dotCrossProduct >= 0 ? angle : -angle
        
        noeud.rotation = SCNVector4(0.0, 1.0, 0.0, finalAngle)
        
        // Calculer le déplacement
        var position = startPos
        position.x += Float(sin(finalAngle) * distance / 2)
        position.z += Float(cos(finalAngle) * distance / 2)
        noeud.position = SCNVector3FromGLKVector3(position)
        
        // Ajout du noeud à l'arbre de rendu
        let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
        table.addChildNode(noeud)
    }
    
    func createPortal(startUuid: String, portal1Pos: GLKVector3, endUuid: String, portal2Pos: GLKVector3) {
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        let noeud1 = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_PORTAIL, uuid: startUuid) as! NoeudPortail
        noeud1.assignerPositionRelative(positionRelative: portal1Pos)
        
        let noeud2 = arbre.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_PORTAIL, uuid: endUuid) as! NoeudPortail
        noeud2.assignerPositionRelative(positionRelative: portal2Pos)
    
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
