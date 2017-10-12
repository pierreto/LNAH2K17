///////////////////////////////////////////////////////////////////////////////
/// @file Light.h
/// @author Nam Lesage
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#ifndef __UTILITAIRE_LIGHT_H__
#define __UTILITAIRE_LIGHT_H__

#include <vector>
#include <string>

#include "glm/glm.hpp"
#include "OpenGL_Programme.h"
#include "Utilitaire.h"
#include "LightAnimation.h"

#define LIGHT_UB_INDEX 2
#define LIGHT_BYTE_SIZE 112

#define LIGHT_OPEN_OFFSET 0
#define LIGHT_TYPE_OFFSET 4
#define LIGHT_AMBIANT_OFFSET 16
#define LIGHT_DIFFUSE_OFFSET 32
#define LIGHT_SPECULAR_OFFSET 48
#define LIGHT_POSITION_OFFSET 64
#define LIGHT_SPOTDIRECTION_OFFSET 80
#define LIGHT_SPOTEXPONENT_OFFSET 92
#define LIGHT_SPOTCUTOFF_OFFSET 96
#define LIGHT_CONSTANTATT_OFFSET 100
#define LIGHT_LINEARATT_OFFSET 104
#define LIGHT_QUADATT_OFFSET 108


namespace light {

	enum LightType {
		AMBIANTE = 0,
		DIRECTIONNELLE = 1,
		SPOT = 2,
		POINT = 3
	};

	///////////////////////////////////////////////////////////////////////////
	/// @struct Light
	/// @brief Structure comprennant les divers composants d'une source de lumiere.
	///
	/// @author Nam Lesage
	/// @date 2016-11-12
	///////////////////////////////////////////////////////////////////////////
	struct LightParameters {
	public:
		/// Indique si la lumière est ouverte ou fermée
		bool ouverte_{ false };
		/// Type de lumiere
		LightType type_{ DIRECTIONNELLE };
		/// Lumiere ambiante
		glm::vec4 ambiant_;
		/// Lumiere diffuse
		glm::vec4 diffuse_;
		/// Lumiere speculaire
		glm::vec4 speculaire_;
		/// Position de la lumiere
		glm::vec4 position_;

		// Information sur les spot
		/// Orientation du spot
		glm::vec3 spotDirection_;
		/// Exposant du spot
		float spotExponent_{ 1.0f };
		/// Angle du spot
		float spotCutoff_{ 45.0f };
		/// Facteur d'atténuation constant
		float constantAttenuation{ 0.0f };
		/// Facteur d'atténuation linéaire
		float linearAttenuation{ 0.05f };
		/// Facteur d'atténuation quadratique
		float quadraticAttenuation{ 0.05f };
	};



	///////////////////////////////////////////////////////////////////////////
	/// @class Light
	/// @brief Classe repésentant une lumière. Elle gère la communication avec le nuanceur
	///
	/// @author Nam Lesage
	/// @date 2016-11-12
	///////////////////////////////////////////////////////////////////////////
	class Light 
	{
	public:
		/// Constructeur
		Light(unsigned int bufferIndex, const LightParameters& parameters);
		/// Destructeur
		~Light();

		/// Anime la lumière
		void animer(float temps);

		/// Obtient les paramètres de la lumière
		LightParameters getLightParameters() const;

		/// Modifie les paramètres de la lumière
		void modifyLightParameters(const LightParameters& parameters);
		/// Ajoute une animation
		void addAnimation(LightAnimation* animation);
		/// Ouvre/Ferme la lumière
		void turnOnOff(bool ouverte);
		/// Ouvre/Ferme la lumière
		void toggleOnOff();

		// Modificateur
		/// Modifie la composante diffuse de la lumière
		void modifyDiffuse(const glm::vec4& diffuse);
		/// Modifie la direction du spot
		void modifySpotDirection(const glm::vec3& direction);


		/// Obtient l'index du buffer dans le nuanceur
		unsigned int getBufferIndex() const;
		/// Obtient le handle du ubo
		GLuint getUbo() const;

	private:

		/// Constructeur copie désactivé.
		Light(const Light&) = delete;
		/// Met à jour le ubo au complet
		void updateData();
		/// Index dans le nuanceur
		unsigned int bufferIndex_;
		/// Uniform buffer object
		GLuint ubo_;

		/// Paramètres de la lumière
		LightParameters parameters_;
		/// Liste des animations
		std::vector<LightAnimation*> animations_;
	};


}




#endif /// __UTILITAIRE_LIGHT_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////