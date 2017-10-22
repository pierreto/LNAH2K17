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
    
    /// Identifiant de la table (utilisé pour le hit test)
    public static let CATEGORY_BIT_MASK = 0b100
    
    /// Le modèle de la table
    private let table = Table()
    
    /// Les buts associés à la table
    private var buts = [NoeudBut]()
    
    /// Ligne du centre
    private var ligneCentreNoeud: NoeudCommun?
    
    /// Les sommets du modèle de la table
    public  var sommets = [SCNVector3]()
    
    /// La table n'a pas un modèle obj
    required init(type: String, geometry: SCNGeometry) {
        fatalError("init(type:geometry:) has not been implemented")
    }
    
    /// Constructeur
    required init(type: String) {
        super.init(type: type)
        
        self.initialiserTable()
        self.creerPointsDeControle()
        self.creerLigneCentre()
        
        // Mettre à jour la géométrie de la table
        self.updateGeometry()
        
        self.categoryBitMask = 0b100
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
    override func accepterVisiteur(visiteur: VisiteurAbstrait) {
        // Envoie le visiteur aux enfants
        super.accepterVisiteur(visiteur: visiteur)
        
        visiteur.visiterTable(noeud: self)
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
    
    // Remettre à jour la geometrie de la table
    func updateGeometry() {
        var sources = [SCNGeometrySource]()
        sources.append(SCNGeometrySource(vertices: self.sommets))
        sources.append(SCNGeometrySource(textureCoordinates: table.obtenirTexCoords()))
        
        self.geometry = SCNGeometry(sources: sources, elements: initElements())
        self.initialiserMateriau()
        
        self.updateLigneCentre()
    }
    
    /// Initialiser le matériau de la table sur la géométrie
    func initialiserMateriau() {
        let material = table.obtenirMateriau()
        self.geometry?.firstMaterial = material
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
        
        // Ajuster les sommets de la table
        for noeud in noeuds {
            noeud.ajusterPoints()
        }
    }
    
    /// Cette fonction permet de créer des buts
    func creerButs(pointGauche: NoeudPointControl, pointDroite: NoeudPointControl) {
        // Obtention de l'arbre
        let arbre = ArbreRendu.instance
        let butGauche = arbre.creerNoeud(typeNouveauNoeud: arbre.NOM_BUT) as! NoeudBut
        let butDroite = arbre.creerNoeud(typeNouveauNoeud: arbre.NOM_BUT) as! NoeudBut
    
        // Deplacer les buts
        //let offset: Float = 10.0
        //butGauche.position = SCNVector3FromGLKVector3( GLKVector3Add(pointGauche.obtenirPositionRelative(), GLKVector3(v: (0.0, 0.0, offset))) )
        //butDroite.position = SCNVector3FromGLKVector3( GLKVector3Add(pointDroite.obtenirPositionRelative(), GLKVector3(v: (0.0, 0.0, -offset))) )
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
    
    /// Cette fonction crée la ligne du centre
    func creerLigneCentre() {
        // Sommets de la ligne
        let sommetA = self.sommets[31]
        let sommetB = self.sommets[11]
        let vecteurSommets = GLKVector3Make(sommetA.x - sommetB.x, sommetA.y - sommetB.y, sommetA.z - sommetB.z)
        
        // Géométrie de la ligne
        let ligne = SCNPlane(width: 2.5, height: CGFloat(GLKVector3Length(vecteurSommets)) + 5.0)
        self.ligneCentreNoeud = NoeudCommun(type: ArbreRendu.instance.NOM_LIGNE_CENTRE, geometry: ligne)
        
        let xAngle = SCNMatrix4MakeRotation(Float(Double.pi / 2.0), 0, 1, 0)
        let yAngle = SCNMatrix4MakeRotation(Float(Double.pi / 2.0), 0, 0, 1)
        let zAngle = SCNMatrix4MakeRotation(0, 1, 0, 0)
        let rotationMatrix = SCNMatrix4Mult(SCNMatrix4Mult(xAngle, yAngle), zAngle)
        
        // Effectuer la rotation de la ligne du centre
        self.ligneCentreNoeud?.transform = SCNMatrix4Mult(rotationMatrix, (self.ligneCentreNoeud?.transform)!)
        
        // Effectuer la translation de la ligne du centre
        self.ligneCentreNoeud?.transform = SCNMatrix4Translate((self.ligneCentreNoeud?.transform)!, 0, 0.2, 0)
        
        // Matériel de la ligne
        self.ligneCentreNoeud?.geometry?.firstMaterial?.diffuse.contents = UIColor(red: 1.0, green: 0, blue: 0, alpha: 1.0)
        
        // La ligne du centre n'est pas sélectionnable
        self.ligneCentreNoeud?.assignerEstSelectionnable(selectionnable: false)
        
        self.addChildNode(self.ligneCentreNoeud!)
    }
    
    /// Cette fonction met à jour la géométrie de la ligne du centre
    func updateLigneCentre() {
        // Sommets de la ligne du centre
        let sommetA = self.sommets[31]
        let sommetB = self.sommets[11]
        let vecteurSommets = GLKVector3Make(sommetA.x - sommetB.x, sommetA.y - sommetB.y, sommetA.z - sommetB.z)
        
        // Géométrie de la ligne
        let ligne = SCNPlane(width: 2.5, height: CGFloat(GLKVector3Length(vecteurSommets)) + 5.0)
        self.ligneCentreNoeud?.geometry = ligne
        
        // Matériel de la ligne
        self.ligneCentreNoeud?.geometry?.firstMaterial?.diffuse.contents = UIColor(red: 1.0, green: 0, blue: 0, alpha: 1.0)
    }
    
    /// Obtient les sommets composant la patinoire
    func obtenirSommetsPatinoire() -> [SCNVector3] {
        var patinoire = [SCNVector3]()
        patinoire.append(self.sommets[0])
        
        var i = 1
        let max = self.sommets.count - 1
        
        while (i < max) {
            patinoire.append(sommets[i])
            i += 5
        }
    
        return patinoire
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
