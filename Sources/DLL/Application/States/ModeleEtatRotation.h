///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatRotation.h
/// @author Nam Lesage
/// @date 2016-09-27
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATROTATION_H__
#define __APPLICATION_STATES_MODELEETATROTATION_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatRotation
/// @brief Classe concrète du patron State pour l'état de rotation
///
///        Cette classe représente l'état de rotation. Implémente aussi le
///        patron Singleton.
///
/// @author Julien Charbonneau
/// @date 2016-09-23
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatRotation : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatRotation* obtenirInstance();
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
	ModeleEtatRotation();
	/// Destructeur.
	virtual ~ModeleEtatRotation();
	/// Constructeur copie désactivé.
	ModeleEtatRotation(const ModeleEtatRotation&) = delete;
	/// Opérateur d'assignation désactivé.
	ModeleEtatRotation& operator =(const ModeleEtatRotation&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatRotation* instance_;

	/// Centre de rotation
	glm::vec3 centreRotation_;

	/// Rotation totale
	float angle_;

	/// Noeuds sélectionnés
	std::vector<NoeudAbstrait*> noeuds_;
};


#endif // __APPLICATION_STATES_MODELEETATROTATION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

