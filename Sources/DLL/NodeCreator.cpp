#include "NodeCreator.h"
#include "NoeudPortail.h"
#include "ArbreRenduINF2990.h"
#include "FacadeModele.h"
#include "NoeudMur.h"
#include <glm/gtx/vector_angle.inl>

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
void NodeCreator::createPortal(const char* startUuid, const glm::vec3 portal1Pos, const char* endUuid, const glm::vec3 portal2Pos)
{

	NoeudPortail* noeud1 = (NoeudPortail*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL,startUuid);

	noeud1->assignerPositionRelative(portal1Pos);

	NoeudPortail* noeud2 = (NoeudPortail*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL, endUuid);

	noeud2->assignerPositionRelative(portal2Pos);

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

void NodeCreator::createBoost(const char* uuid, glm::vec3 pos)
{
	NoeudAbstrait* noeud = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR, uuid);

	noeud->assignerPositionRelative(pos);

	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud);
}
