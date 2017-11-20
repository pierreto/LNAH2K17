///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurScale.h
/// @author Nam Lesage
/// @date 2016-09-28
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURSCALE_H__
#define __APPLICATION_VISITEURS_VISITEURSCALE_H__

#include "Visiteurs/VisiteurAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurScale
/// @brief Permet de scaler les noeuds
///
///
/// @author Nam Lesage
/// @date 2016-09-28
///////////////////////////////////////////////////////////////////////////
class VisiteurScale : public VisiteurAbstrait
{
public:
	/// Constructeur
	VisiteurScale(const glm::vec3& scale = glm::vec3(1,1,1), bool ajusterScaling = false);
	/// Destructeur
	virtual ~VisiteurScale();

	/// Visiter un accélérateur pour la mise en échelle
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour la mise en échelle
	virtual void visiterMaillet(NoeudMaillet* noeud);
	/// Visiter une table pour la mise en échelle
	virtual void visiterTable(NoeudTable* noeud);
	/// Visiter un point de contrôle pour la mise en échelle
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter un mur pour la mise en échelle
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour la mise en échelle
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud);

	/// Assigne au noeud le facteur de redimenssionnement
	void assignerScale(const glm::vec3& scale);

	std::vector<NoeudAbstrait*> getSelectedNodes() { return selectedNodes_; }

private:
	/// Vecteur trois dimensions pour le redimensionnement
	glm::vec3 scale_;
	/// Bool d'ajustement pour le redimensionnement
	bool ajusterScaling_;

	std::vector<NoeudAbstrait*> selectedNodes_;
};


#endif // __APPLICATION_VISITEURS_VISITEURSCALE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////