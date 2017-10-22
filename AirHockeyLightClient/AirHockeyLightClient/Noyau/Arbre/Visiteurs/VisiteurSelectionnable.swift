///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSelectionnable.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
/// @version 0.1
///
/// @addtogroup log3900 LOG3900
/// @{
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSelectionnable
/// @brief Permet de rendre selectionnable ou non un type de noeud
///
///
/// @author Mikael Ferland et Pierre
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class VisiteurSelectionnable : VisiteurAbstrait {

    /// Type du noeud
    private var type: String

    /// Bool pour la caractéristique de sélection du noeud
    private var selectionnable: Bool = false
    
    /// Constructeur
    init(type: String, selectionnable: Bool) {
        self.type = type
        self.selectionnable = selectionnable
    }
    
    /// Assigne un type au noeud
    func assignerType(type: String) {
        self.type = type
    }
    
    /// Rend un noeud sélectionnable
    func assignerSelectionnable(selectionnable: Bool) {
        self.selectionnable = selectionnable
    }
    
    /// Visiter un accéléateur pour le rendre sélectionnable
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        self.implementationDefaut(noeud: noeud)
    }
    /// Visiter un maillet pour le rendre sélectionnable
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour le rendre sélectionnable
    func visiterTable(noeud: NoeudTable) {
        self.implementationDefaut(noeud: noeud)
    }
    
    /// Visiter un point de contrôle pour le rendre sélectionnable
    func visiterPointControl(noeud: NoeudPointControl) {
        self.implementationDefaut(noeud: noeud)
    }
    
    /// Visiter un mur pour le rendre sélectionnable
    func visiterMur(noeud: NoeudMur) {
        self.implementationDefaut(noeud: noeud)
    }
    
    /// Visiter un portail pour le rendre sélectionnable
    func visiterPortail(noeud: NoeudPortail) {
        self.implementationDefaut(noeud: noeud)
    }

    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
    /// Cette fonction permet d'implémenter un noeud par défaut et de lui assigner
    /// la caractéristique de sélectionnable.
    private func implementationDefaut(noeud: NoeudCommun) {
        if (noeud.obtenirType() == type)
        {
            noeud.assignerEstSelectionnable(selectionnable: selectionnable);
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
