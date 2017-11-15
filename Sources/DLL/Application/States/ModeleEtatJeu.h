///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatJeu.h
/// @author Anthony Abboud
/// @date 2016-10-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETATJEU_H__
#define __APPLICATION_STATES_MODELEETATJEU_H__

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtatJeu
/// @brief Classe concrète du patron State pour l'état du jeu
///
///        Cette classe représente l'état du jeu.
///
/// @author Anthony Abboud
/// @date 2016-10-19
///////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"
#include "../VirtualPlayer.h"
#include "Segment2D.h"
#include "Text2D.h"
#include "Panel2D.h"
#include <time.h>
#include "FacadeInterfaceNative.h"

class ModeleEtatJeu : public ModeleEtat
{
public:

	enum OpponentType{
		LOCAL_PLAYER=0,
		VIRTUAL_PLAYER=1,
		ONLINE_PLAYER=2
	};

	enum OnlineClientType {
		SLAVE = 0,
		MASTER = 1,
		OFFLINE_EDITION = 2,
		ONLINE_EDITION = 3
	};
	/// Obtient l'instance unique de la classe.
	static ModeleEtatJeu* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	// Fonctions gérant les entrées de l'utilisateur
	/// Évènement appelé lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	void opponentMouseMove(int i, int y);


	virtual void escape();
	virtual void afficher();
	void moveMaillet();
	void resetGame();
	void miseAuJeu();

	void setPlayerNames(std::string player1, std::string player2);
	void setPlayerColors(glm::vec4 player1, glm::vec4 player2);
	void setLocalPlayerSkin(std::string skinkName);
	void setOpponentPlayerSkin(std::string skinkName);
	void getGameScore(int score[]) const;
	bool isGameOver(const int& neededGoals);
	void aiActiveProfile(int speed, int passivity);

	void toggleTestMode(bool isActive) { testMode_ = isActive; }
	void setSpeedXMaillet(float speedZ) { speedMailletZ_ = speedZ; }
	void setSpeedYMaillet(float speedX) { speedMailletX_ = speedX; }
	void setLocalPlayerSkinToDefault();
	void setOpponentPlayerSkinToDefault();;
	void player1Goal();
	void player2Goal();

	bool isGameStarted() { return gameStarted_; };


	glm::vec3 getPlayerPosition() const { return maillet1_->obtenirPositionRelative();};
	glm::vec3 getOpponentPosition() const { return maillet2_->obtenirPositionRelative(); };
	glm::vec3 getPuckPosition() const { return rondelle_->obtenirPositionRelative(); };

	void setPlayerPosition(float* newPositions) const {  maillet1_->assignerPositionRelative(glm::make_vec3(newPositions)); };
	void setOpponentPosition(float* newPositions)  const {  maillet2_->assignerPositionRelative(glm::make_vec3(newPositions)); };
	void setPuckPosition(float* newPositions) const {  rondelle_->assignerPositionRelative(glm::make_vec3(newPositions)); };

	OpponentType currentOpponentType() const;
	void setCurrentOpponentType(const OpponentType currentOpponentType);

	OnlineClientType currentOnlineClientType() const;
	void setCurrentOnlineClientType(const OnlineClientType currentOnlineClientType_);
	void setOnGoalCallback(GoalCallback goalCallback) { goalCallback_ = goalCallback; };



	void setTransformEventCallback(TransformEventCallback callback){ transformEventCallback_ = callback; }
	TransformEventCallback getTransformEventCallback() { return transformEventCallback_; }

	void setDeleteEventCallback(DeleteEventCallback callback) { deleteEventCallback_=callback; }
	DeleteEventCallback getDeleteEventCallback() { return deleteEventCallback_; }

private:
	/// Constructeur.
	ModeleEtatJeu();
	/// Destructeur.
	virtual ~ModeleEtatJeu();
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatJeu* instance_;
	void calculateSegments();
	void tryNewPosition(NoeudMaillet* maillet, glm::vec3 position, std::vector<utilitaire::Segment2D>& segments);

	bool gameStarted_;
	bool gamePaused_;
	bool gameEnded_;

	int aiSpeed_;
	int aiPassivity_;
	bool initiateOffense_;
	bool testMode_;

	JoueurVirtuel jv_;

	std::vector<utilitaire::Segment2D> segmentsLeft_;
	std::vector<utilitaire::Segment2D> segmentsRight_;

	NoeudMaillet* maillet1_;
	NoeudMaillet* maillet2_;
	NoeudRondelle* rondelle_;

	glm::vec3 pTopLeft_;
	glm::vec3 pMidLeft_;
	glm::vec3 pBotLeft_;
	glm::vec3 pTopRight_;
	glm::vec3 pMidRight_;
	glm::vec3 pBotRight_;
	glm::vec3 pTopMid_;
	glm::vec3 pBotMid_;

	float speedMailletZ_;
	float speedMailletX_;
	int scorePlayer1_;
	int scorePlayer2_;
	glm::vec3 savedSpeedRondelle_;

	Text2D* audiMB32_;
	Panel2D* panel_;
	clock_t initClock_;
	clock_t pausedClock_;

	std::string player1Name_;
	std::string player2Name_;

	glm::vec4 player1Color_;
	glm::vec4 player2Color_;

	const glm::vec4 WHITE = glm::vec4(1.0f, 1.0f, 1.0f, 1.0f);
	const glm::vec4 GREY = glm::vec4(0.25f, 0.25f, 0.25f, 1.0f);
	const glm::vec4 BLACK_ALPHA50 = glm::vec4(0.0f, 0.0f, 0.0f, 0.5f);
	const glm::vec4 DARK_VIOLET = glm::vec4(0.58f, 0.0f, 0.83f, 1.0f);

	OpponentType currentOpponentType_;
	OnlineClientType onlineClientType_;
	GoalCallback goalCallback_;

	TransformEventCallback transformEventCallback_;
	DeleteEventCallback deleteEventCallback_;

};


#endif // __APPLICATION_STATES_MODELEETATJEU_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

