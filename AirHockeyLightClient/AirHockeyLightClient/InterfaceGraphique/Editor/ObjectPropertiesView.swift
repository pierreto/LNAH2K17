///////////////////////////////////////////////////////////////////////////////
/// @file ObjectPropertiesView.swift
/// @author Pierre To
/// @date 2017-10-29
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class ObjectPropertiesView
/// @brief Vue de la configuration des propriétés d'un objet
///
/// @author Pierre To
/// @date 2017-10-29
///////////////////////////////////////////////////////////////////////////
class ObjectPropertiesView: UIView {
    
    /// Object Properties View
    @IBOutlet weak var editorViewController: EditorViewController!
    @IBOutlet weak var objectProperties: UIView!
    @IBOutlet weak var showButton: UIButton!
    @IBOutlet weak var hideButton: UIButton!
    @IBOutlet weak var resetButton: UIBarButtonItem!
    @IBOutlet weak var applyButton: UIBarButtonItem!
    @IBOutlet weak var positionX: UITextField!
    @IBOutlet weak var positionY: UITextField!
    @IBOutlet weak var positionZ: UITextField!
    @IBOutlet weak var rotationX: UITextField!
    @IBOutlet weak var rotationY: UITextField!
    @IBOutlet weak var rotationZ: UITextField!
    @IBOutlet weak var scaleX: UITextField!
    @IBOutlet weak var scaleY: UITextField!
    @IBOutlet weak var scaleZ: UITextField!
    
    /// Affiche la vue de configuration des propriétés de l'objet
    @IBAction func showObjectProperties(_ sender: Any) {
        self.showAnimate()
        self.showObjectPropertiesButtons()
    }
    
    /// Fermer la vue de configuration des propriétés de l'objet
    @IBAction func hideObjectProperties(_ sender: Any) {
        self.removeAnimate()
        self.hideObjectPropertiesButtons()
    }
    
    /// Animation à l'ouverture
    func showAnimate() {
        self.objectProperties.isHidden = false
        self.objectProperties.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
        self.objectProperties.alpha = 0.0
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.objectProperties.alpha = 1.0
                self.objectProperties.transform = CGAffineTransform(scaleX: 1.0, y: 1)
            }
        )
    }
    
    /// Animation à la fermeture
    func removeAnimate() {
        UIView.animate(
            withDuration: 0.25,
            animations: {
                self.objectProperties.transform = CGAffineTransform(scaleX: 1.3, y: 1.3)
                self.objectProperties.alpha = 0.0
            },
            completion: {
                (finished: Bool) in
                if finished {
                    self.objectProperties.isHidden = true
                }
            }
        )
    }
    
    /// Afficher les boutons de configuration de propriétés d'objet
    func showObjectPropertiesButtons() {
        self.showButton.isHidden = true
        self.hideButton.isHidden = false
        self.resetButton.isEnabled = true
        self.applyButton.isEnabled = true
    }
    
    /// Cacher les boutons de configuration de propriétés d'objet
    func hideObjectPropertiesButtons() {
        self.showButton.isHidden = false
        self.hideButton.isHidden = true
        self.resetButton.isEnabled = false
        self.applyButton.isEnabled = false
    }
    
    /// Permet le chargement des propriétés à afficher
    func loadObjectProperties() {
        var infos: [Float]? = [Float]()
        let success = FacadeModele.instance.selectedNodeInfos(infos: &infos)
        
        if success {
            self.positionX.text = String(format: "%.2f", (infos?[0])!)
            self.positionY.text = String(format: "%.2f", (infos?[1])!)
            self.positionZ.text = String(format: "%.2f", (infos?[2])!)
            
            self.rotationX.text = String(format: "%.2f", (infos?[3])!)
            self.rotationY.text = String(format: "%.2f", (infos?[4])!)
            self.rotationZ.text = String(format: "%.2f", (infos?[5])!)
            
            self.scaleX.text = String(format: "%.2f", (infos?[6])!)
            self.scaleY.text = String(format: "%.2f", (infos?[7])!)
            self.scaleZ.text = String(format: "%.2f", (infos?[8])!)
        }
        else {
            self.editorViewController.showObjectPropertiesView(activer: false)
        }
    }
    
    /// Appliquer les changements à la configuration des propriétés de l'objet
    @IBAction func applyObjectProperties(_ sender: Any) {
        self.validateObjectProperties()
        
        var infos = [Float]()
        
        infos.append(Float.init(self.positionX.text!)!)
        infos.append(Float.init(self.positionY.text!)!)
        infos.append(Float.init(self.positionZ.text!)!)
        
        infos.append(Float.init(self.rotationX.text!)!)
        infos.append(Float.init(self.rotationY.text!)!)
        infos.append(Float.init(self.rotationZ.text!)!)
        
        infos.append(Float.init(self.scaleX.text!)!)
        infos.append(Float.init(self.scaleY.text!)!)
        let scaleZ = MathHelper.clamp(value: Float.init(self.scaleZ.text!)!,
                                      lower: Float(1.0),
                                      upper: Float.greatestFiniteMagnitude)
        self.scaleZ.text = String(format: "%.2f", scaleZ)
        infos.append(scaleZ)
        
        FacadeModele.instance.applyNodeInfos(infos: infos)
    }
    
    /// Réinitialiser les changements à la configuration des propriétés de l'objet
    @IBAction func resetObjectProperties(_ sender: Any) {
        self.loadObjectProperties()
    }
    
    /// Vérifie si les valeurs sont correctes
    private func validateObjectProperties() {
        var infos: [Float]? = [Float]()
        _ = FacadeModele.instance.selectedNodeInfos(infos: &infos)
        
        self.positionX.text = (Float.init(self.positionX.text!) == nil) ?
            String(format: "%.2f", (infos?[0])!) : self.positionX.text
        self.positionY.text = (Float.init(self.positionY.text!) == nil) ?
            String(format: "%.2f", (infos?[1])!) : self.positionY.text
        self.positionZ.text = (Float.init(self.positionZ.text!) == nil) ?
            String(format: "%.2f", (infos?[2])!) : self.positionZ.text
        
        self.rotationX.text = (Float.init(self.rotationX.text!) == nil) ?
            String(format: "%.2f", (infos?[3])!) : self.rotationX.text
        self.rotationY.text = (Float.init(self.rotationY.text!) == nil) ?
            String(format: "%.2f", (infos?[4])!) : self.rotationY.text
        self.rotationZ.text = (Float.init(self.rotationZ.text!) == nil) ?
            String(format: "%.2f", (infos?[5])!) : self.rotationZ.text
        
        self.scaleX.text = (Float.init(self.scaleX.text!) == nil) ?
            String(format: "%.2f", (infos?[6])!) : self.scaleX.text
        self.scaleY.text = (Float.init(self.scaleY.text!) == nil) ?
            String(format: "%.2f", (infos?[7])!) : self.scaleY.text
        self.scaleZ.text = (Float.init(self.scaleZ.text!) == nil) ?
            String(format: "%.2f", (infos?[8])!) : self.scaleZ.text
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
