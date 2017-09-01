///////////////////////////////////////////////////////////////////////////////
/// @file Light.cpp
/// @author Nam Lesage
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "Light.h"
#include "LightAnimation.h"
#include <iostream>

namespace light {

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn LightGradientAnim::LightGradientAnim(const glm::vec4 & color1, const glm::vec4 & color2, float frequence)
	///
	/// Constructuer avec paramètres
	///
	/// @param[in] light : Un pointeur vers la lumière à animer
	/// @param[in] color1 : Première couleur du gradient
	/// @param[in] color2 : Seconde couleur du gradient
	/// @param[in] frequence : Frequence de l'animation
	///
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	LightGradientAnim::LightGradientAnim(Light* light, const glm::vec4 & color1, const glm::vec4 & color2, float frequence)
		: LightAnimation(light), color1_(color1), color2_(color2), currentColor_(color1), frequence_(frequence), angle_(0)
	{
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightGradientAnim::animer( float temps)
	///
	/// Anime la couleur de la lumière
	///
	/// @param[in] temps : Intervalle entre 2 frames
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void LightGradientAnim::animer(float temps)
	{
		angle_ = fmod(angle_ + temps * frequence_ * 2 * (float)utilitaire::PI, 2 * (float)utilitaire::PI);
		currentColor_ = color1_ + float(glm::sin(angle_) + 1 ) / 2 * (color2_ - color1_);
		
		light_->modifyDiffuse(currentColor_);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn LightRotateAnim::LightRotateAnim(Light* light, const glm::vec3& spotDirection, const glm::vec3 & angles, float frequence)
	///
	/// Constructuer avec paramètres
	///
	/// @param[in] light : Un pointeur vers la lumière à animer
	/// @param[in] angles : Rotation maximale sur chacun des axes
	/// @param[in] frequence : Frequence de l'animation
	///
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	LightRotateAnim::LightRotateAnim(Light* light, const glm::vec3 & angles, float frequence)
		: LightAnimation(light), frequence_(frequence), currentAngles_(0)
	{
		angles_ = glm::clamp(glm::radians(angles), 0.0f , (float) utilitaire::PI);
		spotDirection_ = light->getLightParameters().spotDirection_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightRotateAnim::animer( float temps)
	///
	/// Anime la direction du spot
	///
	/// @param[in] temps : Intervalle entre 2 frames
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void LightRotateAnim::animer( float temps)
	{
		static float angle = 0;
		angle = fmod(angle + temps * frequence_ * 2 * (float)utilitaire::PI, 2 * (float)utilitaire::PI);

		currentAngles_ = sin(angle) * angles_;
		glm::vec3 spotDirection = glm::vec3(glm::vec4(spotDirection_, 1.0f)
									* glm::rotate(glm::mat4(1), currentAngles_.x, glm::vec3(1, 0, 0)) 
									* glm::rotate(glm::mat4(1), currentAngles_.y, glm::vec3(0, 1, 0))
									* glm::rotate(glm::mat4(1), currentAngles_.z, glm::vec3(0, 0, 1)));

		light_->modifySpotDirection(spotDirection);
	}
}
