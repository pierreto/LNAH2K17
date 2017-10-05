///////////////////////////////////////////////////////////////////////////////
/// @file Table.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-01
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit
import GLKit

class Table
{
    /// Nombre de points par coin
    public let VERTEX_PER_CORNER = 5
    /// Largeur de la bordure
    public let TABLE_BORDER_WIDTH = 11.5
    /// Hauteur
    public let TABLE_BORDER_HEIGHT = 3.5
    /// Hauteur de la base
    public let TABLE_BASE_HEIGHT = 15.0
    /// Grandeur de la table maximale
    public let MAX_TABLE_LENGTH = Float(300.0)
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn obtenirSommets()
    ///
    /// Cette fonction obtient tous les sommets de la table
    ///
    /// @return Les sommets.
    ///
    ////////////////////////////////////////////////////////////////////////
    func obtenirSommets() -> [SCNVector3]
    {
            var table: [SCNVector3] = [SCNVector3]();
            
            // Middle
            table.append(SCNVector3Zero);						// 0
            
            // Right
            table.append(SCNVector3(0, 0, 125));                                        // 1
            table.append(SCNVector3(0, TABLE_BORDER_HEIGHT, 125));						// 2
            table.append(SCNVector3(0, TABLE_BORDER_HEIGHT, 125 + TABLE_BORDER_WIDTH));	// 3
            table.append(SCNVector3(0, 0, 125 + TABLE_BORDER_WIDTH));					// 4
            table.append(SCNVector3(0, -TABLE_BASE_HEIGHT, 115));					    // 5
            
            // Top Right
            table.append(SCNVector3(60, 0, 125));																// 6
            table.append(SCNVector3(60, TABLE_BORDER_HEIGHT, 125));												// 7
            table.append(SCNVector3(60 + TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 125 + TABLE_BORDER_WIDTH));	// 8
            table.append(SCNVector3(60 + TABLE_BORDER_WIDTH, 0, 125 + TABLE_BORDER_WIDTH));						// 9
            table.append(SCNVector3(50, -TABLE_BASE_HEIGHT, 115));												// 10
            
            // Top
            table.append(SCNVector3(60, 0, 0));											// 11
            table.append(SCNVector3(60, TABLE_BORDER_HEIGHT, 0));						// 12
            table.append(SCNVector3(60 + TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 0));	// 13
            table.append(SCNVector3(60 + TABLE_BORDER_WIDTH, 0, 0));					// 14
            table.append(SCNVector3(50, -TABLE_BASE_HEIGHT, 0));						// 15
            
            // Top Left
            table.append(SCNVector3(60, 0, -125));																// 16
            table.append(SCNVector3(60, TABLE_BORDER_HEIGHT, -125));											// 17
            table.append(SCNVector3(60 + TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, -125 - TABLE_BORDER_WIDTH));	// 18
            table.append(SCNVector3(60 + TABLE_BORDER_WIDTH, 0, -125 - +TABLE_BORDER_WIDTH));					// 19
            table.append(SCNVector3(50, -TABLE_BASE_HEIGHT, -115));												// 20
            
            // Right
            table.append(SCNVector3(0, 0, -125));										// 21
            table.append(SCNVector3(0, TABLE_BORDER_HEIGHT, -125));						// 22
            table.append(SCNVector3(0, TABLE_BORDER_HEIGHT, -125 - TABLE_BORDER_WIDTH));// 23
            table.append(SCNVector3(0, 0, -125 - TABLE_BORDER_WIDTH));					// 24
            table.append(SCNVector3(0, -TABLE_BASE_HEIGHT, -115));						// 25
            
            // Bottom Left
            table.append(SCNVector3(-60, 0, -125));																// 26
            table.append(SCNVector3(-60, TABLE_BORDER_HEIGHT, -125));											// 27
            table.append(SCNVector3(-60 - TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, -125 - TABLE_BORDER_WIDTH)); // 28
            table.append(SCNVector3(-60 - TABLE_BORDER_WIDTH, 0, -125 - TABLE_BORDER_WIDTH));					// 29
            table.append(SCNVector3(-50, -TABLE_BASE_HEIGHT, -115));											// 30
            
            // Bottom
            table.append(SCNVector3(-60, 0, 0));										// 31
            table.append(SCNVector3(-60, TABLE_BORDER_HEIGHT, 0));						// 32
            table.append(SCNVector3(-60 - TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 0));	// 33
            table.append(SCNVector3(-60 - TABLE_BORDER_WIDTH, 0, 0));					// 34
            table.append(SCNVector3(-50, -TABLE_BASE_HEIGHT, 0));						// 35
            
            // Bottom Right
            table.append(SCNVector3(-60, 0, 125));																// 36
            table.append(SCNVector3(-60, TABLE_BORDER_HEIGHT, 125));											// 37
            table.append(SCNVector3(-60 - TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 125 + TABLE_BORDER_WIDTH));	// 38
            table.append(SCNVector3(-60 - TABLE_BORDER_WIDTH, 0, 125 + TABLE_BORDER_WIDTH));					// 39
            table.append(SCNVector3(-50, -TABLE_BASE_HEIGHT, 115));												// 40
            
            // Middle
            table.append(SCNVector3(0, -TABLE_BASE_HEIGHT, 0));			    // 41
            
            return table;
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn obtenirFaces()
    ///
    /// Cette fonction obtient toutes les faces de la table
    ///
    /// @return Les faces.
    ///
    ////////////////////////////////////////////////////////////////////////
    func obtenirFaces() -> [GLuint]
    {
        let faces: [GLuint] = [
            0,1,6,
            0,6,11,
            0,11,16,
            0,16,21,
            0,21,26,
            0,26,31,
            0,31,36,
            0,36,1,
            
            41,10,5,
            41,15,10,
            41,20,15,
            41,25,20,
            41,30,25,
            41,35,30,
            41,40,35,
            41,5,40,
            
            1,7,6,
            1,2,7,
            2,3,7,
            3,8,7,
            3,9,8,
            4,9,3,
            5,10,4,
            4,10,9,
            
            7,12,11,
            6,7,11,
            7,8,12,
            8,13,12,
            8,14,13,
            9,14,8,
            10,15,9,
            9,15,14,
            
            11,17,16,
            11,12,17,
            12,13,17,
            13,18,17,
            13,19,18,
            14,19,13,
            15,20,14,
            14,20,19,
            
            16,22,21,
            16,17,22,
            17,18,22,
            18,23,22,
            18,24,23,
            19,24,18,
            20,25,19,
            19,25,24,
            
            21,27,26,
            21,22,27,
            22,23,27,
            23,28,27,
            23,29,28,
            24,29,23,
            25,30,24,
            24,30,29,
            
            26,32,31,
            26,27,32,
            27,28,32,
            28,33,32,
            28,34,33,
            29,34,28,
            30,35,29,
            29,35,34,
            
            31,37,36,
            31,32,37,
            32,33,37,
            33,38,37,
            33,39,38,
            34,39,33,
            35,40,34,
            34,40,39,
            
            36,2,1,
            36,37,2,
            37,38,2,
            38,3,2,
            38,4,3,
            39,4,38,
            40,5,39,
            39,5,4,
        ];
        
        return faces
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn obtenirMateriau()
    ///
    /// Cette fonction obtient le matériau de la table
    ///
    /// @return Le matériau de la table.
    ///
    ////////////////////////////////////////////////////////////////////////
    func obtenirMateriau() -> SCNMaterial
    {
        let materiau: SCNMaterial = SCNMaterial();
        materiau.ambient.contents = SCNVector3(0.1, 0.1, 0.1);
        materiau.diffuse.contents = UIImage(named: "table_texture.png")
        materiau.specular.contents = SCNVector3Zero;
        materiau.shininess = 0;
        //materiau.shininessStrength_ = 0;
        materiau.transparency = 1;
        materiau.emission.contents = SCNVector3Zero;
        return materiau;
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn obtenirTexCoords()
    ///
    /// Cette fonction obtient les coordonnées de texture de la table
    ///
    /// @return les coordonnées de texture de la table.
    ///
    ////////////////////////////////////////////////////////////////////////
    func obtenirTexCoords() -> [CGPoint]
    {
        var texCoords: [CGPoint] = [CGPoint]();
        // Middle
        texCoords.append(CGPoint(x: 0.5, y: 0.5));			    // 0
        
        // Right
        texCoords.append(CGPoint(x: 0.8, y: 0.5));				// 1
        texCoords.append(CGPoint(x: 0.8, y: 0.5));				// 2
        texCoords.append(CGPoint(x: 1.0, y: 0.5));			    // 3
        texCoords.append(CGPoint(x: 1.0, y: 0.5));		        // 4
        texCoords.append(CGPoint(x: 1.0, y: 0.5));			    // 5
        
        // Top Right
        texCoords.append(CGPoint(x: 0.8, y: 0.8));				// 6
        texCoords.append(CGPoint(x: 0.8, y: 0.8));				// 7
        texCoords.append(CGPoint(x: 1.0, y: 1.0));			    // 8
        texCoords.append(CGPoint(x: 1.0, y: 1.0));		        // 9
        texCoords.append(CGPoint(x: 1.0, y: 1.0));			    // 10
        
        // Top
        texCoords.append(CGPoint(x: 0.5, y: 0.8));				// 11
        texCoords.append(CGPoint(x: 0.5, y: 0.8));				// 12
        texCoords.append(CGPoint(x: 0.5, y: 1.0));			    // 13
        texCoords.append(CGPoint(x: 0.5, y: 1.0));		        // 14
        texCoords.append(CGPoint(x: 0.5, y: 1.0));			    // 15
        
        // Top Left
        texCoords.append(CGPoint(x: 0.2, y: 0.8));				// 16
        texCoords.append(CGPoint(x: 0.2, y: 0.8));				// 17
        texCoords.append(CGPoint(x: 0.0, y: 1.0));			    // 18
        texCoords.append(CGPoint(x: 0.0, y: 1.0));		        // 19
        texCoords.append(CGPoint(x: 0.0, y: 1.0));			    // 20
        
        // Right
        texCoords.append(CGPoint(x: 0.2, y: 0.5));				// 21
        texCoords.append(CGPoint(x: 0.2, y: 0.5));				// 22
        texCoords.append(CGPoint(x: 0.0, y: 0.5));			    // 23
        texCoords.append(CGPoint(x: 0.0, y: 0.5));		        // 24
        texCoords.append(CGPoint(x: 0.0, y: 0.5));			    // 25
        
        // Bottom Left
        texCoords.append(CGPoint(x: 0.2, y: 0.2));				// 26
        texCoords.append(CGPoint(x: 0.2, y: 0.2));				// 27
        texCoords.append(CGPoint(x: 0.0, y: 0.0));			    // 28
        texCoords.append(CGPoint(x: 0.0, y: 0.0));		        // 29
        texCoords.append(CGPoint(x: 0.0, y: 0.0));			    // 30
        
        // Bottom
        texCoords.append(CGPoint(x: 0.5, y: 0.2));				// 31
        texCoords.append(CGPoint(x: 0.5, y: 0.2));				// 32
        texCoords.append(CGPoint(x: 0.5, y: 0.0));			    // 33
        texCoords.append(CGPoint(x: 0.5, y: 0.0));		        // 34
        texCoords.append(CGPoint(x: 0.5, y: 0.0));			    // 35
        
        // Bottom Right
        texCoords.append(CGPoint(x: 0.8, y: 0.2));				// 36
        texCoords.append(CGPoint(x: 0.8, y: 0.2));				// 37
        texCoords.append(CGPoint(x: 1.0, y: 0.0));			    // 38
        texCoords.append(CGPoint(x: 1.0, y: 0.0));		        // 39
        texCoords.append(CGPoint(x: 1.0, y: 0.0));			    // 40
        
        // Middle
        texCoords.append(CGPoint(x: 1.0, y: 0.0));			    // 41
        
        return texCoords;
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
