///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerBoost.h
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATCREATIONMURET_H__
#define __APPLICATION_STATES_MODELEETATCREATIONMURET_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerBoost
/// @brief Classe concrète du patron State pour l'état de création des accélérateur
///
///        Cette classe représente l'état de création des accélérateur. Implémente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatCreerBoost : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatCreerBoost* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	// Fonctions gérant les entrées de l'utilisateur
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();

private:

	/// Constructeur.
	ModeleEtatCreerBoost();
	/// Destructeur.
	virtual ~ModeleEtatCreerBoost();
	/// Constructeur copie désactivé.
	ModeleEtatCreerBoost(const ModeleEtatCreerBoost&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatCreerBoost& operator =(const ModeleEtatCreerBoost&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatCreerBoost* instance_;
};


#endif // __APPLICATION_STATES_MODELEETATCREATIONMURET_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

