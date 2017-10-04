///////////////////////////////////////////////////////////////////////////////
/// @file UsineNoeud.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class UsineNoeud
/// @brief Classe template permettant de créer un type de noeud concret 
///        pour l'arbre de rendu
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
///////////////////////////////////////////////////////////////////////////
class UsineNoeud<T: NoeudCommun> : UsineAbstraite {
    
    // Nom de l'usine, à savoir le type d'un noeud créé par l'usine
    private var nom: String = ""
    
    // Modèle du noeud
    private var sceneModel: SCNScene?
    
    /// Constructeur
    init(nomUsine: String, nomModele: String) {
        self.nom = nomUsine
        
        // Charger le modèle depuis un fichier .dae
        if nomModele != "noModel" {
            self.sceneModel = SCNScene(named: nomModele)
        }
    }
    
    /// Retourne le type du Noeud créé par l'usine
    func obtenirNom() -> String {
        return self.nom
    }
    
    /// Retourne un noeud nouvellement créé du type produit par cette usine
    func creerNoeud() -> SCNNode {
        let newNode = T(type: self.nom)
        
        // Associer le modèle avec le noeud
        if self.sceneModel != nil {
            newNode.addChildNode((self.sceneModel?.rootNode.clone())!)
        }
        // TODO : ajouter le collider du noeud (voir SCNPhysicsBody)
        
        return newNode
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
