///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSauvegarde.h
/// @author Francis Dalpé
/// @date 2016-10-01
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURSAUVEGARDE_H__
#define __APPLICATION_VISITEURS_VISITEURSAUVEGARDE_H__

#include "rapidjson/document.h" // rapidjson's DOM-style API
#include "rapidjson/writer.h" // for stringify JSON
#include "rapidjson/stringbuffer.h"
#include "rapidjson/prettywriter.h"
#include "rapidjson/filewritestream.h"
#include "rapidjson/filereadstream.h"

#include "Visiteurs/VisiteurAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSauvegarde
/// @brief Permet de sauvegarder la scène de l'éditeur
///
///
/// @author Francis Dalpé
/// @date 2016-10-01
///////////////////////////////////////////////////////////////////////////
class VisiteurSauvegarde : public VisiteurAbstrait {
public:
	/// Constructeur
	VisiteurSauvegarde() {};
	/// Destructeur
	virtual ~VisiteurSauvegarde() {};

	/// Visiter un accélérateur pour la sauvegarde
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour la sauvegarde
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visiter une table pour la sauvegarde
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visiter un point de contrôle pour la sauvegarde
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter un mur pour la sauvegarde
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour la sauvegarde
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud) {};

private:

	/// Effecteur la sauvegarde d'un noeud
	void sauvegarderNoeud(NoeudAbstrait* noeud, char* nom);
	std::vector<NoeudPortail*> linkedPortals_;
};


#endif // __APPLICATION_VISITEURS_VISITEURSAUVEGARDE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////