///////////////////////////////////////////////////////////////////////////
/// @file Logger.h
/// @author Anthony Abboud
/// @date 2016-11-16
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __CONSOLE_LOGGER_H__
#define __CONSOLE_LOGGER_H__

#include "GL/glew.h"
#include <string>

///////////////////////////////////////////////////////////////////////////
/// @class Logger
/// @brief Classe pour afficher les debogages
///
/// @author Anthony Abboud
/// @date 2016-11-16
///////////////////////////////////////////////////////////////////////////
class Logger {
public:
	/// Obtient l'instance unique de la classe.
	static Logger* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	std::string afficherTemps();
	void afficherCollision(std::string objet);
	void afficherVitesse(float vitesse);
	void afficherLumiere(int lumType);
	void afficherLumiere(int lumType, bool lumEtat);
	void setLogger(bool enableCollision, bool enableSpeed, bool enableLight, bool enablePortal);
	void activer(bool activer);

	bool obtenirDebugPortal() const;

protected:
	/// Constructeur vide déclaré protected.
	Logger();
	/// Destructeur vide déclaré protected.
	~Logger();

private:
	/// Instance unique de la classe.
	static Logger* instance_;
	std::string time_;
	bool enableCollision_, enableSpeed_, enableLight_, enablePortal_;
	bool activer_;
	bool toggleLightsA_{ true };
	bool toggleLightsB_{ true };
	bool toggleLightsC_{ true };
};

#endif // __CONSOLE_DEBUGGERDISPLAY_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


