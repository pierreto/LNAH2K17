///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatPointControl.h
/// @author Nam Lesage
/// @date 2016-09-29
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATPOINTCONROL_H__
#define __APPLICATION_STATES_MODELEETATPOINTCONROL_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatPointControl
/// @brief Classe concrète du patron State pour l'état de gestion des points de controle
///
///        Cette classe représente l'état de gestion des points de controle. Implémente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatPointControl : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatPointControl* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();
	/// Fonction qui initialise l'état
	virtual void initialiser();
	/// Évènement appelé lorsque le bouton gauche de la souris est descendu
	virtual void mouseDownL();
	/// Évènement appelé lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque la touche escape est enfoncée
	virtual void escape();
	/// Évènement appelé pour nettoyer l'état
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatPointControl();
	/// Destructeur.
	virtual ~ModeleEtatPointControl();
	/// Constructeur copie désactivé.
	ModeleEtatPointControl(const ModeleEtatPointControl&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatPointControl& operator =(const ModeleEtatPointControl&) = delete;
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatPointControl* instance_;

	// Fonction pour sauver et revert le scaling
	/// Fonction qui sauve  la position des points de controle
	void savePosition();
	/// Fonction qui réinitialise la postion des points avec la position sauvée
	void revertPosition();
};


#endif // __APPLICATION_STATES_MODELEETATPOINTCONROL_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

