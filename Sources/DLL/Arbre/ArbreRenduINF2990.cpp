///////////////////////////////////////////////////////////////////////////
/// @file ArbreRenduINF2990.cpp
/// @author Martin Bisson
/// @date 2007-03-23
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#include "ArbreRenduINF2990.h"
#include "Usines/UsineNoeud.h"
#include "EtatOpenGL.h"
#include "Noeuds/NoeudTypes.h"

/// La chaîne représentant le type des maillets.
const std::string ArbreRenduINF2990::NOM_MAILLET{ "maillet" };
/// La chaîne représentant le type de la table
const std::string ArbreRenduINF2990::NOM_TABLE{ "table" };
/// La chaîne représentant le type des points de control.
const std::string ArbreRenduINF2990::NOM_POINT_CONTROL{ "point_control" };
/// La chaîne représentant le type des accelerateurs.
const std::string ArbreRenduINF2990::NOM_ACCELERATEUR{ "accelerateur" };
/// La chaîne représentant le type des murs.
const std::string ArbreRenduINF2990::NOM_MUR{ "mur" };
/// La chaîne représentant le type des portails.
const std::string ArbreRenduINF2990::NOM_PORTAIL{ "portail" };
/// La chaîne représentant le type des buts.
const std::string ArbreRenduINF2990::NOM_BUT{ "but" };
/// La chaîne représentant le type de la rondelle.
const std::string ArbreRenduINF2990::NOM_RONDELLE{ "rondelle" };
/// La chaîne représentant le type des boosters.
const std::string ArbreRenduINF2990::NOM_BOOSTER{ "booster" };

////////////////////////////////////////////////////////////////////////
///
/// @fn ArbreRenduINF2990::ArbreRenduINF2990()
///
/// Ce constructeur crée toutes les usines qui seront utilisées par le
/// projet de INF2990et les enregistre auprès de la classe de base.
/// Il crée également la structure de base de l'arbre de rendu, c'est-à-dire
/// avec les noeuds structurants.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
ArbreRenduINF2990::ArbreRenduINF2990()
{
	// Construction des usines
	ajouterUsine(NOM_MAILLET, new UsineNoeud<NoeudMaillet>{ NOM_MAILLET, std::string{ "media/models/maillet.obj" } });
	ajouterUsine(NOM_TABLE, new UsineNoeud<NoeudTable>{ NOM_TABLE, std::string{ "noModel" } });
	ajouterUsine(NOM_POINT_CONTROL, new UsineNoeud<NoeudPointControl>{ NOM_POINT_CONTROL, std::string{ "media/models/controlPoint.obj" } });
	ajouterUsine(NOM_ACCELERATEUR, new UsineNoeud<NoeudAccelerateur>{ NOM_ACCELERATEUR, std::string{ "media/models/accelerateur.obj" } });
	ajouterUsine(NOM_MUR, new UsineNoeud<NoeudMur>{ NOM_MUR, std::string{ "media/models/mur.obj" } });
	ajouterUsine(NOM_PORTAIL, new UsineNoeud<NoeudPortail>{ NOM_PORTAIL, std::string{"media/models/portail.obj"} });
	ajouterUsine(NOM_BUT, new UsineNoeud<NoeudBut>{ NOM_BUT, std::string{ "noModel" } });
	ajouterUsine(NOM_RONDELLE, new UsineNoeud<NoeudRondelle>{ NOM_RONDELLE, std::string{ "media/models/rondelle.obj" } });
	ajouterUsine(NOM_BOOSTER, new UsineNoeud<NoeudBooster>{ NOM_BOOSTER, std::string{ "media/models/booster.obj" } });
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ArbreRenduINF2990::~ArbreRenduINF2990()
///
/// Ce destructeur ne fait rien pour le moment.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
ArbreRenduINF2990::~ArbreRenduINF2990()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduINF2990::initialiser()
///
/// Cette fonction crée la structure de base de l'arbre de rendu, c'est-à-dire
/// avec les noeuds structurants (pour les objets, les murs, les billes,
/// les parties statiques, etc.)
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ArbreRenduINF2990::initialiser()
{
	// On vide l'arbre
	vider();

	// On ajoute les noeud de base
	NoeudAbstrait* noeudTable{ creerNoeud(NOM_TABLE) };
	ajouter(noeudTable);

}
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
