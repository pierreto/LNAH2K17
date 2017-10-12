///////////////////////////////////////////////////////////////////////////
/// @file NoeudAccelerateur.h
/// @author Nam Lesage
/// @date 2016-09-08
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDACCELERATEUR_H__
#define __ARBRE_NOEUDS_NOEUDACCELERATEUR_H__

#include "NoeudAbstrait.h"
#include "GL/glew.h"

///////////////////////////////////////////////////////////////////////////
/// @class NoeudAccelerateur
/// @brief Classe pour afficher un acc�l�rateur
///
/// @author Nam Lesage
/// @date 2016-09-08
///////////////////////////////////////////////////////////////////////////
class NoeudAccelerateur : public NoeudAbstrait {
public:
	/// Constructeur � partir du type du noeud.
	NoeudAccelerateur(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudAccelerateur();

	/// Affiche l'acc�larateur.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	/// Anime l'acc�l�rateur
	virtual void animer(float temps);

	/// Calcul le collider de l'acc�l�rateur
	virtual void calculerCollider();
	/// Obtenir le collider
	utilitaire::CylindreEnglobant obtenirCollider();

	// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

	/// Indique si l'acc�l�rateur est d�sactiv�
	bool estDesactiver() const;
	/// Activer/Desactiver le noeud
	void assignerDesactiver(bool desactiver);

private: 

	/// Angle selon l'axe des Y.
	float angleY_{ 0.f };
	/// Collider de l'acc�l�rateur
	utilitaire::CylindreEnglobant collider_;
	/// Retient si le portail est actif
	bool desactiver_;

};

#endif // __ARBRE_NOEUDS_NOEUDACCELERATEUR_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
