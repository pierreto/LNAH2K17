#pragma once
#include "ModeleEtat.h"
#include "Segment2D.h"
#include "Text2D.h"
#include "Panel2D.h"

class ModeleEtatJeuOnline : public ModeleEtat
{
public:
	/// Obtient l'instance unique de la classe.
	static ModeleEtatJeuOnline* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	// Fonctions gérant les entrées de l'utilisateur
	/// Évènement appelé lorsque la souris bouge
	virtual void mouseMove(int x, int y);
	virtual void mouseMoveOpponent(int x, int y);

	virtual void escape();
	virtual void afficher();
	void moveMaillet();
	void resetGame();
	void miseAuJeu();

	void setPlayerNames(std::string player1, std::string player2);
	void setPlayerColors(glm::vec4 player1, glm::vec4 player2);
	void getGameScore(int score[]) const;
	bool isGameOver(const int& neededGoals);
	void aiActiveProfile(int speed, int passivity);

	void toggleTestMode(bool isActive) { testMode_ = isActive; };
	void setSpeedXMaillet(float speedZ) { speedMailletZ_ = speedZ; };
	void setSpeedYMaillet(float speedX) { speedMailletX_ = speedX; };
	void aiStatus(bool enabled) { aiIsActive_ = enabled; };
	void player1Goal() { scorePlayer1_++; };
	void player2Goal() { scorePlayer2_++; };
	bool isGameStarted() { return gameStarted_; };

private:
	/// Constructeur.
	ModeleEtatJeuOnline();
	/// Destructeur.
	virtual ~ModeleEtatJeuOnline();
	/// Pointeur vers l'instance unique de la classe.
	static ModeleEtatJeuOnline* instance_;
	void calculateSegments();
	void tryNewPosition(NoeudMaillet* maillet, glm::vec3 position, std::vector<utilitaire::Segment2D>& segments);

	bool gameStarted_;
	bool gamePaused_;
	bool gameEnded_;

	bool aiIsActive_;
	int aiSpeed_;
	int aiPassivity_;
	bool initiateOffense_;
	bool testMode_;
//	JoueurVirtuel jv_;


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
};
