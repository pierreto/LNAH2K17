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

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurDeplacement
/// @brief Permet de deplacer les noeuds
///
/// @author Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class VisiteurDeplacement: VisiteurAbstrait {
    
    /// Vecteur trois dimensions pour le changement de position de l'objet
    var delta: GLKVector3
    
    /// Constructeur
    init(delta: GLKVector3) {
        self.delta = delta
    }
    
    /// Visiter un accélérateur pour le déplacer
    //virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
    
    /// Visiter un maillet pour le déplacer
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour le déplacer
    func visiterTable(noeud: NoeudTable) {
    }
    
    /// Visiter un point de contrôle pour le déplacer
    func visiterPointControl(noeud: NoeudPointControl) {
        // TODO : implémenter la sélection
        if (noeud.estSelectionne()) {
            var pos = noeud.obtenirPositionRelative()
            pos = GLKVector3Add(pos, self.delta)
            
            // Deplacer le premier noeud
            noeud.deplacer(position: pos)
            
            // Deplacer son opposé
            let posNoeudOppose = GLKVector3Multiply(pos, noeud.obtenirSymmetrie())
            noeud.obtenirNoeudOppose().deplacer(position: posNoeudOppose)
        }
    }
    
    /// Visiter un mur pour le déplacer
    //virtual void visiterMur(NoeudMur* noeud);
    
    /// Visiter un portail pour le déplacer
    //virtual void visiterPortail(NoeudPortail* noeud);
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
