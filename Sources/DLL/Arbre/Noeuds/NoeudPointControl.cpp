///////////////////////////////////////////////////////////////////////////////
/// @file NoeudPointControl.cpp
/// @author Nam Lesage
/// @date 2015-09-24
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudPointControl.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPointControl::NoeudPointControl(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPointControl::NoeudPointControl(const std::string& typeNoeud)
	: NoeudComposite{ typeNoeud }, symmetrie_(glm::vec3(1,1,1)), rayonModele3D_(-1.0)
{

}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPointControl::~NoeudPointControl()
///
/// Ce destructeur désallouee la liste d'affichage du point de controle.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPointControl::~NoeudPointControl()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::afficherConcret(const glm::mat4& vueProjection) const
{
	// Appel à la version de la classe de base pour l'affichage des enfants.
	NoeudComposite::afficherConcret(vueProjection);

	// Matrices utiles shader program
	MatricesPipeline matrices = obtenirMatricePipeline();

	// Affichage du modèle.
	vbo_->dessiner(vueProjection * transformationRelative_, options_, matrices);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::assignerPositionRelative(const glm::vec3& positionRelative)
{
	glm::vec3 anciennePosition = obtenirPositionRelative();
	glm::vec3 positionCorrigee = positionRelative;

	// Limiter les points selon leur zone de deplacement
	if (possedeZoneDeplacement_) {
		positionCorrigee.x = glm::clamp(positionCorrigee.x, zoneDeplacement_[0], zoneDeplacement_[1]);
		positionCorrigee.z = glm::clamp(positionCorrigee.z, zoneDeplacement_[2], zoneDeplacement_[3]);
	}

	// Appel a l'implementation de la classe de base
	NoeudAbstrait::assignerPositionRelative(positionCorrigee);

	// Update de la position des points openGl
	if (voisins_.size()) {
		ajusterPoints();
		voisins_[0]->ajusterPoints();
		voisins_[1]->ajusterPoints();
	}

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::assignerPoints(std::vector<glm::vec3*> points)
///
/// Cette fonction assigne des points de la table au noeud
///
/// @param[in] points : Points de la table
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::assignerPoints(std::vector<glm::vec3*> points)
{
	points_ = points;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::assignerVoisins(std::vector<NoeudPointControl*> voisins)
///
/// Cette fonction assigne des voisins au point de controle
///
/// @param[in] voisins : Voisins du point de controle
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::assignerVoisins(std::vector<NoeudPointControl*> voisins)
{
	voisins_ = voisins;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::assignerNoeudOppose(NoeudAbstrait * noeud, const glm::vec3& symmetrie)
///
/// Cette fonction assigne un noeud oppose au noeud present.
///
/// @param[in] noeud : Le noeud oppose
/// @param[in] symmetrie : La symmetrie a appliquee au point oppose
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::assignerNoeudOppose(NoeudAbstrait * noeud, const glm::vec3& symmetrie)
{
	noeudOppose_ = noeud;
	symmetrie_ = symmetrie;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::afficherConcret() const
///
/// Cette fonction renvoie la symmetrie
///
/// @return La symmetrie.
///
////////////////////////////////////////////////////////////////////////
const glm::vec3 NoeudPointControl::obtenirSymmetrie() const
{
	return symmetrie_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPointControl* NoeudPointControl::obtenirNoeudOppose() const
///
/// Cette fonction retourne un pointeur vers le noeud oppose
///
/// @return Un pointeur vers le noeud oppose.
///
////////////////////////////////////////////////////////////////////////
NoeudPointControl* NoeudPointControl::obtenirNoeudOppose() const
{
	return (NoeudPointControl*) noeudOppose_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::assignerBut(NoeudBut * noeud)
///
/// Cette fonction assigne un but au point de controle
///
/// @param[in] noeud : le but
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::assignerBut(NoeudBut * noeud)
{
	but_ = noeud;
	noeud->assignerPointControl(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudBut * NoeudPointControl::obtenirBut() const
///
/// Cette fonction retourne le but auquel le noeud est associe
///
/// @return Le but s'il existe, sinon nullptr.
///
////////////////////////////////////////////////////////////////////////
NoeudBut * NoeudPointControl::obtenirBut() const {
	return but_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn std::vector<NoeudPointControl*> NoeudPointControl::obtenirVoisins() const
///
/// Retourne les voisins du point de contrôle
///
/// @return Les voisins
///
////////////////////////////////////////////////////////////////////////
std::vector<NoeudPointControl*> NoeudPointControl::obtenirVoisins() const
{
	return voisins_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::assignerZoneDeplacement(const glm::vec4& zone)
///
/// Cette fonction assigne une zone dans lequel un noeud peut se deplacer
///
/// @param[in] zone : La zone de déplacement
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::assignerZoneDeplacement(const glm::vec4& zone) {
	possedeZoneDeplacement_ = true;
	zoneDeplacement_ = zone;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::assignerBooster(NoeudBooster * booster)
///
/// Cette fonction assigne un booster au noeud de control
///
/// @param[in] booster : Le booster
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::assignerBooster(NoeudBooster * booster)
{
	booster_ = booster;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::accepterVisiteur(VisiteurAbstrait* visiteur) {
	visiteur->visiterPointControl(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn double NoeudPointControl::getRayonModele3D()
///
/// Cette fonction calcule et retourne le rayon de la sphere englobante 
/// du modele 3D
///
/// @return Rayon de la spere englobante du Modele3D.
///
////////////////////////////////////////////////////////////////////////
double NoeudPointControl::getRayonModele3D() {
	if (rayonModele3D_ == -1.0)
		rayonModele3D_ = utilitaire::calculerSphereEnglobante(*modele_).rayon;
	return rayonModele3D_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPointControl::ajusterPoints()
///
/// Cette fonction ajuste la position des points reliés au point de controle.
/// Essayez meme pas de la comprendre
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudPointControl::ajusterPoints()
{
	// Ajustment des points triviaux
	glm::vec3 position = obtenirPositionRelative();
	points_[0]->x = position.x; points_[0]->z = position.z;
	points_[1]->x = position.x; points_[1]->z = position.z;

	glm::vec3 v1 = glm::normalize(voisins_[0]->obtenirPositionRelative() - position);
	glm::vec3 v2 = glm::normalize(voisins_[1]->obtenirPositionRelative() - position);

	// Calcul de la bisectrice
	glm::vec3 bisect;
	if ( glm::length(v1 + v2) < 0.001f) { // Droite perpendiculaire
		bisect = glm::normalize(glm::vec3(-v1.z, 0, v1.x));
	}	
	else {
		bisect = glm::normalize(v1 + v2);
	}
	
	// Inverse bisect if needed
	float dotProduct = glm::clamp(glm::dot(bisect, glm::normalize(position)),-1.0f,1.0f);
	if (glm::acos(dotProduct) > utilitaire::PI/2)
		bisect = -bisect;
		
	// Calcul de la diagonale de la bordure
	float angle = utilitaire::PI/2 - glm::acos(glm::clamp(glm::dot(v1, v2),-1.0f, 1.0f))/2;
	float h =  TABLE_BORDER_WIDTH / glm::cos(angle);

	// Ajustement des points de la bordure interne
	glm::vec3 p0 = position + (bisect * h);
	points_[2]->x = p0.x; points_[2]->z = p0.z;
    points_[3]->x = p0.x; points_[3]->z = p0.z;

	glm::vec3 p4 = position + -bisect * 10.0f;
	points_[4]->x = p4.x; points_[4]->z = p4.z;

	// Update de la position du but
	if (but_) {
		but_->ajusterPoints();
	}

	// Update de la position du booster
	if (booster_ != nullptr)
	{
		glm::vec3 p5 = p4 - bisect * 5.0f;
		booster_->deplacer(p5);
	}
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
