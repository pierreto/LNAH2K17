//////////////////////////////////////////////////////////////////////////////
/// @file FacadeModele.h
/// @author DGI
/// @date 2005-06-15
/// @version 1.0 
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_FACADEMODELE_H__
#define __APPLICATION_FACADEMODELE_H__


#include <windows.h>
#include <string>
#include <iostream>
#include <cstdio>
#include <fstream>

#include "rapidjson/document.h" // rapidjson's DOM-style API
#include "rapidjson/writer.h" // for stringify JSON
#include "rapidjson/stringbuffer.h"
#include "rapidjson/prettywriter.h"
#include "rapidjson/filewritestream.h"
#include "rapidjson/filereadstream.h"
#include "BoiteEnvironnement.h"

class NoeudAbstrait;
class ArbreRenduINF2990;
class ModeleEtat;


namespace vue {
   class Vue;
}

namespace light {
	class LightManager;
}

enum class MODELE_ETAT {
	AUCUN = 0,
	SELECTION = 1,
	CREATION_ACCELERATEUR = 2,
	CREATION_MURET = 3,
	CREATION_PORTAIL = 4,
	DEPLACEMENT = 5,
	ROTATION = 6,
	MISE_A_ECHELLE = 7,
	DUPLIQUER = 8,
	ZOOM = 9,
	POINTS_CONTROLE = 10,
	JEU = 11,
	JEU_ONLINE=12
};


///////////////////////////////////////////////////////////////////////////
/// @class FacadeModele
/// @brief Classe qui constitue une interface (une fa�ade) sur l'ensemble
///        du mod�le et des classes qui le composent.
///
/// @author Martin Bisson
/// @date 2007-02-20
///////////////////////////////////////////////////////////////////////////
class FacadeModele {
public:
   /// Obtient l'instance unique de la classe.
   static FacadeModele* obtenirInstance();
   /// Lib�re l'instance unique de la classe.
   static void libererInstance();

   /// Cr�e un contexte OpenGL et initialise celui-ci.
   void initialiserOpenGL(HWND hWnd);
   /// Charge la configuration � partir d'un fichier XML.
   void chargerConfiguration() const;
   /// Enregistre la configuration courante dans un fichier XML.
   void enregistrerConfiguration() const;
   /// Lib�re le contexte OpenGL.
   void libererOpenGL();
   /// Affiche le contenu du mod�le.
   void afficher() const;
   /// Affiche la base du contenu du mod�le.
   void afficherBase() const;
 
   /// Retourne la vue courante.
   inline vue::Vue* obtenirVue();
   /// Retourne l'arbre de rendu.
   inline const ArbreRenduINF2990* obtenirArbreRenduINF2990() const;
   /// Retourne l'arbre de rendu.
   inline ArbreRenduINF2990* obtenirArbreRenduINF2990();

   /// R�initialise la sc�ne.
   void reinitialiser();
   /// Anime la sc�ne.
   void animer(float temps);
   /// Change l'�tat du modl�le

   // Fonctions g�rant les entr�es de l'utilisateur (Impl�mentation provenant de l'�tat du mod�le)

   /// D�place la vue selon x, y
   void fleches(double x, double y);
   /// Touche escape est appuy�e
   void escape();
   /// Touche d'espacement est appuy�e
   void space();
   /// Modifie le mod�le �tat en cours
   void changerModeleEtat(MODELE_ETAT etat);
   /// Clic gauche enfonc� souris
   void mouseDownL();
   /// Clic droit enfonc� souris
   void mouseDownR();
   /// Clic gauche relach� souris
   void mouseUpL();
   /// Clic droit relach� souris
   void mouseUpR();
   /// Le curseur souris est d�plac�
   void playerMouseMove(int x, int y);
   /// Ajuste l'�tat des touches modificatrices (alt et ctrl)
   void modifierKeys(bool alt, bool ctrl);
   /// Retourne l'information sur un noeud s�lectionn�
   bool selectedNodeInfos(float infos[]);
   /// Applique l'information sur un noeud s�lectionn�
   void applyNodeInfos(float infos[]);
   /// Remplis le parametre map avec la representation json d'une carte
   void getMapJson(float coefficients[], char* map);
   /// Enregistrer l'arbre en cours dans un nouveau fichier
   void enregistrerSous(std::string filePath, float coefficients[]);
   /// Charger un fichier d'enregistrement de l'arbre
   void ouvrir(std::string filePath, float coefficients[]);
   /// Charger un fichier d'enregistrement de l'arbre
   void chargerCarte(std::string json, float coefficients[]);
   /// V�rifie si le curseur est au dessus de la table
   bool mouseOverTable();
   /// V�rifie si le curseur est au dessus d'un point de contr�le
   bool mouseOverControlPoint();
   /// Supprime la s�lection en cours
   void deleteSelection();
   /// Changer la visibilit� de la grid
   void changeGridVisibility(bool visibility);
   /// Reinitialise la vue
   void resetCameraPosition();
   /// Gere les creations et suppressions de la rondelle et des maillets
   void gererRondelleMaillets(bool toggle);
   /// Change la visibilit� des points de cont�les
   void toggleControlPointsVisibility(bool visible);
   /// Empeche de raffrachir la scene durant le rectangle �lastique
   inline void changerAffichageRectangle(bool estActif);
   /// Charge tous les sons du jeu
   void loadSounds();
   /// Active ou desactive la musique de fond
   void playMusic(bool quickPlay);

   ///Active ou desactive le mode orbite de la cam�ra
   void toggleOrbit(bool orbit);

   void rotateCamera(float angle);

   bool modeOrbite_;


   /// Document rapidJSON
   rapidjson::Document docJSON_;

private:
   /// Constructeur par d�faut.
	FacadeModele();
   /// Destructeur.
   ~FacadeModele();
   /// Constructeur copie d�sactiv�.
   FacadeModele(const FacadeModele&) = delete;
   /// Op�rateur d'assignation d�sactiv�.
   FacadeModele& operator =(const FacadeModele&) = delete;

   /// Nom du fichier XML dans lequel doit se trouver la configuration.
   static const std::string FICHIER_CONFIGURATION;

   /// Pointeur vers l'instance unique de la classe.
   static FacadeModele* instance_;

   /// Poign�e ("handle") vers la fen�tre o� l'affichage se fait.
   HWND  hWnd_{ nullptr };
   /// Poign�e ("handle") vers le contexte OpenGL.
   HGLRC hGLRC_{ nullptr };
   /// Poign�e ("handle") vers le "device context".
   HDC   hDC_{ nullptr };

   /// Vue courante de la sc�ne.
   vue::Vue* vue_{ nullptr };
   /// Arbre de rendu contenant les diff�rents objets de la sc�ne.
   ArbreRenduINF2990* arbre_{ nullptr };
   /// Gestionnaire de lumiere de la scene
   light::LightManager* lightManager_{ nullptr };

   /// Etat du mod�le
   ModeleEtat* etat_{ nullptr };
   /// Etat de l'affichage du rectangle �lastique
   bool rectangleActif_;
   /// Permet de cr�er des noeud selon le type chosi
   void creerNoeuds(char* type, std::string nomType);
   /// Permet de charge les points de contr�le sur la table
   void chargerPntCtrl();

   /// Retourne la representation Json d'une carte
   std::string _getMapJson(float coefficients[]);

   utilitaire::BoiteEnvironnement* skybox_;
};


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void FacadeModele::changerAffichageRectangle(bool estAffiche)
///
/// Cette fonction empeche de raffrachir la scene durant le rectangle 
/// �lastique lorsque la valeur est true. 
///
/// @param[in] estAffiche : Etat du rectangle de s�lection
///
/// @return Rien.
///
////////////////////////////////////////////////////////////////////////
inline void FacadeModele::changerAffichageRectangle(bool estAffiche) {
	rectangleActif_ = estAffiche;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline vue::Vue* FacadeModele::obtenirVue()
///
/// Cette fonction retourne la vue qui est pr�sentement utilis�e pour
/// voir la sc�ne.
///
/// @return La vue courante.
///
////////////////////////////////////////////////////////////////////////
inline vue::Vue* FacadeModele::obtenirVue()
{
   return vue_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline const ArbreRenduINF2990* FacadeModele::obtenirArbreRenduINF2990() const
///
/// Cette fonction retourne l'arbre de rendu de la sc�ne (version constante
/// de la fonction).
///
/// @return L'arbre de rendu de la sc�ne.
///
////////////////////////////////////////////////////////////////////////
inline const ArbreRenduINF2990* FacadeModele::obtenirArbreRenduINF2990() const
{
   return arbre_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline ArbreRenduINF2990* FacadeModele::obtenirArbreRenduINF2990()
///
/// Cette fonction retourne l'arbre de rendu de la sc�ne (version non constante
/// de la fonction).
///
/// @return L'arbre de rendu de la sc�ne.
///
////////////////////////////////////////////////////////////////////////
inline ArbreRenduINF2990* FacadeModele::obtenirArbreRenduINF2990()
{
   return arbre_;
}


#endif // __APPLICATION_FACADEMODELE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
