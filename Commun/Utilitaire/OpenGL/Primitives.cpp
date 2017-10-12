///////////////////////////////////////////////////////////////////////////////
/// @file Primitives.cpp
/// @author Nam Lesage
/// @date 2016-11-19
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "Primitives.h"
#include "GL/glew.h"
#include "Utilitaire.h"
#include <iostream>

namespace opengl {
	

	opengl::Programme Primitives::programme_;
	opengl::Nuanceur Primitives::nuanceurFragment_;
	opengl::Nuanceur Primitives::nuanceurSommet_;
	opengl::Nuanceur Primitives::nuanceurGeo_;

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Primitives::initialiserNuanceurs()
	///
	/// Initialise le programme de nuanceurs
	///
	/// @return Aucune 
	///
	////////////////////////////////////////////////////////////////////////
	void Primitives::initialiserNuanceurs()
	{
		if (!programme_.estInitialise())
		{
			nuanceurFragment_.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/Base/fragment.glsl");
			nuanceurSommet_.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/Base/sommet.glsl");
			nuanceurGeo_.initialiser(opengl::Nuanceur::Type::NUANCEUR_GEOMETRIE, "nuanceurs/Lighting/geometrieLambert.glsl");
			programme_.initialiser();
			programme_.attacherNuanceur(nuanceurFragment_);
			programme_.attacherNuanceur(nuanceurGeo_);
			programme_.attacherNuanceur(nuanceurSommet_);
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Primitives::Primitives()
	///
	/// Constructeur
	///
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	Primitives::Primitives(const glm::vec4& couleur)
	{
		initialiserNuanceurs();

		// Création des uniform buffers
		glGenBuffers(1, &ubo);

		// Matériau générique
		materiau_.ambiant_ = glm::vec3(0.1f, 0.1f, 0.1f);
		materiau_.diffuse_ = glm::vec3(couleur);
		materiau_.speculaire_ = glm::vec3(0.5f);
		materiau_.shininess_ = 20;
		materiau_.shininessStrength_ = 1;
		materiau_.opacite_ = couleur.a;
		materiau_.emission_ = glm::vec3(0.0f, 0.0f, 0.0f);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Primitives::~Primitives()
	///
	/// Ce destructeur désallouee l'affichage.
	///
	/// @return Aucune (destructeur).
	///
	////////////////////////////////////////////////////////////////////////
	Primitives::~Primitives()
	{
		glDeleteBuffers(1, &ubo);	
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Primitives::assignerCouleur(const glm::vec4& couleur)
	///
	/// Assigne une couleur de dessin
	///
	///	@param[in] couleur : Nouvelle couleur pour dessiner
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Primitives::assignerCouleur(const glm::vec4& couleur)
	{
		materiau_.diffuse_ = glm::vec3(couleur);
		materiau_.opacite_ = couleur.a;
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Primitives::updateBuffer(const std::vector<glm::vec3>& sommets)
	///
	/// Fait la mise à jour des données du vbo
	///
	///	@param[in] sommets : Les nouveaux sommets
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Primitives::updateBuffer(const std::vector<glm::vec3>& sommets)
	{
		glBindBuffer(GL_ARRAY_BUFFER, ubo);
		glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec3) * sommets.size(), &sommets[0], GL_STATIC_DRAW);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);
		glEnableVertexAttribArray(0);

		nombreDePoints_ = sommets.size();
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Primitives::appliquerMateriau() const
	///
	/// Envoi le matériau au programme de nuanceurs
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Primitives::appliquerMateriau() const
	{
		programme_.assignerUniforme("material.diffuse", materiau_.diffuse_);
		programme_.assignerUniforme("material.transparence", materiau_.opacite_);
		programme_.assignerUniforme("material.emission", materiau_.emission_);
		programme_.assignerUniforme("material.speculaire", materiau_.speculaire_);
		programme_.assignerUniforme("material.ambiant", materiau_.ambiant_);
		programme_.assignerUniforme("material.shininess", 0.0f);
		programme_.assignerUniforme("useDiffuseColor", 1);
		// TODO :  Enlever ce uniform quand la specularité va fonctionner
		programme_.assignerUniforme("disableSpeculaire", 1);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Primitives::dessiner(GLenum mode, const MatricesPipeline& matrices) const
	///
	/// Dessine la primitive
	///
	///	@param[in] mode : Le mode de dessin opengl
	///	@param[in] matrices : Matrices de transformation
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Primitives::dessiner(GLenum mode, const MatricesPipeline& matrices) const
	{
		opengl::Programme::Start(programme_);
		programme_.assignerUniforme("modelViewProjection", matrices.matrProj * matrices.matrVisu);
		programme_.assignerUniforme("matrModel", glm::mat4(1));
		programme_.assignerUniforme("positionCamera", matrices.positionCamera);

		appliquerMateriau();

		if (materiau_.opacite_ < 1)
		{
			glEnable(GL_BLEND);
			glDepthMask(false);
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		}

		glBindBuffer(GL_ARRAY_BUFFER, ubo);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);
		glEnableVertexAttribArray(0);

		glDrawArrays(mode, 0, nombreDePoints_);

		if (materiau_.opacite_ < 1)
		{
			glDisable(GL_BLEND);
			glDepthMask(true);
		}
		
		opengl::Programme::Stop(programme_);
	}

	
	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Cercle::Cercle(const glm::vec3 & centre, float rayon, unsigned int nbrPoints, const glm::vec4 & couleur)
	///
	/// Constructeur avec paramètres
	///
	///	@param[in] centre : Le centre du cercle
	///	@param[in] rayon : Rayon du cercle
	///	@param[in] nbrPoints : Le nombre de points autour du cercle
	///	@param[in] couleur : La couleur du cercle
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	Cercle::Cercle(const glm::vec3 & centre, float rayon, unsigned int nbrPoints, const glm::vec4 & couleur)
		: Primitives(couleur), centre_(centre), rayon_(rayon), nbrPoints_(nbrPoints)
	{
		genererSommets();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Cercle::~Cercle()
	///
	/// Destructeur
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	Cercle::~Cercle()
	{
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Cercle::afficher(const MatricesPipeline& matrices) const
	///
	/// Affiche le cercle
	///
	///	@param[in] matrices : Les matrices de transformation, visualisation et projection
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Cercle::afficher(const MatricesPipeline& matrices) const
	{
		dessiner(GL_TRIANGLE_FAN, matrices);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Cercle::afficher(const MatricesPipeline& matrices) const
	///
	/// Assigne un nouveau centre au cercle
	///
	///	@param[in] centre : Nouveau centre du cercle
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Cercle::assignerCentre(const glm::vec3 & centre)
	{
		centre_ = centre;
		genererSommets();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Cercle::genererSommets()
	///
	/// Génère les sommets du cercle
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Cercle::genererSommets()
	{
		std::vector<glm::vec3> sommets;

		double angle = 2.0f * utilitaire::PI / nbrPoints_;
		sommets.push_back(centre_);
		for (unsigned int i = 0; i < nbrPoints_; ++i)
		{
			glm::dvec3 point{ rayon_ * glm::sin(angle * i), 0, rayon_ * glm::cos(angle * i) };
			point += centre_;
			sommets.push_back(glm::vec3(point));
		}
		sommets.push_back(glm::vec3(centre_.x, centre_.y, centre_.z + rayon_));

		updateBuffer(sommets);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Rectangle::Rectangle(const glm::vec3 & position, float largeur, float hauteur, const glm::vec4 & couleur)
	///
	/// Constructeur avec paramètres
	///
	///	@param[in] position : Position du centre du rectangle
	///	@param[in] largeur : Largeur du rectangle par rapport à l'axe des z
	///	@param[in] hauteur : Hauteur du rectangle par rapport à l'axe des x
	///	@param[in] couleur : La couleur du rectangle
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	Rectangle::Rectangle(const glm::vec3 & position, float largeur, float hauteur, const glm::vec4 & couleur)
		: Primitives(couleur), position_(position), largeur_(largeur), hauteur_(hauteur)
	{
		genererSommets();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Rectangle::~Rectangle()
	///
	/// Destructeur
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	Rectangle::~Rectangle()
	{
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Rectangle::afficher(const MatricesPipeline& matrices) const
	///
	/// Affiche le rectangle
	///
	///	@param[in] matrices : Les matrices de transformation, visualisation et projection
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Rectangle::afficher(const MatricesPipeline& matrices) const
	{
		dessiner(GL_TRIANGLE_STRIP, matrices);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Rectangle::genererSommets()
	///
	/// Génère les sommets du rectangle
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void Rectangle::genererSommets()
	{
		std::vector<glm::vec3> sommets;
		sommets.push_back(glm::vec3(position_.x + hauteur_ / 2, 0.1f, position_.z + largeur_ / 2.0f));
		sommets.push_back(glm::vec3(position_.x + hauteur_ / 2, 0.1f, position_.z - largeur_ / 2.0f));
		sommets.push_back(glm::vec3(position_.x - hauteur_ / 2, 0.1f, position_.z + largeur_ / 2.0f));
		sommets.push_back(glm::vec3(position_.x - hauteur_ / 2, 0.1f, position_.z - largeur_ / 2.0f));
		
		updateBuffer(sommets);
	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
