///////////////////////////////////////////////////////////////////////////////
/// @file Light.h
/// @author Nam Lesage
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#ifndef __UTILITAIRE_LIGHT_ANIMATION_H__
#define __UTILITAIRE_LIGHT_ANIMATION_H__

#include <vector>
#include <string>

#include "glm/glm.hpp"
#include "GL/glew.h"
#include "OpenGL_Programme.h"
#include "Utilitaire.h"
#include "glm\gtc\matrix_transform.hpp"


namespace light {

	class Light;

	///////////////////////////////////////////////////////////////////////////
	/// @class LightAnimation
	/// @brief Classe abstraite pour animer la lumi�re
	///
	/// @author Nam Lesage
	/// @date 2016-11-12
	///////////////////////////////////////////////////////////////////////////
	class LightAnimation
	{
	public:
		/// Constructeur
		LightAnimation(Light* light) : light_(light) {}
		/// Destructeur
		virtual ~LightAnimation() {}
		/// Anime une ou plusieurs composantes de la lumi�re 
		virtual void animer(float temps) = 0;

	protected:
		Light* light_;
	};


	///////////////////////////////////////////////////////////////////////////
	/// @class LightGradientAnim
	/// @brief Classe pour animer la couleur de la lumi�re selon un gradient
	///
	/// @author Nam Lesage
	/// @date 2016-11-12
	///////////////////////////////////////////////////////////////////////////
	class LightGradientAnim : public LightAnimation
	{
	public:
		/// Constructeur
		LightGradientAnim(Light* light, const glm::vec4& color1, const glm::vec4& color2, float frequence);
		/// Destructeur
		virtual ~LightGradientAnim() {};
		/// Amime la couleur
		virtual void animer(float temps);

	private:
		/// Premi�re couleur du gradient
		glm::vec4 color1_;
		/// Deuxi�me couelur du gradient
		glm::vec4 color2_;
		/// Couleur courante
		glm::vec4 currentColor_;
		/// Angle pour la fonction sinus
		float angle_;
		/// Fr�quence de l'animation 
		float frequence_;
	};

	
	///////////////////////////////////////////////////////////////////////////
	/// @class LightRotateAnim
	/// @brief Classe qui effectue une rotaion sur un spot
	///
	/// @author Nam Lesage
	/// @date 2016-11-12
	///////////////////////////////////////////////////////////////////////////
	class LightRotateAnim : public LightAnimation
	{
	public:
		/// Constructeur
		LightRotateAnim(Light* light, const glm::vec3& angles, float frequence);
		/// Destructeur
		virtual ~LightRotateAnim() {};
		/// Applique une rotation � la direction du spot
		virtual void animer(float temps);

	private:
		/// Angles maximales de rotation pour chaque axe
		glm::vec3 angles_;
		/// Fr�quence de l'animation
		float frequence_;
		/// Angles courants pour chaque axe
		glm::vec3 currentAngles_;
		/// Direction initiale du spot
		glm::vec3 spotDirection_;
	};
	
}

#endif /// __UTILITAIRE_LIGHT_ANIMATION_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////