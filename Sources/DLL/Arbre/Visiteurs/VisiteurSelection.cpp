///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSelection.cpp
/// @author JUlien Charbonneau
/// @date 2016-09-17
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurSelection.h"
#include "ModeleEtatJeu.h"
#include "ModeleEtatSelection.h"


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::VisiteurSelection(glm::dvec3 rayStart, glm::dvec3 rayEnd, bool ctrl) 
///
/// Constructeur pour la s�lection clic
///
///	@param[in] rayStart : Point initial de la s�lection par raycast
///	@param[in] rayEnd :	Point final de la s�lection par raycast
///	@param[in] ctrl : Si la touche CTRL est appuy�e ou non
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurSelection::VisiteurSelection(glm::dvec3 rayStart, glm::dvec3 rayEnd, bool ctrl)
: nbSelections_(0), multiSelection_(false) {
	ctrl_ = ctrl;
	pointDebut_ = rayStart;
	pointFin_ = rayEnd;

}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::VisiteurSelection(glm::dvec3 pointAncrage, glm::dvec3 pointFinal, bool multiSelection, bool ctrl) 
///
/// Constructeur pour la s�lection avec rectangle �lastique
///
///	@param[in] pointAncrage : Point initial du rectangle �lastique
///	@param[in] pointFinal :	Point final du rectangle �lastique
///	@param[in] ctrl : Si la touche CTRL est appuy�e ou non
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurSelection::VisiteurSelection(glm::dvec3 pointAncrage, glm::dvec3 pointFinal, bool multiSelection, bool ctrl)
 : nbSelections_(0) {
	multiSelection_ = multiSelection;
	ctrl_ = ctrl;
	pointDebut_ = pointAncrage;
	pointFin_ = pointFinal;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiter un acc�l�rateur pour la s�lection.
///
///	@param[in] noeud : Un acc�l�rateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::visiterAccelerateur(NoeudAccelerateur* noeud) {
	if (!multiSelection_ && nbSelections_ == 0)
		cylinderCollisionTest(noeud);
	else if (multiSelection_)
		selectionCentreNoeud(noeud);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::visiterPointControl(NoeudPointControl* noeud)
///
/// Cette fonction permet de visiter un point de contr�le pour la s�lection.
///
///	@param[in] noeud : Un point de contr�le
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::visiterPointControl(NoeudPointControl* noeud) {
	if (!multiSelection_ && noeud->estSelectionnable())
	{
		sphereCollisionTest(noeud);
		//handleSelection(noeud->obtenirNoeudOppose());
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::visiterMur(NoeudMur* noeud) 
///
/// Cette fonction permet de visiter un mur pour la s�lection.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::visiterMur(NoeudMur* noeud) {
	if (!multiSelection_ && nbSelections_ == 0)
		cubeCollisionTest(noeud);
	else if (multiSelection_)
		selectionCentreNoeud(noeud);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiter un portail pour la s�lection.
///
///	@param[in] noeud : Un portail
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::visiterPortail(NoeudPortail* noeud) {
	if (!multiSelection_ && nbSelections_ == 0)
		cylinderCollisionTest(noeud);
	else if (multiSelection_)
		selectionCentreNoeud(noeud);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::sphereCollisionTest(NoeudAbstrait* noeud)
///
/// Cette fonction permet de qu'il y a bien eu un collision sph�rique 
/// avec le noeud � l'aide de raycast. Cela permet de confirmer qu'il 
/// y a bien eu une s�lection.
///
///	@param[in] noeud : Un noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::sphereCollisionTest(NoeudAbstrait* noeud) {
	utilitaire::SphereEnglobante sphereCollider = utilitaire::calculerSphereEnglobante(*noeud->obtenirModele3D());
	aidecollision::DetailsCollision collisionDetails = aidecollision::calculerCollisionSegment(pointDebut_, pointFin_, noeud->obtenirPositionRelative(), sphereCollider.rayon, true);

	if (collisionDetails.type != aidecollision::COLLISION_AUCUNE) {
		handleSelection(noeud);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::cubeCollisionTest(NoeudAbstrait* noeud)
///
/// Cette fonction permet de qu'il y a bien eu un collision cubique avec 
/// le noeud � l'aide de raycast. Cela permet de confirmer qu'il y a 
/// bien eu une s�lection.
///
///	@param[in] noeud : Un noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::cubeCollisionTest(NoeudAbstrait* noeud) {
	utilitaire::BoiteEnglobante cubeCollider = utilitaire::calculerBoiteEnglobante(*noeud->obtenirModele3D());
	aidecollision::DetailsCollision collisionDetails = aidecollision::calculerCollisionSegment(pointDebut_, pointFin_, cubeCollider.coinMin, cubeCollider.coinMax, noeud->obtenirScale(), noeud->obtenirMatriceRotationTranslation());

	if (collisionDetails.type != aidecollision::COLLISION_AUCUNE) {
		handleSelection(noeud);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::cylinderCollisionTest(NoeudAbstrait* noeud)
///
/// Cette fonction permet de qu'il y a bien eu un collision cylindrique 
/// avec le noeud � l'aide de raycast. Cela permet de confirmer qu'il 
/// y a bien eu une s�lection.
///
///	@param[in] noeud : Un noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::cylinderCollisionTest(NoeudAbstrait* noeud) {
	utilitaire::CylindreEnglobant cylinderCollider = utilitaire::calculerCylindreEnglobant(*noeud->obtenirModele3D());
	glm::vec3 pos = noeud->obtenirPositionRelative();

	if(utilitaire::segmentCercleIntersect(pointDebut_, pointFin_, glm::vec3(pos.x, 0, pos.z), cylinderCollider.rayon)) {

		handleSelection(noeud);
	}
}

void VisiteurSelection::handleSelection(NoeudAbstrait* node)
{
	if(!node->isSelectedByAnotherUser())
	{
		if (ctrl_)
		{
			if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
			{
				ModeleEtatSelection::obtenirInstance()->getSelectionEventCallback()(node->getUUID(), !node->estSelectionne(), false);
				node->inverserSelection();

			}else
			{
				node->inverserSelection();
			}
		}
		else
		{
			node->selectionnerTout();
			if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
			{
				ModeleEtatSelection::obtenirInstance()->getSelectionEventCallback()(node->getUUID(), node->estSelectionne(), false);
			}
		}
		nbSelections_++;
	}


}
////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelection::selectionCentreNoeud(NoeudAbstrait* noeud, std::string typeObjet)
///
/// Cette fonction permet de s�lectionner les noeuds par leur centre 
/// � l'aide des coordonn�es de la souris.
///
///	@param[in] noeud : Un noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelection::selectionCentreNoeud(NoeudAbstrait* noeud) {
	glm::vec3 positionRelative = noeud->obtenirPositionRelative();

	if ((positionRelative.x >= min(pointDebut_.x, pointFin_.x)) && (positionRelative.x <= max(pointDebut_.x, pointFin_.x)) && (positionRelative.z >= min(pointDebut_.z, pointFin_.z)) && (positionRelative.z <= max(pointDebut_.z, pointFin_.z))) {
		handleSelection(noeud);
			
	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////