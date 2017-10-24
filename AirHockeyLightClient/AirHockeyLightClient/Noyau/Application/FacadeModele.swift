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
                //self.etat = ModeleEtatDuplication.instance
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
            let json = try JSONSerialization.jsonObject(with: data) as? [String: Any]
            let pointControle = json?["PointControle"]
        } catch {
            print("error")
        }
        /*
        chargerPntCtrl();
    
        creerNoeuds("Accelerateur", ArbreRenduINF2990::NOM_ACCELERATEUR);
        creerNoeuds("Portail", ArbreRenduINF2990::NOM_PORTAIL);
        creerNoeuds("Muret", ArbreRenduINF2990::NOM_MUR);
    
        coefficients[0] = docJSON_["Coefficients"][0].GetDouble();
        coefficients[1] = docJSON_["Coefficients"][1].GetDouble();
        coefficients[2] = docJSON_["Coefficients"][2].GetDouble();
        */
    }
    
    /// Placer les points de contrôle sur la patinoire
    func chargerPntCtrl() {
        /*
         NoeudComposite* table = (NoeudComposite*)arbre_->chercher(ArbreRenduINF2990::NOM_TABLE);
         int i = 0;
         glm::dvec3 temp;
         for (auto it = table->obtenirIterateurBegin(); it != table->obtenirIterateurEnd(); ++it)
         {
         if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL) {
         temp.x = docJSON_["PointControle"][i][0].GetDouble();
         temp.y = docJSON_["PointControle"][i][1].GetDouble();
         temp.z = docJSON_["PointControle"][i][2].GetDouble();
         glm::vec3 pos(temp);
        (*it)->assignerPositionRelative(pos);
         i++;
         }
         }
         */
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
