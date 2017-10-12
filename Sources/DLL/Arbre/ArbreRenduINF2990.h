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
/// @brief Classe qui repr�sente l'arbre de rendu sp�cifique au projet de
///        INF2990.
///
///        Cette classe s'occupe de configurer les usines des noeuds qui
///        seront utilis�s par le projet.
///
/// @author Martin Bisson
/// @date 2007-03-23
///////////////////////////////////////////////////////////////////////////
class ArbreRenduINF2990 : public ArbreRendu
{
public:
   /// Constructeur par d�faut.
   ArbreRenduINF2990();
   /// Destructeur.
   virtual ~ArbreRenduINF2990();

   /// Initialise l'arbre de rendu � son �tat initial.
   void initialiser();


   /// La cha�ne repr�sentant le type des maillets.
   static const std::string NOM_MAILLET;
   /// La cha�ne repr�sentant le type des maillets.
   static const std::string NOM_TABLE;
   /// La cha�ne repr�sentant le type des points de control.
   static const std::string NOM_POINT_CONTROL;
   /// La cha�ne repr�sentant le type des maillets.
   static const std::string NOM_ACCELERATEUR;
   /// La cha�ne repr�sentant le type des murs.
   static const std::string NOM_MUR;
   /// La cha�ne repr�sentant le type des portails.
   static const std::string NOM_PORTAIL;
   /// La cha�ne repr�sentant le type des buts.
   static const std::string NOM_BUT;
   /// La cha�ne repr�sentant le type des rondelles.
   static const std::string NOM_RONDELLE;
   /// La cha�ne repr�sentant le type des boosters.
   static const std::string NOM_BOOSTER;
};


#endif // __ARBRE_ARBRERENDUINF2990_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
