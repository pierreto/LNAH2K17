///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerMuret.cpp
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatCreerMuret.h"

#include "FacadeModele.h"
#include "Vue.h"
#include "ArbreRenduINF2990.h"

#include "Modele3D.h"
#include "Noeud.h"
#include "Mesh.h"

#include "glm/gtx/vector_angle.hpp"
#include "ModeleEtatJeu.h"

/// Pointeur vers l'instance unique de la classe.
ModeleEtatCreerMuret* ModeleEtatCreerMuret::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerMuret* ModeleEtatCreerMuret::obtenirInstance()
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
ModeleEtatCreerMuret* ModeleEtatCreerMuret::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatCreerMuret();

	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->deselectionnerTout();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerMuret::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerMuret::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerMuret::ModeleEtatCreerMuret()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatCreerMuret::ModeleEtatCreerMuret()
	: ModeleEtat(), actionCommencee_(false)
{
	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerMuret::~ModeleEtatCreerMuret() 
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatCreerMuret::~ModeleEtatCreerMuret() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerMuret::mouseUpL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est levé.
/// Cette fonction s'occupe de la création du murets
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerMuret::mouseUpL() {
	if (mouseDownL_ && isAClick() && mouseOverTable()) {
		// Premier clic
		if (noeud_ == nullptr) {
			// Création du noeud
			noeud_ = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_MUR);

			// Déplacement du noeud
			// Transformation du point dans l'esapce virtuelle
			glm::dvec3 point;
			obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, point);

			noeud_->assignerAxisLock(glm::ivec3(1, 1, 1));
			noeud_->assignerPositionRelative(glm::vec3(point.x, point.y + 0.01f, point.z));
			noeud_->assignerAxisLock(glm::ivec3(1, 0, 1));

			pointInitial_ = point;

			// Activer effet fantome
			noeud_->effetFantome(true);

			// Ajout du noeud à l'arbre de rendu
			FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud_);

		}
		// Second Clic
		else {
			if (noeudsSurLaTable()) {
				// Desactiver effet fantome
				noeud_->effetFantome(false);
				wallCreationCallback_(noeud_->getUUID(), glm::value_ptr(noeud_->obtenirPositionRelative()), noeud_->obtenirRotation().y, glm::value_ptr(noeud_->obtenirScale()));

				noeud_ = nullptr;

			}
			else {
				escape();
			}
		}
	}

	mouseDownL_ = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerMuret::playerMouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge.
/// Cette fonction agrandit le mur avec le déplacement de la souris
///
/// @param[in] x : Position de la souris en x 
/// @param[in] y : Position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerMuret::playerMouseMove(int x, int y) {
	ModeleEtat::playerMouseMove(x, y);

	if (noeud_ != nullptr) {
		// Transformation du point dans l'esapce virtuelle
		glm::dvec3 point;
		obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, point);

		// Ajuster le scaling
		double distance = glm::distance(glm::vec3(point),  pointInitial_);
		noeud_->scale(glm::vec3(1, 1, distance));

		// Ajuster la rotation
		glm::vec3 node2mouse = glm::vec3(point) - pointInitial_;
		float angle = glm::orientedAngle(glm::vec3(0, 0, 1), glm::normalize(node2mouse), glm::vec3(0,1,0));
		noeud_->rotate(angle, glm::vec3(0, 1, 0));		

		// Ajuster le deplacement
		glm::vec3 position = pointInitial_;
		position.z += float(glm::cos(angle) * distance/2);
		position.x += float(glm::sin(angle) * distance/2);
		noeud_->deplacer(position);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerMuret::escape() 
///
/// Cette fonction annule l'action de création du portail
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerMuret::escape() 
{
	if (noeud_ != nullptr)
	{
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->effacer(noeud_);
		// Desactiver effet fantome
		noeud_->effetFantome(false);
		noeud_ = nullptr;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerMuret::nettoyerEtat()
///
/// Cette fonction nettoie l'état des changements apportés
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerMuret::nettoyerEtat()
{
	escape();
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////