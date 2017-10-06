///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatDuplication.h
/// @author Clint Phoeuk
/// @date 2016-09-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATDUPLICATION_H__
#define __APPLICATION_STATES_MODELEETATDUPLICATION_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatDuplication
/// @brief Classe concr�te du patron State pour l'�tat de duplication
///
///        Cette classe repr�sente l'�tat de duplication. Impl�mente aussi le
///        patron Singleton.
///
/// @author Clint Phoeuk
/// @date 2016-09-28
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatDuplication : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatDuplication* obtenirInstance();
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	// Fonctions g�rant les entr�es de l'utilisateur
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque le bouton droit de la souris est lev�
	virtual void mouseUpR();
	/// �v�nement appel� lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	/// �v�nement appel� pour nettoyer l'�tat
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatDuplication();
	/// Destructeur.
	virtual ~ModeleEtatDuplication();
	/// Constructeur copie d�sactiv�.
	ModeleEtatDuplication(const ModeleEtatDuplication&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatDuplication& operator =(const ModeleEtatDuplication&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatDuplication* instance_;

	/// Bool�en pour savoir si on est en copie
	bool estCopie_;
};

#endif // __APPLICATION_STATES_MODELEETATDUPLICATION_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////