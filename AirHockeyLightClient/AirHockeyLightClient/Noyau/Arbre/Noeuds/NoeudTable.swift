///////////////////////////////////////////////////////////////////////////////
/// @file NoeudTable.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-03
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation
import SceneKit

class NoeudTable : SCNNode {
    
    private let table = Table()
    
    override init() {
        super.init()
        
        let sources = self.initSources()
        let elements = self.initElements()
        
        // Create geometry
        self.geometry = SCNGeometry(sources: sources, elements: elements)
        
        self.addMaterial()
    }
    
    /* Xcode required this */
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Create geometry sources
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
    
    /// Create geometry elements
    func initElements() -> [SCNGeometryElement] {
        var elements = [SCNGeometryElement]()
        
        let faces = table.obtenirFaces()
        let indexData = NSData(bytes: faces, length: faces.count * MemoryLayout<GLuint>.size)
        elements.append(SCNGeometryElement(data: indexData as Data, primitiveType: .triangles, primitiveCount: 80, bytesPerIndex: MemoryLayout<GLuint>.size))
        
        return elements
    }
    
    /// Add material
    func addMaterial() {
        let material = table.obtenirMateriau()
        self.geometry?.firstMaterial = material
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
