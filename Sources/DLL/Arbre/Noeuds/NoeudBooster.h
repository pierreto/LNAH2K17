///////////////////////////////////////////////////////////////////////////
/// @file NoeudBooster.h
/// @author Nam Lesage
/// @date 2016-11-24
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDBOOSTER_H__
#define __ARBRE_NOEUDS_NOEUDBOOSTER_H__

#include "NoeudAbstrait.h"
#include "GL/glew.h"
#include "FireEffect.h"

///////////////////////////////////////////////////////////////////////////
/// @class NoeudAccelerateur
/// @brief Classe pour afficher un accélérateur
///
/// @author Nam Lesage
/// @date 2016-09-08
///////////////////////////////////////////////////////////////////////////
class NoeudBooster : public NoeudAbstrait {
public:
	/// Constructeur à partir du type du noeud.
	NoeudBooster(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudBooster();

	/// Affiche l'accélarateur.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	/// Anime l'accélérateur
	virtual void animer(float temps);

	// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

private :
	FireEffect* particleGenerator_;
};

#endif // __ARBRE_NOEUDS_NOEUDACCELERATEUR_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
