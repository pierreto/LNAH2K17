#include "VisitorByUUID.h"


VisitorByUUID::VisitorByUUID(const char* wantedUuid)
{
	wantedUuid_ = wantedUuid;
	hasFound = false;
}

void VisitorByUUID::visiterAccelerateur(NoeudAccelerateur* noeud)
{
	defaultImpl(noeud);
}

void VisitorByUUID::visiterPointControl(NoeudPointControl* noeud)
{
	defaultImpl(noeud);
}

void VisitorByUUID::visiterMur(NoeudMur* noeud)
{
	defaultImpl(noeud);
}

void VisitorByUUID::visiterPortail(NoeudPortail* noeud)
{
	defaultImpl(noeud);
}

void VisitorByUUID::visiterRondelle(NoeudRondelle* noeud)
{

}
void VisitorByUUID::defaultImpl(NoeudAbstrait* noeud)
{
	if(isUUIDWanted(noeud->getUUID()))
	{
		wantedNode_ = noeud;
		hasFound = true;
	}
}

bool VisitorByUUID::isUUIDWanted(char* nodeUUID)
{

	if (strcmp(nodeUUID, wantedUuid_) == 0) {
		return true;
	}

	return false;
}