///////////////////////////////////////////////////////////////////////////////
/// @file FacadeModele.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import SceneKit
import UIKit.UIGestureRecognizerSubclass

/// Les différents états du modèle
enum MODELE_ETAT : Int {
    case AUCUN = 0
    case SELECTION = 1
    case CREATION_ACCELERATEUR = 2
    case CREATION_MURET = 3
    case CREATION_PORTAIL = 4
    case DEPLACEMENT = 5
    case ROTATION = 6
    case MISE_A_ECHELLE = 7
    case DUPLIQUER = 8
    case ZOOM = 9
    case POINTS_CONTROLE = 10
    // TODO : Avoir une caméra libre en tout temps
    case CAMERA_CONTROLE = 11
}

///////////////////////////////////////////////////////////////////////////
/// @class FacadeModele
/// @brief Classe qui constitue une interface (une façade) sur l'ensemble
///        du modèle et des classes qui le composent.
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
class FacadeModele {
    
    /// Instance singleton
    static var instance = FacadeModele()
    
    /// Vue courante de la scène.
    private var viewController: EditorViewController?
    
    /// Arbre de rendu contenant les différents objets de la scène.
    private var arbre: ArbreRendu?
    
    /// Document JSON
    var docJson: [String : Any]?
    
    /// Les différentes couleurs pour chaque utilisateur en édition
    // TODO : Mettre ceci dans les infos de l'utilisateur lors de l'édition en ligne
    private let COLOR_USERS : [UIColor] =
        [
            UIColor(red: 0.0,       green: 123.0/255.0, blue: 194.0/255.0, alpha: 1.0), // USER 1
            UIColor(red: 194.0/255, green: 0.0,         blue: 158.0/255.0, alpha: 1.0), // USER 2
            UIColor(red: 194.0/255, green: 87.0/255.0,  blue: 0.0,         alpha: 1.0), // USER 3
            UIColor(red: 107.0/255, green: 194.0/255.0, blue: 0.0,         alpha: 1.0)  // USER 4
        ]
    private var colorPicker = 0
    public func getUserColor() -> UIColor {
        let color = COLOR_USERS[(colorPicker % COLOR_USERS.count)]
        colorPicker += 1
        return color
    }
    
    /// Gesture recognizer
    var tapGestureRecognizer: UITapGestureRecognizer?
    var panGestureRecognizer: ImmediatePanGestureRecognizer?
    var pinchGestureRecognizer: UIPinchGestureRecognizer?
    
    /// Etat du modèle
    private var etat: ModeleEtat?
    
    /// Initialise la vue, l'arbre et l'état
    func initialiser() {
        self.arbre = ArbreRendu.instance
        self.viewController = EditorViewController.instance
        self.etat = ModeleEtatCameraControl.instance
        
        self.tapGestureRecognizer = UITapGestureRecognizer(target: self, action:  #selector (self.tapGesture (_:)))
        self.panGestureRecognizer = ImmediatePanGestureRecognizer(target: self, action: #selector (self.panGesture(_:)))
        self.pinchGestureRecognizer = UIPinchGestureRecognizer(target: self, action: #selector (self.pinchGesture(_:)))
        
        self.arbre?.initialiser()
        self.etat?.initialiser()
        self.initVue()
    }
    
    /// Retourne la vue courante.
    func obtenirVue() -> EditorViewController {
        return self.viewController!
    }
    
    /// Retourne l'arbre de rendu.
    func obtenirArbreRendu() -> ArbreRendu {
        return self.arbre!
    }
    
    /// Retourne l'état courant
    func obtenirEtat() -> ModeleEtat {
        return self.etat!
    }
    
    func initVue() {
        self.viewController?.editorScene.rootNode.addChildNode(self.arbre!)
        
        /// Entrées de l'utilisateur
        self.viewController?.editorView.addGestureRecognizer(self.tapGestureRecognizer!)
    }
    
    /// Réaction aux entrées de l'utilisateur
    @objc func tapGesture(_ sender: UITapGestureRecognizer) {
        self.etat?.tapGesture(point: sender.location(in: sender.view))
    }
    
    @objc func panGesture(_ sender: ImmediatePanGestureRecognizer) {
        self.etat?.panGesture(sender: sender)
    }
    
    @objc func pinchGesture(_ sender: UIPinchGestureRecognizer) {
        self.etat?.pinchGesture(sender: sender)
    }
    
    /// Réinitialise la scène.
    func reinitialiser() {
        self.arbre?.initialiser()
    }
    
    /// Modifie le modèle état en cours
    func changerModeleEtat(etat: MODELE_ETAT) {
        // Nettoyer l'état courant
        self.etat?.nettoyerEtat();
        
        switch (etat) {
            case .AUCUN:
                self.etat = nil;
                break;
            case .SELECTION:
                self.etat = ModeleEtatSelection.instance
                break;
            case .DEPLACEMENT:
                self.etat = ModeleEtatDeplacement.instance
                break;
            case .ROTATION:
                self.etat = ModeleEtatRotation.instance
                break;
            case .MISE_A_ECHELLE:
                self.etat = ModeleEtatScale.instance
                break;
            case .ZOOM:
                //self.etat = ModeleEtatZoom.instance
                break;
            case .DUPLIQUER:
                self.etat = ModeleEtatDuplication.instance
                break;
            case .POINTS_CONTROLE:
                self.etat = ModeleEtatPointControl.instance
                break;
            case .CREATION_ACCELERATEUR:
                self.etat = ModeleEtatCreerBoost.instance
                break;
            case .CREATION_MURET:
                self.etat = ModeleEtatCreerMuret.instance
                break;
            case .CREATION_PORTAIL:
                self.etat = ModeleEtatCreerPortail.instance
                break;
            case .CAMERA_CONTROLE:
                self.etat = ModeleEtatCameraControl.instance
                break;
        }
        
        // Initialisation de l'etat
        self.etat?.initialiser();
    }
    
    /// Charger une patinoire préalablement suavegarder
    func ouvrir() {
        
        let jsonTmp = "{\"PointControle\":[[0.0,10.0,125.8864974975586,1.0,1.0,1.0,0.0],[126.68693542480469,10.0,212.6820068359375,1.0,1.0,1.0,0.0],[92.35729217529297,10.0,0.0,1.0,1.0,1.0,0.0],[126.68693542480469,10.0,-212.6820068359375,1.0,1.0,1.0,0.0],[0.0,10.0,-125.8864974975586,1.0,1.0,1.0,0.0],[-125.60102844238281,10.0,-212.32012939453126,1.0,1.0,1.0,0.0],[-92.35729217529297,10.0,0.0,1.0,1.0,1.0,0.0],[-125.60102844238281,10.0,212.32012939453126,1.0,1.0,1.0,0.0]],\"Accelerateur\":[[31.0,0.0,-126.79999542236328,1.0,1.0,1.0,0.0],[90.19999694824219,0.0,-81.5999984741211,1.0,1.0,1.0,0.0],[98.19999694824219,0.0,101.5999984741211,1.0,1.0,1.0,0.0],[47.0,0.0,144.79998779296876,1.0,1.0,1.0,0.0],[-55.0,0.0,150.0,1.0,1.0,1.0,0.0],[-101.79999542236328,0.0,119.99999237060547,1.0,1.0,1.0,0.0],[-94.19999694824219,0.0,-84.39999389648438,1.0,1.0,1.0,0.0],[-36.20000076293945,0.0,-134.39999389648438,1.0,1.0,1.0,0.0],[-0.6000000238418579,0.0,-97.5999984741211,1.0,1.0,1.0,0.0],[0.6000000238418579,0.0,94.39999389648438,1.0,1.0,1.0,0.0],[81.80000305175781,0.0,15.199999809265137,1.0,1.0,1.0,0.0],[83.0,0.0,-18.799999237060548,1.0,1.0,1.0,0.0],[-83.4000015258789,0.0,-18.0,1.0,1.0,1.0,0.0],[-84.5999984741211,0.0,16.799999237060548,1.0,1.0,1.0,0.0],[-65.0,0.0,0.3999999761581421,1.0,1.0,1.0,0.0],[64.19999694824219,0.0,0.0,1.0,1.0,1.0,0.0]],\"Muret\":[[59.19821548461914,0.0,-100.96479034423828,1.0,1.0,38.38910675048828,-2.200441598892212],[-66.65220642089844,0.0,-107.81365966796875,1.0,1.0,39.73455810546875,-0.8960553407669067],[71.63633728027344,0.0,120.19480895996094,1.0,1.0,40.82229995727539,-0.8468590974807739],[-79.6523208618164,0.0,133.20057678222657,1.0,1.0,30.613128662109376,-2.170846700668335]],\"Portail\":[[-91.79999542236328,0.0,-150.39999389648438,1.0,1.0,1.0,0.0],[95.0,0.0,157.1999969482422,1.0,1.0,1.0,0.0],[-94.5999984741211,0.0,165.1999969482422,1.0,1.0,1.0,0.0],[89.0,0.0,-149.1999969482422,1.0,1.0,1.0,0.0],[81.0,0.0,-0.7999999523162842,1.0,1.0,1.0,0.0],[-83.0,0.0,-0.3999999761581421,1.0,1.0,1.0,0.0]],\"Coefficients\":[0.4000000059604645,0.0,40.0]}"
    
        self.obtenirArbreRendu().initialiser();
        
        do {
            let data = jsonTmp.data(using: String.Encoding.utf8)!
            self.docJson = try JSONSerialization.jsonObject(with: data) as? [String : Any]
        } catch {
            print("error")
        }
        
        self.chargerPntCtrl();

        self.creerNoeuds(type: "Accelerateur", nomType: ArbreRendu.instance.NOM_ACCELERATEUR)
        self.creerNoeuds(type: "Portail", nomType: ArbreRendu.instance.NOM_PORTAIL)
        self.creerNoeuds(type: "Muret", nomType: ArbreRendu.instance.NOM_MUR)
        
        /*
        coefficients[0] = docJSON_["Coefficients"][0].GetDouble();
        coefficients[1] = docJSON_["Coefficients"][1].GetDouble();
        coefficients[2] = docJSON_["Coefficients"][2].GetDouble();
        */
    }
    
    /// Placer les points de contrôle sur la patinoire
    func chargerPntCtrl() {
        let table = self.arbre?.childNode(withName: ArbreRendu.instance.NOM_TABLE, recursively: true) as! NoeudTable
        let pointsControle = self.docJson!["PointControle"] as! [Any]
        var count = 0;
        
        for child in table.childNodes {
            if child.name == ArbreRendu.instance.NOM_POINT_CONTROL {
                let pointControle = pointsControle[count] as! [Any]
                let x = pointControle[0] as? Float
                let y = pointControle[1] as? Float
                let z = pointControle[2] as? Float
                let pos = GLKVector3.init(v: (x!, y!, z!))
                let noeud = child as! NoeudCommun
                noeud.assignerPositionRelative(positionRelative: pos)
                count += 1
            }
        }
        table.updateGeometry()
    }

    /// Cette fonction permet de créer les noeuds sur la patinoire
    func creerNoeuds(type: String, nomType: String) {
        let table = self.arbre?.childNode(withName: ArbreRendu.instance.NOM_TABLE, recursively: true)
        
        if nomType == ArbreRendu.instance.NOM_ACCELERATEUR || nomType == ArbreRendu.instance.NOM_MUR {
            let typeJson = self.docJson![type] as! [Any]
            
            for i in 0...typeJson.count - 1 {
                let typeInfo = typeJson[i] as! [Any]
                
                let noeud: NoeudCommun
                if nomType == ArbreRendu.instance.NOM_ACCELERATEUR {
                    noeud = self.arbre?.creerNoeud(typeNouveauNoeud: nomType) as! NoeudAccelerateur
                } else {
                    noeud = self.arbre?.creerNoeud(typeNouveauNoeud: nomType) as! NoeudMur
                }
                
                // Appliquer rotation
                let angle = typeInfo[6] as! Float
                noeud.appliquerRotation(angle: angle, axes: GLKVector3.init(v: (0, 1, 0)))
                
                // Appliquer scale
                var scale = SCNVector3.init()
                scale.x = typeInfo[3] as! Float
                scale.y = typeInfo[4] as! Float
                scale.z = typeInfo[5] as! Float
                noeud.scale = scale
                table?.addChildNode(noeud)
                
                // Appliquer déplacement
                var deplacement = GLKVector3.init()
                deplacement.x = typeInfo[0] as! Float
                deplacement.y = typeInfo[1] as! Float
                deplacement.z = typeInfo[2] as! Float
                noeud.appliquerDeplacement(deplacement: deplacement)
            }
        }
        else if nomType == ArbreRendu.instance.NOM_PORTAIL {
            let typeJson = self.docJson![type] as! [Any]
            
            for i in stride(from: 0, to: typeJson.count - 1, by: 2) {
                var linkedPortals = Set<NoeudPortail>()
                
                for j in 0...1 {
                    let portal = self.arbre?.creerNoeud(typeNouveauNoeud: nomType) as! NoeudPortail
                    linkedPortals.insert(portal)
                    
                    // Appliquer rotation
                    let typeInfo = typeJson[i + j] as! [Any]
                    let angle = typeInfo[6] as! Float
                    portal.appliquerRotation(angle: angle, axes: GLKVector3.init(v: (0, 1, 0)))
                    
                    // Appliquer scale
                    var scale = SCNVector3.init()
                    scale.x = typeInfo[3] as! Float
                    scale.y = typeInfo[4] as! Float
                    scale.z = typeInfo[5] as! Float
                    portal.scale = scale
                    table?.addChildNode(portal)
                    
                    // Appliquer déplacement
                    var deplacement = GLKVector3.init()
                    deplacement.x = typeInfo[0] as! Float
                    deplacement.y = typeInfo[1] as! Float
                    deplacement.z = typeInfo[2] as! Float
                    portal.appliquerDeplacement(deplacement: deplacement)
                }
                
                // Assigner portails opposés
                let portalA = linkedPortals[linkedPortals.index(linkedPortals.startIndex, offsetBy: 0)]
                let portalB = linkedPortals[linkedPortals.index(linkedPortals.startIndex, offsetBy: 1)]
                portalA.assignerOppose(portail: portalB)
                portalB.assignerOppose(portail: portalA)
            }
        }
    }
    
    /// Cette fonction permet d'enregistrer le patinoire en format JSON
    func enregistrerSous(filePath: String, coefficients: [Float]) {
        let json = self.getMapJson(coefficients: coefficients);
    }
    
    func getMapJson(coefficients: [Float]) -> String {
        let dict: [String: Any] = ["PointControle":"", "Accelerateur":"", "Muret":"", "Portail": [coefficients[0], coefficients[1], coefficients[2]]]
        
        if let data = try? JSONSerialization.data(withJSONObject: dict, options: .prettyPrinted) {
            let str = String(bytes: data, encoding: .utf8)
            print(str)
        }
        
        /*
        VisiteurSauvegarde visiteur = VisiteurSauvegarde();
        arbre_->accepterVisiteur(&visiteur);
     
        rapidjson::StringBuffer buffer2;
        rapidjson::Writer<rapidjson::StringBuffer>writer(buffer2);
        docJSON_.Accept(writer);
        std::string data(buffer2.GetString(), buffer2.GetSize());
        return data;
         */
        
        return ""
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
