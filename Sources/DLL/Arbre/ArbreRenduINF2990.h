///////////////////////////////////////////////////////////////////////////
/// @file ArbreRenduINF2990.h
/// @author Martin Bisson
/// @date 2007-03-23
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_ARBRERENDUINF2990_H__
#define __ARBRE_ARBRERENDUINF2990_H__


#include "ArbreRendu.h"

#include <map>
#include <string>


///////////////////////////////////////////////////////////////////////////
/// @class ArbreRenduINF2990
/// @brief Classe qui représente l'arbre de rendu spécifique au projet de
///        INF2990.
///
///        Cette classe s'occupe de configurer les usines des noeuds qui
///        seront utilisés par le projet.
///
/// @author Martin Bisson
/// @date 2007-03-23
///////////////////////////////////////////////////////////////////////////
class ArbreRenduINF2990 : public ArbreRendu
{
public:
   /// Constructeur par défaut.
   ArbreRenduINF2990();
   /// Destructeur.
   virtual ~ArbreRenduINF2990();

   /// Initialise l'arbre de rendu à son état initial.
   void initialiser();


   /// La chaîne représentant le type des maillets.
   static const std::string NOM_MAILLET;
   /// La chaîne représentant le type des maillets.
   static const std::string NOM_TABLE;
   /// La chaîne représentant le type des points de control.
   static const std::string NOM_POINT_CONTROL;
   /// La chaîne représentant le type des maillets.
   static const std::string NOM_ACCELERATEUR;
   /// La chaîne représentant le type des murs.
   static const std::string NOM_MUR;
   /// La chaîne représentant le type des portails.
   static const std::string NOM_PORTAIL;
   /// La chaîne représentant le type des buts.
   static const std::string NOM_BUT;
   /// La chaîne représentant le type des rondelles.
   static const std::string NOM_RONDELLE;
   /// La chaîne représentant le type des boosters.
   static const std::string NOM_BOOSTER;
};


#endif // __ARBRE_ARBRERENDUINF2990_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
