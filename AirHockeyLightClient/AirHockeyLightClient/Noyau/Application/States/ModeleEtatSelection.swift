///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatSelection.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatSelection
/// @brief Cette classe représente l'état de sélection. Implémente aussi le
///        patron Singleton.
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class ModeleEtatSelection: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatSelection()
    
    /// Cette fonction initialise l'état. Elle décide quels objects sont
    /// sélectionnable
    override func initialiser() {
        // Rendre tous les objets non selectionnables
        let arbre = FacadeModele.instance.obtenirArbreRendu()
        
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_MUR, selectionnable: true));
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_PORTAIL, selectionnable: true));
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_ACCELERATEUR, selectionnable: true));
        arbre.accepterVisiteur(visiteur: VisiteurSelectionnable(type: arbre.NOM_POINT_CONTROL, selectionnable: false));
        
        /// Afficher la configuration des propriétés de l'objet
        FacadeModele.instance.obtenirVue().showObjectPropertiesView(activer: true)
    }
    
    override func nettoyerEtat() {
        /// Cacher la configuration des propriétés de l'objet
        FacadeModele.instance.obtenirVue().showObjectPropertiesView(activer: false)
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    override func tapGesture(point: CGPoint) {
        super.tapGesture(point: point)
        
        let visiteur = VisiteurSelection(point: self.position)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
