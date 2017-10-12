///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerBoost.h
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATCREATIONMURET_H__
#define __APPLICATION_STATES_MODELEETATCREATIONMURET_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerBoost
/// @brief Classe concr�te du patron State pour l'�tat de cr�ation des acc�l�rateur
///
///        Cette classe repr�sente l'�tat de cr�ation des acc�l�rateur. Impl�mente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatCreerBoost : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatCreerBoost* obtenirInstance();
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	// Fonctions g�rant les entr�es de l'utilisateur
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();

private:

	/// Constructeur.
	ModeleEtatCreerBoost();
	/// Destructeur.
	virtual ~ModeleEtatCreerBoost();
	/// Constructeur copie d�sactiv�.
	ModeleEtatCreerBoost(const ModeleEtatCreerBoost&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatCreerBoost& operator =(const ModeleEtatCreerBoost&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatCreerBoost* instance_;
};


#endif // __APPLICATION_STATES_MODELEETATCREATIONMURET_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

