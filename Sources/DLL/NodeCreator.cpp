#include "NodeCreator.h"
#include "NoeudPortail.h"
#include "ArbreRenduINF2990.h"
#include "FacadeModele.h"

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
void NodeCreator::createPortal(char* startUuid, glm::vec3 portal1Pos, char* endUuid, glm::vec3 portal2Pos)
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