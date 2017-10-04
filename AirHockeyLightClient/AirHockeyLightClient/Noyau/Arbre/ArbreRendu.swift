///////////////////////////////////////////////////////////////////////////////
/// @file ArbreRendu.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SceneKit

///////////////////////////////////////////////////////////////////////////
/// @class ArbreRendu
/// @brief Représente l'arbre de rendu qui contient les noeuds et qui permet leur instanciation
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
///////////////////////////////////////////////////////////////////////////
class ArbreRendu: SCNScene {

    // Singleton
    static let arbreRendu = ArbreRendu()
    
    /// La chaine representant le type des maillets.
    public let NOM_MAILLET = "maillet"
    /// La chaine representant le type de la table
    public let NOM_TABLE = "table"
    /// La chaine representant le type des points de control.
    public let NOM_POINT_CONTROL = "point_control"
    /// La chaine representant le type des accelerateurs.
    public let NOM_ACCELERATEUR = "accelerateur"
    /// La chaine representant le type des murs.
    public let NOM_MUR = "mur"
    /// La chaine representant le type des portails.
    public let NOM_PORTAIL = "portail"
    /// La chaine representant le type des buts.
    public let NOM_BUT = "but"
    /// La chaine representant le type de la rondelle.
    public let NOM_RONDELLE = "rondelle"
    /// La chaine representant le type des boosters.
    public let NOM_BOOSTER = "booster"
    
    // Usines
    private var usines = [String: AnyObject]()
    
    /// Constructeur
    override init() {
        super.init()
        
        /// Construction des usines
        //self.ajouterUsine(type: self.NOM_POINT_CONTROL, usine: UsineNoeud<NoeudPointControl>(self.NOM_POINT_CONTROL, "controlPoint.dae"))
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Ajoute une usine à noeud
    func ajouterUsine(type: String, usine: AnyObject) {
        self.usines[type] = usine
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
