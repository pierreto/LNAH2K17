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
/// @brief Classe qui constitue une interface (une façade) sur l'ensemble
///        du modèle et des classes qui le composent.
///
/// @author Martin Bisson
/// @date 2007-02-20
///////////////////////////////////////////////////////////////////////////
class FacadeModele {
public:
   /// Obtient l'instance unique de la classe.
   static FacadeModele* obtenirInstance();
   /// Libère l'instance unique de la classe.
   static void libererInstance();

   /// Crée un contexte OpenGL et initialise celui-ci.
   void initialiserOpenGL(HWND hWnd);
   /// Charge la configuration à partir d'un fichier XML.
   void chargerConfiguration() const;
   /// Enregistre la configuration courante dans un fichier XML.
   void enregistrerConfiguration() const;
   /// Libère le contexte OpenGL.
   void libererOpenGL();
   /// Affiche le contenu du modèle.
   void afficher() const;
   /// Affiche la base du contenu du modèle.
   void afficherBase() const;
 
   /// Retourne la vue courante.
   inline vue::Vue* obtenirVue();
   /// Retourne l'arbre de rendu.
   inline const ArbreRenduINF2990* obtenirArbreRenduINF2990() const;
   /// Retourne l'arbre de rendu.
   inline ArbreRenduINF2990* obtenirArbreRenduINF2990();

   /// Réinitialise la scène.
   void reinitialiser();
   /// Anime la scène.
   void animer(float temps);
   /// Change l'état du modlèle

   // Fonctions gérant les entrées de l'utilisateur (Implémentation provenant de l'état du modèle)

   /// Déplace la vue selon x, y
   void fleches(double x, double y);
   /// Touche escape est appuyée
   void escape();
   /// Touche d'espacement est appuyée
   void space();
   /// Modifie le modèle état en cours
   void changerModeleEtat(MODELE_ETAT etat);
   /// Clic gauche enfoncé souris
   void mouseDownL();
   /// Clic droit enfoncé souris
   void mouseDownR();
   /// Clic gauche relaché souris
   void mouseUpL();
   /// Clic droit relaché souris
   void mouseUpR();
   /// Le curseur souris est déplacé
   void playerMouseMove(int x, int y);
   /// Ajuste l'état des touches modificatrices (alt et ctrl)
   void modifierKeys(bool alt, bool ctrl);
   /// Retourne l'information sur un noeud sélectionné
   bool selectedNodeInfos(float infos[]);
   /// Applique l'information sur un noeud sélectionné
   void applyNodeInfos(float infos[]);
   /// Remplis le parametre map avec la representation json d'une carte
   void getMapJson(float coefficients[], char* map);
   /// Enregistrer l'arbre en cours dans un nouveau fichier
   void enregistrerSous(std::string filePath, float coefficients[]);
   /// Charger un fichier d'enregistrement de l'arbre
   void ouvrir(std::string filePath, float coefficients[]);
   /// Charger un fichier d'enregistrement de l'arbre
   void chargerCarte(std::string json, float coefficients[]);
   /// Vérifie si le curseur est au dessus de la table
   bool mouseOverTable();
   /// Vérifie si le curseur est au dessus d'un point de contrôle
   bool mouseOverControlPoint();
   /// Supprime la sélection en cours
   void deleteSelection();
   /// Changer la visibilité de la grid
   void changeGridVisibility(bool visibility);
   /// Reinitialise la vue
   void resetCameraPosition();
   /// Gere les creations et suppressions de la rondelle et des maillets
   void gererRondelleMaillets(bool toggle);
   /// Change la visibilité des points de contôles
   void toggleControlPointsVisibility(bool visible);
   /// Empeche de raffrachir la scene durant le rectangle élastique
   inline void changerAffichageRectangle(bool estActif);
   /// Charge tous les sons du jeu
   void loadSounds();
   /// Active ou desactive la musique de fond
   void playMusic(bool quickPlay);

   ///Active ou desactive le mode orbite de la caméra
   void toggleOrbit(bool orbit);

   void rotateCamera(float angle);

   bool modeOrbite_;


   /// Document rapidJSON
   rapidjson::Document docJSON_;

private:
   /// Constructeur par défaut.
	FacadeModele();
   /// Destructeur.
   ~FacadeModele();
   /// Constructeur copie désactivé.
   FacadeModele(const FacadeModele&) = delete;
   /// Opérateur d'assignation désactivé.
   FacadeModele& operator =(const FacadeModele&) = delete;

   /// Nom du fichier XML dans lequel doit se trouver la configuration.
   static const std::string FICHIER_CONFIGURATION;

   /// Pointeur vers l'instance unique de la classe.
   static FacadeModele* instance_;

   /// Poignée ("handle") vers la fenêtre où l'affichage se fait.
   HWND  hWnd_{ nullptr };
   /// Poignée ("handle") vers le contexte OpenGL.
   HGLRC hGLRC_{ nullptr };
   /// Poignée ("handle") vers le "device context".
   HDC   hDC_{ nullptr };

   /// Vue courante de la scène.
   vue::Vue* vue_{ nullptr };
   /// Arbre de rendu contenant les différents objets de la scène.
   ArbreRenduINF2990* arbre_{ nullptr };
   /// Gestionnaire de lumiere de la scene
   light::LightManager* lightManager_{ nullptr };

   /// Etat du modèle
   ModeleEtat* etat_{ nullptr };
   /// Etat de l'affichage du rectangle élastique
   bool rectangleActif_;
   /// Permet de créer des noeud selon le type chosi
   void creerNoeuds(char* type, std::string nomType);
   /// Permet de charge les points de contrôle sur la table
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
/// élastique lorsque la valeur est true. 
///
/// @param[in] estAffiche : Etat du rectangle de sélection
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
/// Cette fonction retourne la vue qui est présentement utilisée pour
/// voir la scène.
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
/// Cette fonction retourne l'arbre de rendu de la scène (version constante
/// de la fonction).
///
/// @return L'arbre de rendu de la scène.
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
/// Cette fonction retourne l'arbre de rendu de la scène (version non constante
/// de la fonction).
///
/// @return L'arbre de rendu de la scène.
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
