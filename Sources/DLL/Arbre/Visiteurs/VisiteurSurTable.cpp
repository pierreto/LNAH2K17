///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSurTable.cpp
/// @author Nam Lesage
/// @date 2016-10-1
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurSurTable.h"
#include "FacadeModele.h"
#include "ArbreRenduINF2990.h"

#include "Modele3D.h"
#include "Mesh.h"

#include <set>
#include <algorithm>

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSurTable::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Constructeur de VisiteurSurTable
///
/// @param[in] Aucun
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
VisiteurSurTable::VisiteurSurTable()
	: sontSurTable_(true)
{
	table_ = (NoeudTable*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_TABLE);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSurTable::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Destructeur de VisiteurSurTable
///
/// @param[in] Aucun
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
VisiteurSurTable::~VisiteurSurTable() {

}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSurTable::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction test si un accelerateur est sur la table
///
/// @param[in] noeud : Un accelerateur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::visiterAccelerateur(NoeudAccelerateur* noeud) {
	if (table_ != nullptr) {
		testPoints(noeud);
		intersectCercleTable(noeud, 3.75);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSurTable::visiterMaillet(NoeudMaillet* noeud)
///
/// Cette fonction test si un maillet est sur la table
///
/// @param[in] noeud : Un maillet
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::visiterMaillet(NoeudMaillet* noeud) {
	sontSurTable_ = true;
	if (table_ != nullptr) {
		if (!pointSurTable(noeud->obtenirPositionRelative()) || (noeud->obtenirId() == 1 && noeud->obtenirPositionRelative().z + noeud->obtenirCollider().rayon > 0) || (noeud->obtenirId() == 2 && noeud->obtenirPositionRelative().z - noeud->obtenirCollider().rayon < 0))
			sontSurTable_ = false;
		else {
			intersectCercleTable(noeud, noeud->obtenirCollider().rayon);
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::visiterTable(NoeudTable* noeud)
///
/// Cette fonction ne fait rien
///
/// @param[in] noeud : Une table
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::visiterTable(NoeudTable* noeud) {
	// NE RIEN FAIRE
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::visiterPointControl(NoeudPointControl * noeud)
///
/// Cette fonction ne fait rien
///
/// @param[in] noeud : Un point de controle
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::visiterPointControl(NoeudPointControl * noeud) {
	// DO NOTHING
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::visiterMur(NoeudMur* noeud)
///
/// Cette fonction test si un mur est sur la table
///
/// @param[in] noeud : Un mur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::visiterMur(NoeudMur* noeud) {

	if (table_ != nullptr) {
		// Test de l'intersection d'un cube avec la table
		testPoints(noeud);
		intersectCubeTable(noeud);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction test si un portail est sur la table
///
/// @param[in] noeud : Un portail
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::visiterPortail(NoeudPortail* noeud) {
	if (table_ != nullptr) {
		// Test de l'intersection d'un cube avec la table
		testPoints(noeud);
		intersectCercleTable(noeud, 7);
	}
}

void VisiteurSurTable::visiterRondelle(NoeudRondelle * noeud)
{
	if (table_ != nullptr) {
		if (!pointSurTable(noeud->obtenirPositionRelative()))
			sontSurTable_ = false;
		else {
			intersectCercleTable(noeud, noeud->obtenirCollider().rayon);
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool VisiteurSurTable::sontSurTable() const
///
/// Cette fonction test si un mur est sur la table
///
/// @param[in] Aucun
///
/// @return Vrai si les objets sont sur la table, faux autrement.
///
////////////////////////////////////////////////////////////////////////
bool VisiteurSurTable::sontSurTable() const
{
	return sontSurTable_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::testPoints(NoeudAbstrait * noeud)
///
/// Cette fonction test si les points d'un noeud sont tous sur la table
///
/// @param[in] noeud : Un noeud
///
/// @return Vrai si les points sont sur la table, faux autrement.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::testPoints(NoeudAbstrait * noeud)
{
	// Ramasser les sommets du noeuds
	std::vector<glm::vec3> sommets = obtenirSommets(noeud);

	// Appliquer les transformations sur les points
	for (auto& sommet : sommets) {
		glm::vec4 sommetTransforme(sommet.x, sommet.y, sommet.z, 1);
		sommetTransforme = noeud->obtenirMatriceTransformation() * sommetTransforme;
		sommet = glm::vec3(sommetTransforme.x, sommetTransforme.y, sommetTransforme.z);
	}

	// Ajouter le centre du noeud
	sommets.push_back(noeud->obtenirPositionRelative());

	// Enleve les doublons
	enleverDoublons(sommets);

	// Verifier que tous les noeuds sont sur la table
	for (auto& sommet : sommets ) {
		if (!pointSurTable(sommet))
		{
			sontSurTable_ = false;
			return;
		}		
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn std::vector<glm::vec3> VisiteurSurTable::obtenirSommets(NoeudAbstrait * noeud) const
///
/// Cette fonction retourne les points d'un noeud de l'arbre de rendu
///
/// @param[in] noeud : Un noeud
///
/// @return Un vecteur de sommets.
///
////////////////////////////////////////////////////////////////////////
std::vector<glm::vec3> VisiteurSurTable::obtenirSommets(NoeudAbstrait * noeud) const {
	return obtenirSommets(noeud->obtenirModele3D()->obtenirNoeudRacine());
}

////////////////////////////////////////////////////////////////////////
///
/// @fn std::vector<glm::vec3> VisiteurSurTable::obtenirSommets(const modele::Noeud& noeud) const
///
/// Cette fonction retourne les points d'un noeud du modèle 3D
///
/// @param[in] noeud : Un noeud
///
/// @return Un vecteur de sommets.
///
////////////////////////////////////////////////////////////////////////
std::vector<glm::vec3> VisiteurSurTable::obtenirSommets(const modele::Noeud& noeud) const {

	std::vector<glm::vec3> sommets;
	
	// Ajouter les meshs du noeuds
	for (auto mesh : noeud.obtenirMeshes()) {
		sommets.insert(sommets.end(), mesh.obtenirSommets().begin(), mesh.obtenirSommets().end());
	}
	
	// Appeler recursivement la methode sur les enfants
    const std::vector<modele::Noeud>& enfants = noeud.obtenirEnfants();
	
	for (int i = 0; i < enfants.size(); i++)
	{
		auto sommetsEnfant = obtenirSommets(enfants[i]);
		sommets.insert(sommets.end(), sommetsEnfant.begin(), sommetsEnfant.end());
	}
	
	return sommets;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::intersectCubeTable(NoeudAbstrait* noeud)
///
/// Cette fonction trouve si les segments d'un cube croise le contour de la
/// table.
///
/// @param[in] noeud : Un noeud
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::intersectCubeTable(NoeudAbstrait* noeud) {

	// Les sommets de la table
	std::vector<glm::vec3> sommetsTable = table_->obtenirSommetsPatinoire();

	// Recherche des points pertinents
	utilitaire::BoiteEnglobante c = utilitaire::calculerBoiteEnglobante(*noeud->obtenirModele3D());
	std::vector<glm::vec3> sommets;
	sommets.push_back(glm::vec3(c.coinMin.x, 0, c.coinMin.z));
	sommets.push_back(glm::vec3(c.coinMin.x, 0, c.coinMax.z));
	sommets.push_back(glm::vec3(c.coinMax.x, 0, c.coinMax.z));
	sommets.push_back(glm::vec3(c.coinMax.x, 0, c.coinMin.z));

	// Appliquer les transformations sur les points
	for (auto& sommet : sommets) {
		glm::vec4 sommetTransforme(sommet.x, sommet.y, sommet.z, 1);
		sommetTransforme = noeud->obtenirMatriceTransformation() * sommetTransforme;
		sommet = glm::vec3(sommetTransforme.x, sommetTransforme.y, sommetTransforme.z);		
	}

	for (int i = 1; i < sommetsTable.size(); ++i) {
		//(aidecollision::pointDansTriangle(sommet, sommetsTable[0], sommetsTable[i], sommetsTable[(i % (sommetsTable.size() - 1)) + 1])) 
		for (int j = 0; j < sommets.size(); ++j) {
			if (utilitaire::segmentsIntersect(sommetsTable[i], sommetsTable[(i % (sommetsTable.size() - 1)) + 1], sommets[j], sommets[(j + 1) % sommets.size()])) {
				sontSurTable_ = false;
			}
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::intersectBoostTable(NoeudAbstrait * noeud)
///
/// Cette fonction trouve si un accélérateur croise les côtés de la table
///
/// @param[in] noeud : Un noeud
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::intersectCercleTable(NoeudAbstrait * noeud, float rayon) {
	// TODO : Rendre la fonction générique en trouvant le rayon 
	// Les sommets de la table
	std::vector<glm::vec3> sommetsTable = table_->obtenirSommetsPatinoire();

	// Obtenir le scale maximal
	glm::vec3 scale = noeud->obtenirScale();
	float scaleValues[] = { scale.x,scale.y,scale.z };
	float max_scale = *std::max_element(scaleValues, scaleValues + 3);

	// Rayon et centre du cercle
	//utilitaire::SphereEnglobante sphere = utilitaire::calculerSphereEnglobante(*noeud->obtenirModele3D());
	rayon = rayon * max_scale;

	glm::vec3 centre = noeud->obtenirPositionRelative();

	// Effectuer le test
	for (int i = 1; i < sommetsTable.size(); ++i) {
		if (utilitaire::segmentCercleIntersect(sommetsTable[i], sommetsTable[(i % (sommetsTable.size() - 1)) + 1], centre, rayon) ) {
			sontSurTable_ = false;
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool VisiteurSurTable::pointSurTable(const glm::vec3 & sommet)
///
/// Cette fonction trouve si un point est situé sur la table
///
/// @param[in] sommet : Un sommet
///
/// @return Vrai si le point est sur la table, faux autrement.
///
////////////////////////////////////////////////////////////////////////
bool VisiteurSurTable::pointSurTable(const glm::vec3 & sommet)
{
	std::vector<glm::vec3> sommetsTable = table_->obtenirSommetsPatinoire();

	// Si le point est dans l'un des triangles formant la table, on retourne vrai
	for (int i = 1; i < sommetsTable.size(); ++i) {
		if (aidecollision::pointDansTriangle(sommet, sommetsTable[0], sommetsTable[i], sommetsTable[(i % (sommetsTable.size() - 1)) + 1])) {
			return true;
		}
	}

	return false;
}

void VisiteurSurTable::reinitialiser()
{
	sontSurTable_ = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSurTable::enleverDoublons(std::vector<glm::vec3>& vec)
///
/// Cette fonction enleve les doublons pour un vecteur de sommets
///
/// @param[in,out] vec : Vecteur de sommets
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSurTable::enleverDoublons(std::vector<glm::vec3>& vec) const
{
	auto is_smaller_than = [](glm::vec3 first, glm::vec3 second) -> bool
	{
		if (first.x < second.x) {
			return true;
		}
		else if (first.x == second.x) {
			if (first.y < second.y) {
				return true;
			}
			else if (first.y == second.y) {
				if (first.z < second.z) {
					return true;
				}
			}
		}
		return false;
	};

	std::sort(vec.begin(), vec.end(), is_smaller_than);
	vec.erase(std::unique(vec.begin(), vec.end()), vec.end());
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
