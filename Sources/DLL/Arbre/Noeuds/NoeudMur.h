///////////////////////////////////////////////////////////////////////////
/// @file NoeudMur.h
/// @author Nam Lesage
/// @date 2016-09-18
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDMUR_H__
#define __ARBRE_NOEUDS_NOEUDMUR_H__


#include "NoeudAbstrait.h"
#include "GL/glew.h"


///////////////////////////////////////////////////////////////////////////
/// @class NoeudMur
/// @brief Classe pour afficher un muret
///
/// @author Nam Lesage
/// @date 2016-09-18
///////////////////////////////////////////////////////////////////////////
class NoeudMur : public NoeudAbstrait {
public:
	/// Constructeur à partir du type du noeud.
	NoeudMur(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudMur();

	/// Affiche l'accélarateur.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	/// Obtenir le collider
	utilitaire::BoiteEnglobante obtenirCollider();
	/// Calcule le collider du mur
	virtual void calculerCollider();

	// Patron visiter
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

private :

	utilitaire::BoiteEnglobante collider_;
};

#endif // __ARBRE_NOEUDS_NOEUDMUR_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
