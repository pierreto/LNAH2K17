///////////////////////////////////////////////////////////////////////////////
/// @file NoeudCommun.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class NoeudCommun
/// @brief Classe de base du patron composite utilisée pour créer l'arbre de rendu
///        Cette classe comprend l'interface de base que doivent implanter 
///        tous les noeuds pouvant être présent dans l'arbre de rendu
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
///////////////////////////////////////////////////////////////////////////
class NoeudCommun : SCNNode {
    
    /// Identifiant du noeud
    private var uuid: String = ""
    
    /// Vecteur spécifiant les axes bloqués pour le déplacement
    private var axisLock: GLKVector3 = GLKVector3(v: (1.0, 0.0, 1.0))
    
    /// Sélection du noeud
    private var selectionne: Bool = false
    
    /// Vrai si le noeud est sélectionnable
    private var selectionnable: Bool = true
    
    /// Sélection par un utilisateur
    private var selectionneByAnotherUser: Bool = false
    
    // Attributs de sauvegarde
    private var savedScale = SCNVector3()
    private var savedRotation = SCNVector4()
    private var savedPosition = SCNVector3()
    
    // Attributs de couleur du noeud
    private var defaultColor: UIColor = UIColor(red: 1.0, green: 1.0, blue: 1.0, alpha: 1.0)
    private var selectionnableColor: UIColor = UIColor(red: 1.0, green: 1.0, blue: 1.0, alpha: 1.0)
    private var selectionneColor: UIColor = FacadeModele.instance.getUserColor()
    
    /// Constructeur avec géométrie
    required init(type: String, geometry: SCNGeometry, uuid: String) {
        super.init()
        self.geometry = geometry
        self.name = type
        self.uuid = uuid.isEmpty ? self.genererUUID() : uuid
    }
    
    /// Constructeur sans géométrie
    required init(type: String, uuid: String) {
        super.init()
        self.name = type
        self.uuid = uuid.isEmpty ? self.genererUUID() : uuid
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Cette fonction permet d'itérer à travers tous les noeuds enfants avec le visiteur
    func accepterVisiteur(visiteur: VisiteurAbstrait) {
        for child in self.childNodes {
            if (child is NoeudCommun) {
                let noeud = child as! NoeudCommun
                noeud.accepterVisiteur(visiteur: visiteur)
            }
        }
    }
    
    func obtenirUUID() -> String {
        return self.uuid
    }
    
    func assignerUUID(uuid: String) {
        self.uuid = uuid
    }
    
    func genererUUID() -> String {
        return UUID().uuidString
    }
    
    /// Obtient le type du noeud
    func obtenirType() -> String {
        return self.name!
    }
    
    /// Cette fonction permet d'assigner l'état d'être sélectionné ou non du noeud.
    func assignerSelection(selectionne: Bool)
    {
        // Un objet non sélectionnable n'est jamais sélectionné.
        self.selectionne = (selectionne && self.selectionnable);
    
        // Changer le matériel selon la sélection
        if (self.selectionne) {
            self.appliquerMaterielSelection(activer: true)
        }
        else {
            self.appliquerMaterielSelection(activer: false)
            
            // Appliquer une autre couleur si le noeud n'est pas sélectionné et est sélectionnable
            if self.selectionnable {
                self.useOtherColor(activer: true, color: self.selectionnableColor)
            }
        }
        
        // Activer les fonctonnalités liées à la sélection d'au moins un objet
        let selection = VisiteurObtenirSelection()
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: selection)
        let noeuds = selection.obtenirNoeuds()
        FacadeModele.instance.obtenirVue().editorHUDScene?.showSelectContextButtons(activer: (noeuds.count > 0))
        
        // Afficher les propriétés sur l'objet sélectionné
        FacadeModele.instance.obtenirVue().showObjectPropertiesView(activer: noeuds.count == 1)
    }

    /// Cette fonction retourne l'état d'être sélectionné ou non du noeud.
    func estSelectionne() -> Bool
    {
        // Un objet non sélectionnable n'est jamais sélectionné.
        return (self.selectionne && self.selectionnable);
    }
    
    /// Désélectonne tous les noeuds qui sont sélectitonnés parmi les descendants de ce noeud, lui-même étant inclus
    func deselectionnerTout() {
        self.assignerSelection(selectionne: false)
        
        for child in self.childNodes {
            if child is NoeudCommun {
                let noeud = child as! NoeudCommun
                noeud.deselectionnerTout()
            }
        }
    }
    
    /// Écrit si le noeud peut être sélectionné ou non.
    func assignerEstSelectionnable(selectionnable: Bool) {
        self.selectionnable = selectionnable
        
        // Colorer le noeud devenu sélectionnable
        if self.defaultColor != self.selectionnableColor {
            self.useOtherColor(activer: selectionnable, color: self.selectionnableColor)
        }
    }
    
    /// Vérifie si le noeud est sélectionnable.
    func estSelectionnable() -> Bool {
        return self.selectionnable
    }
    
    /// Écrit si le noeud est sélectionné par un autre utilisateur
    func assignerSelectionneByAnotherUser(estSelectionneByAnotherUser: Bool) {
        self.selectionneByAnotherUser = estSelectionneByAnotherUser
    }
    
    /// Vérifie si le noeud est sélectionnable.
    func estSelectionneByAnotherUser() -> Bool {
        return self.selectionneByAnotherUser
    }
    
    /// Cette fonction sauve la position courante du noeud
    func savePosition() {
        self.savedPosition = self.position
    }
    
    /// Cette fonction applique (revert) la position sauvee au noeud
    func revertPosition() {
        self.deplacer(position: SCNVector3ToGLKVector3(self.savedPosition))
    }
    
    /// Cette fonction sauve le scaling du noeud
    func saveScale() {
        self.savedScale = self.scale
    }
    
    /// Cette fonction applique le scaling sauvee au noeud
    func revertScale() {
        self.scale = self.savedScale
    }
    
    /// Modifie la couleur par défaut
    func assignerDefaultColor(color: UIColor) {
        self.defaultColor = color
    }
    
    /// Modifie la couleur lorsqu'un noeud est sélectionnable
    func assignerSelectionnableColor(color: UIColor) {
        self.selectionnableColor = color
    }
    
    /// Cette fonction permet de changer la position relative en appliquant
    /// une transformation a la suite des autres changements
    func appliquerDeplacement(deplacement: GLKVector3) {
        let translation = GLKVector3Multiply(deplacement, self.axisLock)
        self.transform = SCNMatrix4FromGLKMatrix4(
                            GLKMatrix4Multiply(
                                GLKMatrix4MakeTranslation(translation.x, translation.y, translation.z),
                                SCNMatrix4ToGLKMatrix4(self.transform)))
    }
    
    /// Cette fonction permet de changer le rotation en appliquant
    /// une transformation à la suite des autres changements
    func appliquerRotation(angle: Float, axes: GLKVector3) {
        let rotation = SCNMatrix4MakeRotation(angle, axes.x, axes.y, axes.z)
        self.transform = SCNMatrix4FromGLKMatrix4(
                            GLKMatrix4Multiply(
                                SCNMatrix4ToGLKMatrix4(rotation),
                                SCNMatrix4ToGLKMatrix4(self.transform)))
    }
    
    /// Obtient la position relative du noeud.
    func obtenirPositionRelative() -> GLKVector3 {
        let vector = GLKMatrix4GetColumn(SCNMatrix4ToGLKMatrix4(self.transform), 3)
        return GLKVector3Make(vector[0], vector[1], vector[2])
    }
    
    /// Assigne la position relative du noeud.
    func assignerPositionRelative(positionRelative: GLKVector3) {
        var inverse = self.axisLock
        
        inverse.x = inverse.x == 0.0 ? 1.0 : 0.0
        inverse.y = inverse.y == 0.0 ? 1.0 : 0.0
        inverse.z = inverse.z == 0.0 ? 1.0 : 0.0
        
        let p = GLKVector3Add(GLKVector3Multiply(positionRelative, self.axisLock), GLKVector3Multiply(self.obtenirPositionRelative(), inverse))
        
        var translateMatrix = GLKMatrix4()
        translateMatrix.m00 = 1.0
        translateMatrix.m11 = 1.0
        translateMatrix.m22 = 1.0
        translateMatrix.m33 = 1.0
        let transform = GLKMatrix4Translate(translateMatrix, p[0], p[1], p[2])
        self.transform = SCNMatrix4FromGLKMatrix4(transform)
    }
    
    /// Cette fonction permet de changer la position relative en conservant
    /// le scaling et la rotation
    func deplacer(position: GLKVector3) {
        // Sauvegarder rotation et scaling
        self.savedRotation = self.rotation
        self.savedScale = self.scale
        
        self.assignerPositionRelative(positionRelative: position)
        
        // Réappliquer rotation et scaling
        self.rotation = self.savedRotation
        self.scale = self.savedScale
        
        //let glkRotation = SCNVector4ToGLKVector4(self.rotation)
        //let glkScale = GLKVector4MakeWithVector3(SCNVector3ToGLKVector3(self.scale), 0.0)
        //var glkTransform = SCNMatrix4ToGLKMatrix4(self.transform)
        //glkTransform = GLKMatrix4MultiplyVector4(glkTransform, GLKVector4Multiply(glkRotation, glkScale))
        
        //self.transform = SCNMatrix4FromGLKMatrix4(glkTransform)
        //transformationRelative_ *= rotation_ * scale_;
        
        //self.position = SCNVector3FromGLKVector3(position)
    }
    
    /// Permet de changer les axes de déplacement valide
    func assignerAxisLock(axisLock: GLKVector3) {
        self.axisLock = axisLock
    }
    
    /// Cette fonction applique une autre couleur sur le noeud
    func useOtherColor(activer: Bool, color: UIColor = UIColor(red: 1.0, green: 1.0, blue: 1.0, alpha: 1.0)) {
        let material = self.geometry?.firstMaterial
        
        if (activer) {
            material?.diffuse.contents = color
        } else {
            material?.diffuse.contents = self.defaultColor
        }
    }
    
    func appliquerMaterielSelection(activer: Bool) {
        self.colorerSelection(activer: activer)
        self.effetFantome(activer: activer)
    }
    
    /// Cette fonction active ou désactive un effet fantome sur le mur
    private func effetFantome(activer: Bool) {
        let material = self.geometry?.firstMaterial?.copy() as! SCNMaterial
        
        if (material.diffuse.contents is UIColor) {
            self.geometry?.firstMaterial = material
            self.geometry?.firstMaterial?.lightingModel = SCNMaterial.LightingModel.phong
            self.geometry?.firstMaterial?.transparency = activer ? 0.50 : 1.0
        }
    }
    
    /// Cette fonction colore les noeuds sélectionnables lors de la sélection
    private func colorerSelection(activer: Bool) {
        let material = self.geometry?.firstMaterial?.copy() as! SCNMaterial
        
        if material.diffuse.contents is UIColor && self.estSelectionnable() {
            self.geometry?.firstMaterial = material
            self.geometry?.firstMaterial?.locksAmbientWithDiffuse = true
            // TODO : pour color utiliser selectionneColor à la place de getUserColor()
            self.useOtherColor(activer: activer, color: FacadeModele.instance.getUserColor())
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
