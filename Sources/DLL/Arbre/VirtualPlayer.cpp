///////////////////////////////////////////////////////////////////////////////
/// @file VirtualPlayer.cpp
/// @author Francis Dalpé
/// @date 2016-10-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VirtualPlayer.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn JoueurVirtuel::JoueurVirtuel()
///
/// Constructeur par défaut d'un joueur virtuel.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
JoueurVirtuel::JoueurVirtuel() {
}

////////////////////////////////////////////////////////////////////////
///
/// @fn int JoueurVirtuel::testCase(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 vitesseRondelle, bool initiateOffense)
///
/// Cette fonction permet de déterminer la situation dans laquelle se
/// retrouve le maillet contrôlé par le joueur virtuel.
///
/// @param[in] maillet : La position du maillet.
/// @param[in] rondelle : La position de la rondelle.
/// @param[in] vitesseRondelle : La vitesse de la rondelle.
/// @param[in] initiateOffense : Si le joueur virtuel peut effectuer une attaque.
///
/// @return Le cas à traiter par les fonctions xSpeed et zSpeed.
///
////////////////////////////////////////////////////////////////////////
int JoueurVirtuel::testCase(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 vitesseRondelle, bool initiateOffense) {
	if (rondelle.z <= 0)
		return 4; // Other side
	else if (rondelle.z > 0 && rondelle.z > maillet.z && rondelle.x <= 0)
		return 2; // Behind mallet, upper
	else if (rondelle.z > 0 && rondelle.z > maillet.z && rondelle.x > 0)
		return 3; // Behind mallet, lower
	else if (rondelle.z > 0 && rondelle.z <= maillet.z && vitesseRondelle.z > -100 && initiateOffense)
		return 1; // In front of mallet
	else
		return 4;

}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 getTarget(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity)
///
/// Cette fonction permet de déterminer l'endroit où le maillet veut se rendre,
/// selon le cas dans lequel il se trouve.
///
/// @param[in] maillet : La position du maillet.
/// @param[in] rondelle : La position de la rondelle.
/// @param[in] P2But : La position du but de droite.
/// @param[in] tCase : Le cas à traiter.
/// @param[in] speed : La vitesse maximale du maillet.
/// @param[in] passivity : La passivité du joueur virtuel.
///
/// @return La position vers laquelle le maillet doit se diriger.
///
////////////////////////////////////////////////////////////////////////
glm::vec3 getTarget(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity) {
	switch (tCase) {
	case 1:
		return glm::vec3(rondelle.x, rondelle.y, rondelle.z + 2);
		break;
	case 2:
		return glm::vec3(-30, rondelle.y, rondelle.z + 2);
		break;
	case 3:
		return glm::vec3(30, rondelle.y, rondelle.z + 2);
		break;
	case 4:
	default:
		int height = (rondelle.x + P2But.x) / 2;
		return glm::vec3(height, maillet.y, P2But.z - (60 - 5 * passivity));
		break;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn float JoueurVirtuel::xSpeed(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity)
///
/// Cette fonction permet de déterminer la vitesse en X qui doit être
/// appliqué au maillet du joueur virtuel.
///
/// @param[in] maillet : La position du maillet.
/// @param[in] rondelle : La position de la rondelle.
/// @param[in] P2But : La position du but de droite.
/// @param[in] tCase : Le cas à traiter.
/// @param[in] speed : La vitesse maximale du maillet.
/// @param[in] passivity : La passivité du joueur virtuel.
///
/// @return La vitesse en X à appliquer au maillet
///
////////////////////////////////////////////////////////////////////////
float JoueurVirtuel::xSpeed(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity) {
	glm::vec3 target = getTarget(maillet, rondelle, P2But, tCase, speed, passivity);

	glm::vec3 direction = target - maillet;

	int x;
	if (direction.x + 2 < 0) x = -1;
	else if (direction.x - 2 > 0) x = 1;
	else x = 0;

	if (glm::length(direction) < 25 && tCase == 1) {
		return (float)(2 * x * (float)speed / 5);
	}
	else
		return (float)(x * (float)speed / 5);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn float JoueurVirtuel::zSpeed(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity)
///
/// Cette fonction permet de déterminer la vitesse en Z qui doit être
/// appliqué au maillet du joueur virtuel.
///
/// @param[in] maillet : La position du maillet.
/// @param[in] rondelle : La position de la rondelle.
/// @param[in] P2But : La position du but de droite.
/// @param[in] tCase : Le cas à traiter.
/// @param[in] speed : La vitesse maximale du maillet.
/// @param[in] passivity : La passivité du joueur virtuel.
///
/// @return La vitesse en Z à appliquer au maillet
///
////////////////////////////////////////////////////////////////////////
float JoueurVirtuel::zSpeed(glm::vec3 maillet, glm::vec3 rondelle, glm::vec3 P2But, int tCase, int speed, int passivity) {
	glm::vec3 target = getTarget(maillet, rondelle, P2But, tCase, speed, passivity);

	glm::vec3 direction = target - maillet;

	int z;
	if (direction.z + 2 < 0) z = -1;
	else if (direction.z - 2 > 0) z = 1;
	else z = 0;

	if (glm::length(direction) < 25 && tCase == 1) {
		return (float)(2 * z * (float)speed / 5);
	}
	else
		return (float)(z * (float)speed / 5);
}