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
    private var delta: GLKVector3?
    
    /// Constructeur
    init(delta: GLKVector3) {
        self.delta = delta
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
            pos = GLKVector3Add(pos, self.delta!)
            
            // Deplacer le premier noeud
            noeud.deplacer(position: pos)
            
            // Deplacer son opposé
            let posNoeudOppose = GLKVector3Multiply(pos, noeud.obtenirSymmetrie())
            noeud.obtenirNoeudOppose().deplacer(position: posNoeudOppose)
            
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserControlPointChanged(uuid: noeud.obtenirUUID(), pos: noeud.position)
            FacadeModele.instance.obtenirEtatEdition().currentUserControlPointChanged(uuid: noeud.obtenirNoeudOppose().obtenirUUID(),
                                                                                      pos: noeud.obtenirNoeudOppose().position)
            
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
        pos = GLKVector3Add(pos, self.delta!)
        noeud.deplacer(position: pos)
        
        // Envoyer la commande
        FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: noeud.obtenirUUID(),
                                                                                     pos: noeud.position, rotation: MathHelper.determinerAngleAxeY(rotation: noeud.rotation), scale: noeud.scale)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
