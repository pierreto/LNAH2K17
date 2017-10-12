///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerPortail.h
/// @author Nam Lesage
/// @date 2016-09-20
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATCREATIONPORTAIL_H__
#define __APPLICATION_STATES_MODELEETATCREATIONPORTAIL_H__

#include "ModeleEtat.h"

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerPortail
/// @brief Classe concrete du patron State pour l'�tat de cr�ation des portails
///
///        Cette classe repr�sente l'�tat de cr�ation d'un portail. 
///        Impl�mente aussi le patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-20
///////////////////////////////////////////////////////////////////////////

class ModeleEtatCreerPortail : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatCreerPortail* obtenirInstance();
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	// Fonctions g�rant les entr�es de l'utilisateur
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque la touche escape est enfonc�e
	virtual void escape();
	/// Fonction pour nettoyer l'�tat
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatCreerPortail();
	/// Destructeur.
	virtual ~ModeleEtatCreerPortail();
	/// Constructeur copie d�sactiv�.
	ModeleEtatCreerPortail(const ModeleEtatCreerPortail&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatCreerPortail& operator =(const ModeleEtatCreerPortail&) = delete;

	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatCreerPortail* instance_;

	/// Premier portail 
	NoeudPortail* premierNoeud_{nullptr};
};


#endif // __APPLICATION_STATES_MODELEETATCREATION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

