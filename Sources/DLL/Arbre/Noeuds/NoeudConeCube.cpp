///////////////////////////////////////////////////////////////////////////////
/// @file NoeudConeCube.cpp
/// @author Julien Gascon-Samson
/// @date 2011-05-19
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudConeCube.h"
#include "Visiteurs\VisiteurAbstrait.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"

#include "Utilitaire.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudConeCube::NoeudConeCube(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudConeCube::NoeudConeCube(const std::string& typeNoeud)
: NoeudAbstrait{ typeNoeud }
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudConeCube::~NoeudConeCube()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudConeCube::~NoeudConeCube()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudConeCube::afficherConcret() const
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudConeCube::afficherConcret(const glm::mat4& vueProjection) const
{
	// Révolution autour du centre.
	auto modele = glm::rotate(transformationRelative_, angleRotation_, glm::vec3(0, 0, 1));
	// Translation.
	modele = glm::translate(modele, glm::vec3(10, 0, 0));
	// Rotation autour de l'axe des X.
	modele = glm::rotate(modele, angleX_, glm::vec3(1, 0, 0));
	// Rotation autour de l'axe des Y.
	modele = glm::rotate(modele, angleY_, glm::vec3(0, 1, 0));
	// Recentrage du cube.
	modele = glm::translate(modele, glm::vec3(0, 0, -10));
	// Affichage du modèle.
	vbo_->dessiner(vueProjection * modele);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudCube::animer(float temps)
///
/// Cette fonction effectue l'animation du noeud pour un certain
/// intervalle de temps.
///
/// @param[in] temps : Intervalle de temps sur lequel faire l'animation.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudConeCube::animer(float temps)
{
	// Le cube effectue un tour à toutes les 7 secondes sur l'axe des X.
	angleX_ = fmod(angleX_ + temps / 7.0f * 2 * (float)utilitaire::PI, 2 * (float)utilitaire::PI);
	// Le cube effectue un tour à toutes les 3 secondes sur l'axe des Y.
	angleY_ = fmod(angleY_ + temps / 3.0f * 2 * (float)utilitaire::PI, 2 * (float)utilitaire::PI);
	// Le cube effectue une révolution à toutes les 15 secondes.
	angleRotation_ = fmod(angleRotation_ + temps / 15.0f * 2 * (float)utilitaire::PI, 2 * (float)utilitaire::PI);
}


void NoeudConeCube::accepterVisiteur(VisiteurAbstrait* visiteur) {

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
