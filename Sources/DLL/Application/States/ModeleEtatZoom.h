///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatZoom.h
/// @author Francis Dalp�
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
/// @brief Classe concr�te du patron State pour l'�tat de zoom.
///
///        Cette classe repr�sente l'�tat de zoom. Impl�mente aussi le
///        patron Singleton.
///
/// @author Francis Dalp�
/// @date 2016-09-20
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatZoom : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatZoom* obtenirInstance();
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	// Fonctions g�rant les entr�es de l'utilisateur
	/// �v�nement appel� lorsque le bouton gauche de la souris est descendu
	virtual void mouseDownL();
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	/// �v�nement appel� lorsque la touche escape est enfonc�e
	virtual void escape();

private:
	
	/// Bool�en pour savoir si le rectangle est activ�
	bool rectangleActif_;

	/// Constructeur.
	ModeleEtatZoom();
	/// Destructeur.
	virtual ~ModeleEtatZoom();
	/// Constructeur copie d�sactiv�.
	ModeleEtatZoom(const ModeleEtatZoom&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatZoom& operator =(const ModeleEtatZoom&) = delete;
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatZoom* instance_;
};


#endif // __APPLICATION_STATES_MODELEETATSELECTION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

