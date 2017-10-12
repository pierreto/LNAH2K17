///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatDuplication.h
/// @author Clint Phoeuk
/// @date 2016-09-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATDUPLICATION_H__
#define __APPLICATION_STATES_MODELEETATDUPLICATION_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatDuplication
/// @brief Classe concrète du patron State pour l'état de duplication
///
///        Cette classe représente l'état de duplication. Implémente aussi le
///        patron Singleton.
///
/// @author Clint Phoeuk
/// @date 2016-09-28
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatDuplication : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatDuplication* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	// Fonctions gérant les entrées de l'utilisateur
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque le bouton droit de la souris est levé
	virtual void mouseUpR();
	/// Évènement appelé lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	/// Évènement appelé pour nettoyer l'état
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatDuplication();
	/// Destructeur.
	virtual ~ModeleEtatDuplication();
	/// Constructeur copie désactivé.
	ModeleEtatDuplication(const ModeleEtatDuplication&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatDuplication& operator =(const ModeleEtatDuplication&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatDuplication* instance_;

	/// Booléen pour savoir si on est en copie
	bool estCopie_;
};

#endif // __APPLICATION_STATES_MODELEETATDUPLICATION_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////