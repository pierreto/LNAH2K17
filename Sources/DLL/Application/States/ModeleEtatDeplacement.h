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
/// @brief Classe concr�te du patron State pour l'�tat de d�placement
///
///        Cette classe repr�sente l'�tat de d�placement. Impl�mente aussi le
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
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	/// Fonction qui initialise l'�tat
	virtual void initialiser();
	/// �v�nement appel� lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque la touche escape est enfonc�e
	virtual void escape();

private:

	/// Constructeur.
	ModeleEtatDeplacement();
	/// Destructeur.
	virtual ~ModeleEtatDeplacement();
	/// Constructeur copie d�sactiv�.
	ModeleEtatDeplacement(const ModeleEtatDeplacement&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatDeplacement& operator =(const ModeleEtatDeplacement&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatDeplacement* instance_;

	/// D�placement total appliqu�
	glm::vec3 deplacementTotal_;
};


#endif // __APPLICATION_STATES_MODELEETATDEPLACEMENT_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

