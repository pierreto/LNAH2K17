///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDeplacement.swift
/// @author Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurDeplacement
/// @brief Permet de deplacer les noeuds
///
/// @author Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class VisiteurDeplacement: VisiteurAbstrait {
    
    /// Vecteur trois dimensions pour le changement de position de l'objet
    var delta: GLKVector3?
    
    /// Dernière position du toucher
    var lastPosition: CGPoint?
    
    /// Position du toucher
    var position: CGPoint?
    
    /// Constructeur
    init(lastPosition: CGPoint, position: CGPoint) {
        self.lastPosition = lastPosition
        self.position = position
    }
    
    /// Visiter un accélérateur pour le déplacement
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
    }
    
    /// Visiter un maillet pour le déplacement
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour le déplacement
    func visiterTable(noeud: NoeudTable) {
    }
    
    /// Visiter un point de contrôle pour le déplacement
    func visiterPointControl(noeud: NoeudPointControl) {
        if (noeud.estSelectionne()) {
            var pos = noeud.obtenirPositionRelative()
            
            let start = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView, depth: pos.z, point: self.lastPosition!)
            let end = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView, depth: pos.z, point: self.position!)
            self.delta = GLKVector3Make(end.x - start.x, end.y - start.y, end.z - start.z)
            
            pos = GLKVector3Add(pos, self.delta!)
            
            // Deplacer le premier noeud
            noeud.deplacer(position: pos)
            
            // Deplacer son opposé
            let posNoeudOppose = GLKVector3Multiply(pos, noeud.obtenirSymmetrie())
            noeud.obtenirNoeudOppose().deplacer(position: posNoeudOppose)
            
            // TODO : À vérifier si on change la géométrie de la table ici ou non
            let table = noeud.parent as! NoeudTable
            table.updateGeometry()
        }
    }
    
    /// Visiter un mur pour le déplacement
    func visiterMur(noeud: NoeudMur) {
    }
    
    /// Visiter un portail pour le déplacement
    func visiterPortail(noeud: NoeudPortail) {
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
