<<<<<<< HEAD
///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtat.cpp
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"
#include "FacadeModele.h"
#include "Vue.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtat::ModeleEtat()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtat::ModeleEtat() {
	mousePosX_ = 0;
	mousePosY_ = 0;
	lastMousePosX_ = 0;
	lastMousePosY_ = 0;
	initMousePosX_ = 0;
	initMousePosY_ = 0;
	mouseDownL_ = false;
	mouseDownR_ = false;
	altDown_ = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtat::~ModeleEtat()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtat::~ModeleEtat() {}


////////////////////////////////////////////////////////////////////////
///
/// @fn vue::Vue* ModeleEtat::obtenirVue() 
///
/// Fonction pour obtenir la vue rapidement
///
/// @return La vue.
///
////////////////////////////////////////////////////////////////////////
vue::Vue* ModeleEtat::obtenirVue() 
{
	return FacadeModele::obtenirInstance()->obtenirVue();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::fleches(double x, double y) 
///
/// Cette déplace la vue selon la direction des flèches
///
/// @param[in] x : Déplacement en x
/// @param[in] y : Déplacement en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::fleches(double x, double y)
{
	obtenirVue()->deplacerXY(x, y);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::playerMouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge. Elle met à jour la 
/// position courante de la souris.
///
/// @param x[in] : position de la souris en x
/// @param y[in] : position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::playerMouseMove(int x, int y) {
	lastMousePosX_ = mousePosX_;
	lastMousePosY_ = mousePosY_;
	
	mousePosX_ = x;
	mousePosY_ = y;

	if (mouseDownR_) {
		glm::ivec2 delta(mousePosX_ - lastMousePosX_, mousePosY_ - lastMousePosY_);
		if (FacadeModele::obtenirInstance()->modeOrbite_) {
			obtenirVue()->obtenirCamera().orbiterXY(-delta.y/3.0, -delta.x/3.0);
		}
		else obtenirVue()->deplacerXY(delta);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseDownL() 
///
/// Évènement appelé lorsque le bouton gauche de la souris est descendu.
/// Entre autre, cette fonction met à jour la position initiale d'un clic.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseDownL() {
	if (!mouseDownR_) {
		initMousePosX_ = mousePosX_;
		initMousePosY_ = mousePosY_;
		mouseDownL_ = true;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseDownR()
///
/// Évènement appelé lorsque le bouton droit de la souris est descendu.
/// Entre autre, cette fonction met à jour la position initiale d'un clic.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseDownR() {
	if (!mouseDownL_) {
		mouseDownR_ = true;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseUpL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est levé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseUpL() {
	if (mouseDownL_) {
		mouseDownL_ = false;
		altDown_ = false;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseUpR()
///
/// Évènement appelé lorsque le bouton droit de la souris est levé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseUpR() {
	if (mouseDownR_) {
		mouseDownR_ = false;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::modifierKeys(bool alt, bool ctrl)
///
/// Évènement appelé lorsque les touches ctrl ou/et alt sont appuyées.
///
/// @param[in] alt : Vrai si le bouton alt est appuyé 
/// @param[in] ctrl : Vrai si le bouton ctrl est appuyé
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::modifierKeys(bool alt, bool ctrl) {
	altDown_ = alt;
	ctrlDown_ = ctrl;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::isAClick()
///
/// Fonction pour savoir si on est dans un état de clic.
///
/// @return Vrai si on a un clic.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::isAClick() {
	if (std::abs(mousePosX_ - initMousePosX_) <= 3 && std::abs(mousePosY_ - initMousePosY_) <= 3)
		return true;
	return false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::noeudsSurLaTable() const
///
/// Vérifie que les objets sont sur la table.
///
/// @return Vrai si les noeuds sont sur la table.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::noeudsSurLaTable() const {
	VisiteurSurTable visiteur;
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);
	return visiteur.sontSurTable();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::mouseOverTable()
///
/// Fonction pour déterminer si le curseur est au-dessus de la table.
///
/// @return Vrai si le curseur est au-dessus de la table.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::mouseOverTable() {
	glm::dvec3 mousePos;
	FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, mousePos);

	NoeudAbstrait* table = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_TABLE);
	std::vector<glm::vec3> sommetsTable = ((NoeudTable*)table)->obtenirSommetsPatinoire();

	// Si le point est dans l'un des triangles formant la table, on retourne vrai
	for (int i = 1; i < sommetsTable.size(); ++i) {
		if (aidecollision::pointDansTriangle(mousePos, sommetsTable[0], sommetsTable[i], sommetsTable[(i % (sommetsTable.size() - 1)) + 1])) {
			return true;
		}
	}

	return false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::mouseOverControlPoint()
///
/// Fonction pour déterminer si le curseur est au-dessus d'un point de controle
///
/// @return Vrai si le curseur est au-dessus d'un point de controle.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::mouseOverControlPoint() {
	NoeudTable* table = (NoeudTable*) FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_TABLE);
	auto it = table->obtenirIterateurBegin();
	auto endIt = table->obtenirIterateurEnd();
	Raycast ray(mousePosX_, mousePosY_);

	while (it != endIt) {
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL) {
			NoeudPointControl* noeud = (NoeudPointControl*)(*it);
			aidecollision::DetailsCollision collisionDetails = aidecollision::calculerCollisionSegment(ray.getRayStart(), ray.getRayEnd(), noeud->obtenirPositionRelative(), noeud->getRayonModele3D(), true);

			if (collisionDetails.type != aidecollision::COLLISION_AUCUNE)
				return true;
		}
		it++;
	}
	return false;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
=======
///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtat.cpp
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "ModeleEtat.h"
#include "FacadeModele.h"
#include "Vue.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtat::ModeleEtat()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtat::ModeleEtat() {
	mousePosX_ = 0;
	mousePosY_ = 0;
	lastMousePosX_ = 0;
	lastMousePosY_ = 0;
	initMousePosX_ = 0;
	initMousePosY_ = 0;
	mouseDownL_ = false;
	mouseDownR_ = false;
	altDown_ = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtat::~ModeleEtat()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtat::~ModeleEtat() {}


////////////////////////////////////////////////////////////////////////
///
/// @fn vue::Vue* ModeleEtat::obtenirVue() 
///
/// Fonction pour obtenir la vue rapidement
///
/// @return La vue.
///
////////////////////////////////////////////////////////////////////////
vue::Vue* ModeleEtat::obtenirVue() 
{
	return FacadeModele::obtenirInstance()->obtenirVue();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::fleches(double x, double y) 
///
/// Cette déplace la vue selon la direction des flèches
///
/// @param[in] x : Déplacement en x
/// @param[in] y : Déplacement en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::fleches(double x, double y)
{
	obtenirVue()->deplacerXY(x, y);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge. Elle met à jour la 
/// position courante de la souris.
///
/// @param x[in] : position de la souris en x
/// @param y[in] : position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseMove(int x, int y) {
	lastMousePosX_ = mousePosX_;
	lastMousePosY_ = mousePosY_;
	
	mousePosX_ = x;
	mousePosY_ = y;

	if (mouseDownR_) {
		glm::ivec2 delta(mousePosX_ - lastMousePosX_, mousePosY_ - lastMousePosY_);
		if (FacadeModele::obtenirInstance()->modeOrbite_) {
			obtenirVue()->obtenirCamera().orbiterXY(-delta.y/3.0, -delta.x/3.0);
		}
		else obtenirVue()->deplacerXY(delta);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseDownL() 
///
/// Évènement appelé lorsque le bouton gauche de la souris est descendu.
/// Entre autre, cette fonction met à jour la position initiale d'un clic.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseDownL() {
	if (!mouseDownR_) {
		initMousePosX_ = mousePosX_;
		initMousePosY_ = mousePosY_;
		mouseDownL_ = true;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseDownR()
///
/// Évènement appelé lorsque le bouton droit de la souris est descendu.
/// Entre autre, cette fonction met à jour la position initiale d'un clic.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseDownR() {
	if (!mouseDownL_) {
		mouseDownR_ = true;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseUpL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est levé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseUpL() {
	if (mouseDownL_) {
		mouseDownL_ = false;
		altDown_ = false;
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::mouseUpR()
///
/// Évènement appelé lorsque le bouton droit de la souris est levé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::mouseUpR() {
	if (mouseDownR_) {
		mouseDownR_ = false;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtat::modifierKeys(bool alt, bool ctrl)
///
/// Évènement appelé lorsque les touches ctrl ou/et alt sont appuyées.
///
/// @param[in] alt : Vrai si le bouton alt est appuyé 
/// @param[in] ctrl : Vrai si le bouton ctrl est appuyé
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtat::modifierKeys(bool alt, bool ctrl) {
	altDown_ = alt;
	ctrlDown_ = ctrl;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::isAClick()
///
/// Fonction pour savoir si on est dans un état de clic.
///
/// @return Vrai si on a un clic.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::isAClick() {
	if (std::abs(mousePosX_ - initMousePosX_) <= 3 && std::abs(mousePosY_ - initMousePosY_) <= 3)
		return true;
	return false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::noeudsSurLaTable() const
///
/// Vérifie que les objets sont sur la table.
///
/// @return Vrai si les noeuds sont sur la table.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::noeudsSurLaTable() const {
	VisiteurSurTable visiteur;
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);
	return visiteur.sontSurTable();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::mouseOverTable()
///
/// Fonction pour déterminer si le curseur est au-dessus de la table.
///
/// @return Vrai si le curseur est au-dessus de la table.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::mouseOverTable() {
	glm::dvec3 mousePos;
	FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, mousePos);

	NoeudAbstrait* table = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_TABLE);
	std::vector<glm::vec3> sommetsTable = ((NoeudTable*)table)->obtenirSommetsPatinoire();

	// Si le point est dans l'un des triangles formant la table, on retourne vrai
	for (int i = 1; i < sommetsTable.size(); ++i) {
		if (aidecollision::pointDansTriangle(mousePos, sommetsTable[0], sommetsTable[i], sommetsTable[(i % (sommetsTable.size() - 1)) + 1])) {
			return true;
		}
	}

	return false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtat::mouseOverControlPoint()
///
/// Fonction pour déterminer si le curseur est au-dessus d'un point de controle
///
/// @return Vrai si le curseur est au-dessus d'un point de controle.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtat::mouseOverControlPoint() {
	NoeudTable* table = (NoeudTable*) FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_TABLE);
	auto it = table->obtenirIterateurBegin();
	auto endIt = table->obtenirIterateurEnd();
	Raycast ray(mousePosX_, mousePosY_);

	while (it != endIt) {
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL) {
			NoeudPointControl* noeud = (NoeudPointControl*)(*it);
			aidecollision::DetailsCollision collisionDetails = aidecollision::calculerCollisionSegment(ray.getRayStart(), ray.getRayEnd(), noeud->obtenirPositionRelative(), noeud->getRayonModele3D(), true);

			if (collisionDetails.type != aidecollision::COLLISION_AUCUNE)
				return true;
		}
		it++;
	}
	return false;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
>>>>>>> 73328ffab2ef0a4c7261556303ae80c2fc34398d
///////////////////////////////////////////////////////////////////////////////