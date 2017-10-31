///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSauvegarde.cpp
/// @author Francis Dalpé
/// @date 2016-10-01
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurSauvegarde.h"


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSauvegarde::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiteur un accélérateur pour la sauvegarde. 
///
///	@param[in] noeud : Un accélérateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSauvegarde::visiterAccelerateur(NoeudAccelerateur* noeud) {
	sauvegarderNoeud(noeud, "Accelerateur");
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSauvegarde::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiteur un mur pour la sauvegarde.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSauvegarde::visiterMur(NoeudMur* noeud) {
	sauvegarderNoeud(noeud, "Muret");
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSauvegarde::visiterPointControl(NoeudPointControl* noeud)
///
/// Cette fonction permet de visiteur un point de contrôle pour la sauvegarde. 
///
///	@param[in] noeud : Un point de contrôle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSauvegarde::visiterPointControl(NoeudPointControl* noeud){
	sauvegarderNoeud(noeud, "PointControle");
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSauvegarde::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiteur un portail pour la sauvegarde.
///
///	@param[in] noeud : Un portail
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSauvegarde::visiterPortail(NoeudPortail* noeud) {
	if (std::find(linkedPortals_.begin(), linkedPortals_.end(), noeud) == linkedPortals_.end()) {
		sauvegarderNoeud(noeud, "Portail");
		sauvegarderNoeud(noeud->obtenirOppose(), "Portail");

		linkedPortals_.push_back(noeud);
		linkedPortals_.push_back(noeud->obtenirOppose());
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSauvegarde::sauvegarderNoeud(NoeudAbstrait* noeud, char* nom)
///
/// Cette fonction permet de sauvegarder les propriétés du noeud en question
/// à l'aide de JSON. 
///
///	@param[in] noeud : Un noeud
///	@param[in] char : Nom du document de sauvegarde
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSauvegarde::sauvegarderNoeud(NoeudAbstrait* noeud, char* nom) {
	glm::dvec3 pos = noeud->obtenirPositionRelative();
	glm::dvec3 scale = noeud->obtenirScale();
	double angle = noeud->obtenirRotation().y;

	rapidjson::Value tempArray(rapidjson::kArrayType);
	tempArray.PushBack(pos.x, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	tempArray.PushBack(pos.y, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	tempArray.PushBack(pos.z, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	tempArray.PushBack(scale.x, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	tempArray.PushBack(scale.y, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	tempArray.PushBack(scale.z, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	tempArray.PushBack(angle, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	if(noeud->getUUID()!=nullptr)
	{
		std::string uuidString = std::string(noeud->getUUID());

		rapidjson::Value strVal;
		strVal.SetString(uuidString.c_str(), uuidString.length(), FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
		tempArray.PushBack(strVal, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
	}


	FacadeModele::obtenirInstance()->docJSON_[nom].PushBack(tempArray, FacadeModele::obtenirInstance()->docJSON_.GetAllocator());
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////