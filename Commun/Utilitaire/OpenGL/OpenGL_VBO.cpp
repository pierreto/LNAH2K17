#include "OpenGL_VBO.h"

///////////////////////////////////////////////////////////////////////////////
/// @file ModeleStorage_VBO.cpp
/// @author Martin Paradis
/// @date 2015-08-28
/// @version 1.0
///
/// @addtogroup modele Modele
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "glm/glm.hpp"
#include "glm/gtc/type_ptr.hpp"
#include "Modele3D.h"
#include "AideGL.h"
#include "Utilitaire.h"
#include <iostream>
#include "LightManager.h"

/// Position de l'attribut de location dans le nuanceur de sommet
#define VERTEX_LOCATION 0
#define TEXCOORD_LOCATION 1
#define NORMAL_LOCATION 2

namespace opengl{

	Programme VBO::programme_;
	Nuanceur VBO::nuanceurFragment_;
	Nuanceur VBO::nuanceurSommet_;

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn VBO::VBO(modele::Modele3D const* modele)
	///
	/// Assigne le modèle 3D.
	///
	/// @param[in] modele : le modele 3D à dessiner.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	VBO::VBO(modele::Modele3D const* modele)
		: modele_{ modele }
	{}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn VBO::~VBO()
	///
	/// Destructeur, relâche le VBO.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	VBO::~VBO()
	{
		liberer();
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VBO::charger()
	///
	/// Charge les données du modèles 3D sur la mémoire de la carte graphique
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VBO::charger()
	{
		if(!programme_.estInitialise())
		{
			// Initialisation des nuanceurs
			nuanceurFragment_.initialiser(Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/Base/fragment.glsl");
			nuanceurSommet_.initialiser(Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/Base/sommet.glsl");
			programme_.initialiser();
			programme_.attacherNuanceur(nuanceurFragment_);
			programme_.attacherNuanceur(nuanceurSommet_);
		}
		creerVBO(modele_->obtenirNoeudRacine());
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VBO::creerVBO(modele::Noeud const& noeud)
	///
	/// Création des VBO selon les données propres à chaque Mesh. L'ordre 
	/// des identifiants OpenGL se calque sur le modèle 3D, en prenant pour
	/// acquis que la hiérarchie interne des noeuds internes n'est pas 
	/// modifiée.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VBO::creerVBO(modele::Noeud const& noeud)
	{
		for (auto const& mesh : noeud.obtenirMeshes())
		{
			enregistrerTampon(mesh.obtenirSommets());
			if (mesh.possedeNormales())
				enregistrerTampon(mesh.obtenirNormales());
			if (mesh.possedeTexCoords())
				enregistrerTampon(mesh.obtenirTexCoords());
			if (mesh.possedeFaces())
				enregistrerTampon(mesh.obtenirFaces());
		}
		/// Création récursive.
		for (auto const& n : noeud.obtenirEnfants())
		{
			creerVBO(n);
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VBO::dessiner() const
	///
	/// Appelle le dessin du modèle à partir des VBO.  Utilise le modèle 3D
	/// pour obtenir la matériau propre à chaque Mesh.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VBO::dessiner(const glm::mat4& transformation, const OptionsDessin& options, MatricesPipeline& matrices) const
	{
		unsigned int bufferIndex = 0;
		if (!options.invisible_)
		dessiner(modele_->obtenirNoeudRacine(), bufferIndex, transformation, options, matrices);
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VBO::dessiner(modele::Noeud const& noeud, unsigned int& bufferIndex) const
	///
	/// Dessin récursif du modèle 3D.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VBO::dessiner(modele::Noeud const& noeud, unsigned int& bufferIndex, const glm::mat4& transformation, const OptionsDessin& options, MatricesPipeline& matrices) const
	{
		// Matrice de transformation
		glm::mat4x4 const& m{ transformation * noeud.obtenirTransformation() };
		matrices.matrModel = matrices.matrModel * glm::mat4(noeud.obtenirTransformation());

		// Appliquer le nuanceur
		Programme::Start(programme_);
		programme_.assignerUniforme("modelViewProjection", m);
		programme_.assignerUniforme("matrModel", matrices.matrModel);
		programme_.assignerUniforme("matrVisu", matrices.matrVisu);
		programme_.assignerUniforme("matrNormale", glm::transpose(glm::inverse(glm::mat3(matrices.matrModel))));
		programme_.assignerUniforme("positionCamera", matrices.positionCamera);

		int disableAmbiant = light::LightManager::obtenirInstance()->obtenirAmbiantState() ? 1 : 0;
		programme_.assignerUniforme("disableAmbiant", disableAmbiant);

		for (auto const& mesh : noeud.obtenirMeshes())
		{
			// Appliquer le matériau pour le mesh courant
			appliquerMateriau(mesh.obtenirMateriau());

			bool possedeNormales{ mesh.possedeNormales() };
			bool possedeCouleurs{ mesh.possedeCouleurs() };
			bool possedeTexCoords{ mesh.possedeTexCoords() };
			bool possedeSommets{ mesh.possedeSommets() };
			bool possedeFaces{ mesh.possedeFaces() };

			if (possedeSommets)
			{
				glBindBuffer(GL_ARRAY_BUFFER, handles_[bufferIndex]); ++bufferIndex;
				glEnableVertexAttribArray(VERTEX_LOCATION);
				glVertexAttribPointer(VERTEX_LOCATION, 3, GL_FLOAT, GL_FALSE, 0, nullptr);
			}
			if (possedeNormales)
			{
				glBindBuffer(GL_ARRAY_BUFFER, handles_[bufferIndex]); ++bufferIndex;
				glEnableVertexAttribArray(NORMAL_LOCATION);
				glVertexAttribPointer(NORMAL_LOCATION, 3, GL_FLOAT, GL_FALSE, 0, nullptr);
			}
			if (possedeTexCoords)
			{
				glBindBuffer(GL_ARRAY_BUFFER, handles_[bufferIndex]); ++bufferIndex;
				glEnableVertexAttribArray(TEXCOORD_LOCATION);
				glVertexAttribPointer(TEXCOORD_LOCATION, 2, GL_FLOAT, GL_FALSE, 0, nullptr);
			}

			// Condition pour la transparence
			if ((options.useOtherColor_ && options.color_.w < 1) || options.effetFantome_)
			{
				glEnable(GL_BLEND);
				glDepthMask(false);
				glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
			}

			// Utilisation d'une autre couleur
			if (options.effetFantome_)
			{
				programme_.assignerUniforme("material.diffuse",glm::vec3(options.localSelectionColor_.x, options.localSelectionColor_.y, options.localSelectionColor_.z));
				programme_.assignerUniforme("material.transparence", options.localSelectionColor_.a);
				programme_.assignerUniforme("useDiffuseColor", 1);

			}
			else if (options.useOtherColor_)
			{
				programme_.assignerUniforme("material.diffuse", glm::vec3(options.color_));
				programme_.assignerUniforme("material.transparence", options.color_.w);
				programme_.assignerUniforme("useDiffuseColor", 1);
			}

			// Condition qui dessine
			if (possedeFaces)
			{
				glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, handles_[bufferIndex]); ++bufferIndex;
				glDrawElements(GL_TRIANGLES, static_cast<GLsizei>(3 * mesh.obtenirFaces().size()), GL_UNSIGNED_INT, nullptr);
			}

			if (possedeSommets)
				glDisableVertexAttribArray(VERTEX_LOCATION);
			if (possedeTexCoords)
				glDisableVertexAttribArray(TEXCOORD_LOCATION);
			if ((options.useOtherColor_ && options.color_.w < 1) || options.effetFantome_)
			{
				// Desactiver Blend mode
				programme_.assignerUniforme("useDiffuseColor", 0);
				glDepthMask(true);
				glDisable(GL_BLEND);
			}
		}
		Programme::Stop(programme_);

		/// Dessin récursif.
		for (auto const& n : noeud.obtenirEnfants())
		{
			dessiner(n, bufferIndex, m, options, matrices);
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VBO::liberer()
	///
	/// Relâche la mémoire sur la carte graphique.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VBO::liberer()
	{
		for (auto const& handle : handles_)
		{
			glDeleteBuffers(static_cast<GLsizei>(handles_.size()), handles_.data());
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VBO::appliquerMateriau(modele::Materiau const& materiau) const
	///
	/// Assigne un matériau OpenGL selon la méthode dépréciée d'illumination
	/// d'OpenGL
	///
	/// @param[in] materiau : le materiau à assigner
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VBO::appliquerMateriau(modele::Materiau const& materiau) const
	{
		/// Vérifier si texture existe
		GLint useDiffuseColorLoc = glGetUniformLocation(programme_.obtenirHandle(), "useDiffuseColor");
		if (modele_->possedeTexture(materiau.nomTexture_)) {
			// Activer le texturage OpenGL et lier la texture appropriée
			glEnable(GL_TEXTURE_2D);
			glScalef(1.0, -1.0, 1.0);
			glBindTexture(GL_TEXTURE_2D, modele_->obtenirTextureHandle(materiau.nomTexture_));
			glUniform1i(useDiffuseColorLoc, false);
		}
		else {
			// Désactiver le texturage OpenGL puisque cet objet n'a aucune texture
			glBindTexture(GL_TEXTURE_2D, 0);
			glDisable(GL_TEXTURE_2D);
			glUniform1i(useDiffuseColorLoc, true);
		}

		/// Assigner le matériau
		/// À Faire : Envoyez les valeurs du matériau aux nuanceurs
		programme_.assignerUniforme("material.diffuse", materiau.diffuse_);
		programme_.assignerUniforme("material.transparence", materiau.opacite_);
		programme_.assignerUniforme("material.emission", materiau.emission_);
		programme_.assignerUniforme("material.speculaire", materiau.speculaire_);
		programme_.assignerUniforme("material.ambiant", materiau.ambiant_);
		programme_.assignerUniforme("material.shininess", materiau.shininess_ * materiau.shininessStrength_);

		glPolygonMode(
			GL_FRONT_AND_BACK,
			materiau.filDeFer_ ? GL_LINE : GL_FILL);

		materiau.afficherDeuxCotes_ ? glEnable(GL_CULL_FACE) : glDisable(GL_CULL_FACE);

	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////