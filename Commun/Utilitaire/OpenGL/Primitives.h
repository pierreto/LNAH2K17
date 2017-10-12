///////////////////////////////////////////////////////////////////////////
/// @file Primitives.h
/// @author Nam Lesage
/// @date 2016-11-19
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __PRIMITIVES_H__
#define __PRIMITIVES_H__

#include "GL/glew.h"
#include <string>
#include "Light.h"

#include "OpenGL_Nuanceur.h"
#include "OpenGL_Programme.h"
#include "Materiau.h"
#include "OptionsDessin.h"


namespace opengl {
	///////////////////////////////////////////////////////////////////////////
	/// @class Primitives
	/// @brief Classe qui s'occupe de dessiner des primitives
	///
	/// @author Nam Lesage
	/// @date 2016-11-19
	///////////////////////////////////////////////////////////////////////////
	class Primitives {
	public:
		/// Constructeur
		Primitives(const glm::vec4& couleur = glm::vec4(1));
		/// Destructeur
		virtual ~Primitives();

		virtual void afficher(const MatricesPipeline& matrices) const = 0;

		/// Assigne une couleur;
		void assignerCouleur(const glm::vec4& couleur);
		
	protected:

		void initialiserNuanceurs();

		void updateBuffer(const std::vector<glm::vec3>& sommets);

		void appliquerMateriau() const;

		void dessiner(GLenum mode, const MatricesPipeline& matrices) const;

		virtual void genererSommets() = 0;

	private:

		// Uniform buffer object
		GLuint ubo;
		
		// Nombre de points à dessiner
		unsigned int nombreDePoints_;

		// Materiau de dessin
		modele::Materiau materiau_;

		// Shaders
		static opengl::Programme programme_;
		static opengl::Nuanceur nuanceurFragment_;
		static opengl::Nuanceur nuanceurSommet_;
		static opengl::Nuanceur nuanceurGeo_;

	};


	///////////////////////////////////////////////////////////////////////////
	/// @class Cercle
	/// @brief Classe qui s'occupe de dessiner un cercle
	///
	/// @author Nam Lesage
	/// @date 2016-11-19
	///////////////////////////////////////////////////////////////////////////
	class Cercle : public Primitives {
	public:
		/// Constructeur
		Cercle(const glm::vec3& centre, float rayon, unsigned int nbrPoints, const glm::vec4& couleur = glm::vec4(1));
		/// Destructeur
		virtual ~Cercle();

		virtual void afficher(const MatricesPipeline& matrices) const;

		void assignerCentre(const glm::vec3& centre);

	protected :

		virtual void genererSommets();

	private :

		glm::vec3 centre_;
		float rayon_;
		float nbrPoints_;
	};

	///////////////////////////////////////////////////////////////////////////
	/// @class Rectangle
	/// @brief Classe qui s'occupe de dessiner un rectangle
	///
	/// @author Nam Lesage
	/// @date 2016-11-19
	///////////////////////////////////////////////////////////////////////////
	class Rectangle : public Primitives {
	public:
		/// Constructeur
		Rectangle(const glm::vec3& position, float largeur, float hauteur, const glm::vec4& couleur = glm::vec4(1));
		/// Destructeur
		virtual ~Rectangle();

		/// Affiche les sommets
		virtual void afficher(const MatricesPipeline& matrices) const;

	protected:

		/// Génère les sommets
		virtual void genererSommets();

	private:

		/// Centre du rectanble
		glm::vec3 position_;
		/// Largeur 
		float largeur_;
		/// Hauteur 
		float hauteur_;
	};
}


#endif // __PRIMITIVES_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


