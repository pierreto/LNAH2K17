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
    
    /// Le modèle de la table
    private let table = Table()
    
    /// Les sommets du modèle de la table
    public  var sommets = [SCNVector3]()
    
    /// Les buts associés à la table
    private var buts = [NoeudBut]()
    
    /// La table n'a pas un modèle obj
    required init(type: String, geometry: SCNGeometry) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    /// Constructeur
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
        elements.append(SCNGeometryElement(data: indexData as Data,
                                           primitiveType: .triangles,
                                           primitiveCount: self.table.FACES_COUNT,
                                           bytesPerIndex: MemoryLayout<GLuint>.size))
        
        return elements
    }
    
    /// Initialiser le matériau de la table sur la géométrie
    func initialiserMateriau() {
        let material = table.obtenirMateriau()
        self.geometry?.firstMaterial = material
    }
    
    // Remettre à jour la geometrie de la table
    func updateGeometry() {
        var sources = [SCNGeometrySource]()
        sources.append(SCNGeometrySource(vertices: self.sommets))
        sources.append(SCNGeometrySource(textureCoordinates: table.obtenirTexCoords()))
        
        self.geometry = SCNGeometry(sources: sources, elements: initElements())
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
            
            // Ajouter le NoeudPointControl aux enfants du NoeudTable
            self.addChildNode(noeudPointControl)
            
            // TODO : Ajouter noeud booster
            /*NoeudBooster* noeudBooster{ (NoeudBooster*)arbre->creerNoeud(ArbreRenduINF2990::NOM_BOOSTER) };
            noeudBooster->assignerAxisLock(glm::ivec3(1, 1, 1));
            noeudBooster->appliquerDeplacement(glm::vec3(0, -TABLE_BASE_HEIGHT, 0));
            noeudBooster->assignerAxisLock(glm::ivec3(1, 0, 1));*/
            
            /// Index des sommets de la table associés au point de contrôle
            var pointIndexes = [Int]()
            
            for j in 0..<self.table.VERTEX_PER_CORNER {
                pointIndexes.append(i + j)
            }

            noeudPointControl.assignerPointIndexes(pointIndexes: pointIndexes)
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
            voisins.append(noeuds[MathHelper.mod((l - 1), noeuds.count)])
            voisins.append(noeuds[MathHelper.mod((l + 1), noeuds.count)])
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
    
        // Creation des buts
        creerButs(pointGauche: noeuds[4], pointDroite: noeuds[0]);
        
        noeuds[7].assignerPositionRelative(positionRelative: noeuds[6].obtenirPositionRelative())
        
        // Ajuster les sommets de la table
        for noeud in noeuds {
            noeud.ajusterPoints()
        }
        
        // Mettre à jour la géométrie de la table
        self.updateGeometry()
    }
    
    /// Cette fonction permet de créer des buts
    func creerButs(pointGauche: NoeudPointControl, pointDroite: NoeudPointControl) {
        // obtention de l'arbre
        let arbre = ArbreRendu.instance
        let butGauche = arbre.creerNoeud(typeNouveauNoeud: arbre.NOM_BUT) as! NoeudBut
        let butDroite = arbre.creerNoeud(typeNouveauNoeud: arbre.NOM_BUT) as! NoeudBut
    
        // Deplacer les buts
        // let offset: Float = 10.0
        //butGauche.position = SCNVector3FromGLKVector3( GLKVector3Add(pointGauche.obtenirPositionRelative(), GLKVector3(v: (0.0, 0.0, offset))) )
        //butDroite.position = SCNVector3FromGLKVector3( GLKVector3Add( pointDroite.obtenirPositionRelative(), GLKVector3(v: (0.0, 0.0, -offset))) )
        // TODO : Vérifier que la fonction deplacer revient à faire .position. Pas besoin de déplacer les buts ?
        //butGauche->deplacer(pointGauche->obtenirPositionRelative() + glm::vec3(0, 0, offset));
        //butDroite->deplacer(pointDroite->obtenirPositionRelative() + glm::vec3(0, 0, -offset));
    
        // Associer avec un point controle
        pointGauche.assignerBut(noeud: butGauche)
        pointDroite.assignerBut(noeud: butDroite)
    
        // Ajout dans l'arbre
        self.addChildNode(butGauche)
        self.addChildNode(butDroite)
    
        buts.append(butGauche)
        buts.append(butDroite)
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
