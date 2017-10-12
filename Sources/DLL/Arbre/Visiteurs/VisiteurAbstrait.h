///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurAbstrait.h
/// @author Nam Lesage
/// @date 2016-09-147
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURABSTRAIT_H__
#define __APPLICATION_VISITEURS_VISITEURABSTRAIT_H__

#include "NoeudTypes.h"
#include "AideCollision.h"
#include "ArbreRenduINF2990.h"
#include "FacadeModele.h"

#include <algorithm> 
#include <vector>
#include <iostream>
#include <string>
#include <cstdio>
#include <fstream>

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurAbstrait
/// @brief Classe de base du patron Visiteur
///
///        Cette classe abstraite comprend l'interface de base que doivent
///        implanter tous les visiteurs concrets
///
/// @author Nam Lesage
/// @date 2016-09-17
///////////////////////////////////////////////////////////////////////////
class VisiteurAbstrait {
public:
	/// Constructeur
	VisiteurAbstrait() {};
	/// Destructeur
	virtual ~VisiteurAbstrait() {};
	/// Fonctions virtuelles pures
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud) = 0;
	virtual void visiterMaillet(NoeudMaillet* noeud) = 0;
	virtual void visiterPointControl(NoeudPointControl* noeud) = 0;
	virtual void visiterTable(NoeudTable* noeud) = 0;
	virtual void visiterPortail(NoeudPortail* noeud) = 0;
	virtual void visiterMur(NoeudMur* noeud) = 0;
	virtual void visiterRondelle(NoeudRondelle* noeud) = 0;
	virtual void visiterBut(NoeudBut* noeud) {};
};


#endif // __APPLICATION_VISITEURS_VISITEURABSTRAIT_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
