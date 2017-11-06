///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurByUUID.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 0.1
///
/// @addtogroup log3900 LOG3900
/// @{
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurByUUID
/// @brief Permet de trouver un noeud par son UUID
///
///
/// @author Pierre
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class VisiteurByUUID: VisiteurAbstrait {
    
    private var wantedNode: NoeudCommun?
    private var wantedUUID: String
    private var hasFound: Bool = false
    
    /// Constructeur
    init(wantedUUID: String) {
        self.wantedUUID = wantedUUID
    }
    
    /// Retourne le noeud à trouver par wantedUUID
    public func getNode() -> NoeudCommun? {
        return self.wantedNode
    }
    
    /// Visiter un accélérateur pour le rendre sélectionnable
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
        if isWantedUUI(nodeUUID: noeud.obtenirUUID()) && !hasFound {
            self.wantedNode = noeud
            self.hasFound = true
        }
    }
    
    private func isWantedUUI(nodeUUID: String) -> Bool {
        return self.wantedUUID == nodeUUID
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
