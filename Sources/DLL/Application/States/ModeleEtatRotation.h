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
/// @brief Classe concr�te du patron State pour l'�tat de rotation
///
///        Cette classe repr�sente l'�tat de rotation. Impl�mente aussi le
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
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();
	/// �v�nement appel� lorsque le bouton gauche de la souris est descendu
	virtual void mouseDownL();
	/// �v�nement appel� lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque la touche escape est enfonc�e
	virtual void escape();

private:

	/// Constructeur.
	ModeleEtatRotation();
	/// Destructeur.
	virtual ~ModeleEtatRotation();
	/// Constructeur copie d�sactiv�.
	ModeleEtatRotation(const ModeleEtatRotation&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatRotation& operator =(const ModeleEtatRotation&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatRotation* instance_;

	/// Centre de rotation
	glm::vec3 centreRotation_;

	/// Rotation totale
	float angle_;

	/// Noeuds s�lectionn�s
	std::vector<NoeudAbstrait*> noeuds_;
};


#endif // __APPLICATION_STATES_MODELEETATROTATION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

