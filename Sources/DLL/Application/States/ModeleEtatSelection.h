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
/// @brief Classe concr�te du patron State pour l'�tat de s�lection
///
///        Cette classe repr�sente l'�tat de s�lection. Impl�mente aussi le
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
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	/// Fonction qui initialise l'�tat
	virtual void initialiser();
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// �v�nement appel� lorsque la touche escape est enfonc�e
	virtual void escape();

private:
	/// Bool�en pour savoir si le rectangle est visible
	bool rectangleVisible_;
	/// Bool�en pour savoir si le rectangle est initialis�
	bool rectangleInit_;
	/// Bool�en pour savoir si l'action est annul�e
	bool annuler_;

	/// Bool�en pour savoir si les dimensions sont suffisantes
	bool dimensionsSuffisantes();

	/// Constructeur.
	ModeleEtatSelection();
	/// Destructeur.
	virtual ~ModeleEtatSelection();
	/// Constructeur copie d�sactiv�.
	ModeleEtatSelection(const ModeleEtatSelection&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatSelection& operator =(const ModeleEtatSelection&) = delete;
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatSelection* instance_;

};


#endif // __APPLICATION_STATES_MODELEETATSELECTION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

