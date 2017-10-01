///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerMuret.h
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATCREERMURET_H__
#define __APPLICATION_STATES_MODELEETATCREERMURET_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatCreerMuret
/// @brief Classe concr�te du patron State pour l'�tat de cr�ation des murets
///
///        Cette classe repr�sente l'�tat de cr�ation des murets. Impl�mente aussi le
///        patron Singleton.
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"

class ModeleEtatCreerMuret : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatCreerMuret* obtenirInstance();
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	// Fonctions g�rant les entr�es de l'utilisateur
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	/// �v�nement appel� lorsque la touche escape est enfonc�e
	virtual void escape();
	/// Fonction pour nettoyer l'�tat
	virtual void nettoyerEtat();

private:

	/// Constructeur.
	ModeleEtatCreerMuret();
	/// Destructeur.
	virtual ~ModeleEtatCreerMuret();
	/// Constructeur copie d�sactiv�.
	ModeleEtatCreerMuret(const ModeleEtatCreerMuret&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	ModeleEtatCreerMuret& operator =(const ModeleEtatCreerMuret&) = delete;


	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatCreerMuret* instance_;

	/// D�termine si l'action est d�but�e
	bool actionCommencee_;
	/// Noeud en cr�ation
	NoeudAbstrait* noeud_{nullptr};

	/// Point initial pour la cr�ation du muret
	glm::vec3 pointInitial_;
};


#endif // __APPLICATION_STATES_MODELEETATCREERMURET_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

