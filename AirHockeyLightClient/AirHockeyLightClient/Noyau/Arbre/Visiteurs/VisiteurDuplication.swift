///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDuplication.swift
/// @author Pierre To
/// @date 2017-10-24
/// @version 0.1
///
/// @addtogroup log3900 LOG3900
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurDuplication
/// @brief Permet de dupliquer des noeuds
///
///
/// @author Pierre To
/// @date 2017-10-24
///////////////////////////////////////////////////////////////////////////
class VisiteurDuplication: VisiteurAbstrait {
    
    /// Nombre d'items à dupliquer
    private var nbItems: Int = 0
    
    /// Vecteur trois dimensions du point centre pour la duplication
    private var centreDuplication = GLKVector3()
    
    /// Premier noeud sélectionné pour la duplication
    private var premierNoeud: NoeudPortail?
    
    /// Constructeur
    init() {
        self.nbItems = 0
        self.centreDuplication = GLKVector3.init(v: (0.0, 0.0, 0.0))
    }
    
    /// Cette fonction permet de chercher le centre de la duplication
    func obtenirCentreDuplication() -> GLKVector3 {
        self.centreDuplication = GLKVector3DivideScalar(self.centreDuplication, Float(self.nbItems))
        return self.centreDuplication
    }

    /// Visiter un accéléateur pour la duplication
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        if noeud.estSelectionne() {
            let arbre = FacadeModele.instance.obtenirArbreRendu()
            
            // Création du noeud
            let noeudDouble = arbre.creerNoeud(typeNouveauNoeud: arbre.NOM_ACCELERATEUR) as! NoeudAccelerateur
            
            // Assigner les mêmes propriétés
            self.copyProperties(node: noeud, nodeCopy: noeudDouble)
            
            // Ajout du noeud à l'arbre de rendu
            let table = arbre.childNode(withName: arbre.NOM_TABLE, recursively: true) as! NoeudTable
            table.addChildNode(noeudDouble)
        }
    }
    
    /// Visiter un maillet pour la duplication
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour la duplication
    func visiterTable(noeud: NoeudTable) {
        // Ne fait rien
    }
    
    /// Visiter un point de contrôle pour la duplication
    func visiterPointControl(noeud: NoeudPointControl) {
        // Ne fait rien
    }
    
    /// Visiter un mur pour la duplication
    func visiterMur(noeud: NoeudMur) {
        
    }
    
    /// Visiter un portail pour la duplication
    func visiterPortail(noeud: NoeudPortail) {
        
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
    /// Cette fonction permet de copier les propriétés de l'objet dupliqué
    /// Met aussi le noeud fantôme
    private func copyProperties(node: NoeudCommun, nodeCopy: NoeudCommun) {
        nodeCopy.transform = node.transform
        //nodeCopy.assignerPositionRelative(node.obtenirPositionRelative())
        //nodeCopy.scale(node.obtenirScale())
        //nodeCopy.rotate(node.obtenirRotation().y, GLKVector3.init(v: (0.0, 1.0, 0.0))
        
        // Déselectionner l'objet à dupliquer
        node.assignerSelection(selectionne: false)
        
        // Sélectionner l'objet dupliqué
        nodeCopy.assignerSelection(selectionne: true)
    
        self.centreDuplication = GLKVector3Add(self.centreDuplication, nodeCopy.obtenirPositionRelative())
        self.nbItems += 1
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
