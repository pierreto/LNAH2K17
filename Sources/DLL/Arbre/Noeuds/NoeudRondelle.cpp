///////////////////////////////////////////////////////////////////////////////
/// @file NoeudRondelle.cpp
/// @author Nam Lesage
/// @date 2015-10-13
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudRondelle.h"
#include "VisiteurAbstrait.h"
#include "VisiteurCollisionRondelle.h"
#include "VisiteurSurTable.h"
#include "Utilitaire.h"
#include "Logger.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudRondelle::NoeudRondelle(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudRondelle::NoeudRondelle(const std::string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }, collider_{ 0,0,0 }
{
	// Pour le testing
	rigidbody_ = RigidBody( glm::vec3(0, 0,0));
}



////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudRondelle::~NoeudRondelle()
///
/// Ce destructeur désallouee la liste d'affichage de la rondelle.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudRondelle::~NoeudRondelle()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRondelle::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRondelle::afficherConcret(const glm::mat4& vueProjection) const
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
/// @fn void NoeudRondelle::animer(float temps)
///
/// Cette fonction anime l'objet
///
/// @param[in] temps : Le temps entre chaque frame
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRondelle::animer(float temps)
{
	// Obtenir l'arbre de rendu
	auto arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();

	// Calculer la force de collision
	VisiteurCollisionRondelle visiteur(this);
	arbre->accepterVisiteur(&visiteur);
	
	// Ajoute les forces a l'objet
	glm::vec3 vitesse = visiteur.calculerVitesseFinale();
	rigidbody_.assignerVitesse(vitesse);
	rigidbody_.ajouterForce(visiteur.obtenirForce());

	if (visiteur.obtenirNombreCollisions() > 0)
		Logger::obtenirInstance()->afficherVitesse(glm::length(rigidbody_.obtenirVitesse()));
	
	savePosition();
	appliquerDeplacement(rigidbody_.calculerDeplacement(temps));
	
	if (!visiteur.rondelleSurTable(obtenirPositionRelative()))
		revertPosition();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn utilitaire::CylindreEnglobant NoeudRondelle::obtenirCollider()
///
/// Cette fonction obtient le collider
///
/// @return Le collider
///
////////////////////////////////////////////////////////////////////////
utilitaire::CylindreEnglobant NoeudRondelle::obtenirCollider()
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
/// @fn void NoeudRondelle::calculerCollider()
///
/// Cette fonction calcule le collider de l'accélérateur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRondelle::calculerCollider()
{
	collider_ = utilitaire::calculerCylindreEnglobant(*obtenirModele3D());
}


////////////////////////////////////////////////////////////////////////
///
/// @fn RigidBody * NoeudRondelle::obtenirRigidBody()
///
/// Obtient la physique de l'objet
///
/// @return Un pointeur vers l'objet de physique.
///
////////////////////////////////////////////////////////////////////////
RigidBody * NoeudRondelle::obtenirRigidBody()
{
	return &rigidbody_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudRondelle::NoeudRondelle(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudRondelle::accepterVisiteur(VisiteurAbstrait* visiteur) {

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
