///////////////////////////////////////////////////////////////////////////
/// @file NoeudBut.h
/// @author Nam Lesage
/// @date 2016-10-2
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDBUT_H__
#define __ARBRE_NOEUDS_NOEUDBUT_H__

#include "NoeudAbstrait.h"
#include "GL/glew.h"

#include "OpenGL_Nuanceur.h"
#include "OpenGL_Programme.h"
#include "Materiau.h"

#define GOAL_WIDTH 40.0f

class NoeudPointControl;

///////////////////////////////////////////////////////////////////////////
/// @class NoeudBut
/// @brief Classe pour afficher un but
///
/// @author Nam Lesage
/// @date 2016-09-08
///////////////////////////////////////////////////////////////////////////
class NoeudBut : public NoeudAbstrait {
public:
	/// Constructeur à partir du type du noeud.
	NoeudBut(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudBut();

	/// Affiche l'accélarateur.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	virtual void appliquerMateriau() const;

	// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

	// Modificateurs
	/// Assigne un point de contrôle au but
	void assignerPointControl(NoeudPointControl* pointControl);
	/// Ajuste les sommets du but
	void ajusterPoints();
	/// Obtient le collider du but
	std::vector<glm::vec3> obtenirCollider();

private:

	// Fonctions privees
	/// Initialise le openGl des buts
	void initialiserBut();
	/// Initialise le programme des nuanceurs
	void initialiserNuanceurs();

	/// Dessine le but
	void dessiner(const glm::mat4& matrice) const;

	// Attributs
	/// Vertex array object du but
	GLuint vao_;
	/// Buffer de sommets
	GLuint tampon_sommets_;
	/// Buffer de connectivité
	GLuint tampon_faces_;

	modele::Materiau materiau;

	/// Les sommets du but
	std::vector<glm::vec3> sommets_;
	/// La connectivité du but
	std::vector<GLuint> faces_;

	/// Pointeur vers le point de controle
	NoeudPointControl* pointControl_{ nullptr };
	/// Collider du but
	std::vector<glm::vec3> collider_;
	
	// Shaders
	static opengl::Programme programme_;
	static opengl::Nuanceur nuanceurFragment_;
	static opengl::Nuanceur nuanceurSommet_;
	static opengl::Nuanceur nuanceurGeo_;

};

#endif // __ARBRE_NOEUDS_NOEUDACCELERATEUR_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
