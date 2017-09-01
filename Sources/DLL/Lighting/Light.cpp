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
#include <iostream>

namespace light {


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Light::Light(unsigned int bufferIndex, const LightParameters & parameters)
	///
	/// Constructeur avec paramètres
	///
	///	@param[in] bufferIndex : Index du buffer dans le nuanceur
	///	@param[in] parameters : Paramètre de la lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	Light::Light(unsigned int bufferIndex, const LightParameters & parameters)
		: bufferIndex_(bufferIndex), parameters_(parameters)
	{
		glGenBuffers(1, &ubo_);
		glBindBuffer(GL_UNIFORM_BUFFER, ubo_);
		glBindBufferBase(GL_UNIFORM_BUFFER, LIGHT_UB_INDEX + bufferIndex_, ubo_);
		glBufferData(GL_UNIFORM_BUFFER, LIGHT_BYTE_SIZE, NULL, GL_DYNAMIC_DRAW);
		glBindBuffer(GL_UNIFORM_BUFFER, 0);

		updateData();

	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Light::~Light()
	///
	/// Destructeur
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	Light::~Light() {
		glDeleteBuffers(1, &ubo_);

		for (auto animation : animations_)
			delete animation;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::animer(float temps) const
	///
	/// Anime les composantes de la lumière
	///
	///	@param[in] temps : Temps entre 2 frames
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::animer(float temps) {
		for (auto& animation : animations_) {
			animation->animer(temps);
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn LightParameters Light::getLightParameters() const
	///
	/// Obtient les paramètres de la lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	LightParameters Light::getLightParameters() const {
		return parameters_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::modifyLightParameters(const LightParameters & parameters)
	///
	/// Modifie les paramètres de la lumière au complet
	///
	///	@param[in] parameters : Les paramètres
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::modifyLightParameters(const LightParameters & parameters) {
		parameters_ = parameters;
		updateData();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::addAnimation(LightAnimation* animation)
	///
	/// Permet d'ajouter des animations au vecteur d'animations à appliquer.
	///
	///	@param[in] LightAnimation : Animation à ajouter
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::addAnimation(LightAnimation* animation) {
		animations_.push_back(animation);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::turnOnOff(bool ouverte)
	///
	/// Ouvre/Ferme la lumière
	///
	///	@param[in] ouverte : Statut de la lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::turnOnOff(bool ouverte) {
		parameters_.ouverte_ = ouverte;
		int open = parameters_.ouverte_ ? 1 : 0;
		glBindBuffer(GL_UNIFORM_BUFFER, ubo_);
		glBufferSubData(GL_UNIFORM_BUFFER, 0, sizeof(GLint), (GLint*)(&open));
		glBindBuffer(GL_UNIFORM_BUFFER, 0);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::toggleOnOff()
	///
	/// Ouvre/Ferme la lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::toggleOnOff() {
		turnOnOff(!parameters_.ouverte_);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::modifyDiffuse(const glm::vec4& diffuse)
	///
	/// Modifie la composante diffuse de la lumière
	///
	/// @param[in] diffuse : Nouvelle couleur diffuse
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::modifyDiffuse(const glm::vec4& diffuse) {
		parameters_.diffuse_ = diffuse;
		glBindBuffer(GL_UNIFORM_BUFFER, ubo_);
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_DIFFUSE_OFFSET, sizeof(glm::vec4), glm::value_ptr(parameters_.diffuse_));
		glBindBuffer(GL_UNIFORM_BUFFER, 0);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::modifySpotDirection(const glm::vec3& direction)
	///
	/// Modifie la direction du spot
	///
	/// @param[in] diffuse : La direction du spot
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::modifySpotDirection(const glm::vec3& direction) {
		parameters_.spotDirection_ = direction;
		glBindBuffer(GL_UNIFORM_BUFFER, ubo_);
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_SPOTDIRECTION_OFFSET, sizeof(glm::vec3), glm::value_ptr(parameters_.spotDirection_));
		glBindBuffer(GL_UNIFORM_BUFFER, 0);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn unsigned int Light::getBufferIndex() const
	///
	/// Obtient l'index du buffer dans le nuanceur
	///
	/// @return Index du buffer
	///
	////////////////////////////////////////////////////////////////////////
	unsigned int Light::getBufferIndex() const {
		return bufferIndex_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn GLuint Light::getUbo() const
	///
	/// Obtient le handle du ubo
	///
	/// @return Handle du ubo
	///
	////////////////////////////////////////////////////////////////////////
	GLuint Light::getUbo() const {
		return ubo_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Light::updateData()
	///
	/// Met à jour le ubo au complet
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void Light::updateData() {
		int ouverte = parameters_.ouverte_ ? 1 : 0;
		glBindBuffer(GL_UNIFORM_BUFFER, ubo_);
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_OPEN_OFFSET, sizeof(GLint), (GLint*)(&ouverte));
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_TYPE_OFFSET, sizeof(GLint), (GLint*)(&parameters_.type_));
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_AMBIANT_OFFSET, sizeof(glm::vec4), glm::value_ptr(parameters_.ambiant_));
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_DIFFUSE_OFFSET, sizeof(glm::vec4), glm::value_ptr(parameters_.diffuse_));
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_SPECULAR_OFFSET, sizeof(glm::vec4), glm::value_ptr(parameters_.speculaire_));
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_POSITION_OFFSET, sizeof(glm::vec4), glm::value_ptr(parameters_.position_));
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_SPOTDIRECTION_OFFSET, sizeof(glm::vec3), glm::value_ptr(parameters_.spotDirection_));
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_SPOTEXPONENT_OFFSET, sizeof(GLfloat), &parameters_.spotExponent_);
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_SPOTCUTOFF_OFFSET, sizeof(GLfloat), &parameters_.spotCutoff_);
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_CONSTANTATT_OFFSET, sizeof(GLfloat), &parameters_.constantAttenuation);
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_LINEARATT_OFFSET, sizeof(GLfloat), &parameters_.linearAttenuation);
		glBufferSubData(GL_UNIFORM_BUFFER, LIGHT_QUADATT_OFFSET, sizeof(GLfloat), &parameters_.quadraticAttenuation);
		glBindBuffer(GL_UNIFORM_BUFFER, 0);
	}

}
