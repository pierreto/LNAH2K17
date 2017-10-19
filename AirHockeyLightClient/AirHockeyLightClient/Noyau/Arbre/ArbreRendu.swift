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
/// @brief Représente l'arbre de rendu qui contient les noeuds 
///        et qui permet leur instanciation
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-04
///////////////////////////////////////////////////////////////////////////
class ArbreRendu: SCNNode {

    // Singleton
    static let instance = ArbreRendu()
    
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
    /// La chaine representant le type des boosters.
    public let NOM_LIGNE_CENTRE = "ligne_centre"
    
    // Usines
    private var usines = [String: UsineAbstraite]()
    
    /// Constructeur
    override init() {
        super.init()
        
        /// Construction des usines (noModel correspond à aucun modèle Blender)
        self.ajouterUsine(type: self.NOM_TABLE, usine: UsineNoeud<NoeudTable>(nomUsine: self.NOM_TABLE, nomModele: "noModel"))
        self.ajouterUsine(type: self.NOM_BUT, usine: UsineNoeud<NoeudBut>(nomUsine: self.NOM_BUT, nomModele: "noModel"))
        self.ajouterUsine(type: self.NOM_POINT_CONTROL, usine: UsineNoeud<NoeudPointControl>(nomUsine: self.NOM_POINT_CONTROL, nomModele: "controlPoint"))
        self.ajouterUsine(type: self.NOM_PORTAIL, usine: UsineNoeud<NoeudPortail>(nomUsine: self.NOM_PORTAIL, nomModele: "portail"))
    }
    
    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    /// Ajoute une usine à noeud
    func ajouterUsine(type: String, usine: UsineAbstraite) {
        self.usines[type] = usine
    }
    
    /// Permet de créer un nouveau noeud, sans l'ajouter directement à l'arbre de rendu
    func creerNoeud(typeNouveauNoeud: String) -> SCNNode {
        assert(usines.keys.contains(typeNouveauNoeud), "Incapable de trouver l'usine")
        return (usines[typeNouveauNoeud]?.creerNoeud())!
    }
    
    /// Cette fonction crée la structure de base de l'arbre de rendu, c'est-à-dire
    /// avec les noeuds structurants (pour les objets, les murs, les billes,
    /// les parties statiques, etc.)
    func initialiser() {
        // On vide l'arbre
        self.vider()
        // On ajoute les noeud de base
        self.addChildNode(self.creerNoeud(typeNouveauNoeud: ArbreRendu.instance.NOM_TABLE))
    }
    
    /// Cette fonction vide le noeud de tous ses enfants.  Elle effectue une
    /// itération prudente sur les enfants afin d'être assez robuste pour
    /// supporter la possibilité qu'un enfant en efface un autre dans son
    /// destructeur, par exemple si deux objets ne peuvent pas exister l'un
    /// sans l'autre.  Elle peut toutefois entrer en boucle infinie si un
    /// enfant ajoute un nouveau noeud lorsqu'il se fait effacer.
    func vider() {
        // L'itération doit être faite ainsi pour éviter les problèmes lorsque
        // le desctructeur d'un noeud modifie l'arbre, par exemple en retirant
        // d'autres noeuds.  Il pourrait y avoir une boucle infinie si la
        // desctruction d'un enfant entraînerait l'ajout d'un autre.
        while (!self.childNodes.isEmpty) {
            let enfantAEffacer = self.childNodes.first
            enfantAEffacer?.removeFromParentNode()
        }
    }
    
    /// Cette fonction permet d'itérer à travers tous les noeuds enfants avec le visiteur
    func accepterVisiteur(visiteur: VisiteurAbstrait) {
        for child in self.childNodes {
            let noeud = child as! NoeudCommun
            noeud.accepterVisiteur(visiteur: visiteur)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
