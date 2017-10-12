///////////////////////////////////////////////////////////////////////////
/// @file NoeudRondelle.h
/// @author Nam Lesage
/// @date 2016-10-13
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDRONDELLE_H__
#define __ARBRE_NOEUDS_NOEUDRONDELLE_H__

#include "NoeudAbstrait.h"
#include "GL/glew.h"

///////////////////////////////////////////////////////////////////////////
/// @class NoeudRondelle
/// @brief Classe pour afficher un rondelle
///
/// @author Nam Lesage
/// @date 2016-10-13
///////////////////////////////////////////////////////////////////////////
class NoeudRondelle : public NoeudAbstrait {
public:
	/// Constructeur à partir du type du noeud.
	NoeudRondelle(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudRondelle();

	/// Affiche l'accélarateur.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	/// Anime l'accélérateur
	virtual void animer(float temps);

	/// Obtenir le collider
	utilitaire::CylindreEnglobant obtenirCollider();
	/// Calcule le collider de la rondelle
	virtual void calculerCollider();

	/// Obtenir le rigidbody
	RigidBody* obtenirRigidBody();

	// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

private :
	/// Collider de la rondelle
	utilitaire::CylindreEnglobant collider_;
	/// Physique de l'objet
	RigidBody rigidbody_;
};

#endif // __ARBRE_NOEUDS_NOEUDACCELERATEUR_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
