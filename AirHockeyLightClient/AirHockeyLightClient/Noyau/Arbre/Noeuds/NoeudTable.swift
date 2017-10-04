///////////////////////////////////////////////////////////////////////////////
/// @file NoeudTable.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-03
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class NoeudTable
/// @brief Classe pour afficher une table
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-03
///////////////////////////////////////////////////////////////////////////
class NoeudTable : NoeudCommun {
    
    private let table = Table()
    
    /// La table n'a pas un modèle obj
    required init(type: String, geometry: SCNGeometry) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    required init(type: String) {
        super.init(type: type)
        
        let sources = self.initSources()
        let elements = self.initElements()
        
        // Create geometry
        self.geometry = SCNGeometry(sources: sources, elements: elements)
        
        self.addMaterial()
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Crée la liste des sources de la géométrie
    func initSources() -> [SCNGeometrySource] {
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
    }
    
    /// Crée la liste des éléments de la géométrie
    func initElements() -> [SCNGeometryElement] {
        var elements = [SCNGeometryElement]()
        
        let faces = table.obtenirFaces()
        let indexData = NSData(bytes: faces, length: faces.count * MemoryLayout<GLuint>.size)
        elements.append(SCNGeometryElement(data: indexData as Data, primitiveType: .triangles, primitiveCount: 80, bytesPerIndex: MemoryLayout<GLuint>.size))
        
        return elements
    }
    
    /// Ajoute le matériau de la table sur la géométrie
    func addMaterial() {
        let material = table.obtenirMateriau()
        self.geometry?.firstMaterial = material
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
