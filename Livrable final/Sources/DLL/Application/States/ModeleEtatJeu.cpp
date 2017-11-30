///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatJeu.cpp
/// @author Anthony Abboud
/// @date 2016-10-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "ModeleEtatJeu.h"

#include "FacadeModele.h"
#include "Vue.h"
#include "ArbreRenduINF2990.h"

#define ALIGN_LEFT 0
#define ALIGN_TOP 0
#define ALIGN_CENTER 1
#define ALIGN_RIGHT 2
#define ALIGN_BOT 2
#define PADDING1 5
#define PADDING2 10
#define PADDING3 15
#define PADDING4 20
#define PADDING8 40
#define BORDER1 3
#define BORDER2 6
#define TIME_BEFORE_START 2
#define MAX_NAME_LENGHT 12

/// Pointeur vers l'instance unique de la classe.
ModeleEtatJeu* ModeleEtatJeu::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatJeu* ModeleEtatJeu::obtenirInstance()
///
/// Cette fonction retourne l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatJeu* ModeleEtatJeu::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatJeu();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}

ModeleEtatJeu::OpponentType ModeleEtatJeu::currentOpponentType() const
{
	return currentOpponentType_;
}

void ModeleEtatJeu::setCurrentOpponentType(const OpponentType currentOpponentType)
{
	currentOpponentType_ = currentOpponentType;
}

ModeleEtatJeu::OnlineClientType ModeleEtatJeu::currentOnlineClientType() const
{
	return onlineClientType_;
}

void ModeleEtatJeu::setCurrentOnlineClientType(const OnlineClientType currentOnlineClientType_)
{
	onlineClientType_ = currentOnlineClientType_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatJeu::ModeleEtatJeu()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatJeu::ModeleEtatJeu()
	: ModeleEtat() {
	speedMailletZ_ = 0;
	speedMailletX_ = 0;

	audiMB32_ = new Text2D(32, "media/fonts/audimatMonoB.ttf");
	panel_ = new Panel2D();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatJeu::~ModeleEtatJeu()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatJeu::~ModeleEtatJeu() {
	delete panel_;
	delete audiMB32_;
	panel_ = nullptr;
	audiMB32_ = nullptr;

	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::playerMouseMove(int x, int y) 
///
/// Évènement appelé lorsque la souris bouge. S'occupe de déplacer le
/// maillet du joueur 1 lorsque le jeu n'est pas en pause.
///
/// @param x[in] : position de la souris en x
/// @param y[in] : position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::playerMouseMove(int x, int y) {
	ModeleEtat::playerMouseMove(x, y);

	if (!gamePaused_ && gameStarted_ && !gameEnded_) {
		glm::dvec3 mousePos;
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, mousePos);
		tryNewPosition(maillet1_, mousePos, segmentsLeft_);
	}
}

void ModeleEtatJeu::opponentMouseMove(int x, int y)
{
	if (!gamePaused_ && gameStarted_ && !gameEnded_) {
		glm::dvec3 mousePos;
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(x, y, mousePos);
		tryNewPosition(maillet2_, mousePos, segmentsRight_);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatJeu::tryNewPosition(NoeudMaillet* maillet, glm::vec3 position, std::vector<utilitaire::Segment2D>& segments)
///
/// Essaie de placer le maillet à la position spécifiée
///
/// @param[in] maillet : Maillet à déplacer
/// @param[in] position : Position désirée pour le déplacement
/// @param[in] segments : Segments de la table à gérer
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::tryNewPosition(NoeudMaillet* maillet, glm::vec3 position, std::vector<utilitaire::Segment2D>& segments) {
	maillet->savePosition();

	// Try placing the object under mouse position
	VisiteurSurTable visiteurTable;
	maillet->deplacer(position);
	maillet->accepterVisiteur(&visiteurTable);

	// If fails, try remote tracking...
	if (!visiteurTable.sontSurTable()) {
		float distance = INFINITY;
		utilitaire::Segment2D segment;

		// Find closest segment
		for (unsigned int i = 0; i < segments.size(); i++) {
			float newDistance = segments[i].distance(position);
			if (newDistance < distance) {
				distance = newDistance;
				segment = segments[i];
			}
		}

		// Find position on one side of the segment
		glm::dvec3 newPosition = segment.projectedPos(position);
		maillet->deplacer(segment.correctedPosA(newPosition));

		// Check if in table, else find position on the other side of the segment
		maillet->accepterVisiteur(&visiteurTable);
		if (!visiteurTable.sontSurTable())
			maillet->deplacer(segment.correctedPosB(newPosition));

		// If still not in the table, revert position
		maillet->accepterVisiteur(&visiteurTable);
		if (!visiteurTable.sontSurTable())
			maillet->revertPosition();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::escape() 
///
/// Évènement appelé lorsque la touche escape est appuyée. Met le jeu 
/// en pause pour l'état modele état jeu.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::escape() {
	if (gameStarted_ && !gameEnded_) {
		gamePaused_ = !gamePaused_;

		if (gamePaused_) {
			RigidBody::disablePhysic();
			pausedClock_ = clock();
			savedSpeedRondelle_ = rondelle_->obtenirRigidBody()->obtenirVitesse();
			rondelle_->obtenirRigidBody()->assignerVitesse(glm::vec3(0));
		}
		else {
			RigidBody::enablePhysic();
			initClock_ += clock() - pausedClock_;
			rondelle_->obtenirRigidBody()->assignerVitesse(savedSpeedRondelle_);
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::calculateSegments() 
///
/// Calcule les équations de droite des segments de la table
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::calculateSegments() {
	segmentsLeft_.clear(); segmentsRight_.clear();
	float rayon = maillet1_->obtenirCollider().rayon;

	utilitaire::Segment2D segmentLUpper = (pTopLeft_.z > pMidLeft_.z) ? utilitaire::Segment2D(pMidLeft_, pTopLeft_, rayon) : utilitaire::Segment2D(pTopLeft_, pMidLeft_, rayon);
	utilitaire::Segment2D segmentLLower = (pBotLeft_.z > pMidLeft_.z) ? utilitaire::Segment2D(pMidLeft_, pBotLeft_, rayon) : utilitaire::Segment2D(pBotLeft_, pMidLeft_, rayon);
	segmentsLeft_.push_back(segmentLUpper); segmentsLeft_.push_back(segmentLLower);

	utilitaire::Segment2D segmentRUpper = (pTopRight_.z > pMidRight_.z) ? utilitaire::Segment2D(pMidRight_, pTopRight_, rayon) : utilitaire::Segment2D(pTopRight_, pMidRight_, rayon);
	utilitaire::Segment2D segmentRLower = (pBotRight_.z > pMidRight_.z) ? utilitaire::Segment2D(pMidRight_, pBotRight_, rayon) : utilitaire::Segment2D(pBotRight_, pMidRight_, rayon);
	segmentsRight_.push_back(segmentRUpper); segmentsRight_.push_back(segmentRLower);

	utilitaire::Segment2D segmentULeft = utilitaire::Segment2D(pTopLeft_, pTopMid_, rayon);
	utilitaire::Segment2D segmentURight = utilitaire::Segment2D(pTopMid_, pTopRight_, rayon);
	segmentsLeft_.push_back(segmentULeft); segmentsRight_.push_back(segmentURight);

	utilitaire::Segment2D segmentDLeft = utilitaire::Segment2D(pBotLeft_, pBotMid_, rayon);
	utilitaire::Segment2D segmentDRight = utilitaire::Segment2D(pBotMid_, pBotRight_, rayon);
	segmentsLeft_.push_back(segmentDLeft); segmentsRight_.push_back(segmentDRight);

	utilitaire::Segment2D segmentCenter = utilitaire::Segment2D(pTopMid_, pBotMid_, rayon);
	segmentsLeft_.push_back(segmentCenter); segmentsRight_.push_back(segmentCenter);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu:moveMaillet() 
///
/// Cette fonction déplace le maillet du joueur 2.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::moveMaillet() {
	if (!gamePaused_ && gameStarted_ && !gameEnded_) {
		glm::vec3 position = maillet2_->obtenirPositionRelative();

		// Local Player
		if (currentOpponentType_==LOCAL_PLAYER) {
			if (speedMailletX_ != 0 && speedMailletZ_ != 0)
				position += glm::vec3(copysign(sqrt(pow(speedMailletX_, 2.0f) / 2.0f), speedMailletX_), 0, copysign(sqrt(pow(speedMailletZ_, 2.0f) / 2.0f), speedMailletZ_));
			else
				position += glm::vec3(speedMailletX_, 0, speedMailletZ_);

			tryNewPosition(maillet2_, position, segmentsRight_);
		}
		// Virtual player
		else if(currentOpponentType_==VIRTUAL_PLAYER){
			if (rondelle_->obtenirPositionRelative().z < 0)
				initiateOffense_ = false;
			else if (rondelle_->obtenirPositionRelative().z > -2 && rondelle_->obtenirPositionRelative().z < 2 && rondelle_->obtenirPositionRelative() != glm::vec3(0)) {
				if ((rand() % aiPassivity_ + 1) == aiPassivity_)
					initiateOffense_ = true;
			}

			if (rondelle_->obtenirRigidBody()->obtenirVitesse().z < 15)
				initiateOffense_ = true;

			glm::vec3 vitesseRondelle = rondelle_->obtenirRigidBody()->obtenirVitesse();
			int testCase = jv_.testCase(position, rondelle_->obtenirPositionRelative(), vitesseRondelle, initiateOffense_);
			float X = jv_.xSpeed(position, rondelle_->obtenirPositionRelative(), pMidRight_, testCase, aiSpeed_, aiPassivity_);
			float Z = jv_.zSpeed(position, rondelle_->obtenirPositionRelative(), pMidRight_, testCase, aiSpeed_, aiPassivity_);

			position += glm::vec3(copysign(sqrt(pow(X, 2.0f) / 2.0f), X), 0, copysign(sqrt(pow(Z, 2.0f) / 2.0f), Z));

			NoeudMaillet* mailletToMove = maillet2_;
			if (onlineClientType_ != ModeleEtatJeu::OFFLINE_GAME)
			{
				if (currentOpponentType_ == ModeleEtatJeu::SLAVE) {
					mailletToMove = maillet1_;
				}
			}
			tryNewPosition(mailletToMove, position, segmentsRight_);
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtatJeu::isGameOver(const int& neededGoals)
///
/// Vérifie si la partie est terminé selon le nombre de buts 
/// nécessaires pour gagner.
///
/// @param[in] neededGoals : Nombre de but requis pour la partie.
///
/// @return Vrai si la partie est terminée
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtatJeu::isGameOver(const int& neededGoals) {
	if (scorePlayer1_ == neededGoals || scorePlayer2_ == neededGoals) {
		gameEnded_ = true;
		RigidBody::disablePhysic();
		return true;
	}

	return false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatJeu::getGameScore(float score[]) const
///
/// Retourne le pointage de la partie en cours.
///
/// @param[in,out] score : Score de la partie (pour les 2 joueurs)
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::getGameScore(int score[]) const {
	score[0] = scorePlayer1_;
	score[1] = scorePlayer2_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void resetGame()
///
/// Réinitialise la partie. Ceci reninitialise les positions des objets
/// sur la table et trouve la position des points de flexion des 
/// bordures.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::resetGame() {
	gamePaused_ = false;
	gameEnded_ = false;

	if (testMode_) {
		RigidBody::enablePhysic();
		gameStarted_ = true;
	}
	else {
		RigidBody::disablePhysic();
		gameStarted_ = false;
	}

	// Deselect everything
	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	arbre->deselectionnerTout();

	// Rondelle
	rondelle_ = (NoeudRondelle*)(FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_RONDELLE));

	// Maillets
	for (auto it = arbre->obtenirIterateurBegin(); it != arbre->obtenirIterateurEnd(); it++) {
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_MAILLET) {
			NoeudMaillet* node = (NoeudMaillet*)(*it);
			glm::vec3 pos = node->obtenirPositionRelative();
			// Player 1
			if (pos.z < 0)
				maillet1_ = node;
			// Player 2
			else if (pos.z > 0)
				maillet2_ = node;
		}
	}

	// Sommets internes
	NoeudTable* table = (NoeudTable*)(FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_TABLE));
	for (auto it = table->obtenirIterateurBegin(); it != table->obtenirIterateurEnd(); it++) {
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL) {
			NoeudPointControl* node = (NoeudPointControl*)(*it);
			glm::vec3 pos = node->obtenirPositionRelative();
			// Left
			if (pos.x == 0 && pos.z < 0)
				pMidLeft_ = node->getInternalPosition();
			else if (pos.x > 0 && pos.z < 0)
				pTopLeft_ = node->getInternalPosition();
			else if (pos.x < 0 && pos.z < 0)
				pBotLeft_ = node->getInternalPosition();
			// Right
			else if (pos.x == 0 && pos.z > 0)
				pMidRight_ = node->getInternalPosition();
			else if (pos.x > 0 && pos.z > 0)
				pTopRight_ = node->getInternalPosition();
			else if (pos.x < 0 && pos.z > 0)
				pBotRight_ = node->getInternalPosition();
			// Top
			else if (pos.x > 0 && pos.z == 0)
				pTopMid_ = node->getInternalPosition();
			// Bottom
			else if (pos.x < 0 && pos.z == 0)
				pBotMid_ = node->getInternalPosition();
		}
	}

	calculateSegments();
	
	miseAuJeu();
	// Reset score
	scorePlayer1_ = 0;
	scorePlayer2_ = 0;
	// Reset time
	initClock_ = clock();
	pausedClock_ = clock();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::aiActiveProfile(int speed, int passivity)
///
/// Charge les valeurs de vitesse et de passivité du AI
///
/// @param[in] speed : Vitesse du AI
/// @param[in] passivity : Niveau de passivité du AI
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::aiActiveProfile(int speed, int passivity) {
	aiSpeed_ = speed;
	aiPassivity_ = passivity;
}


void ModeleEtatJeu::player1Goal()
{
	scorePlayer1_++;
	//if (currentOpponentType_ == OpponentType::ONLINE_PLAYER && onlineClientType_ == OnlineClientType::MASTER)
	if (onlineClientType_ == OnlineClientType::MASTER)
	{
		goalCallback_(1);

	}

}

void ModeleEtatJeu::player2Goal()
{
	scorePlayer2_++;
	//if (currentOpponentType_ == OpponentType::ONLINE_PLAYER && onlineClientType_ == OnlineClientType::MASTER)
	if (onlineClientType_ == OnlineClientType::MASTER)
	{
		goalCallback_(2);
	}
};

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::miseAuJeu()
///
/// Repositionne les maillets et la rondelle pour la mise au jeu
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::miseAuJeu() {
	rondelle_->deplacer(glm::vec3(0, 0, 0));
	maillet1_->deplacer(glm::vec3(0, 0, fmin(pMidLeft_.z / 2, 0)));
	maillet2_->deplacer(glm::vec3(0, 0, fmax(pMidRight_.z / 2, 0)));

	rondelle_->obtenirRigidBody()->assignerVitesse(glm::vec3(0));
	savedSpeedRondelle_ = glm::vec3(0);

	speedMailletZ_ = 0;
	speedMailletX_ = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::afficher()
///
/// S'occupe d'afficher le texte openGL pour les noms, le score et le
/// timer du jeu.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::afficher() {
	GLint Cloture[4];
	glGetIntegerv(GL_VIEWPORT, Cloture);

	if(!gameStarted_ || gamePaused_ || gameEnded_)
		panel_->drawElement(0, 0, Cloture[2], Cloture[3], BLACK_ALPHA50, ALIGN_LEFT, ALIGN_TOP);

	if (!gameStarted_) {
		if (((clock() - initClock_) / CLOCKS_PER_SEC) < TIME_BEFORE_START) {
			std::string matchPlayers = player1Name_ + " VS " + player2Name_;
			panel_->drawElement((Cloture[2] / 2), (Cloture[3] / 2), audiMB32_->textWidth(matchPlayers) + PADDING8, (audiMB32_->getFontHeight() * 3), DARK_VIOLET, ALIGN_CENTER, ALIGN_CENTER);
			panel_->drawElement((Cloture[2] / 2), (Cloture[3] / 2), audiMB32_->textWidth(matchPlayers) + PADDING8 - BORDER2, (audiMB32_->getFontHeight() * 3) - BORDER2, GREY, ALIGN_CENTER, ALIGN_CENTER);
			audiMB32_->renderText2D(matchPlayers, (Cloture[2] / 2), (Cloture[3] / 2) - (audiMB32_->getFontHeight() / 2), WHITE, ALIGN_CENTER);
		}
		else {
			gameStarted_ = true;
			RigidBody::enablePhysic();
			initClock_ = clock();
		}
	}
	else if (gamePaused_) {
		// Draw pause panel
		panel_->drawElement((Cloture[2] / 2), (Cloture[3] / 2), audiMB32_->textWidth("EN PAUSE") + PADDING8, (audiMB32_->getFontHeight() * 3), DARK_VIOLET, ALIGN_CENTER, ALIGN_CENTER);
		panel_->drawElement((Cloture[2] / 2), (Cloture[3] / 2), audiMB32_->textWidth("EN PAUSE") + PADDING8 - BORDER2, (audiMB32_->getFontHeight() * 3) - BORDER2, GREY, ALIGN_CENTER, ALIGN_CENTER);
		audiMB32_->renderText2D("EN PAUSE", (Cloture[2] / 2), (Cloture[3] / 2) - (audiMB32_->getFontHeight() / 2), WHITE, ALIGN_CENTER);
	}
	else if(!testMode_ && !gameEnded_) {
		// Find current game time and score
		std::string timeSec = std::to_string(((clock() - initClock_) / CLOCKS_PER_SEC) % 60); if (timeSec.length() == 1) timeSec.insert(0, "0");
		std::string timeMin = std::to_string(((clock() - initClock_) / CLOCKS_PER_SEC) / 60); if (timeMin.length() == 1) timeMin.insert(0, "0");
		std::string time = std::to_string(scorePlayer1_) + "|" + timeMin + ":" + timeSec + "|" + std::to_string(scorePlayer2_);

		// Draw player 1 name and panel
		panel_->drawElement(0, Cloture[3], audiMB32_->textWidth(player1Name_) + PADDING3, (audiMB32_->getFontHeight() * 2), player1Color_, ALIGN_LEFT, ALIGN_BOT);
		panel_->drawElement(0, Cloture[3], audiMB32_->textWidth(player1Name_) + PADDING3 - BORDER1, (audiMB32_->getFontHeight() * 2) - BORDER1, GREY, ALIGN_LEFT, ALIGN_BOT);
		audiMB32_->renderText2D(player1Name_, PADDING1, Cloture[3] - audiMB32_->getFontHeight() - PADDING2, player1Color_, ALIGN_LEFT);

		// Draw player 2 name and panel
		panel_->drawElement(Cloture[2], Cloture[3], audiMB32_->textWidth(player2Name_) + PADDING3, (audiMB32_->getFontHeight() * 2), player2Color_, ALIGN_RIGHT, ALIGN_BOT);
		panel_->drawElement(Cloture[2], Cloture[3], audiMB32_->textWidth(player2Name_) + PADDING3 - BORDER1, (audiMB32_->getFontHeight() * 2) - BORDER1, GREY, ALIGN_RIGHT, ALIGN_BOT);
		audiMB32_->renderText2D(player2Name_, Cloture[2] - PADDING1, Cloture[3] - audiMB32_->getFontHeight() - PADDING2, player2Color_, ALIGN_RIGHT);

		// Draw time, score and panel
		panel_->drawElement((Cloture[2] / 2), Cloture[3], audiMB32_->textWidth(time) + PADDING4, (audiMB32_->getFontHeight() * 2), WHITE, ALIGN_CENTER, ALIGN_BOT);
		panel_->drawElement((Cloture[2] / 2), Cloture[3], audiMB32_->textWidth(time) + PADDING4 - BORDER2, (audiMB32_->getFontHeight() * 2) - BORDER1, GREY, ALIGN_CENTER, ALIGN_BOT);
		audiMB32_->renderText2D(time, Cloture[2] / 2, Cloture[3] - audiMB32_->getFontHeight() - PADDING2, WHITE, ALIGN_CENTER);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::setPlayerNames(std::string player1, std::string player2)
///
/// Change les noms des joueurs, afin qu'ils puissent être affichées 
/// par la suite.
///
/// @param[in] player1 : Nom du joueur 1
/// @param[in] player2 : Nom du joueur 2
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::setPlayerNames(std::string player1, std::string player2) {
	player1Name_ = (player1.length() > 12) ? player1.substr(0, 12) : player1;
	player2Name_ = (player2.length() > 12) ? player2.substr(0, 12) : player2;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatJeu::setPlayerColors(glm::vec3 player1, glm::vec3 player2)
///
/// Change les couleurs des joueurs, afin qu'elles puissent être affichées 
/// par la suite.
///
/// @param[in] player1 : Couleur du joueur 1
/// @param[in] player2 : Couleur du joueur 2
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatJeu::setPlayerColors(glm::vec4 player1, glm::vec4 player2) {
	player1Color_ = player1 / 255.0f;
	player2Color_ = player2 / 255.0f;
}

void ModeleEtatJeu::setLocalPlayerSkin(std::string skinkName) {
	maillet1_->setCurrentTexture(skinkName);
}
void ModeleEtatJeu::setOpponentPlayerSkin(std::string skinkName) {
	maillet2_->setCurrentTexture(skinkName);
}

void ModeleEtatJeu::setLocalPlayerSkinToDefault()
{
	maillet1_->freeCurrentTexture();
}

void ModeleEtatJeu::setOpponentPlayerSkinToDefault()
{
	maillet2_->freeCurrentTexture();
}

void ModeleEtatJeu::setGameEnded()
{
	gameEnded_ = true;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
