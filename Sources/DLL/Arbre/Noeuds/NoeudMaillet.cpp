///////////////////////////////////////////////////////////////////////////////
/// @file NoeudMaillet.cpp
/// @author Nam Lesage
/// @date 2015-09-08
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudMaillet.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudMaillet::NoeudMaillet(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudMaillet::NoeudMaillet(const std::string& typeNoeud, const char* uuid)
	: NoeudAbstrait{ typeNoeud }, collider_{ 0,0,0 }, vitesse_(0), lastPosition_(0)
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudMaillet::~NoeudMaillet()
///
/// Ce destructeur désallouee la liste d'affichage du maillet.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudMaillet::~NoeudMaillet()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMaillet::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMaillet::afficherConcret(const glm::mat4& vueProjection) const
{
	// Appel à la version de la classe de base pour l'affichage des enfants.
	NoeudAbstrait::afficherConcret(vueProjection);

	// Matrices utiles shader program
	MatricesPipeline matrices = obtenirMatricePipeline();

	// Affichage du modèle.
	vbo_->dessiner(vueProjection * transformationRelative_, options_, matrices);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMaillet::animer(float temps)
///
/// Cette fonction anime l'objet
///
/// @param[in] temps : Le temps entre chaque frame
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMaillet::animer(float temps)
{
	vitesse_ = (obtenirPositionRelative() - lastPosition_) / temps;
	if (glm::length(vitesse_) > VITESSE_MAX) {
		vitesse_ = VITESSE_MAX * glm::normalize(vitesse_);
	}
	lastPosition_ = obtenirPositionRelative();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 NoeudMaillet::obtenirVitesse() const
///
/// Cette fonction obtient la vitesse
///
/// @return Le vecteur de vitesse.
///
////////////////////////////////////////////////////////////////////////
glm::vec3 NoeudMaillet::obtenirVitesse() const {
	return vitesse_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn int NoeudMaillet::obtenirId() const
///
/// Cette fonction obtient l'id du maillet
///
/// @return L'Id du maillet.
///
////////////////////////////////////////////////////////////////////////
int NoeudMaillet::obtenirId() const {
	return id_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMaillet::assignerId(const int& id)
///
/// Cette fonction obtient l'Id du maillet
///
/// @param[in] id : L'Id du maillet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMaillet::assignerId(const int& id) {
	id_ = id;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn utilitaire::CylindreEnglobant NoeudMaillet::obtenirCollider()
///
/// Cette fonction obtient le collider
///
/// @return Le collider
///
////////////////////////////////////////////////////////////////////////
utilitaire::CylindreEnglobant NoeudMaillet::obtenirCollider()
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
/// @fn void NoeudMaillet::calculerCollider()
///
/// Cette fonction calcule le collider du maillet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMaillet::calculerCollider()
{
	collider_ = utilitaire::calculerCylindreEnglobant(*obtenirModele3D());
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMaillet::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMaillet::accepterVisiteur(VisiteurAbstrait* visiteur) {
	visiteur->visiterMaillet(this);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
