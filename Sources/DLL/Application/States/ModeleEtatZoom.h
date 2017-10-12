///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatZoom.h
/// @author Francis Dalpé
/// @date 2016-09-20
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATZOOM_H__
#define __APPLICATION_STATES_MODELEETATZOOM_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatZoom
/// @brief Classe concrète du patron State pour l'état de zoom.
///
///        Cette classe représente l'état de zoom. Implémente aussi le
///        patron Singleton.
///
/// @author Francis Dalpé
/// @date 2016-09-20
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatZoom : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatZoom* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	// Fonctions gérant les entrées de l'utilisateur
	/// Évènement appelé lorsque le bouton gauche de la souris est descendu
	virtual void mouseDownL();
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	/// Évènement appelé lorsque la touche escape est enfoncée
	virtual void escape();

private:
	
	/// Booléen pour savoir si le rectangle est activé
	bool rectangleActif_;

	/// Constructeur.
	ModeleEtatZoom();
	/// Destructeur.
	virtual ~ModeleEtatZoom();
	/// Constructeur copie désactivé.
	ModeleEtatZoom(const ModeleEtatZoom&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatZoom& operator =(const ModeleEtatZoom&) = delete;
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatZoom* instance_;
};


#endif // __APPLICATION_STATES_MODELEETATSELECTION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

