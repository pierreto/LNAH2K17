///////////////////////////////////////////////////////////////////////////////
/// @file Logger.cpp
/// @author Anthony Abboud
/// @date 2015-11-16
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "LightManager.h"
#include "LightAnimation.h"
#include "GL/glew.h"
#include "Modele3D.h"
#include <iostream>
#include <numeric>
#include <algorithm>

namespace light {
	/// Pointeur vers l'instance unique de la classe.
	LightManager* LightManager::instance_{ nullptr };

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn inline LightManager* LightManager::obtenirInstance()
	///
	/// Cette fonction retourne l'instance unique de la classe. Si l'instance
	/// n'existe pas, elle est créée. Ainsi, une seule instance sera créée.
	/// Cette fonction n'est pas "thread-safe".
	///
	/// @return L'instance unique de la classe.
	///
	////////////////////////////////////////////////////////////////////////
	LightManager* LightManager::obtenirInstance() {
		if (instance_ == nullptr)
			instance_ = new LightManager();

		return instance_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn inline void LightManager::libererInstance()
	///
	/// Détruit l'instance unique de la classe.  Cette fonction n'est pas
	/// "thread-safe".
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::libererInstance() {
		delete instance_;
		instance_ = nullptr;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::usePresetLighting()
	///
	/// Construit les systèmes de lumières avec les lumières par défaut
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::usePresetLighting() {
		LightParameters ambiante;
		ambiante.ouverte_ = true;
		ambiante.type_ = AMBIANTE;
		ambiante.ambiant_ = glm::vec4(1.0f);
		ambiante.diffuse_ = glm::vec4(0.6f);
		ambiante.speculaire_ = glm::vec4(0.8f);
		ambiante.position_ = glm::vec4(0.0f, 100.0f, 0.0f, 1.0f);
		ambiante.spotDirection_ = glm::vec3(0.0f, -1.0f, -1.0f);

		Light* ambiantLight = createLight(ambiante);

		LightParameters directional;
		directional.ouverte_ = true;
		directional.type_ = DIRECTIONNELLE;
		directional.ambiant_ = glm::vec4(0.0f);
		directional.diffuse_ = glm::vec4(0.5f);
		directional.speculaire_ = glm::vec4(0.8f);
		directional.position_ = glm::vec4(0.0f, 100.0f, 0.0f, 1.0f);
		directional.spotDirection_ = glm::vec3(0.0f, -1.0f, -1.0f);

		Light* directionalLight = createLight(directional);

		glm::vec4 spot1Color = glm::vec4(153.0f / 255, 54.0f / 255, 200.0f / 255, 1.0f);
		glm::vec4 spot2Color = glm::vec4(58.0f / 255, 99.0f / 255, 200.0f / 255, 1.0f);

		LightParameters spot1;
		
		spot1.ouverte_ = true;
		spot1.type_ = SPOT;
		spot1.ambiant_ = glm::vec4(0.0f);
		spot1.diffuse_ = spot1Color;
		spot1.speculaire_ = glm::vec4(1);
		spot1.position_ = glm::vec4(0.0f, 200.0f, 120.0f, 1.0f);
		spot1.spotDirection_ = glm::vec3(0.0f, -1.0f, -0.5f);
		spot1.spotCutoff_ = 45;
		spot1.spotExponent_ = 7;
		spot1.constantAttenuation = 1.0f;
		spot1.linearAttenuation = 0.005f;
		spot1.quadraticAttenuation = 0.0000f;

		Light* spot1Light =  createLight(spot1);

		spot1Light->addAnimation(new LightGradientAnim(spot1Light, spot1Color, spot2Color, 0.5f));
		spot1Light->addAnimation(new LightRotateAnim(spot1Light ,glm::vec3(0,90,0) , 0.1f));

		LightParameters spot2;
		spot2.ouverte_ = true;
		spot2.type_ = SPOT;
		spot2.ambiant_ = glm::vec4(0.0f);
		spot2.diffuse_ = spot1Color;
		spot2.speculaire_ = glm::vec4(1);
		spot2.position_ = glm::vec4(0.0f, 200.0f, -120.0f, 1.0f);
		spot2.spotDirection_ = glm::vec3(0.0f, -1.0f, 0.5f);
		spot2.spotCutoff_ = 45;
		spot2.spotExponent_ = 7;
		spot2.constantAttenuation = 1.0f;
		spot2.linearAttenuation = 0.005f;
		spot2.quadraticAttenuation = 0.0000f;

		Light* spot2Light = createLight(spot2);

		spot2Light->addAnimation(new LightGradientAnim(spot2Light, spot2Color, spot1Color, 0.5f));
		spot2Light->addAnimation(new LightRotateAnim(spot2Light, glm::vec3(0, 90, 0), 0.1f));


		LightParameters spot3;
		spot3.ouverte_ = true;
		spot3.type_ = SPOT;
		spot3.ambiant_ = glm::vec4(0.0f);
		spot3.diffuse_ = spot1Color;
		spot3.speculaire_ = glm::vec4(1);
		spot3.position_ = glm::vec4(0.0f, 200.0f, 0.0f, 1.0f);
		spot3.spotDirection_ = glm::vec3(0.0f, -1.0f, 0.0f);
		spot3.spotCutoff_ = 20;
		spot3.spotExponent_ = 15;
		spot3.constantAttenuation = 1.0f;
		spot3.linearAttenuation = 0.01f;
		spot3.quadraticAttenuation = 0.0000f;

		Light* spot3Light = createLight(spot3);

		spot3Light->addAnimation(new LightGradientAnim(spot3Light, spot2Color, spot1Color, 0.5f));
		spot3Light->addAnimation(new LightRotateAnim(spot3Light, glm::vec3(0, 0, 70), 0.025f));
		
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::animer(float temps) const
	///
	/// Anime les lumières
	///
	///	@param[in] temps : Le temps entre 2 frames
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::animer(float temps) const {
		for (auto light : lights_)
			light->animer(temps);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Light* LightManager::createLight(const LightParameters& parameters)
	///
	/// Ajoute une lumière à la scène
	///
	///	@param[in] parameters : Paramètres de la lumière
	///
	/// @return Pointeur vers la lumière créée.
	///
	////////////////////////////////////////////////////////////////////////
	Light* LightManager::createLight(const LightParameters& parameters) {
		if (lights_.size() == MAX_LIGHT_SOURCES) {
			std::cout << "Impossible d'ajouter plus de trois sources de lumière dans la scène" << std::endl;
			return nullptr;
		}

		unsigned int index = getIndex();

		if (index == -1)
			return nullptr;


		Light* light = new Light(index, parameters);
		lights_.push_back(light);

		return light;	
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::deleteLight(Light * light)
	///
	/// Supprime une lumière
	///
	///	@param[in] light : Pointeur vers la lumière à supprimer
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::deleteLight(Light * light) {
		auto it = std::find(lights_.begin(), lights_.end(), light);
		if ( it  != lights_.end()) {
			releaseIndex((*it)->getBufferIndex());
			delete (*it);
			lights_.erase(it);
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::toggleLight(LightType type)
	///
	/// Ouvre/Ferme la lumière selon le type
	///
	///	@param[in] type : Type de lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::toggleLight(LightType type) {
		for (int i = 0; i < lights_.size(); ++i) {
			if (lights_[i]->getLightParameters().type_ == type)
				toggleLight(i);
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::setLight(LightType type, bool ouverte)
	///
	/// Ouvre/Ferme la lumière selon le type et l'état
	///
	///	@param[in] type : Type de lumière
	///	@param[in] ouverte : Etat de la lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::setLight(LightType type, bool ouverte) {
		for (int i = 0; i < lights_.size(); ++i) {
			if (lights_[i]->getLightParameters().type_ == type)
				setLight(i, ouverte);
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::toggleLight(unsigned int index)
	///
	/// Ouvre/Ferme la lumière selon l'index
	///
	///	@param[in] index : Index
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::toggleLight(unsigned int index) {
		if (index < lights_.size())
			lights_[index]->toggleOnOff();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::setLight(unsigned int index, bool ouverte)
	///
	/// Ouvre/Ferme la lumière selon l'index
	///
	///	@param[in] index : Index
	///	@param[in] ouverte : Etat de la lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::setLight(unsigned int index, bool ouverte) {
		if (index < lights_.size())
			lights_[index]->turnOnOff(ouverte);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::toggleAmbiant()
	///
	/// Ouvre/Ferme la lumière ambiante
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::toggleAmbiant() {
		disableAmbiant_ = !disableAmbiant_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::setAmbiant(bool ouverte)
	///
	/// Ouvre/Ferme la lumière ambiante selon l'état
	///
	///	@param[in] ouverte : Etat de la lumière
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::setAmbiant(bool ouverte) {
		disableAmbiant_ = !ouverte;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::toggleAmbiant()
	///
	/// Ouvre/Ferme la lumière ambiante
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	bool LightManager::obtenirAmbiantState() {
		return disableAmbiant_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn int LightManager::getLightsCount() const
	///
	/// Obtient les lumières
	///
	/// @return Des pointeurs vers les lumières
	///
	////////////////////////////////////////////////////////////////////////
	std::vector<Light*> LightManager::getLights() const {
		return lights_;
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn int LightManager::getLightsCount() const
	///
	/// Obtient le nombre de lumières
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	int LightManager::getLightsCount() const {
		return lights_.size();
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn LightManager::LightManager()
	///
	/// Appel le constructeur de la classe de base
	///
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	LightManager::LightManager()
		: disableAmbiant_(false) 
	{
		for (int i = 0; i < MAX_LIGHT_SOURCES; ++i) {
			unusedBufferIndex_.push(i);
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn LightManager::~LightManager()
	///
	/// Ce destructeur désallouee l'affichage.
	///
	/// @return Aucune (destructeur).
	///
	////////////////////////////////////////////////////////////////////////
	LightManager::~LightManager() {
		if (instance_ != nullptr) {

			for (int i = 0; i < lights_.size(); ++i)
				delete lights_[i];

			lights_.clear();

			libererInstance();
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn int LightManager::getIndex()
	///
	/// Récupère un index
	///
	/// @return Index
	///
	////////////////////////////////////////////////////////////////////////
	int LightManager::getIndex() {
		if (unusedBufferIndex_.empty())
			return -1;

		int id = unusedBufferIndex_.top();
		unusedBufferIndex_.pop();

		return id;
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void LightManager::releaseIndex(unsigned int index)
	///
	/// Relâche un index
	///
	/// @param[in] index : index à relâcher
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	void LightManager::releaseIndex(unsigned int index) {
		unusedBufferIndex_.push(index);
	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
