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
import SceneKit.ModelIO

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
    
    // Modèle du noeud (certains noeuds n'ont pas un modèle)
    private var model: SCNNode?
    
    /// Constructeur
    init(nomUsine: String, nomModele: String) {
        self.nom = nomUsine
        
        // Charger le modèle depuis un fichier .obj
        if nomModele != "noModel" {
            let urlForAsset = Bundle.main.url(forResource: nomModele, withExtension: "obj")!
            let asset = MDLAsset(url: urlForAsset).object(at: 0)
            
            // Créer un noeud de base avec le modèle
            self.model = SCNNode(mdlObject: asset)
        }
    }
    
    /// Retourne le type du Noeud créé par l'usine
    func obtenirNom() -> String {
        return self.nom
    }
    
    /// Retourne un noeud nouvellement créé du type produit par cette usine
    func creerNoeud() -> SCNNode {
        let newNode = (model != nil) ?
            T(type: self.nom, geometry: (self.model?.geometry)!) :
            T(type: self.nom)
        // TODO : ajouter le collider du noeud (voir SCNPhysicsBody)
        
        return newNode
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
