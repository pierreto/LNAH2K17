///////////////////////////////////////////////////////////////////////////////
/// @file Audio.h
/// @author Anthony Abboud
/// @date 2016-11-18
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __AUDIO_AUDIO_H__
#define __AUDIO_AUDIO_H__

#include <iostream>
#include "fmod.h"
#include "fmod.hpp"
#include "Windows.h"
#include <fstream>

#define DEFAULT_SOUND_VOLUME 0.2
#define SOUND_MAILLET 1
#define SOUND_BUT 2
#define SOUND_MUR 3
#define SOUND_PORTAIL 4
#define SOUND_BOOSTER 5


///////////////////////////////////////////////////////////////////////////
/// @class Audio
/// @brief Permet de  traiter les effets sonores et la musique du jeu.
///
/// @author Anthony Abboud
/// @date 2016-11-18
///////////////////////////////////////////////////////////////////////////
class Audio
{
public:
	/// Creer l'instance
	static Audio* obtenirInstance();

	/// Liberer l'instance
	static void libererInstance();

	/// Constructeur.
	Audio();
	/// Destructeur.
	~Audio();
	/// Charge les sons
	void loadSounds();
	/// Joue la musique de fond
	void playMusic(bool quickPlay);
	/// Joue un son specifique
	void playSound(int sound, float volume);

private :
	static Audio* instance_;

	FMOD::System     *system;
	FMOD::Sound      *soundMaillet, *soundBut, *soundMur, *musique, *soundPortail, *soundBooster;
	FMOD::Channel    *channel = 0, *channelMusic = 0;
	FMOD_RESULT       result;
};




#endif // __AUDIO_AUDIO_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////