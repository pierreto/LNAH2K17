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
    static let instance = FacadeModele()
    
    /// Vue courante de la scène.
    var vue: EditorViewController?
    
    /// Arbre de rendu contenant les différents objets de la scène.
    var arbre: ArbreRendu?
    
    /// Etat du modèle
    var etat: ModeleEtat?
    
    /// Constructeur
    init() {
        self.vue = EditorViewController.instance
        
        self.arbre = ArbreRendu.instance
        self.arbre?.initialiser()
        
        self.etat = ModeleEtatSelection.instance
        self.etat?.initialiser()
        
        self.initVue()
    }
    
    /// Retourne la vue courante.
    func obtenirVue() -> UIViewController {
        return self.vue!
    }
    
    /// Retourne l'arbre de rendu.
    func obtenirArbreRendu() -> ArbreRendu {
        return ArbreRendu.instance
    }
    
    func initVue() {
        self.vue?.editorScene.rootNode.addChildNode(self.arbre!)
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
            /*case .DEPLACEMENT:
                self.etat = ModeleEtatDeplacement.instance
                break;
            case .ROTATION:
                self.etat = ModeleEtatRotation.instance
                break;
            case .MISE_A_ECHELLE:
                self.etat = ModeleEtatScale.instance
                break;
            case .ZOOM:
                self.etat = ModeleEtatZoom.instance
                break;
            case .DUPLIQUER:
                self.etat = ModeleEtatDuplication.instance
                break;
            case .POINTS_CONTROLE:
                self.etat = ModeleEtatPointControl.instance
                break;
            case .CREATION_ACCELERATEUR:
                self.etat = ModeleEtatCreerBoost.instance
                break;
            case .CREATION_MURET:
                self.etat = ModeleEtatCreerMuret.instance
                break;
            case .CREATION_PORTAIL:
                self.etat = ModeleEtatCreerPortail.instance
                break;*/
            default:
                break;
        }
        
        // Initialisation de l'etat
        self.etat?.initialiser();
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
