///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerMuret.swift
/// @author Mikael Ferland
/// @date 2017-10-18
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import GLKit

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerMuret
/// @brief Classe concrète du patron State pour l'état de création des murets
///        
///        Cette classe représente l'état de création des murets. Implémente aussi le
///        patron Singleton.
///
/// @author Mikael Ferland
/// @date 2017-10-18
///////////////////////////////////////////////////////////////////////////
class ModeleEtatCreerMuret: ModeleEtat {
    
    /// Instance singleton
    static let instance = ModeleEtatCreerMuret()
    
    /// Noeud en création
    private var noeud : NoeudCommun?
    
    /// Point initial pour la création du muret
    private var pointInitial = GLKVector3()
    
    /// Détermine si l'action est débutée
    private var actionCommencee : boolean?
    
    /// Fonction qui initialise l'état de déplacement
    override func initialiser() {
        super.initialiser();
        
        // TODO : déselectionner tous les noeuds
        // super.obtenirArbreRendu().deselectionnerTout();
        
        self.actionCommencee = false;
    }
    
    // Fonctions gérant les entrées de l'utilisateur
    
    override func tapGesture(point: CGPoint) {
        super.tapGesture(point: point)
        
        self.noeud = FacadeModel.instance.obtenirArbreRendu().creerNoeud(typeNouveauNoeud: arbre.NOM_MUR) as! NoeudMur
    }
    
    /// Évènement appelé lorsque le bouton gauche de la souris est descendu
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
