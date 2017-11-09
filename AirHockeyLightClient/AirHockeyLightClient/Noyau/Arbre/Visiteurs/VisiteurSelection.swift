///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSelection.swift
/// @author Mikael et Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSelection
/// @brief Permet de selectionner les noeuds
///
/// @author Mikael et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class VisiteurSelection: VisiteurAbstrait {
    
    /// Vecteur trois dimensions pour la position lors d'une sélection
    private var point = CGPoint()
    
    /// Nombre de noeuds sélectionnés
    private var nbSelections: Int = 0
    
    /// Bool pour une sélection multiple ou non
    private var multiSelection: Bool = false
    
    /// Constructeur
    init(point: CGPoint, nbSelections: Int = 0, multiSelection: Bool = false) {
        self.point = point
        self.nbSelections = nbSelections
        self.multiSelection = multiSelection
    }
    
    /// Visiter un accélérateur pour la sélection
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
    }
    
    /// Visiter un maillet pour la sélection
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour la sélection
    func visiterTable(noeud: NoeudTable) {
        if (!self.multiSelection) {
            let vue = FacadeModele.instance.obtenirVue()
            let options = [SCNHitTestOption.sortResults: NSNumber(value: true)]
            
            let hitResults = vue.editorView.hitTest(self.point, options: options)
            
            if (hitResults.count > 0) {
                let result: AnyObject = hitResults[0]
                if  result is SCNHitTestResult {
                    let hitResult = result as! SCNHitTestResult
                    
                    if hitResult.node is NoeudCommun {
                        let noeud = hitResult.node as! NoeudCommun
                        self.handleSelection(noeud: noeud)
                    }
                }
            }
        }
    }
    
    /// Visiter un point de contrôle pour la sélection
    func visiterPointControl(noeud: NoeudPointControl) {
    }
    
    /// Visiter un mur pour la sélection
    func visiterMur(noeud: NoeudMur) {
    }
    
    /// Visiter un portail pour la sélection
    func visiterPortail(noeud: NoeudPortail) {
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
    private func handleSelection(noeud: NoeudCommun) {
        if noeud.estSelectionnable() && !noeud.estSelectionneByAnotherUser() {
            // Selectioner le noeud
            if !noeud.estSelectionne() {
                noeud.assignerSelection(selectionne: true)
                
                // Envoyer la commande
                FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: noeud.obtenirUUID(),
                                                                                     isSelected: noeud.estSelectionne(),
                                                                                     deselectAll: false)
                // Si le noeud sélectionné est un point de contrôle, on sélectionne l'opposé en mode en ligne
                if noeud.obtenirType() == FacadeModele.instance.obtenirArbreRendu().NOM_POINT_CONTROL {
                    FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: noeud.obtenirUUID(),
                                                                                         isSelected: true,
                                                                                         deselectAll: false)
                    let noeudOppose = (noeud as! NoeudPointControl).obtenirNoeudOppose()
                    FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: noeudOppose.obtenirUUID(),
                                                                                         isSelected: true,
                                                                                         deselectAll: false)
                }
                
                self.nbSelections += 1
            }
            // Déselectionner le noeud sauf pour le point de contrôle
            else if (noeud.obtenirType() != FacadeModele.instance.obtenirArbreRendu().NOM_POINT_CONTROL) {
                noeud.assignerSelection(selectionne: false)
                
                // Envoyer la commande
                FacadeModele.instance.obtenirEtatEdition().currentUserSelectedObject(uuidSelected: noeud.obtenirUUID(),
                                                                                     isSelected: noeud.estSelectionne(),
                                                                                     deselectAll: false)
                self.nbSelections -= 1
            }
        }
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
