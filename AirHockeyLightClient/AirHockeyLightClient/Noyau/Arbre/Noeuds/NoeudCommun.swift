///////////////////////////////////////////////////////////////////////////////
/// @file NoeudCommun.swift
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
/// @class NoeudCommun
/// @brief Classe de base du patron composite utilisée pour créer l'arbre de rendu
///        Cette classe comprend l'interface de base que doivent implanter 
///        tous les noeuds pouvant être présent dans l'arbre de rendu
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
///////////////////////////////////////////////////////////////////////////
class NoeudCommun : SCNNode {
    
    /// Type du noeud
    private var type: String = ""
    
    /// Constructeur avec géométrie
    required init(type: String, geometry: SCNGeometry) {
        super.init()
        self.geometry = geometry
        self.type = type
    }
    
    /// Constructeur sans géométrie
    required init(type: String) {
        super.init()
        
        self.type = type
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
