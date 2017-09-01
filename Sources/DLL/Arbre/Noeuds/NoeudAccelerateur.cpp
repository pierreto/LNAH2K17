///////////////////////////////////////////////////////////////////////////////
/// @file NoeudAccelerateur.cpp
/// @author Nam Lesage
/// @date 2015-09-08
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudAccelerateur.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"
#include "Vue.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAccelerateur::NoeudAccelerateur(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudAccelerateur::NoeudAccelerateur(const std::string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }, collider_{0,0,0}, desactiver_(false)
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAccelerateur::~NoeudAccelerateur()
///
/// Ce destructeur d�sallouee la liste d'affichage de l'acc�l�rateur.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudAccelerateur::~NoeudAccelerateur()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAccelerateur::afficherConcret() const
///
/// Cette fonction effectue le v�ritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le mod�le � sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAccelerateur::afficherConcret(const glm::mat4& vueProjection) const
{
	// Appel � la version de la classe de base pour l'affichage des enfants.
	NoeudAbstrait::afficherConcret(vueProjection);

	// R�volution autour du centre.
	auto modele = glm::rotate(transformationRelative_, angleY_, glm::vec3(0, 1, 0));

	MatricesPipeline matrices =  obtenirMatricePipeline();
	matrices.matrModel = modele;

	// Affichage du mod�le.
	vbo_->dessiner(vueProjection * modele, options_, matrices);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAccelerateur::animer(float temps) 
///
/// Cette fonction anime l'objet
///
/// @param[in] temps : Le temps entre chaque frame
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAccelerateur::animer(float temps) 
{
	// Le cube effectue un tour � toutes les 3 secondes sur l'axe des Y.
	angleY_ = fmod(angleY_ + temps / 3.0f * 2 * (float)utilitaire::PI, 2 * (float)utilitaire::PI);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAccelerateur::calculerCollider() 
///
/// Cette fonction calcule le collider de l'acc�l�rateur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAccelerateur::calculerCollider()
{
	collider_ = utilitaire::calculerCylindreEnglobant(*obtenirModele3D());
}


////////////////////////////////////////////////////////////////////////
///
/// @fn utilitaire::CylindreEnglobant NoeudAccelerateur::obtenirCollider()
///
/// Cette fonction obtient le collider
///
/// @return Le collider
///
////////////////////////////////////////////////////////////////////////
utilitaire::CylindreEnglobant NoeudAccelerateur::obtenirCollider()
{
	static float lastScale = obtenirScale().x;
	static utilitaire::CylindreEnglobant collider{ collider_.rayon * lastScale, collider_.bas, collider_.haut };

	if (lastScale != obtenirScale().x)
	{
		lastScale = obtenirScale().x;
		collider = utilitaire::CylindreEnglobant{ collider_.rayon * lastScale, collider_.bas, collider_.haut };
	}

	return collider;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAccelerateur::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAccelerateur::accepterVisiteur(VisiteurAbstrait* visiteur) {
	visiteur->visiterAccelerateur(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool NoeudAccelerateur::estDesactiver() const
///
/// Cette fonction indique si le noeud est d�sactiv�
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
bool NoeudAccelerateur::estDesactiver() const
{
	return desactiver_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAccelerateur::assignerDesactiver(bool desactiver)
///
/// Cette fonction assigne l'�tat activ�/d�sactiv� au portail
///
/// @param[in] desactiver : Vrai si le noeud doit �tre d�sactiver, sinon False
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudAccelerateur::assignerDesactiver(bool desactiver)
{
	desactiver_ = desactiver;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
