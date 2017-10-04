///////////////////////////////////////////////////////////////////////////////
/// @file But.swift
/// @author Pierre To
/// @date 2017-10-03
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit
import GLKit

class But
{
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn std::vector<GLuint> obtenirFaces()
    ///
    /// Cette fonction obtient toutes les faces du but
    ///
    /// @return Les faces.
    ///
    ////////////////////////////////////////////////////////////////////////
    func obtenirFaces() -> [GLuint]
    {
        let faces: [GLuint] = [
            0,3,1,
            0,2,3,
            1,5,7,
            1,3,5,
            6,7,5,
            6,5,4,
            0,6,4,
            0,4,2,
            2,5,3,
            2,4,5,
            10,1,11,
            10,0,1,
            7,11,1,
            7,9,11,
            6,9,7,
            6,8,9,
            8,6,0,
            8,0,10,
            8,11,9,
            8,10,11
        ];
        
        return faces
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn obtenirMateriau()
    ///
    /// Cette fonction obtient le matériau d'un but
    ///
    /// @return Le matériau du but.
    ///
    ////////////////////////////////////////////////////////////////////////
    func obtenirMateriau() -> SCNMaterial
    {
        let materiau: SCNMaterial = SCNMaterial();
        materiau.ambient.contents = SCNVector3(0.1, 0.1, 0.1);
        materiau.diffuse.contents = SCNVector3(1.0, 1.0, 1.0);
        materiau.specular.contents = SCNVector3Zero;
        materiau.shininess = 0;
        //materiau.shininessStrength_ = 0;
        materiau.transparency = 1;
        materiau.emission.contents = SCNVector3Zero;
        return materiau;
    }
}
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
