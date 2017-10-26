///////////////////////////////////////////////////////////////////////////////
/// @file Audio.cpp
/// @author Anthony Abboud
/// @date 2016-11-22
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "Audio.h"
#include "glm\glm.hpp"

/// Pointeur vers l'instance unique de la classe.
Audio* Audio::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn Audio* Audio::obtenirInstance()
///
/// Cette fonction retourne un pointeur vers l'instance unique de la
/// classe.  Si cette instance n'a pas été créée, elle la crée.  Cette
/// création n'est toutefois pas nécessairement "thread-safe", car
/// aucun verrou n'est pris entre le test pour savoir si l'instance
/// existe et le moment de sa création.
///
/// @return Un pointeur vers l'instance unique de cette classe.
///
////////////////////////////////////////////////////////////////////////
Audio* Audio::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new Audio();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Audio::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void Audio::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn Audio::Audio()
///
/// Constructeur.
///
/// @return Aucune (Constructeur).
///
////////////////////////////////////////////////////////////////////////
Audio::Audio()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn Audio::~Audio()
///
/// Destructeur.
///
/// @return Aucune (Destructeur).
///
////////////////////////////////////////////////////////////////////////
Audio::~Audio() {
	result = soundMaillet->release();
	result = soundBut->release();
	result = soundMur->release();
	result = soundPortail->release();
	result = soundBooster->release();
	result = musique->release();
	result = system->release();

	if (instance_ != nullptr) {
		libererInstance();
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Audio::loadSounds() 
///
/// Charge tous les sons
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Audio::loadSounds() {
	result = FMOD::System_Create(&system);

	result = system->init(100, FMOD_INIT_NORMAL, NULL);

	result = system->createSound("media/audio/effets/CollisionMaillet.mp3", FMOD_DEFAULT | FMOD_LOOP_OFF, 0, &soundMaillet);
	result = system->createSound("media/audio/effets/But.mp3", FMOD_DEFAULT | FMOD_LOOP_OFF, 0, &soundBut);
	result = system->createSound("media/audio/effets/CollisionMur.mp3", FMOD_DEFAULT | FMOD_LOOP_OFF, 0, &soundMur);
	result = system->createSound("media/audio/effets/CollisionPortail.mp3", FMOD_DEFAULT | FMOD_LOOP_OFF, 0, &soundPortail);
	result = system->createSound("media/audio/effets/CollisionBooster.mp3", FMOD_DEFAULT | FMOD_LOOP_OFF, 0, &soundBooster);
	result = system->createSound("media/audio/musique/Gameplay.mp3", FMOD_DEFAULT | FMOD_LOOP_NORMAL, 0, &musique);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void Audio::playMusic(bool quickPlay)
///
/// Joue la musique de fond en mode Partie Rapide seulement
///
/// @return Aucune (Destructeur).
///
////////////////////////////////////////////////////////////////////////
void Audio::playMusic(bool quickPlay) {
	try {

		if (quickPlay) {
			result = system->playSound(FMOD_CHANNEL_FREE, musique, false, &channelMusic);
			channelMusic->setVolume(0.2);
			system->update();
		}
		else {
			result = channelMusic->stop();
			system->update();
		}
	}
	catch (...)
	{
		// dans le cas qu'il n'y a pas de carte de son, on ignore le son
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Audio::playSound(int sound)
///
/// Joue un son specifique
///
/// @return Aucune (Destructeur).
///
////////////////////////////////////////////////////////////////////////
void Audio::playSound(int sound, float volume) {
	try
	{

		switch (sound) {
		case SOUND_MAILLET:
			result = system->playSound(FMOD_CHANNEL_FREE, soundMaillet, false, &channel);
			break;
		case SOUND_BUT:
			result = system->playSound(FMOD_CHANNEL_FREE, soundBut, false, &channel);
			break;
		case SOUND_MUR:
			result = system->playSound(FMOD_CHANNEL_FREE, soundMur, false, &channel);
			break;
		case SOUND_PORTAIL:
			result = system->playSound(FMOD_CHANNEL_FREE, soundPortail, false, &channel);
			break;
		case SOUND_BOOSTER:
			result = system->playSound(FMOD_CHANNEL_FREE, soundBooster, false, &channel);
			break;
		default:
			break;
		}
		channel->setVolume(glm::clamp(volume, 0.0f, 1.0f));
		system->update();
	}
	catch (...)
	{
		// dans le cas qu'il n'y a pas de carte de son, on ignore le son
	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////