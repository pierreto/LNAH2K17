///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurInformation.swift
/// @author Pierre To
/// @date 2017-10-29
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit
import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurInformation
/// @brief Permet de récupérer l'information d'un noeud sélectionné
///
/// @author Pierre To
/// @date 2017-10-29
///////////////////////////////////////////////////////////////////////////
class VisiteurInformation: VisiteurAbstrait {
    
    /// Vecteurs trois dimensions regroupant les informations du noeud
    private var position: SCNVector3?
    private var rotation: SCNVector3? // angle de rotation selon chaque axe en degré
    private var scale: SCNVector3?
    
    /// Nombre de noeuds sélectionnés
    private var nbSelections = 0
    
    /// Bool pour le mode écriture
    private var modeEcriture: Bool = false
    
    /// Constructeur
    init() {
    }
    
    /// Visiter un accélérateur pour l'obtention d'information
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        self.traiterNoeud(noeud: noeud, scaleX: true)
    }
    
    /// Visiter un maillet pour l'obtention d'information
    //virtual void visiterMaillet(NoeudMaillet* noeud);
    
    /// Visiter une table pour l'obtention d'information
    func visiterTable(noeud: NoeudTable) {
        // Ne fait rien
    }
    
    /// Visiter un point de contrôle pour l'obtention d'information
    func visiterPointControl(noeud: NoeudPointControl) {
        // Ne fait rien
    }
    
    /// Visiter un mur pour l'obtention d'information
    func visiterMur(noeud: NoeudMur) {
        self.traiterNoeud(noeud: noeud, scaleX: false)
    }
    
    /// Visiter un portail pour l'obtention d'information
    func visiterPortail(noeud: NoeudPortail) {
        self.traiterNoeud(noeud: noeud, scaleX: true)
    }
    
    //virtual void visiterRondelle(NoeudRondelle* noeud);
    
    /// Cette fonction permet de retenir les informations sur le noeud et
    ///	l'actualise si on est en mode écriture
    private func traiterNoeud(noeud: NoeudCommun, scaleX: Bool) {
        if (noeud.estSelectionne() && !self.modeEcriture) {
            self.position = noeud.position;
            self.rotation = noeud.eulerAngles;
            self.scale = noeud.scale;
    
            self.nbSelections += 1;
        }
        
        if (noeud.estSelectionne() && self.modeEcriture) {
            // changer position
            noeud.position = self.position!
            
            // changer rotation
            noeud.eulerAngles = self.rotation!
            
            // changer mise à l'échelle
            if (scaleX) {
                noeud.scale = SCNVector3Make((self.scale?.z)!, (self.scale?.y)!, (self.scale?.z)!)
            }
            else {
                noeud.scale = SCNVector3Make(1.0, (self.scale?.y)!, (self.scale?.z)!)
            }
            
            self.modeEcriture = false;
        }
    }
    
    /// Cette fonction permet d'assigner les données obtenues dans un tableau
    func lireInformations(infos: inout [Float]?) -> Bool {
        if (self.nbSelections != 1) {
            return false;
        }
        else if (infos != nil) {
            infos?.append((self.position?.x)!)
            infos?.append((self.position?.y)!)
            infos?.append((self.position?.z)!)
    
            infos?.append(GLKMathRadiansToDegrees((self.rotation?.x)!))
            infos?.append(GLKMathRadiansToDegrees((self.rotation?.y)!))
            infos?.append(GLKMathRadiansToDegrees((self.rotation?.z)!))

            infos?.append((self.scale?.x)!)
            infos?.append((self.scale?.y)!)
            infos?.append((self.scale?.z)!)
        }
        self.nbSelections = 0;
        return true;
        
    }
    
    /// Cette fonction permet d'écrire les informations retenues dans le tableau.
    func ecrireInformations(infos: [Float]) -> Bool {
        if (!infos.isEmpty) {
            self.position = SCNVector3Make(infos[0], infos[1], infos[2]);
            self.rotation = SCNVector3Make(GLKMathDegreesToRadians(infos[3]),
                                           GLKMathDegreesToRadians(infos[4]),
                                           GLKMathDegreesToRadians(infos[5]));
            self.scale = SCNVector3Make(infos[6], infos[7], infos[8]);
    
            self.modeEcriture = true;
            return true;
        }
        
        return false;
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
