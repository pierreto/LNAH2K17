///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurScale.cpp
/// @author Nam Lesage
/// @date 2016-09-28
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurScale.h"
#include "ModeleEtatJeu.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::VisiteurScale(const glm::vec3& scale, bool ajusterScaling)
///
/// Constructeur pour VisiterScalela
///
///	@param[in] scale : Vecteur des facteurs pour mise à l'échelle
///	@param[in] ajusterScaling : Si une mise à l'échelle est requise ou non
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurScale::VisiteurScale(const glm::vec3& scale, bool ajusterScaling) 
	: scale_(scale), ajusterScaling_(ajusterScaling)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::~VisiteurScale()
///
/// Destructeur pour VisiterScale
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurScale::~VisiteurScale() {

}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiter un accélérateur pour la mise en échelle.
///
///	@param[in] noeud : Un accélérateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurScale::visiterAccelerateur(NoeudAccelerateur* noeud) {
	if (noeud->estSelectionne())
	{
		glm::vec3 position = noeud->obtenirPositionRelative();
		glm::vec3 scale = noeud->obtenirScale() + (ajusterScaling_ ? (scale_ / 6.0f) : scale_);

		if (scale.x <= 1)
			scale = glm::vec3(1, 1, 1);

		noeud->scale(scale);
		sendTransform(noeud);

	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::visiterMaillet(NoeudMaillet* noeud)
///
/// Cette fonction permet de visiter un maillet pour la mise en échelle.
///
///	@param[in] noeud : Un maillet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurScale::visiterMaillet(NoeudMaillet* noeud) {
	// NE RIEN FAIRE
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::visiterTable(NoeudTable* noeud)
///
/// Cette fonction permet de visiter une table pour la mise en échelle.
///
///	@param[in] noeud : Une table
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurScale::visiterTable(NoeudTable* noeud) {
	// NE RIEN FAIRE
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::visiterPointControl(NoeudPointControl * noeud)
///
/// Cette fonction permet de visiter un point de contrôle pour la mise en échelle.
///
///	@param[in] noeud : Un point de contrôle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurScale::visiterPointControl(NoeudPointControl * noeud) {
	// NE RIEN FAIRE
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiter un mur pour la mise en échelle.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurScale::visiterMur(NoeudMur* noeud) {
	if (noeud->estSelectionne())
	{
		glm::vec3 position = noeud->obtenirPositionRelative();
		glm::vec3 scale = noeud->obtenirScale() + scale_;
		if (scale.z <= 1)
			scale.z = 1;

		noeud->scale(glm::vec3(1,1,scale.z));
		sendTransform(noeud);

	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiter un portail pour la mise en échelle.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurScale::visiterPortail(NoeudPortail* noeud) {
	if (noeud->estSelectionne())
	{
		glm::vec3 position = noeud->obtenirPositionRelative();
		glm::vec3 scale = noeud->obtenirScale() + (ajusterScaling_ ? (scale_ / 12.0f) : scale_);

		if (scale.x <= 1)
			scale = glm::vec3(1, 1, 1);

		noeud->scale(scale);
		sendTransform(noeud);
	}
}

void VisiteurScale::visiterRondelle(NoeudRondelle * noeud)
{
}

void VisiteurScale::sendTransform(NoeudAbstrait* node)
{
	TransformEventCallback callback = ModeleEtatJeu::obtenirInstance()->getTransformEventCallback();
	if (callback)
	{
		callback(node->getUUID(), glm::value_ptr(node->obtenirPositionRelative()),node->obtenirRotation().y, glm::value_ptr(node->obtenirScale()));
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurScale::assignerScale(const glm::vec3 & scale)
///
/// Cette fonction permet d'assigner les facteurs de redimensionnement.
///
///	@param[in] scale : Vecteur des facteurs de redimensionnement
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurScale::assignerScale(const glm::vec3 & scale)
{
	scale_ = scale;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////