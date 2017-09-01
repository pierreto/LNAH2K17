///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerPortail.h
/// @author Nam Lesage
/// @date 2016-09-20
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATCREATIONPORTAIL_H__
#define __APPLICATION_STATES_MODELEETATCREATIONPORTAIL_H__

#include "ModeleEtat.h"

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerPortail
/// @brief Classe concrete du patron State pour l'état de création des portails
///
///        Cette classe représente l'état de création d'un portail. 
///        Implémente aussi le patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-20
///////////////////////////////////////////////////////////////////////////

class ModeleEtatCreerPortail : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatCreerPortail* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	// Fonctions gérant les entrées de l'utilisateur
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque la touche escape est enfoncée
	virtual void escape();
	/// Fonction pour nettoyer l'état
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatCreerPortail();
	/// Destructeur.
	virtual ~ModeleEtatCreerPortail();
	/// Constructeur copie désactivé.
	ModeleEtatCreerPortail(const ModeleEtatCreerPortail&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatCreerPortail& operator =(const ModeleEtatCreerPortail&) = delete;

	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatCreerPortail* instance_;

	/// Premier portail 
	NoeudPortail* premierNoeud_{nullptr};
};


#endif // __APPLICATION_STATES_MODELEETATCREATION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

