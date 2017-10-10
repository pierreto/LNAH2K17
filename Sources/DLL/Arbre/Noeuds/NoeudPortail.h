///////////////////////////////////////////////////////////////////////////
/// @file NoeudPortail.h
/// @author Nam Lesage
/// @date 2016-09-20
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDPORTAIL_H__
#define __ARBRE_NOEUDS_NOEUDPORTAIL_H__


#include "NoeudAbstrait.h"
#include "OpenGL_Nuanceur.h"
#include "OpenGL_Programme.h"
#include "GL/glew.h"


///////////////////////////////////////////////////////////////////////////
/// @class NoeudPortail
/// @brief Classe pour afficher un portail
///
/// @author Nam Lesage
/// @date 2016-09-20
///////////////////////////////////////////////////////////////////////////
class NoeudPortail : public NoeudAbstrait {
public:
	/// Constructeur à partir du type du noeud.
	NoeudPortail(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudPortail();

	/// Initialise les nuanceurs
	void NoeudPortail::initialiserNuanceurs();
	/// Affiche le cube.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	/// Anime le portail
	virtual void animer(float temps);

	/// Obtenir le collider
	utilitaire::CylindreEnglobant obtenirCollider() const;
	/// Calcule le collider du noeud
	virtual void calculerCollider();

	/// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

	// Modificateurs
	/// Associer le portail opposé
	void assignerOppose(NoeudPortail* portail);
	/// Activer/Desactiver le noeud
	void assignerDesactiver(bool desactiver);

	// Accesseurs
	/// Atteindre le portail opposé
	NoeudPortail* obtenirOppose();
	/// Savoir si le noeud est desactivé
	bool estDesactiver() const;
	
private:
	/// Angle selon l'axe des Y.
	float angleY_{ 0.f };
	/// Le portail oppose
	NoeudPortail* portailOppose_{nullptr};
	/// Le collider du portail
	utilitaire::CylindreEnglobant collider_;
	/// Retient si le portail est actif
	bool desactiver_;

	/// Programme opengl
	static opengl::Programme programme_;
	static opengl::Nuanceur nuanceurFragment_;
	static opengl::Nuanceur nuanceurSommet_;
};


#endif // __ARBRE_NOEUDS_NOEUDPORTAIL_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
