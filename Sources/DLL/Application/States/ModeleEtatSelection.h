///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatSelection.h
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATSELECTION_H__
#define __APPLICATION_STATES_MODELEETATSELECTION_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatSelection
/// @brief Classe concrète du patron State pour l'état de sélection
///
///        Cette classe représente l'état de sélection. Implémente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatSelection : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatSelection* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	/// Fonction qui initialise l'état
	virtual void initialiser();
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// Évènement appelé lorsque la touche escape est enfoncée
	virtual void escape();

private:
	/// Booléen pour savoir si le rectangle est visible
	bool rectangleVisible_;
	/// Booléen pour savoir si le rectangle est initialisé
	bool rectangleInit_;
	/// Booléen pour savoir si l'action est annulée
	bool annuler_;

	/// Booléen pour savoir si les dimensions sont suffisantes
	bool dimensionsSuffisantes();

	/// Constructeur.
	ModeleEtatSelection();
	/// Destructeur.
	virtual ~ModeleEtatSelection();
	/// Constructeur copie désactivé.
	ModeleEtatSelection(const ModeleEtatSelection&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatSelection& operator =(const ModeleEtatSelection&) = delete;
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatSelection* instance_;

};


#endif // __APPLICATION_STATES_MODELEETATSELECTION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

