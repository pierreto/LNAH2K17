///////////////////////////////////////////////////////////////////////////
/// @file NoeudTable.h
/// @author Nam Lesage
/// @date 2016-09-09
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDTABLE_H__
#define __ARBRE_NOEUDS_NOEUDTABLE_H__

#include "NoeudComposite.h"
#include "GL/glew.h"

#include "OpenGL_Nuanceur.h"
#include "OpenGL_Programme.h"

#include "NoeudPointControl.h"
#include "NoeudBut.h"
#include "Table.h"
#include "Materiau.h"
#include "FireEffect.h"

// Grandeur de table maximale
#define MAX_TABLE_LENGTH 300

///////////////////////////////////////////////////////////////////////////
/// @class NoeudTable
/// @brief Classe pour afficher une table
///
/// @author Nam Lesage
/// @date 2016-09-08
///////////////////////////////////////////////////////////////////////////
class NoeudTable : public NoeudComposite {
public:
	/// Constructeur à partir du type du noeud.
	NoeudTable(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudTable();

	/// Affiche la table.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	/// Desactiver la suppression des points de controle
	virtual void effacerSelection() const;

	/// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

	/// Appliquer un matériau
	virtual void appliquerMateriau() const;

	// Accesseurs
	/// Obtient tous les sommets de la table
	const std::vector<glm::vec3> obtenirSommets() const;
	/// Obtient les sommets composant la patinoire
	const std::vector<glm::vec3> obtenirSommetsPatinoire() const;
	/// Obtenir les buts
	const std::vector<NoeudBut*> obtenirButs() const;
	/// Change la visibilité de la grid
	inline void changeGridVisibility(bool visibility) { showGrid_ = visibility; };

private:

	// Fonctions privees
	void initialiserTable();
	void generateTexture();
	void initialiserNuanceurs();
	void creerPointsDeControle();
	void creerButs(NoeudPointControl* pointGauche, NoeudPointControl* pointDroite);

	/// Dessine la table et ses composantes
	void dessiner(const glm::mat4& matrice) const;

	// Attributs
	GLuint vao_;
	GLuint tampon_sommets_;
	GLuint tampon_faces_;
	GLuint tampon_coords;
	GLuint texture_;
	table::Sommets sommets_;
	std::vector<glm::vec2> texCoords_;
	std::vector<GLuint> faces_;
	std::vector<NoeudBut*> buts_;
	modele::Materiau materiau;
	bool showGrid_;

	// Shaders
	static opengl::Programme programme_;
	static opengl::Nuanceur nuanceurFragment_;
	static opengl::Nuanceur nuanceurSommet_;
	static opengl::Nuanceur nuanceurGeo_;
};


#endif // __ARBRE_NOEUDS_NOEUDTABLE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
