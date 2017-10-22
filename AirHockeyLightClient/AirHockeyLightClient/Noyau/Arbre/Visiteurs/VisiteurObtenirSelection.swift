///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurObtenirSelection.swift
/// @author Pierre To
/// @date 2017-10-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurObtenirSelection
/// @brief Permet d'obtenir tous les noeuds sélectionnés
///
/// @author Pierre To
/// @date 2017-10-22
///////////////////////////////////////////////////////////////////////////
class VisiteurObtenirSelection: VisiteurAbstrait {

    /// Vecteur regroupant les noeuds spécifiques
    private var noeuds = [NoeudCommun]()
    
    /// Constructeur
    init() {
    }
    
    /// Cette fonction retourne le vecteur de noeuds sélectionnés
    func obtenirNoeuds() -> [NoeudCommun] {
        return self.noeuds
    }
    
    /// Visiter un accéléateur pour l'obtention de la sélection
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        self.implementationDefaut(noeud: noeud)
    }
    
    /// Visiter un maillet pour l'obtention de la sélection
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour l'obtention de la sélection
    func visiterTable(noeud: NoeudTable) {
        // Ne fait rien
    }
    
    /// Visiter un point de contrôle pour l'obtention de la sélection
    func visiterPointControl(noeud: NoeudPointControl) {
        self.implementationDefaut(noeud: noeud)
    }
    
    /// Visiter un mur pour l'obtention de la sélection
    func visiterMur(noeud: NoeudMur) {
        self.implementationDefaut(noeud: noeud)
    }
    
    /// Visiter un portail pour l'obtention de la sélection
    func visiterPortail(noeud: NoeudPortail) {
        self.implementationDefaut(noeud: noeud)
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
    /// Si le noeud est sélectionné, on l'ajoute au vecteur.
    private func implementationDefaut(noeud: NoeudCommun) {
        if (noeud.estSelectionne()) {
            noeuds.append(noeud)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
