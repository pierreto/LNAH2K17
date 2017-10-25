///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSauvegarde.swift
/// @author Mikael Ferland
/// @date 2017-10-25
/// @version 0.1
///
/// @addtogroup log3900 LOG3900
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSauvegarde
/// @brief Permet de sauvegarder la scène de l'éditeur
///
/// @author Mikael Ferland
/// @date 2017-10-25
///////////////////////////////////////////////////////////////////////////
class VisiteurSauvegarde: VisiteurAbstrait {
    
    private var linkedPortals = [NoeudPortail]()
    
    /// Cette fonction permet de visiteur un accélérateur pour la sauvegarde
    func visiterAccelerateur(noeud: NoeudAccelerateur) {
        sauvegarderNoeud(noeud: noeud, nom: "Accelerateur")
    }
    
    /// Cette fonction permet de visiteur un point de contrôle pour la sauvegarde
    func visiterPointControl(noeud: NoeudPointControl) {
        sauvegarderNoeud(noeud: noeud, nom: "PointControle")
    }
    
    func visiterTable(noeud: NoeudTable) { }
    
    /// Cette fonction permet de visiteur un portail pour la sauvegarde.
    func visiterPortail(noeud: NoeudPortail) {
        if  (self.linkedPortals.contains{ element in return element == noeud }) {
            let portal = self.linkedPortals.first{ element in return element == noeud }
            let finalPortal = self.linkedPortals[self.linkedPortals.count - 1]
            
            if portal == finalPortal {
                sauvegarderNoeud(noeud: noeud, nom: "Portail")
                sauvegarderNoeud(noeud: noeud.obtenirOppose(), nom: "Portail")
                
                self.linkedPortals.append(noeud)
                self.linkedPortals.append(noeud.obtenirOppose())
            }
        }
    }
    
    /// Cette fonction permet de visiteur un mur pour la sauvegarde
    func visiterMur(noeud: NoeudMur) {
        sauvegarderNoeud(noeud: noeud, nom: "Muret")
    }
    
    /// Cette fonction permet de sauvegarder les propriétés du noeud avec JSON
    func sauvegarderNoeud(noeud: NoeudCommun, nom: String) {
        let pos = noeud.obtenirPositionRelative()
        let scale = noeud.scale
        let angle = noeud.rotation.w
        let angleY = angle * noeud.rotation.y
        
        /*
        rapidjson::Value tempArray(rapidjson::kArrayType);
        tempArray.PushBack(pos.x, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
        tempArray.PushBack(pos.y, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
        tempArray.PushBack(pos.z, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
        tempArray.PushBack(scale.x, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
        tempArray.PushBack(scale.y, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
        tempArray.PushBack(scale.z, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
        tempArray.PushBack(angle, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
        
        FacadeModele::obtenirInstance()->docJSON_[nom].PushBack(tempArray, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
         */
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

