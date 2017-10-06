///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtat.h
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_STATES_MODELEETAT_H__
#define __APPLICATION_STATES_MODELEETAT_H__

class FacadeModele;

#include "VueOrtho.h"
#include "ArbreRenduINF2990.h"
#include "VisiteurTypes.h"
#include "NoeudAbstrait.h"
#include "Raycast.h"

#include <cmath>
#include <vector>
#include <algorithm>

///////////////////////////////////////////////////////////////////////////
/// @class ModeleEtat
/// @brief Classe de base du patron State
///
///        Cette classe abstraite comprend l'interface de base que doivent
///        implanter tous les �tats possibles du mod�le
///
/// @author Nam Lesage
/// @date 2016-09-13
///////////////////////////////////////////////////////////////////////////
class ModeleEtat
{
public:
	/// Constructeur.
	ModeleEtat();

	/// Destructeur.
	virtual ~ModeleEtat() = 0;

	virtual void initialiser() {};

	// Fonctions g�rant les entr�es de l'utilisateur
	/// Effectue l'action pour les fleches directionnelles (cela inclut w,a,s,d)
	virtual void fleches(double x, double y);
	/// Effectue l'action pour la touche escape
	virtual void escape() {};
	/// Effectue l'action de la touche espace
	virtual void space() {};
	/// Effectue l'action pour un zoomIn (+)
	virtual void zoomIn() {};
	/// Effectue l'action pour un zoomOut (-)
	virtual void zoomOut() {};
	/// Annule l'action en cours
	virtual void nettoyerEtat() {};

	/// �v�nement appel� lorsque le bouton gauche de la souris est descendu
	virtual void mouseDownL();
	/// �v�nement appel� lorsque le bouton droit de la souris est descendu
	virtual void mouseDownR();
	/// �v�nement appel� lorsque le bouton gauche de la souris est lev�
	virtual void mouseUpL();
	/// �v�nement appel� lorsque le bouton droit de la souris est lev�
	virtual void mouseUpR();
	/// �v�nement appel� lorsque la souris bouge
	virtual void playerMouseMove(int x, int y);
	/// �v�nement appel� lorsque les touches ctrl ou/et alt sont appuy�es
	virtual void modifierKeys(bool alt, bool ctrl);
	/// Fonction pour d�terminer si le curseur est au-dessus de la table
	bool mouseOverTable();
	/// Fonction pour d�terminer si le curseur est au-dessus d'un point de controle
	bool mouseOverControlPoint();
	/// Afficher openGl
	virtual void afficher() {};
	/// Verifie que les objets sont sur la table
	bool noeudsSurLaTable() const;

protected:
	
	/// Position de la souris en x
	int mousePosX_;
	/// Position de la souris en y
	int mousePosY_;
	/// Ancienne position de la souris en x
	int lastMousePosX_;
	/// Ancienne position de la souris en y
	int lastMousePosY_;
	/// Position initiale d'un clic de la souris en x
	int initMousePosX_;
	/// Position initiale d'un clic de la souris en y
	int initMousePosY_;
	/// Bool�en pour savoir si le bouton gauche de la souris est appuy�
	bool mouseDownL_;
	/// Bool�en pour savoir si le bouton droit de la souris est appuy�
	bool mouseDownR_;
	/// Bool�en pour savoir si le bouton alt est appuy�
	bool altDown_;
	/// Bool�en pour savoir si le bouton ctrl est appuy�
	bool ctrlDown_;
	/// Fonction pour savoir si on est dans un �tat de clic
	bool isAClick();

	/// Fonction pour obtenir la vue
	vue::Vue* obtenirVue();
};


#endif // __APPLICATION_STATES_MODELEETAT_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
