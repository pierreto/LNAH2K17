///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerMuret.h
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATCREERMURET_H__
#define __APPLICATION_STATES_MODELEETATCREERMURET_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerMuret
/// @brief Classe concrète du patron State pour l'état de création des murets
///
///        Cette classe représente l'état de création des murets. Implémente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatCreerMuret : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatCreerMuret* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	// Fonctions gérant les entrées de l'utilisateur
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// Évènement appelé lorsque la touche escape est enfoncée
	virtual void escape();
	/// Fonction pour nettoyer l'état
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatCreerMuret();
	/// Destructeur.
	virtual ~ModeleEtatCreerMuret();
	/// Constructeur copie désactivé.
	ModeleEtatCreerMuret(const ModeleEtatCreerMuret&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatCreerMuret& operator =(const ModeleEtatCreerMuret&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatCreerMuret* instance_;

	/// Détermine si l'action est débutée
	bool actionCommencee_;
	/// Noeud en création
	NoeudAbstrait* noeud_{nullptr};

	/// Point initial pour la création du muret
	glm::vec3 pointInitial_;
};


#endif // __APPLICATION_STATES_MODELEETATCREERMURET_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

