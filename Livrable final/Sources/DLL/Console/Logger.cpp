///////////////////////////////////////////////////////////////////////////////
/// @file Logger.cpp
/// @author Anthony Abboud
/// @date 2015-11-16
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "Logger.h"
#include "Utilitaire.h"
#include <Windows.h>
#include "GL/glew.h"
#include <cmath>
#include <cctype>
#include <ctime>
#include <iostream>
#include "Modele3D.h"
#include "OpenGL_VBO.h"
#include "Light.h"


/// Pointeur vers l'instance unique de la classe.
Logger* Logger::instance_{ nullptr };

////////////////////////////////////////////////////////////////////////
///
/// @fn inline Logger* Logger::obtenirInstance()
///
/// Cette fonction retourne l'instance unique de la classe. Si l'instance
/// n'existe pas, elle est créée. Ainsi, une seule instance sera créée.
/// Cette fonction n'est pas "thread-safe".
///
/// @return L'instance unique de la classe.
///
////////////////////////////////////////////////////////////////////////
Logger* Logger::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new Logger();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void Logger::libererInstance()
///
/// Détruit l'instance unique de la classe.  Cette fonction n'est pas
/// "thread-safe".
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Logger::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn Logger::Logger()
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
Logger::Logger()
	:enableCollision_(false), enableSpeed_(false), enableLight_(false), enablePortal_(false), activer_(true) 
{
	toggleLightsA_ = true;
	toggleLightsB_ = true;
	toggleLightsC_ = true;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn Logger::~Logger()
///
/// Ce destructeur désallouee l'affichage.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
Logger::~Logger() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Logger::afficherTemps()
///
/// Cette fonction affiche la date suivant le format HH:MM:SS:mmm
///
/// @param[in] Aucun
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
std::string Logger::afficherTemps() {
	SYSTEMTIME time;
	GetLocalTime(&time);
	char buffer[256];
	sprintf_s(buffer,
		"%02d:%02d:%02d:%03d",
		time.wHour,
		time.wMinute,
		time.wSecond,
		time.wMilliseconds);
	time_ = std::string(buffer);
	//std::cout << time_ << " - ";

	return time_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Logger::afficherCollision(std::string objet)
///
/// Cette fonction affiche les collisions de la rondelle.
///
/// @param[in] objet : L'objet entre en collision avec la rondelle.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Logger::afficherCollision(std::string objet) {
	if (enableCollision_ && activer_) {
		afficherTemps();
		std::transform(objet.begin(), objet.end(), objet.begin(), ::toupper);
		//std::cout << "Collision : " << objet << std::endl;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Logger::afficherVitesse(float vitesse)
///
/// Cette fonction affiche la vitesse de la rondelle apres chaque collision.
///
/// @param[in] vitesse : La vitesse finale de la rondelle apres une collision.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Logger::afficherVitesse(float vitesse) {
	if (enableSpeed_  && activer_) {
		afficherTemps();
		//std::cout << "Vitesse : " << vitesse << std::endl;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Logger::afficherLumiere(int lumType)
///
/// Cette fonction affiche si chaque type de lumiere s'active ou se desactive.
///
/// @param[in] lumType : Le type de lumiere.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Logger::afficherLumiere(int lumType) {
	if (enableLight_  && activer_) {
		afficherTemps();

		if (lumType == 0) {
			toggleLightsA_ = !toggleLightsA_;
			//std::cout << "Lumiere ambiante " << ((toggleLightsA_) ? "ouverte" : "fermee") << std::endl;
		}
		else if (lumType == 1) {
			toggleLightsB_ = !toggleLightsB_;
			//std::cout << "Lumiere directionnelle " << ((toggleLightsB_) ? "ouverte" : "fermee") << std::endl;
		}
		else if (lumType == 2) {
			toggleLightsC_ = !toggleLightsC_;
			//std::cout << "Lumieres spots " << ((toggleLightsC_) ? "ouverte" : "fermee") << std::endl;
		}	
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Logger::afficherLumiere(int lumType, bool ouverte)
///
/// Cette fonction affiche si chaque type de lumiere s'active ou se desactive.
///
/// @param[in] lumType : Le type de lumiere.
/// @param[in] ouverte : L'etat de la lumiere lumType.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Logger::afficherLumiere(int lumType, bool ouverte) {
	if (enableLight_  && activer_) {
		afficherTemps();

		if (lumType == 0) {
			toggleLightsA_ = ouverte;
			//std::cout << "Lumiere ambiante " << ((toggleLightsA_) ? "ouverte" : "fermee") << std::endl;
		}
		else if (lumType == 1) {
			toggleLightsB_ = ouverte;
			//std::cout << "Lumiere directionnelle " << ((toggleLightsB_) ? "ouverte" : "fermee") << std::endl;
		}
		else if (lumType == 2) {
			toggleLightsC_ = ouverte;
			//std::cout << "Lumieres spots " << ((toggleLightsC_) ? "ouverte" : "fermee") << std::endl;
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void setLogger(bool enableCollision, bool enableSpeed, bool enableLight, bool enablePortal)
///
/// Cette fonction connecte les checkbox d'affichage de debogages avec les fonctions ci-haut.
///
/// @param[in] enableCollision : Active ou desactive l'affichage des collisions.
/// @param[in] enableSpeed : Active ou desactive l'affichage de la vitesse.
/// @param[in] enableLight : Active ou desactive l'affichage de l'eclairage.
/// @param[in] enablePortal : Active ou desactive l'affichage des portails.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Logger::setLogger(bool enableCollision, bool enableSpeed, bool enableLight, bool enablePortal) {
	enableCollision_ = enableCollision;
	enableSpeed_ = enableSpeed;
	enableLight_ = enableLight;
	enablePortal_ = enablePortal;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Logger::activer(bool activer)
///
/// Cette fonction permet d'activer / desactiver le logger
///
/// @param[in] activer : Active ou desactive le logger
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Logger::activer(bool activer) {
	activer_ = activer;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool Logger::obtenirDebugPortal() const 
///
/// Cette fonction permet d'obtenir l'état de l'affichage du debug des
/// portails.
///
/// @return État de l'affichage du debug des portails.
///
////////////////////////////////////////////////////////////////////////
bool Logger::obtenirDebugPortal() const {
	return enablePortal_ && activer_;
}
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
