///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatDeplacement.h
/// @author Julien Charbonneau
/// @date 2016-09-23
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATDEPLACEMENT_H__
#define __APPLICATION_STATES_MODELEETATDEPLACEMENT_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatDeplacement
/// @brief Classe concrète du patron State pour l'état de déplacement
///
///        Cette classe représente l'état de déplacement. Implémente aussi le
///        patron Singleton.
///
/// @author Julien Charbonneau
/// @date 2016-09-23
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatDeplacement : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatDeplacement* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	/// Fonction qui initialise l'état
	virtual void initialiser();
	/// Évènement appelé lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	/// Évènement appelé lorsque le bouton gauche de la souris est levé
	virtual void mouseUpL();
	/// Évènement appelé lorsque la touche escape est enfoncée
	virtual void escape();

private:

	/// Constructeur.
	ModeleEtatDeplacement();
	/// Destructeur.
	virtual ~ModeleEtatDeplacement();
	/// Constructeur copie désactivé.
	ModeleEtatDeplacement(const ModeleEtatDeplacement&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatDeplacement& operator =(const ModeleEtatDeplacement&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatDeplacement* instance_;

	/// Déplacement total appliqué
	glm::vec3 deplacementTotal_;
};


#endif // __APPLICATION_STATES_MODELEETATDEPLACEMENT_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

