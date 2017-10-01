///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSelectionnable.cpp
/// @author Nam Lesage
/// @date 2016-09-29
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurSelectionnable.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::VisiteurSelectionnable(const std::string& type, const bool& selectionnable)
///
/// Constructeur de VisiteurSelectionnable.
///
///	@param[in] type : Type du noeud
///	@param[in] selectionnable : Si le noeud est s�lectionnable ou non
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurSelectionnable::VisiteurSelectionnable(const std::string& type, const bool& selectionnable)
	: type_(type), selectionnable_(selectionnable)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::~VisiteurSelectionnable()
///
/// Destructeur de VisiteurSelectionnable.
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurSelectionnable::~VisiteurSelectionnable() {
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiter un acc�l�rateur pour v�rifier s'il est s�lectionnable.
///
///	@param[in] noeud: Un acc�l�rateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::visiterAccelerateur(NoeudAccelerateur* noeud) {
	implementationDefaut(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::visiterMaillet(NoeudMaillet* noeud)
///
/// Cette fonction permet de visiter un maillet afin de l'impl�menter par d�faut.
///
///	@param[in] noeud: Un maillet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::visiterMaillet(NoeudMaillet* noeud) {
	implementationDefaut(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::visiterTable(NoeudTable* noeud)
///
/// Cette fonction permet de visiter une table afin de l'impl�menter par d�faut.
///
///	@param[in] noeud: Une table
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::visiterTable(NoeudTable* noeud) {
	implementationDefaut(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::visiterPointControl(NoeudPointControl * noeud)
///
/// Cette fonction permet de visiter un point de contr�le afin de l'impl�menter par d�faut.
///
///	@param[in] noeud: Un point de contr�le
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::visiterPointControl(NoeudPointControl * noeud) {
	implementationDefaut(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiter un mur afin de l'impl�menter par d�faut.
///
///	@param[in] noeud: Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::visiterMur(NoeudMur* noeud) {
	implementationDefaut(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiter un portail afin de l'impl�menter par d�faut.
///
///	@param[in] noeud: Un portail
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::visiterPortail(NoeudPortail* noeud) {
	implementationDefaut(noeud);
}

void VisiteurSelectionnable::visiterRondelle(NoeudRondelle * noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::assignerType(const std::string & type)
///
/// Cette fonction permet d'assigner un type envoy� par param�tre.
///
///	@param[in] type: Type du noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::assignerType(const std::string & type)
{
	type_ = type;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::assignerSelectionnable(const bool & selectionnable)
///
/// Cette fonction permet de rendre un certain noeud s�lectionnable.
///
///	@param[in] selectionnable: Si le noeud est s�lectionnable ou non
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::assignerSelectionnable(const bool & selectionnable)
{
	selectionnable_ = selectionnable;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSelectionnable::implementationDefaut(NoeudAbstrait * noeud)
///
/// Cette fonction permet d'impl�menter un noeud par d�faut et de lui assigner
/// la caract�ristique de s�lectionnable.
///
///	@param[in] noeud: Un noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSelectionnable::implementationDefaut(NoeudAbstrait * noeud)
{
	if (noeud->obtenirType() == type_)
	{
		noeud->assignerEstSelectionnable(selectionnable_);
	}
}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////