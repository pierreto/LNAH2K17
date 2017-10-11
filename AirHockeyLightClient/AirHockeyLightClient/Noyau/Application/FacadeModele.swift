///////////////////////////////////////////////////////////////////////////////
/// @file FacadeModele.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

/// Les différents états du modèle
enum MODELE_ETAT : Int {
    case AUCUN = 0
    case SELECTION = 1
    case CREATION_ACCELERATEUR = 2
    case CREATION_MURET = 3
    case CREATION_PORTAIL = 4
    case DEPLACEMENT = 5
    case ROTATION = 6
    case MISE_A_ECHELLE = 7
    case DUPLIQUER = 8
    case ZOOM = 9
    case POINTS_CONTROLE = 10
}

///////////////////////////////////////////////////////////////////////////
/// @class FacadeModele
/// @brief Classe qui constitue une interface (une façade) sur l'ensemble
///        du modèle et des classes qui le composent.
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////

class FacadeModele {
    
    /// Instance singleton
    static var instance = FacadeModele()
    
    /// Vue courante de la scène.
    private var viewController: EditorViewController?
    
    /// Arbre de rendu contenant les différents objets de la scène.
    private var arbre: ArbreRendu?
    
    /// Etat du modèle
    private var etat: ModeleEtat?
    
    /// Initialise la vue, l'arbre et l'état
    func initialiser() {
        self.arbre = ArbreRendu.instance
        self.arbre?.initialiser()
        
        self.etat = ModeleEtatSelection.instance
        self.etat?.initialiser()
        
        self.viewController = EditorViewController.instance
        self.initVue()
        
        // TODO : A enlever
        self.changerModeleEtat(etat: .POINTS_CONTROLE)
    }
    
    /// Retourne la vue courante.
    func obtenirVue() -> UIViewController {
        return self.viewController!
    }
    
    /// Retourne l'arbre de rendu.
    func obtenirArbreRendu() -> ArbreRendu {
        return self.arbre!
    }
    
    func initVue() {
        self.viewController?.editorScene.rootNode.addChildNode(self.arbre!)
        
        let gesture = UITapGestureRecognizer(target: self, action:  #selector (self.tapGesture (_:)))
        self.viewController?.editorView.addGestureRecognizer(gesture)
    }
    
    @objc func tapGesture(_ sender: UITapGestureRecognizer) {
        self.etat?.tapGesture(point: sender.location(in: sender.view))
    }
    
    /// Réinitialise la scène.
    func reinitialiser() {
        self.arbre?.initialiser()
    }
    
    /// Modifie le modèle état en cours
    func changerModeleEtat(etat: MODELE_ETAT) {
        // Nettoyer l'état courant
        self.etat?.nettoyerEtat();
        
        switch (etat) {
            case .AUCUN:
                self.etat = nil;
                break;
            case .SELECTION:
                self.etat = ModeleEtatSelection.instance
                break;
            case .DEPLACEMENT:
                self.etat = ModeleEtatDeplacement.instance
                break;
            case .ROTATION:
                //self.etat = ModeleEtatRotation.instance
                break;
            case .MISE_A_ECHELLE:
                //self.etat = ModeleEtatScale.instance
                break;
            case .ZOOM:
                //self.etat = ModeleEtatZoom.instance
                break;
            case .DUPLIQUER:
                //self.etat = ModeleEtatDuplication.instance
                break;
            case .POINTS_CONTROLE:
                self.etat = ModeleEtatPointControl.instance
                break;
            case .CREATION_ACCELERATEUR:
                //self.etat = ModeleEtatCreerBoost.instance
                break;
            case .CREATION_MURET:
                //self.etat = ModeleEtatCreerMuret.instance
                break;
            case .CREATION_PORTAIL:
                //self.etat = ModeleEtatCreerPortail.instance
                break;
        }
        
        // Initialisation de l'etat
        self.etat?.initialiser();
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
