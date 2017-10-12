///////////////////////////////////////////////////////////////////////////////
/// @file NoeudMur.cpp
/// @author Nam Lesage
/// @date 2015-09-08
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudMur.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudMur::NoeudMur(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudMur::NoeudMur(const std::string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }, collider_{ glm::dvec3(0), glm::dvec3(0) }
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudMur::~NoeudMur()
///
/// Ce destructeur désallouee la liste d'affichage du mur.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudMur::~NoeudMur()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::afficherConcret(const glm::mat4& vueProjection) const
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
/// @fn utilitaire::BoiteEnglobante NoeudMur::obtenirCollider()
///
/// Cette fonction obtient le collider
///
/// @return Le collider
///
////////////////////////////////////////////////////////////////////////
utilitaire::BoiteEnglobante NoeudMur::obtenirCollider()
{
	return collider_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::calculerCollider()
///
/// Cette fonction calcule le collider du mur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::calculerCollider()
{
	collider_ = utilitaire::calculerBoiteEnglobante(*obtenirModele3D());
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudMur::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudMur::accepterVisiteur(VisiteurAbstrait* visiteur) {
	visiteur->visiterMur(this);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
