///////////////////////////////////////////////////////////////////////////////
/// @file VirtualPlayer.h
/// @author Francis Dalp�
/// @date 2016-10-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "Utilitaire.h"
#include "GL/glew.h"
#include "glm\glm.hpp"
#include <vector>
#include "NoeudAbstrait.h"
#include "ArbreRenduINF2990.h"
#include "../Application/FacadeModele.h"
#include "Segment2D.h"

///////////////////////////////////////////////////////////////////////////
/// @class JoueurVirtuel
/// @brief Classe de joueur virtuel qui contr�le le comportement du maillet
///		   adverse lorsqu'il est virtuel.
///
///        Le comportement du joueur virtuel est divis� en plusieurs sc�narios
///		   repr�sentant diff�rentes situations de jeu (par exemple, selon la 
///        position et la vitesse de la rondelle). Chaque sc�nario retourne un
///		   d�placement qui convient � la situation, pour ensuite �tre appliqu�
///		   au maillet dans le ModeleEtatJeu.
///
/// @author Francis Dalp�
/// @date 2016-11-01
///////////////////////////////////////////////////////////////////////////
class JoueurVirtuel
{
public:
	///Constructeur
	JoueurVirtuel();

	//Case 1:  where the puck is on the AI's side, between the mallet and the opposing net with a clear shot
	//Case 2:  where the puck is on the AI's side, between the mallet and its own net
	//Case 3: where the puck is on the AI's side, between the mallet and the opposing net with a wall blocking the direct shot
	//Case 4: where the puck is not on the AI's side

	///D�termine le sc�nario dans le lequel se retrouve le maillet
	int testCase(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 vitesseRondelle, bool initiateOffense);

	///Retourne le d�placement du maillet � effectuer en X en fonction de la situation.
	float xSpeed(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity);

	///Retourne le d�placement du maillet � effectuer en Y en fonction de la situation.
	float zSpeed(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity);
};
