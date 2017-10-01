///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatScale.h
/// @author Nam Lesage
/// @date 2016-09-27
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATSCALE_H__
#define __APPLICATION_STATES_MODELEETATSCALE_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatScale
/// @brief Classe concr�te du patron State pour l'�tat de scaling
///
///        Cette classe repr�sente l'�tat de scaling. Impl�mente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-28
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatScale : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatScale* obtenirInstance();
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
	ModeleEtatScale();
	/// Destructeur.
	virtual ~ModeleEtatScale();
	/// Constructeur copie d�sactiv�.
	ModeleEtatScale(const ModeleEtatScale&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatScale& operator =(const ModeleEtatScale&) = delete;

	// Fonction pour sauver et revert le scaling
	/// Fonction qui sauve la valeur du scale des noeuds
	void saveScale();
	/// Fcontion qui r�initialise la valeur du scale des noeuds avec la valeur sauv�e
	void revertScale();

	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatScale* instance_;
};


#endif // __APPLICATION_STATES_MODELEETATSCALE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

