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

///////////////////////////////////////////////////////////////////////////
/// @class NoeudBut
/// @brief Classe pour afficher un but
///
/// @author Pierre To
/// @date 2017-10-08
///////////////////////////////////////////////////////////////////////////
class NoeudBut : NoeudCommun {
    
    /// Le modèle du but
    private let but = But()
    
    /// Les sommets du modèle du but
    private var sommets = [SCNVector3]()
    
    /// Les faces du modèle du but
    private var faces = [GLuint]()
    
    /// Pointeur vers le point de controle
    private var pointControl : NoeudPointControl?
    
    /// Le but n'a pas un modèle obj
    required init(type : String, geometry: SCNGeometry) {
        fatalError("init(type:) has not been implemented")
    }
    
    /// Constructeur
    required init(type: String) {
        super.init(type: type)
        self.sommets = [SCNVector3].init(repeating: SCNVector3(0.0, 0.0, 0.0), count: 12)
        
        // Les buts ne sont pas sélectionnable
        self.assignerEstSelectionnable(selectionnable: false)
    }
    
    required init(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Crée la liste des éléments de la géométrie
    func initElements() -> [SCNGeometryElement] {
        var elements = [SCNGeometryElement]()
        
        self.faces = but.obtenirFaces()
        let indexData = NSData(bytes: faces, length: faces.count * MemoryLayout<GLuint>.size)
        elements.append(SCNGeometryElement(data: indexData as Data,
                                           primitiveType: .triangles,
                                           primitiveCount: self.but.FACES_COUNT,
                                           bytesPerIndex: MemoryLayout<GLuint>.size))
        
        return elements
    }

    /// Ajoute le matériau de la table sur la géométrie
    func addMaterial() {
        let material = but.obtenirMateriau()
        self.geometry?.firstMaterial = material
    }
    
    // Remettre à jour la geometrie du but
    func updateGeometry() {
        var sources = [SCNGeometrySource]()
        sources.append(SCNGeometrySource(vertices: self.sommets))
        let elements = self.initElements()
        
        self.geometry = SCNGeometry(sources: sources, elements: elements)
        self.addMaterial()
    }
    
    /// Assigne un point de contrôle à un but
    func assignerPointControl(pointControl: NoeudPointControl) {
        self.pointControl = pointControl;
        self.ajusterPoints();
    }
    
    /// Ajuste les sommets du but en fonction de la position du point de contrôle
    func ajusterPoints() {
        if (self.pointControl == nil) {
            return;
        }
    
        let pc0 = self.pointControl?.obtenirPositionRelative();
        let pc1 = self.pointControl?.obtenirVoisins()[0].obtenirPositionRelative();
        let pc2 = self.pointControl?.obtenirVoisins()[1].obtenirPositionRelative();
        
        let v1 = GLKVector3Normalize(GLKVector3Subtract(pc1!, pc0!))
        let v2 = GLKVector3Normalize(GLKVector3Subtract(pc2!, pc0!))
    
        // Calcul de la bisectrice
        var bisect: GLKVector3;
    
        if (GLKVector3Length(GLKVector3Add(v1, v2)) < 0.001) { // Droite perpendiculaire
            bisect = GLKVector3Normalize(GLKVector3Make(-v1.z, 0, v1.x));
        }
        else {
            bisect = GLKVector3Normalize(GLKVector3Add(v1, v2));
        }
    
        // Inverse bisect if needed
        let dotProduct = MathHelper.clamp(value: GLKVector3DotProduct(bisect, GLKVector3Normalize(pc0!)),
                                          lower: -1.0, upper: 1.0);
        if (acos(dotProduct) > Float.pi/2) {
            bisect = GLKVector3Negate(bisect);
        }
    
        // Calcul de la diagonale de la bordure
        let angle = Float.pi/2 - acos(MathHelper.clamp(value: GLKVector3DotProduct(v1, v2), lower: -1.0, upper: 1.0)) / 2;
        let h =  Float(Table.TABLE_BORDER_WIDTH) / cos(angle) * 0.5;
    
        // En bas
        self.sommets[0].x = (pc0?.x)!;
        self.sommets[0].z = (pc0?.z)! - 0.5 * bisect.z;
        // En haut
        self.sommets[1] = SCNVector3FromGLKVector3(pc0!);
        self.sommets[1].y += 1;
        self.sommets[1].z = (pc0?.z)! - 0.5 * bisect.z;
    
        let p1 = GLKVector3Add(pc0!, GLKVector3MultiplyScalar(v1, self.but.GOAL_WIDTH / 2.0));
        // En bas
        self.sommets[2].x = p1.x;
        self.sommets[2].z = p1.z - 0.5 * bisect.z;
        // En haut
        self.sommets[3] = SCNVector3FromGLKVector3(p1);
        self.sommets[3].y += 1;
        self.sommets[3].z = p1.z - 0.5 * bisect.z;
    
        let p2 = GLKVector3Add(
            GLKVector3Add(pc0!, GLKVector3MultiplyScalar(v1, self.but.GOAL_WIDTH / 2.0)),
            GLKVector3MultiplyScalar(bisect, h))
        // En bas
        self.sommets[4].x = p2.x;
        self.sommets[4].z = p2.z;
        // En haut
        self.sommets[5] = SCNVector3FromGLKVector3(p2);
        self.sommets[5].y += 1;
    
        let p3 = GLKVector3Add(pc0!, GLKVector3MultiplyScalar(bisect, h))
        // En bas
        self.sommets[6].x = p3.x;
        self.sommets[6].z = p3.z;
        // En haut
        self.sommets[7] = SCNVector3FromGLKVector3(p3);
        self.sommets[7].y += 1;
    
        let p4 = GLKVector3Add(
            GLKVector3Add(pc0!, GLKVector3MultiplyScalar(v2, self.but.GOAL_WIDTH / 2.0)),
            GLKVector3MultiplyScalar(bisect, h))
        // En bas
        self.sommets[8].x = p4.x;
        self.sommets[8].z = p4.z;
        // En haut
        self.sommets[9] = SCNVector3FromGLKVector3(p4);
        self.sommets[9].y += 1;
    
        let p5 = GLKVector3Add(pc0!, GLKVector3MultiplyScalar(v2, self.but.GOAL_WIDTH / 2.0))
        // En bas
        self.sommets[10].x = p5.x;
        self.sommets[10].z = p5.z - 0.5 * bisect.z;
        // En haut
        self.sommets[11] = SCNVector3FromGLKVector3(p5);
        self.sommets[11].y += 1;
        self.sommets[11].z = p5.z - 0.5 * bisect.z;
        
        // Mettre à jour la géométrie du but
        self.updateGeometry()
    
        // TODO : Ajouter le collider
        /*collider_.clear();
        // Donnees les points du collider
        collider_.push_back(sommets_[2]);
        collider_.push_back(sommets_[4]);
        collider_.push_back(sommets_[6]);
        collider_.push_back(sommets_[8] );
        collider_.push_back(sommets_[10]);
        collider_.push_back(sommets_[0]);*/
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
