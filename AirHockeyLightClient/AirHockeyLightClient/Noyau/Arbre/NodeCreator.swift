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
