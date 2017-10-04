///////////////////////////////////////////////////////////////////////////////
/// @file NoeudBut.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-03
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

class NoeudBut : SCNNode {
    
    private let but = But()
    private var sommets = [SCNVector3]()
    private var faces = [GLuint]()
    
    override init() {
        super.init()
        
        //let sources = self.initSources()
        let elements = self.initElements()
        
        // Create geometry
        //self.geometry = SCNGeometry(sources: sources, elements: elements)
        
        self.addMaterial()
    }
    
    /* Xcode required this */
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn initSources()
    ///
    /// Crée la liste des sources de la géométrie
    ///
    /// @return La liste des sources
    ///
    ////////////////////////////////////////////////////////////////////////
    /*func initSources() -> [SCNGeometrySource] {
        var sources = [SCNGeometrySource]()
        
        // Vertices
        let vertices = table.obtenirSommets()
        let verticesCount = vertices.count
        sources.append(SCNGeometrySource(vertices: vertices))
        
        // Normales
        // TODO : Utiliser un nuanceur (shader)
        var normals = [SCNVector3]()
        for normalIndex in 0...verticesCount-1 {
            normals.append(SCNVector3Make(0, 0, -1))
        }
        sources.append(SCNGeometrySource(normals: normals, count: verticesCount))
        
        // Coordonnées de texture
        let textCoordinates = table.obtenirTexCoords()
        
        let uvData = NSData(bytes: textCoordinates, length: textCoordinates.count *  MemoryLayout<CGPoint>.size)
        let uvSource = SCNGeometrySource(
            data: uvData as Data,
            semantic: SCNGeometrySource.Semantic.texcoord,
            vectorCount: textCoordinates.count,
            usesFloatComponents: true,
            componentsPerVector: 2,
            bytesPerComponent: MemoryLayout<CGFloat>.size,
            dataOffset: 0,
            dataStride: MemoryLayout<CGPoint>.size)
        sources.append(uvSource)
        
        return sources
    }*/
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn initElements()
    ///
    /// Crée la liste des éléments de la géométrie
    ///
    /// @return La liste des éléments
    ///
    ////////////////////////////////////////////////////////////////////////
    func initElements() -> [SCNGeometryElement] {
        var elements = [SCNGeometryElement]()
        
        self.faces = but.obtenirFaces()
        let indexData = NSData(bytes: faces, length: faces.count * MemoryLayout<GLuint>.size)
        elements.append(SCNGeometryElement(data: indexData as Data, primitiveType: .triangles, primitiveCount: 80, bytesPerIndex: MemoryLayout<GLuint>.size))
        
        return elements
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn addMaterial()
    ///
    /// Ajoute le matériau de la table sur la géométrie
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    func addMaterial() {
        let material = but.obtenirMateriau()
        self.geometry?.firstMaterial = material
    }
    
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
