///////////////////////////////////////////////////////////////////////////
/// @file BoiteEnvironnement.cpp
/// @author Nicolas Cloutier
/// @date 2016-07-02
/// @version 1.1
///
/// @addtogroup utilitaire Utilitaire
/// @{
///////////////////////////////////////////////////////////////////////////
#include "BoiteEnvironnement.h"
#include "AideGL.h"

#include "FreeImage.h"
#include "GL/glew.h"

#include "glm\glm.hpp"

// Définition prise de l'en-tête de la version 1.2 d'OpenGL.
#define GL_CLAMP_TO_EDGE 0x812F

// Location de l'attribut des sommets dans le nuanceur de sommets
#define VERTEX_LOCATION 0

namespace utilitaire {

	const GLfloat BoiteEnvironnement::sommets_[] =
	{
		-1.0f, -1.0f, 0.0f,
		 1.0f, -1.0f, 0.0f,
		-1.0f,  1.0f, 0.0f,
		 1.0f, -1.0f, 0.0f,
		 1.0f,  1.0f, 0.0f,
		-1.0f,  1.0f, 0.0f
	};

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn BoiteEnvironnement::BoiteEnvironnement(const std::string& fichierXpos, const std::string& fichierXneg, const std::string& fichierYpos, const std::string& fichierYneg, const std::string& fichierZpos, const std::string& fichierZneg)
	///
	/// Ce constructeur charge les 6 textures correspondant à chacune des faces
	/// de la boîte d'environnement.
	///
	/// @param[in] fichierXpos : Le nom du fichier contenant l'image correspondant
	///                          à l'axe des X positifs.
	/// @param[in] fichierXneg : Le nom du fichier contenant l'image correspondant
	///                          à l'axe des X négatifs.
	/// @param[in] fichierYpos : Le nom du fichier contenant l'image correspondant
	///                          à l'axe des X positifs.
	/// @param[in] fichierYneg : Le nom du fichier contenant l'image correspondant
	///                          à l'axe des Y négatifs.
	/// @param[in] fichierZpos : Le nom du fichier contenant l'image correspondant
	///                          à l'axe des Z positifs.
	/// @param[in] fichierZneg : Le nom du fichier contenant l'image correspondant
	///                          à l'axe des Z négatifs.
	/// @param[in] dimension   : Largeur et hauteur de toutes les images.
	///
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	BoiteEnvironnement::BoiteEnvironnement(
		const std::string& fichierXpos, const std::string& fichierXneg,
		const std::string& fichierYpos, const std::string& fichierYneg,
		const std::string& fichierZpos, const std::string& fichierZneg,
		unsigned int dimension)
	{
		
		// Charger toutes les images
		FIBITMAP* dib = nullptr;
		aidegl::glLoadImage(fichierXpos, dib, imagesEnvironnement_[0]);
		FreeImage_Unload(dib);
		aidegl::glLoadImage(fichierXneg, dib, imagesEnvironnement_[1]);
		FreeImage_Unload(dib);
		aidegl::glLoadImage(fichierYpos, dib, imagesEnvironnement_[2]);
		FreeImage_Unload(dib);
		aidegl::glLoadImage(fichierYneg, dib, imagesEnvironnement_[3]);
		FreeImage_Unload(dib);
		aidegl::glLoadImage(fichierZpos, dib, imagesEnvironnement_[4]);
		FreeImage_Unload(dib);
		aidegl::glLoadImage(fichierZneg, dib, imagesEnvironnement_[5]);
		FreeImage_Unload(dib);

		// Mettre le tout dans une VAO
		glGenVertexArrays(1, &vaoId_);
		glBindVertexArray(vaoId_);

		// Lier les sommets
		glGenBuffers(1, &vboId_);
		glBindBuffer(GL_ARRAY_BUFFER, vboId_);
		glBufferData(GL_ARRAY_BUFFER, sizeof(sommets_), sommets_, GL_STATIC_DRAW);
		
		// On identifie comment les sommets se lie au nuanceur
		glEnableVertexAttribArray(VERTEX_LOCATION);
		glVertexAttribPointer(VERTEX_LOCATION, 3, GL_FLOAT, GL_FALSE, 0, nullptr);

		// Préparer la texture sous format cubemap
		glGenTextures(1, &textureId_);
		glBindTexture(GL_TEXTURE_CUBE_MAP, textureId_);


		//On rotate l'image de bas de 180
		imagesEnvironnement_[3] = FreeImage_Rotate(imagesEnvironnement_[3], 180);

		// On lie les images aux côtés de la texture en cubemap
		for (int i = 0; i < 6; ++i) {
			// Tel que spécifié dans la documentation de FreeImage (voir Pixel Access 
			// Functions, page 29 du PDF FreeImage 3.9.0 documentation), la 
			// disposition des composantes des couleurs est dépendante de 
			// l'architecture.  Sur little endian, le format BGR (plutôt que RGB) est 
			// utilisé.  On utilise donc l'extension GL_EXT_bgra (on pourrait vérifie 
			// à l'exécution que l'extension est présente)

			if (FreeImage_GetBPP(imagesEnvironnement_[i]) == 24)
			{
				glTexImage2D(
					GL_TEXTURE_CUBE_MAP_POSITIVE_X + i,
					0, GL_RGB, dimension, dimension, 0,
					GL_BGRA, GL_UNSIGNED_BYTE,
					FreeImage_GetBits(imagesEnvironnement_[i]));
			}
			else if (FreeImage_GetBPP(imagesEnvironnement_[i]) == 32)
			{
				glTexImage2D(
					GL_TEXTURE_CUBE_MAP_POSITIVE_X + i,
					0, GL_RGBA, dimension, dimension, 0,
					GL_BGRA, GL_UNSIGNED_BYTE,
					FreeImage_GetBits(imagesEnvironnement_[i]));
			}
		}

		// Paramètres de la texture
		glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_EDGE);

		// Relacher les objects
		glBindTexture(GL_TEXTURE_CUBE_MAP, 0);
		glBindVertexArray(0);

		// Initialisation des nuanceurs
		nuanceurFragment_.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/Skybox/fragment_boite_environnement.glsl");
		nuanceurSommet_.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/Skybox/sommet_boite_environnement.glsl");
		programme_.initialiser();
		programme_.attacherNuanceur(nuanceurFragment_);
		programme_.attacherNuanceur(nuanceurSommet_);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn BoiteEnvironnement::~BoiteEnvironnement()
	///
	/// Ce destructeur libère l'espace allouée à chacune des textures des faces
	/// de la boîte d'environnement.
	///
	/// @return Aucune (destructeur).
	///
	////////////////////////////////////////////////////////////////////////
	BoiteEnvironnement::~BoiteEnvironnement()
	{
		for (int i = 0; i < 6; ++i)
			FreeImage_Unload(imagesEnvironnement_[i]);
		glDeleteTextures(1, &textureId_);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void BoiteEnvironnement::afficher(const glm::dvec3& centre, double demiLargeur) const
	///
	/// Cette fonction affiche tout simplement la boîte d'environnement.
	///
	/// @param[in] projection  : La matrice de la projection utilisée.
	/// @param[in] modeleVue : La matrice du modèle du monde et de la
	///	                       vue (caméra).
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void BoiteEnvironnement::afficher(glm::mat4 const& projection, glm::mat4 const& modeleVue)
	{
		glPushAttrib(GL_ENABLE_BIT | GL_DEPTH_BUFFER_BIT);

		// On désactive le test de profondeur car il n'est pas nécessaire puisque
		// la boîte est à l'infini.
		glDisable(GL_DEPTH_TEST);
		// On désactive l'écriture dans le tampon de profondeur pour s'assurer que
		// la boîte est véritablement "à l'infini".
		glDepthMask(GL_FALSE);

		// On active les textures.
		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_CUBE_MAP, textureId_);
		
		// On active les nuanceurs.
		opengl::Programme::Start(programme_);
		programme_.assignerUniforme("projection", projection);
		programme_.assignerUniforme("modelView", modeleVue);

		// On dessine la boîte d'environnement contenue dans la VAO.
		glBindVertexArray(vaoId_);
		glDrawArrays(GL_TRIANGLES, 0, 6);
		glBindVertexArray(0);

		// On désactive les éléments non nécessaires.
		glDisableVertexAttribArray(0);
		opengl::Programme::Stop(programme_);
		glBindTexture(GL_TEXTURE_CUBE_MAP, 0);

		glPopAttrib();
	}


} // Fin de l'espace de nom utilitaire.


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
