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
    case DEPLACEMENT = 5    // CLIENT LOURD
    case ROTATION = 6       // CLIENT LOURD
    case MISE_A_ECHELLE = 7 // CLIENT LOURD
    case DUPLIQUER = 8
    case ZOOM = 9           // CLIENT LOURD
    case POINTS_CONTROLE = 10
    // TODO : Avoir une caméra libre en tout temps
    case CAMERA_CONTROLE = 11
}

/// Les différents états du mode d'édition
enum EDITION_ETAT : Int {
    case OFFLINE_EDITION = 0
    case ONLINE_EDITION = 1
}

extension JSON {
    mutating func appendArray(json:JSON){
        if var arr = self.array{
            arr.append(json)
            self = JSON(arr)
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
    
    /// Propriétés générales de la zone de jeu (coefficients)
    private var generalProperties: GeneralProperties?
        
    /// La couleur de l'utilisateur courant (ex. lors de la sélection)
    private var currentUserColor: UIColor = MathHelper.hexToUIColor(hex: "007BC2")
    
    private let POSITION_THRESHOLD: Float = 0.1
    private let ROTATION_THRESHOLD: Float = 0.2
    private let SCALE_THRESHOLD: Float = 0.5
    
    /// Gesture recognizer
    var tapGestureRecognizer: UITapGestureRecognizer?
    var panGestureRecognizer: ImmediatePanGestureRecognizer?
    var pinchGestureRecognizer: UIPinchGestureRecognizer?
    var rotateGestureRecognizer: UIRotationGestureRecognizer?
    var normalPanGestureRecognizer: UIPanGestureRecognizer?
    
    /// Etat du modèle
    private var etat: ModeleEtat?
    
    /// Etat du mode d'édition
    private var etatEdition: EditorState?
    
    private var userManager: UserManager?
    
    private var alreadySeenOfflineTutorial: Bool = false
    
    /// Initialise la vue, l'arbre et l'état
    func initialiser() {
        self.viewController = EditorViewController.instance
        self.etat = ModeleEtatCameraControl.instance
        self.generalProperties = GeneralProperties()
        self.userManager = UserManager()
        
        self.tapGestureRecognizer = UITapGestureRecognizer(target: self, action:  #selector (self.tapGesture (_:)))
        self.panGestureRecognizer = ImmediatePanGestureRecognizer(target: self, action: #selector (self.panGesture(_:)))
        self.pinchGestureRecognizer = UIPinchGestureRecognizer(target: self, action: #selector (self.pinchGesture(_:)))
        self.rotateGestureRecognizer = UIRotationGestureRecognizer(target: self, action: #selector (self.rotateGesture(_:)))
        self.normalPanGestureRecognizer = UIPanGestureRecognizer(target: self, action: #selector (self.normalPanGesture(_:)))
        
        // Permettre la reconnaissance simultanée de gestures
        self.tapGestureRecognizer?.delegate = self.viewController
        self.pinchGestureRecognizer?.delegate = self.viewController
        self.rotateGestureRecognizer?.delegate = self.viewController
        self.normalPanGestureRecognizer?.delegate = self.viewController
        
        self.initialiserArbre()
        self.etat?.initialiser()
        self.initVue()
    }
    
    func initialiserArbre() {
        self.arbre = ArbreRendu.instance
        self.arbre?.initialiser()
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
    
    /// Retourne l'état du mode d'édition
    func obtenirEtatEdition() -> EditorState {
        return self.etatEdition!
    }
    
    /// Retourne l'état courant
    func obtenirGeneralProperties() -> GeneralProperties {
        return self.generalProperties!
    }
    
    /// Change la configuration de la zone de jeu
    func assignerGeneralProperties(coefficientFriction: Float, coefficientRebond: Float, coefficientAcceleration: Float) {
        self.generalProperties?.setCoefficientValuesFromFloat(coefficientFriction: coefficientFriction,
                                                              coefficientRebond: coefficientRebond,
                                                              coefficientAcceleration: coefficientAcceleration)
    }
    
    /// Retourne le gestionnaire d'utilisateurs
    func obtenirUserManager() -> UserManager? {
        return self.userManager
    }

    public func setCurrentUserColor(userHexColor: String) {
        self.currentUserColor = MathHelper.hexToUIColor(hex: userHexColor)
    }
    
    public func setCurrentUserDefaultColor() {
        self.currentUserColor = MathHelper.hexToUIColor(hex: "007BC2")
    }
    
    public func getCurrentUserColor() -> UIColor {
        return self.currentUserColor
    }
    
    public func setAlreadySeenOfflineTutorial(alreadySeenOfflineTutorial: Bool) {
        self.alreadySeenOfflineTutorial = alreadySeenOfflineTutorial
    }
    
    public func getAlreadySeenOfflineTutorial() -> Bool {
        return self.alreadySeenOfflineTutorial
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
    
    @objc func normalPanGesture(_ sender: UIPanGestureRecognizer) {
        self.etat?.normalPanGesture(sender: sender)
    }
    
    @objc func pinchGesture(_ sender: UIPinchGestureRecognizer) {
        self.etat?.pinchGesture(sender: sender)
    }
    
    @objc func rotateGesture(_ sender: UIRotationGestureRecognizer) {
        self.etat?.rotateGesture(sender: sender)
    }
    
    /// Réinitialise la scène.
    func reinitialiser() {
        self.arbre?.initialiser()
    }
    
    /// Modifie le modèle état en cours
    func changerModeleEtat(etat: MODELE_ETAT) {
        // Nettoyer l'état courant
        self.etat?.nettoyerEtat()
        
        switch (etat) {
            case .AUCUN:
                self.etat = nil
                break
            case .SELECTION:
                self.etat = ModeleEtatSelection.instance
                break
            case .DEPLACEMENT:
                // Ne fait rien
                break
            case .ROTATION:
                // Ne fait rien
                break
            case .MISE_A_ECHELLE:
                // Ne fait rien
                break
            case .ZOOM:
                // Ne fait rien
                break
            case .DUPLIQUER:
                self.etat = ModeleEtatDuplication.instance
                break
            case .POINTS_CONTROLE:
                self.etat = ModeleEtatPointControl.instance
                break
            case .CREATION_ACCELERATEUR:
                self.etat = ModeleEtatCreerBoost.instance
                break
            case .CREATION_MURET:
                self.etat = ModeleEtatCreerMuret.instance
                break
            case .CREATION_PORTAIL:
                self.etat = ModeleEtatCreerPortail.instance
                break
            case .CAMERA_CONTROLE:
                self.etat = ModeleEtatCameraControl.instance
                break
        }
        
        // Initialisation de l'etat
        self.etat?.initialiser()
    }
    
    /// Modifie le modèle état du mode d'édition
    func changerEditorState(etat: EDITION_ETAT) {
        switch (etat) {
            case .OFFLINE_EDITION:
                self.etatEdition = OfflineEditorState.instance
                break
            case .ONLINE_EDITION:
                self.etatEdition = OnlineEditorState.instance
                break
        }
    }

    /// Cette fonction retourne l'information sur un noeud sélectionné.
    func selectedNodeInfos(infos: inout [Float]?) -> Bool {
        let information = VisiteurInformation();
        self.arbre?.accepterVisiteur(visiteur: information);
        return information.lireInformations(infos: &infos);
    }
    
    /// Cette fonction effectue la sélection d'un noeud en mode en ligne
    func selectNode(username: String, uuid: String, isSelected: Bool, deselectAll: Bool) {
        if (self.userManager?.userExist(username: username))! {
            if deselectAll {
                self.userManager?.getUser(username: username).deselectAll()
            }
            else {
                if isSelected {
                    self.userManager?.getUser(username: username).select(uuid: uuid)
                }
                else {
                    self.userManager?.getUser(username: username).deselect(uuid: uuid)
                }
            }
        }
    }
    
    /// Cette fonction effectue la transformation d'un noeud en mode en ligne
    func setTransformByUUID(uuid: String, username: String, position: [Float], rotation: Float, scale: [Float]) {
        let node = self.userManager?.getUser(username: username).findNode(uuid: uuid)
        
        if node != nil {
            let posX = position[0]
            let posZ = position[2]
            
            if (abs((node?.position.x)! - posX) > POSITION_THRESHOLD || abs((node?.position.z)! - posZ) > POSITION_THRESHOLD) {
                node?.position.x = posX
                node?.position.z = posZ
            }
            
            if (abs((node?.rotation.w)! - rotation) > ROTATION_THRESHOLD) {
                node?.rotation.y = 1.0
                node?.rotation.w = rotation
            }
            
            let scaleZ = scale[2]
            if (abs((node?.scale.z)! - scaleZ) > SCALE_THRESHOLD) {
                node?.scale = SCNVector3.init(scale[0], scale[1], scaleZ)
            }
        }
        // Find in the entire tree if it's not in the player's selected nodes
        else {
            let nodeInTree = self.findNodeInTree(uuid: uuid)
            
            if nodeInTree != nil {
                let posX = position[0]
                let posZ = position[2]
                
                if (abs((nodeInTree?.position.x)! - posX) > POSITION_THRESHOLD || abs((nodeInTree?.position.z)! - posZ) > POSITION_THRESHOLD) {
                    nodeInTree?.position.x = posX
                    nodeInTree?.position.z = posZ
                }
                
                if (abs((node?.rotation.w)! - rotation) > ROTATION_THRESHOLD) {
                    nodeInTree?.rotation.y = 1.0
                    nodeInTree?.rotation.w = rotation
                }
                
                let scaleZ = scale[2]
                if (abs((nodeInTree?.scale.z)! - scaleZ) > SCALE_THRESHOLD) {
                    nodeInTree?.scale = SCNVector3.init(scale[0], scale[1], scaleZ)
                }
            }
        }
    }
    
    /// Cette fonction trouve un noeud dans l'arbre
    func findNodeInTree(uuid: String) -> NoeudCommun? {
        let visiteur = VisiteurByUUID(wantedUUID: uuid)
        self.arbre?.accepterVisiteur(visiteur: visiteur)
        let node = visiteur.getNode()
        
        if visiteur.getHasFound() {
            return node
        }
        
        return nil
    }
    
    /// Cette fonction modifie la position du point de contrôle en mode en ligne
    func setControlPointPosition(uuid: String, username: String, pos: [Float]) {
        let node = self.userManager?.getUser(username: username).findNode(uuid: uuid)
        
        if node != nil {
            let posX = pos[0]
            let posZ = pos[2]
            
            if (abs((node?.position.x)! - posX) > POSITION_THRESHOLD || abs((node?.position.z)! - posZ) > POSITION_THRESHOLD) {
                let position = GLKVector3.init(v: (posX, pos[1], posZ))
                node?.assignerPositionRelative(positionRelative: position)
            }
            
            // Changer la géométrie de la table
            let table = self.arbre?.childNode(withName: (self.arbre?.NOM_TABLE)!, recursively: true) as! NoeudTable
            table.updateGeometry()
        }
        // Find in the entire tree if it's not in the player's selected nodes
        else {
            let nodeInTree = self.findNodeInTree(uuid: uuid)
            
            if nodeInTree != nil {
                let posX = pos[0]
                let posZ = pos[2]
                
                if (abs((nodeInTree?.position.x)! - posX) > POSITION_THRESHOLD || abs((nodeInTree?.position.z)! - posZ) > POSITION_THRESHOLD) {
                    let position = GLKVector3.init(v: (posX, pos[1], posZ))
                    nodeInTree?.assignerPositionRelative(positionRelative: position)
                }
                
                // Changer la géométrie de la table
                let table = self.arbre?.childNode(withName: (self.arbre?.NOM_TABLE)!, recursively: true) as! NoeudTable
                table.updateGeometry()
            }
        }
    }
    
    /// Cette fonction supprime un noeud
    func deleteNode(uuid: String, username: String) {
        let node = self.userManager?.getUser(username: username).findNode(uuid: uuid)
        
        if node != nil {
            self.userManager?.getUser(username: username).deselectAll()
            node?.removeFromParentNode()
        }
        // Find in the entire tree if it's not in the player's selected nodes
        else {
            let nodeInTree = self.findNodeInTree(uuid: uuid)
            
            if nodeInTree != nil {
                self.userManager?.getUser(username: username).deselectAll()
                nodeInTree?.removeFromParentNode()
            }
        }
    }
    
    /// Cette fonction applique l'information sur un noeud sélectionné.
    func applyNodeInfos(infos: [Float]) {
        let selection = VisiteurObtenirSelection();
        self.arbre?.accepterVisiteur(visiteur: selection);
        let noeud = selection.obtenirNoeuds().first;
    
        let rotation = noeud?.eulerAngles;
        let scale = noeud?.scale;
        let position = noeud?.position;
    
        let information = VisiteurInformation();
        _ = information.ecrireInformations(infos: infos);
        self.arbre?.accepterVisiteur(visiteur: information);
    
        // Annuler les changements
        if (!(self.etat?.noeudsSurLaTable())!) {
            noeud?.position = position!;
            noeud?.scale = scale!;
            noeud?.eulerAngles = rotation!;
        } else {
            // Envoyer la commande
            FacadeModele.instance.obtenirEtatEdition().currentUserObjectTransformChanged(uuid: (noeud?.obtenirUUID())!,
                                                                                         pos: (noeud?.position)!,
                                                                                         rotation: MathHelper.determinerAngleAxeY(rotation: (noeud?.rotation)!),
                                                                                         scale: (noeud?.scale)!)
        }
    }

    /// Charger une patinoire préalablement sauvegardée
    func chargerCarte(map: MapEntity) {
        self.obtenirArbreRendu().initialiser()
        self.initializeJson()
        
        if map.json == nil {
            return
        }
        
        if let dataFromString = map.json!.data(using: .utf8, allowLossyConversion: false) {
            self.docJSON = JSON(data: dataFromString)
        }
        
        self.chargerPntCtrl()
        self.creerNoeuds(type: "Accelerateur", nomType: ArbreRendu.instance.NOM_ACCELERATEUR)
        self.creerNoeuds(type: "Portail", nomType: ArbreRendu.instance.NOM_PORTAIL)
        self.creerNoeuds(type: "Muret", nomType: ArbreRendu.instance.NOM_MUR)
        self.chargerCoefficients()
    }
    
    /// Placer les points de contrôle sur la patinoire
    func chargerPntCtrl() {
        let table = self.arbre?.childNode(withName: ArbreRendu.instance.NOM_TABLE, recursively: true) as! NoeudTable
        var count = 0
        
        for child in table.childNodes {
            if child.name == ArbreRendu.instance.NOM_POINT_CONTROL {
                let x = self.docJSON!["PointControle"][count][0].float!
                let y = self.docJSON!["PointControle"][count][1].float!
                let z = self.docJSON!["PointControle"][count][2].float!
                let pos = GLKVector3.init(v: (x, y, z))
                let noeud = child as! NoeudCommun
                noeud.assignerPositionRelative(positionRelative: pos)
                
                let uuid = self.docJSON!["PointControle"][count][7].string!
                noeud.assignerUUID(uuid: uuid)
        
                count += 1
            }
        }
        table.updateGeometry()
    }

    /// Cette fonction permet de créer les noeuds sur la patinoire
    func creerNoeuds(type: String, nomType: String) {
        let table = self.arbre?.childNode(withName: ArbreRendu.instance.NOM_TABLE, recursively: true)
        
        if nomType == ArbreRendu.instance.NOM_ACCELERATEUR || nomType == ArbreRendu.instance.NOM_MUR {
            // S'il n'y a aucun accélérateur ou de muret dans la carte à charger
            if self.docJSON![type].count == 0 {
                return
            }
            
            for i in 0...self.docJSON![type].count - 1 {
                let noeud: NoeudCommun
                let uuid = self.docJSON![type][i][7].string!
                if nomType == ArbreRendu.instance.NOM_ACCELERATEUR {
                    noeud = self.arbre?.creerNoeud(typeNouveauNoeud: nomType, uuid: uuid) as! NoeudAccelerateur
                } else {
                    noeud = self.arbre?.creerNoeud(typeNouveauNoeud: nomType, uuid: uuid) as! NoeudMur
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
            // S'il n'y a aucun portail dans la carte à charger
            if self.docJSON![type].count == 0 {
                return
            }
            
            for i in stride(from: 0, to: self.docJSON![type].count - 1, by: 2) {
                var linkedPortals = Set<NoeudPortail>()
                
                for j in 0...1 {
                    let uuid = self.docJSON![type][i + j][7].string!
                    let portal = self.arbre?.creerNoeud(typeNouveauNoeud: nomType, uuid: uuid) as! NoeudPortail
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
    
    func chargerCoefficients() {
        let cFriction = self.docJSON?["Coefficients"][0].rawString(options: [])
        let cRebond = self.docJSON?["Coefficients"][1].rawString(options: [])
        let cAcceleration = self.docJSON?["Coefficients"][2].rawString(options: [])
        
        self.generalProperties?.setCoefficientValues(coefficientFriction: cFriction!, coefficientRebond: cRebond!, coefficientAcceleration: cAcceleration!)
    }
    
    func sauvegarderCoefficients() {
        let coefficients = self.generalProperties?.getCoefficientValues()
        let friction = coefficients?[0] == nil ? 1.0 : coefficients?[0]
        let rebond = coefficients?[1] == nil ? 0.0 : coefficients?[1]
        let acceleration = coefficients?[2] == nil ? 40.0 : coefficients?[2]
        let cJSON = JSON([friction, rebond, acceleration])
        self.docJSON?["Coefficients"] = cJSON
    }
    
    func sauvegarderCarte(map: MapEntity) {
        // Mettre à jour la représentation JSON de la carte
        self.updateMapJson()
        
        // Sauvegarder la carte
        self.etatEdition?.sauvegarderCarte(map: map, json: (self.docJSON?.rawString(options: []))!, icon: nil)
    }
    
    func sauvegarderIconCarte(map: MapEntity) {
        let icon = EditorViewController.instance.takeMapSnapshot()

        // Sauvegarder la carte
        self.etatEdition?.sauvegarderCarte(map: map, json: (self.docJSON?.rawString(options: []))!, icon: icon)
    }
    
    private func updateMapJson() {
        self.initializeJson()
        let visiteur = VisiteurSauvegarde()
        self.arbre?.accepterVisiteur(visiteur: visiteur)
        self.sauvegarderCoefficients()
    }
    
    private func initializeJson() {
        self.docJSON = [
            "PointControle": [],
            "Accelerateur": [],
            "Muret": [],
            "Portail": [],
            "Coefficients": []
        ]
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
