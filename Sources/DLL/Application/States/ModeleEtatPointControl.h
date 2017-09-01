///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatPointControl.h
/// @author Nam Lesage
/// @date 2016-09-29
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATPOINTCONROL_H__
#define __APPLICATION_STATES_MODELEETATPOINTCONROL_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatPointControl
/// @brief Classe concr�te du patron State pour l'�tat de gestion des points de controle
///
///        Cette classe repr�sente l'�tat de gestion des points de controle. Impl�mente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatPointControl : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatPointControl* obtenirInstance();
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();
	/// Fonction qui initialise l'�tat
	virtual void initialiser();
	/// �v�nement appel� lorsque le bouton gauche de la souris est descendu
	virtual void mouseDownL();
	/// �v�nement appel� lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque la touche escape est enfonc�e
	virtual void escape();
	/// �v�nement appel� pour nettoyer l'�tat
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatPointControl();
	/// Destructeur.
	virtual ~ModeleEtatPointControl();
	/// Constructeur copie d�sactiv�.
	ModeleEtatPointControl(const ModeleEtatPointControl&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatPointControl& operator =(const ModeleEtatPointControl&) = delete;
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatPointControl* instance_;

	// Fonction pour sauver et revert le scaling
	/// Fonction qui sauve  la position des points de controle
	void savePosition();
	/// Fonction qui r�initialise la postion des points avec la position sauv�e
	void revertPosition();
};


#endif // __APPLICATION_STATES_MODELEETATPOINTCONROL_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

