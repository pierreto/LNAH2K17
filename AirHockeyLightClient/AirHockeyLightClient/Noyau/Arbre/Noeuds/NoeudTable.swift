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

///////////////////////////////////////////////////////////////////////////
/// @class NoeudTable
/// @brief Classe pour afficher une table
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-03
///////////////////////////////////////////////////////////////////////////
class NoeudTable : NoeudCommun {
    
    private let table = Table()
    var sommets = [SCNVector3]()
    
    /// La table n'a pas un modèle obj
    required init(type: String, geometry: SCNGeometry) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    required init(type: String) {
        super.init(type: type)
        
        self.initialiserTable()
        self.creerPointsDeControle()
        self.initialiserMateriau()
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Initialiser la géométrie de la table
    func initialiserTable() {
        let sources = self.initSources()
        let elements = self.initElements()
        
        // Create geometry
        self.geometry = SCNGeometry(sources: sources, elements: elements)
    }
    
    /// Crée la liste des sources de la géométrie
    func initSources() -> [SCNGeometrySource] {
        var sources = [SCNGeometrySource]()
        
        // Vertices
        self.sommets = table.obtenirSommets()
        sources.append(SCNGeometrySource(vertices: self.sommets))
        
        // Normales
        // TODO : Utiliser un nuanceur (shader)
        
        // Coordonnées de texture
        sources.append(SCNGeometrySource(textureCoordinates: table.obtenirTexCoords()))
        
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
    
    /// Créer et attacher les points de contrôle à la table
    func creerPointsDeControle()
    {
        // obtention de l'arbre
        let arbre = ArbreRendu.instance
        
        // vecteur contenant les noeuds (utilisé pour associer les noeuds entre eux)
        var noeuds = [NoeudPointControl]()
        
        // création des noeuds
        var i = 1
        let max = self.sommets.count - 1
        while (i < max) {
            let noeudPointControl = arbre.creerNoeud(typeNouveauNoeud: arbre.NOM_POINT_CONTROL) as! NoeudPointControl
            
            // TODO : Ajouter noeud booster
            /*NoeudBooster* noeudBooster{ (NoeudBooster*)arbre->creerNoeud(ArbreRenduINF2990::NOM_BOOSTER) };
            noeudBooster->assignerAxisLock(glm::ivec3(1, 1, 1));
            noeudBooster->appliquerDeplacement(glm::vec3(0, -TABLE_BASE_HEIGHT, 0));
            noeudBooster->assignerAxisLock(glm::ivec3(1, 0, 1));*/
            
            var points = [SCNVector3]()
            
            for j in 0..<self.table.VERTEX_PER_CORNER {
                points.append(self.sommets[i + j])
            }
            
            noeudPointControl.assignerPoints(points: points)
            noeudPointControl.assignerAxisLock(axisLock: GLKVector3(v: (1.0, 1.0, 1.0)))
            // TODO: changer la classe table et mettre des GLKVector3
            let sommet = self.sommets[i + 1]
            let positionRelative = GLKVector3(v: (sommet.x, sommet.y, sommet.z))
            noeudPointControl.assignerPositionRelative(positionRelative: positionRelative)
            noeudPointControl.assignerAxisLock(axisLock: GLKVector3(v: (1.0, 0.0, 1.0)))
            
            /*if (noeudControl->obtenirPositionRelative().x != 0) {
                noeudControl->assignerBooster(noeudBooster);
                ajouter(noeudBooster);
            }*/
            
            noeuds.append(noeudPointControl)
            
            i += 5
        }
        
        // Associer chaque noeud avec son oppose
        let association: [Int] = [ 4, 3, 6, 1, 0, 7, 2, 5 ]
        for k in 0..<noeuds.count {
            var symetrie = GLKVector3(v: (1.0, 1.0, -1.0))
            
            if k == 2 || k == 6 {
                symetrie = GLKVector3(v: (-1.0, 1.0, 1.0))
            }
            
            noeuds[k].assignerNoeudOppose(noeud: noeuds[association[k]], symetrie: symetrie)
        }
        
        // Assigner les voisins
        for l in 0..<noeuds.count {
            var voisins = [NoeudPointControl]()
            
            // TODO : CHANGER POUR LE MODULO CORRECT % ne fonctionne pas
            voisins.append(noeuds[(l - 1) % noeuds.count])
            voisins.append(noeuds[(l + 1) % noeuds.count])
            noeuds[l].assignerVoisins(voisins: voisins)
        }
        
        // Blocage du deplacement sur certains points
        noeuds[2].assignerAxisLock(axisLock: GLKVector3(v: (1, 0, 0)))
        noeuds[6].assignerAxisLock(axisLock: GLKVector3(v: (1, 0, 0)))
        noeuds[0].assignerAxisLock(axisLock: GLKVector3(v: (0, 0, 1)))
        noeuds[4].assignerAxisLock(axisLock: GLKVector3(v: (0, 0, 1)))
            
        // Zone de deplacement pour chaque point
        noeuds[0].assignerZoneDeplacement(zone: GLKVector4(v: (-self.table.MAX_TABLE_LENGTH, self.table.MAX_TABLE_LENGTH, 50.0, self.table.MAX_TABLE_LENGTH)))
        noeuds[1].assignerZoneDeplacement(zone: GLKVector4(v: (30, self.table.MAX_TABLE_LENGTH, 50, self.table.MAX_TABLE_LENGTH)))
        noeuds[2].assignerZoneDeplacement(zone: GLKVector4(v: (30, self.table.MAX_TABLE_LENGTH, 0, 0)))
        noeuds[3].assignerZoneDeplacement(zone: GLKVector4(v: (30, self.table.MAX_TABLE_LENGTH, -self.table.MAX_TABLE_LENGTH, -50)))
        noeuds[4].assignerZoneDeplacement(zone: GLKVector4(v: (-self.table.MAX_TABLE_LENGTH, self.table.MAX_TABLE_LENGTH, -self.table.MAX_TABLE_LENGTH, -50 )))
        noeuds[5].assignerZoneDeplacement(zone: GLKVector4(v: (-self.table.MAX_TABLE_LENGTH, -30 , -self.table.MAX_TABLE_LENGTH, -50)))
        noeuds[6].assignerZoneDeplacement(zone: GLKVector4(v: (-self.table.MAX_TABLE_LENGTH, -30, 0, 0)))
        noeuds[7].assignerZoneDeplacement(zone: GLKVector4(v: (-self.table.MAX_TABLE_LENGTH, -30, 50, self.table.MAX_TABLE_LENGTH)))
    
        // TODO : AJOUTER LES BUTS
        // Creation des buts
        //creerButs(noeuds[4], noeuds[0]);
        
        // Ajout des noeuds à la table
        for noeud in noeuds {
            self.addChildNode(noeud)
            
            // TODO : implémenter la fonction
            //noeud->ajusterPoints();
        }
    }
    
    /// Initialiser le matériau de la table sur la géométrie
    func initialiserMateriau() {
        let material = table.obtenirMateriau()
        self.geometry?.firstMaterial = material
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
