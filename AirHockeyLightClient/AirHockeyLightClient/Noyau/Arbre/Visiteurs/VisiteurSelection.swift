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
    //virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
    
    /// Visiter un maillet pour la sélection
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour la sélection
    func visiterTable(noeud: NoeudTable) {
        if (!self.multiSelection && noeud.estSelectionnable()) {
            let vue = FacadeModele.instance.obtenirVue()
            let options = [SCNHitTestOption.sortResults: NSNumber(value: true)]
            
            let hitResults = vue.editorView.hitTest(self.point, options: options)
            
            if (hitResults.count > 0) {
                let result: AnyObject = hitResults[0]
                if  result is SCNHitTestResult {
                    let hitResult = result as! SCNHitTestResult
                    
                    if noeud is NoeudCommun {
                        let noeud = hitResult.node as! NoeudCommun
                        noeud.assignerSelection(selectionne: true)
                    }
                }
            }
        }
    }
    
    /// Visiter un point de contrôle pour la sélection
    func visiterPointControl(noeud: NoeudPointControl) {
    }
    
    /// Visiter un mur pour la sélection
    //virtual void visiterMur(NoeudMur* noeud);
    
    /// Visiter un portail pour la sélection
    //virtual void visiterPortail(NoeudPortail* noeud);
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
