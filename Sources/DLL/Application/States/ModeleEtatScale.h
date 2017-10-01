///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatScale.h
/// @author Nam Lesage
/// @date 2016-09-27
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATSCALE_H__
#define __APPLICATION_STATES_MODELEETATSCALE_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatScale
/// @brief Classe concrète du patron State pour l'état de scaling
///
///        Cette classe représente l'état de scaling. Implémente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-28
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatScale : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatScale* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();
	/// Évènement appelé lorsque le bouton gauche de la souris est descendu
	virtual void mouseDownL();
	/// Évènement appelé lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque la touche escape est enfoncée
	virtual void escape();

private:

	/// Constructeur.
	ModeleEtatScale();
	/// Destructeur.
	virtual ~ModeleEtatScale();
	/// Constructeur copie désactivé.
	ModeleEtatScale(const ModeleEtatScale&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatScale& operator =(const ModeleEtatScale&) = delete;

	// Fonction pour sauver et revert le scaling
	/// Fonction qui sauve la valeur du scale des noeuds
	void saveScale();
	/// Fcontion qui réinitialise la valeur du scale des noeuds avec la valeur sauvée
	void revertScale();

	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatScale* instance_;
};


#endif // __APPLICATION_STATES_MODELEETATSCALE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

