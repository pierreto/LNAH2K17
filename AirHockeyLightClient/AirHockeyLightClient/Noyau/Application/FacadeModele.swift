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
import SwiftyJSON
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

extension JSON {
    mutating func appendArray(json:JSON){
        if var arr = self.array{
            arr.append(json)
            self = JSON(arr);
        }
    }
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
    var docJSON: JSON?
    
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
    func ouvrir(filePath: String, coefficients: [Float]) {
        // TODO : extraire les informations de la carte à partir du file path
        let jsonString = "{\"PointControle\":[[0, 0, 0, 0, 0, 0, 0]]," +
                          "\"Accelerateur\":[[0, 0, 0, 0, 0, 0, 0]]," +
                          "\"Muret\"[[0, 0, 0, 0, 0, 0, 0]]," +
                          "\"Portail\":[[0, 0, 0, 0, 0, 0, 0]]," +
                          "\"Coefficients\":[0, 0, 0]}"
    
        self.obtenirArbreRendu().initialiser()
        self.initializeJson()
        
        if let dataFromString = jsonString.data(using: .utf8, allowLossyConversion: false) {
            self.docJSON = JSON(data: dataFromString)
        }
        
        self.chargerPntCtrl()

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
        var count = 0;
        
        for child in table.childNodes {
            if child.name == ArbreRendu.instance.NOM_POINT_CONTROL {
                let x = self.docJSON!["PointControle"][count][0].float!
                let y = self.docJSON!["PointControle"][count][1].float!
                let z = self.docJSON!["PointControle"][count][2].float!
                let pos = GLKVector3.init(v: (x, y, z))
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
            for i in 0...self.docJSON![type].count - 1 {
                let noeud: NoeudCommun
                if nomType == ArbreRendu.instance.NOM_ACCELERATEUR {
                    noeud = self.arbre?.creerNoeud(typeNouveauNoeud: nomType) as! NoeudAccelerateur
                } else {
                    noeud = self.arbre?.creerNoeud(typeNouveauNoeud: nomType) as! NoeudMur
                }
                
                // Appliquer rotation
                let angle = self.docJSON![type][i][6].float!
                noeud.appliquerRotation(angle: angle, axes: GLKVector3.init(v: (0, 1, 0)))
                
                // Appliquer scale
                var scale = SCNVector3.init()
                scale.x = self.docJSON![type][i][3].float!
                scale.y = self.docJSON![type][i][4].float!
                scale.z = self.docJSON![type][i][5].float!
                noeud.scale = scale
                table?.addChildNode(noeud)
                
                // Appliquer déplacement
                var deplacement = GLKVector3.init()
                deplacement.x = self.docJSON![type][i][0].float!
                deplacement.y = self.docJSON![type][i][1].float!
                deplacement.z = self.docJSON![type][i][2].float!
                noeud.appliquerDeplacement(deplacement: deplacement)
            }
        }
        else if nomType == ArbreRendu.instance.NOM_PORTAIL {
            for i in stride(from: 0, to: self.docJSON![type].count - 1, by: 2) {
                var linkedPortals = Set<NoeudPortail>()
                
                for j in 0...1 {
                    let portal = self.arbre?.creerNoeud(typeNouveauNoeud: nomType) as! NoeudPortail
                    linkedPortals.insert(portal)
                    
                    // Appliquer rotation
                    let angle = self.docJSON![type][i + j][6].float!
                    portal.appliquerRotation(angle: angle, axes: GLKVector3.init(v: (0, 1, 0)))
                    
                    // Appliquer scale
                    var scale = SCNVector3.init()
                    scale.x = self.docJSON![type][i + j][3].float!
                    scale.y = self.docJSON![type][i + j][4].float!
                    scale.z = self.docJSON![type][i + j][5].float!
                    portal.scale = scale
                    table?.addChildNode(portal)
                    
                    // Appliquer déplacement
                    var deplacement = GLKVector3.init()
                    deplacement.x = self.docJSON![type][i + j][0].float!
                    deplacement.y = self.docJSON![type][i + j][1].float!
                    deplacement.z = self.docJSON![type][i + j][2].float!
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
    
    /// Cette fonction permet d'enregistrer la patinoire en format JSON
    func enregistrerSous(filePath: String, coefficients: [Float]) {
        self.initializeJson()
        let visiteur = VisiteurSauvegarde();
        self.arbre?.accepterVisiteur(visiteur: visiteur);
        
        // TODO : sauver le fichier localement ou via le serveur
    }
    
    private func initializeJson() {
        self.docJSON = [
            "PointControle": [],
            "Accelerateur": [],
            "Muret": [],
            "Portail": [],
            "Coefficients": [0, 0, 0]
        ]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
