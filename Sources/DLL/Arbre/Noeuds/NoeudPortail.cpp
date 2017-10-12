///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPortail.cpp
/// @author Nam Lesage
/// @date 2015-09-20
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudPortail.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"
#include "Logger.h"
#include "AideGL.h"
#include "Primitives.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"

opengl::Programme NoeudPortail::programme_;
opengl::Nuanceur NoeudPortail::nuanceurFragment_;
opengl::Nuanceur NoeudPortail::nuanceurSommet_;

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPortail::NoeudPortail(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPortail::NoeudPortail(const std::string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }, collider_{ 0,0,0 }, desactiver_{false}
{
	initialiserNuanceurs();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPortail::~NoeudPortail()
///
/// Ce destructeur désallouee la liste d'affichage du portail.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPortail::~NoeudPortail()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::initialiserNuanceurs()
///
/// Cette fonction permet d'initialiser les nuanceurs
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::initialiserNuanceurs()
{

	if (!programme_.estInitialise())
	{
		nuanceurFragment_.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/Base/fragment.glsl");
		nuanceurSommet_.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/Base/sommet.glsl");
		programme_.initialiser();
		programme_.attacherNuanceur(nuanceurFragment_);
		programme_.attacherNuanceur(nuanceurSommet_);
	}

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::afficherConcret(const glm::mat4& vueProjection) const
{
	// Appel à la version de la classe de base pour l'affichage des enfants.
	NoeudAbstrait::afficherConcret(vueProjection);
	
	// Révolution autour du centre.
	auto modele = glm::rotate(transformationRelative_, angleY_, glm::vec3(0, 1, 0));

	// Matrices utiles shader program
	MatricesPipeline matrices = obtenirMatricePipeline();
	matrices.matrModel = modele;

	// Affichage de la zone d'attraction
	if (Logger::obtenirInstance()->obtenirDebugPortal()) {
		glm::vec3 centre = obtenirPositionRelative();
		centre.y += 0.5f;
		opengl::Cercle cercle(centre, obtenirCollider().rayon * 3, 30, glm::vec4(1, 157.0f / 255, 101.0f / 255, 0.25));
		cercle.afficher(matrices);
	}
	
	// Affichage du modèle.
	vbo_->dessiner(vueProjection * modele, options_, matrices);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::animer(float temps) 
///
/// Cette fonction anime l'objet
///
/// @param[in] temps : Le temps entre chaque frame
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::animer(float temps) {
	// Le portail effectue un tour à toutes les 3 secondes sur l'axe des Y.
	angleY_ = fmod(angleY_ + temps / 3.0f * 2 * (float)utilitaire::PI, 2 * (float)utilitaire::PI);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn utilitaire::CylindreEnglobant NoeudPortail::obtenirCollider() const
///
/// Cette fonction obtient le collider
///
/// @return Le collider
///
////////////////////////////////////////////////////////////////////////
utilitaire::CylindreEnglobant NoeudPortail::obtenirCollider() const
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
/// @fn void NoeudPortail::calculerCollider()
///
/// Cette fonction calcule le collider
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::calculerCollider()
{
	collider_ = utilitaire::calculerCylindreEnglobant(*obtenirModele3D());
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::accepterVisiteur(VisiteurAbstrait* visiteur) {
	visiteur->visiterPortail(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::assignerOppose(NoeudPortail* portail)
///
/// Cette fonction assigne un portail opposé à un portail
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::assignerOppose(NoeudPortail* portail) {
	portailOppose_ = portail;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::obtenirOppose()
///
/// Cette fonction permet d'obtenir le portail opposé d'un portail
///
/// @return Un pointeur vers le noeud opposé.
///
////////////////////////////////////////////////////////////////////////
NoeudPortail* NoeudPortail::obtenirOppose() {
	return portailOppose_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool NoeudPortail::estDesactiver() const
///
/// Cette fonction indique si le noeud est désactivé
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
bool NoeudPortail::estDesactiver() const
{
	return desactiver_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPortail::assignerDesactiver(bool desactiver)
///
/// Cette fonction assigne l'état activé/désactivé au portail
///
/// @param[in] desactiver : Vrai si le noeud doit être désactiver, sinon False
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPortail::assignerDesactiver(bool desactiver)
{
	desactiver_ = desactiver;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
