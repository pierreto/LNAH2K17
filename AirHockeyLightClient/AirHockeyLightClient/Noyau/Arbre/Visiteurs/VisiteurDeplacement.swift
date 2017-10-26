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
    
    /// Dernière position du toucher
    private var lastPosition: CGPoint?
    
    /// Position du toucher
    private var position: CGPoint?
    
    /// Constructeur
    init(lastPosition: CGPoint, position: CGPoint) {
        self.lastPosition = lastPosition
        self.position = position
    }
    
    /// Visiter un accélérateur pour le déplacement
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        if (noeud.estSelectionne()) {
            self.deplacerNoeud(noeud: noeud)
        }
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
            pos = self.obtenirDeplacement(pos: pos)
            
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
        if (noeud.estSelectionne()) {
            // Sauvegarder scale et rotation
            // TODO : trouver une meilleure solution pour conserver le scaling et la rotation lors d'un déplacement
            let scale = noeud.scale
            let rotation = noeud.rotation
            
            self.deplacerNoeud(noeud: noeud)
            
            // Remettre le scale et la rotation du mur
            noeud.scale = scale
            noeud.rotation = rotation
        }
    }
    
    /// Visiter un portail pour le déplacement
    func visiterPortail(noeud: NoeudPortail) {
        if (noeud.estSelectionne()) {
            self.deplacerNoeud(noeud: noeud)
        }
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
    /// Déplace le noeud
    private func deplacerNoeud(noeud: NoeudCommun) {
        var pos = noeud.obtenirPositionRelative()
        pos = self.obtenirDeplacement(pos: pos)
        
        // Deplacer le premier noeud
        noeud.deplacer(position: pos)
    }
    
    /// Détermine le déplacement (delta)
    private func obtenirDeplacement(pos: GLKVector3) -> GLKVector3 {
        let start = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView, depth: pos.z, point: self.lastPosition!)
        let end = MathHelper.CGPointToSCNVector3(view: FacadeModele.instance.obtenirVue().editorView, depth: pos.z, point: self.position!)
        let delta = GLKVector3Make(end.x - start.x, end.y - start.y, end.z - start.z)
        
        return GLKVector3Add(pos, delta)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
