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
	/// @brief Classe qui s'occupe de g�rer la lumi�re
	///
	/// @author Nam Lesage
	/// @date 2016-11-12
	///////////////////////////////////////////////////////////////////////////
	class LightManager {
	public:
		/// Obtient l'instance unique de la classe.
		static LightManager* obtenirInstance();
		/// Lib�re l'instance unique de la classe.
		static void libererInstance();
		
		/// Ajoute les lumieres par default
		void usePresetLighting();
		/// Anime les lumi�res
		void animer(float temps) const;

		/// Ajoute une lumiere a la scene
		Light* createLight(const LightParameters& parameters);
		/// Supprime une lumi�re 
		void deleteLight(Light* light);

		/// Ouvre/Ferme la lumi�re selon le type
		void toggleLight(LightType type);
		/// Ouvre/Ferme la lumi�re selon l'index
		void toggleLight(unsigned int index);
		/// Ouvre/Ferme la lumi�re selon l'index et l'�tat
		void setLight(LightType type, bool ouverte);
		/// Ouvre/Ferme la lumi�re selon l'index et l'�tat
		void setLight(unsigned int index, bool ouverte);
		/// Ouvre/Ferme la lumi�re ambiante
		void toggleAmbiant();
		/// Ouvre/Ferme la lumi�re ambiante selon l'�tat
		void setAmbiant(bool ouverte);

		/// Obtient l'�tat de la lumi�re ambiante
		bool obtenirAmbiantState();
		/// Obtient les lumi�res
		std::vector<Light*> getLights() const;
		/// Obtient le nombre de lumi�res
		int getLightsCount() const;

	protected:
		/// Constructeur vide d�clar� protected.
		LightManager();
		/// Destructeur vide d�clar� protected.
		~LightManager();

	private:
		/// Instance unique de la classe.
		static LightManager* instance_;
		
		/// Sources de lumiere
		std::vector<Light*> lights_;
		/// �tat de la lumi�re ambiante
		bool disableAmbiant_;

		// Index des ubos
		/// Index disponible pour les ubos
		std::priority_queue<unsigned int, std::vector<unsigned int>, std::greater<unsigned int>> unusedBufferIndex_;
		/// R�cup�re un index
		int getIndex();
		/// Rel�che un index
		void releaseIndex(unsigned int index);
	};
}


#endif // __LIGHTMANAGER_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


