#include "NodeCreator.h"
#include "NoeudPortail.h"
#include "ArbreRenduINF2990.h"
#include "FacadeModele.h"
#include "NoeudMur.h"
#include <glm/gtx/vector_angle.inl>
#include <glm/detail/type_mat.hpp>
#include <glm/detail/type_mat.hpp>
#include <glm/detail/type_mat.hpp>
#include <glm/detail/type_mat.hpp>

/// Pointeur vers l'instance unique de la classe.
NodeCreator* NodeCreator::instance_{ nullptr };

NodeCreator* NodeCreator::obtenirInstance()
{
	if (instance_ == nullptr)
		instance_ = new NodeCreator();

	return instance_;
}

void NodeCreator::libererInstance()
{
	delete instance_;
	instance_ = nullptr;
}

NodeCreator::NodeCreator()
{
}


NodeCreator::~NodeCreator()
{
	if (instance_ != nullptr) {
		libererInstance();
	}
}
void NodeCreator::createPortal(const char* startUuid, glm::vec3 startPos, const float  startRotation, glm::vec3 startScale, const char* endUuid, glm::vec3 endPos, const  float endRotation, glm::vec3 endScale)
{

	NoeudPortail* noeud1 = (NoeudPortail*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL,startUuid);

	noeud1->assignerPositionRelative(startPos);
	noeud1->rotate(startRotation, glm::vec3(0, 1, 0));
	noeud1->scale(endScale);

	NoeudPortail* noeud2 = (NoeudPortail*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL, endUuid);

	noeud2->assignerPositionRelative(endPos);
	noeud2->rotate(endRotation, glm::vec3(0, 1, 0));
	noeud2->scale(startScale);


	noeud1->assignerOppose(noeud2);
	noeud2->assignerOppose(noeud1);


	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud1);
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud2);

}

void NodeCreator::createWall(const char* uuid, glm::vec3 position, float angle,  glm::vec3 scale)
{

	// Création du noeud
	NoeudMur* noeud = (NoeudMur*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_MUR, uuid);
	noeud->assignerPositionRelative(position);
	noeud->rotate(angle, glm::vec3(0, 1, 0));
	noeud->scale(scale);
	
	// Ajout du noeud à l'arbre de rendu
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud);

}

void NodeCreator::createBoost(const char* uuid, glm::vec3 pos, const float angle, glm::vec3 scale)
{
	NoeudAbstrait* noeud = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR, uuid);

	noeud->assignerPositionRelative(pos);
	noeud->rotate(angle, glm::vec3(0, 1, 0));
	noeud->scale(scale);

	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud);
}
