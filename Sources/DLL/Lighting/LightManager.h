///////////////////////////////////////////////////////////////////////////
/// @file LightManager.h
/// @author Nam Lesage
/// @date 2016-11-12
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __LIGHTMANAGER_H__
#define __LIGHTMANAGER_H__

#include "GL/glew.h"
#include <string>
#include "Light.h"
#include <vector>
#include <queue>
#include <functional>     // std::greater

#define MAX_LIGHT_SOURCES 10

namespace light {
	///////////////////////////////////////////////////////////////////////////
	/// @class LightManager
	/// @brief Classe qui s'occupe de gérer la lumière
	///
	/// @author Nam Lesage
	/// @date 2016-11-12
	///////////////////////////////////////////////////////////////////////////
	class LightManager {
	public:
		/// Obtient l'instance unique de la classe.
		static LightManager* obtenirInstance();
		/// Libère l'instance unique de la classe.
		static void libererInstance();
		
		/// Ajoute les lumieres par default
		void usePresetLighting();
		/// Anime les lumières
		void animer(float temps) const;

		/// Ajoute une lumiere a la scene
		Light* createLight(const LightParameters& parameters);
		/// Supprime une lumière 
		void deleteLight(Light* light);

		/// Ouvre/Ferme la lumière selon le type
		void toggleLight(LightType type);
		/// Ouvre/Ferme la lumière selon l'index
		void toggleLight(unsigned int index);
		/// Ouvre/Ferme la lumière selon l'index et l'état
		void setLight(LightType type, bool ouverte);
		/// Ouvre/Ferme la lumière selon l'index et l'état
		void setLight(unsigned int index, bool ouverte);
		/// Ouvre/Ferme la lumière ambiante
		void toggleAmbiant();
		/// Ouvre/Ferme la lumière ambiante selon l'état
		void setAmbiant(bool ouverte);

		/// Obtient l'état de la lumière ambiante
		bool obtenirAmbiantState();
		/// Obtient les lumières
		std::vector<Light*> getLights() const;
		/// Obtient le nombre de lumières
		int getLightsCount() const;

	protected:
		/// Constructeur vide déclaré protected.
		LightManager();
		/// Destructeur vide déclaré protected.
		~LightManager();

	private:
		/// Instance unique de la classe.
		static LightManager* instance_;
		
		/// Sources de lumiere
		std::vector<Light*> lights_;
		/// État de la lumière ambiante
		bool disableAmbiant_;

		// Index des ubos
		/// Index disponible pour les ubos
		std::priority_queue<unsigned int, std::vector<unsigned int>, std::greater<unsigned int>> unusedBufferIndex_;
		/// Récupère un index
		int getIndex();
		/// Relâche un index
		void releaseIndex(unsigned int index);
	};
}


#endif // __LIGHTMANAGER_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


