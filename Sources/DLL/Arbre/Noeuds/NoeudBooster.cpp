///////////////////////////////////////////////////////////////////////////////
/// @file NoeudBooster.cpp
/// @author Nam Lesage
/// @date 2016-11-24
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudBooster.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"
#include "Vue.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudBooster::NoeudBooster(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudBooster::NoeudBooster(const std::string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }
{
	particleGenerator_ = new FireEffect();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudBooster::~NoeudBooster()
///
/// Ce destructeur désallouee la liste d'affichage de l'accélérateur.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudBooster::~NoeudBooster()
{
	delete particleGenerator_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBooster::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBooster::afficherConcret(const glm::mat4& vueProjection) const
{
	MatricesPipeline matrices =  obtenirMatricePipeline();

	// Affichage du modèle.
	vbo_->dessiner(vueProjection * transformationRelative_, options_, matrices);

	// Afficher les particules de feu
	particleGenerator_->renderFire(glm::vec3(0, -5.0 ,0), 7.5f, 2.5f, 0.5f, 75.0f, 30.0f, matrices);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBooster::animer(float temps) 
///
/// Cette fonction anime l'objet
///
/// @param[in] temps : Le temps entre chaque frame
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBooster::animer(float temps)
{
}



////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBooster::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBooster::accepterVisiteur(VisiteurAbstrait* visiteur) {

}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
