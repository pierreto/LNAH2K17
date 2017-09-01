///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurCollisionRondelle.cpp
/// @author Nam Lesage
/// @date 2016-10-20
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurCollisionRondelle.h"
#include "VisiteurSurTable.h"
#include "glm\gtx\projection.hpp"
#include "ModeleEtatJeu.h"
#include "Logger.h"
#include "Audio.h"

using namespace utilitaire;
using namespace aidecollision;

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurCollisionRondelle::VisiteurCollision()
///
/// Constructeur pour VisiterCollision
///
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurCollisionRondelle::VisiteurCollisionRondelle(NoeudRondelle* rondelle)
	: rondelle_(rondelle), force_(0), nombreCollisions_(0), vitesseFinale_(rondelle->obtenirRigidBody()->obtenirVitesse()), dansMur_(false)
{
	rayonR_ = rondelle_->obtenirCollider().rayon;
	vitesseR_ = rondelle_->obtenirRigidBody()->obtenirVitesse();
	masseR_ = rondelle_->obtenirRigidBody()->obtenirMasse();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurCollisionRondelle::~VisiteurCollision()
///
/// Destructeur pour VisiteurCollision
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurCollisionRondelle::~VisiteurCollisionRondelle() {

}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurCollisionRondelle::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiter un accélérateur pour la collision.
///
///	@param[in] noeud : Un accélérateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::visiterAccelerateur(NoeudAccelerateur* noeud) {
	
	DetailsCollision details = calculerCollisionSphere(noeud->obtenirPositionRelative(),
		noeud->obtenirCollider().rayon,
		rondelle_->obtenirPositionRelative(),
		rayonR_);

	if (details.type != COLLISION_AUCUNE) {

		if (!noeud->enCollision()) {
			nombreCollisions_++;
			noeud->setEnCollision(true);
			ajouterCollision(noeud->obtenirType(), ResultatsCollision{ noeud, details, glm::vec3(0) });
			// Affichage de la collision
			Logger::obtenirInstance()->afficherCollision(std::string(noeud->obtenirType())); 
			// Son
			Audio::obtenirInstance()->playSound(SOUND_BOOSTER, DEFAULT_SOUND_VOLUME);
		}
	}
	else {
		noeud->setEnCollision(false);
	}

}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteuVisiteurCollisionRondellerCollision::visiterMaillet(NoeudMaillet* noeud)
///
/// Cette fonction permet de visiter un maillet pour la collision.
///
///	@param[in] noeud : Un maillet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::visiterMaillet(NoeudMaillet* noeud) {
    // Details du maillet
	double rayonM = noeud->obtenirCollider().rayon;
	glm::dvec3 positionM = noeud->obtenirPositionRelative();
	glm::dvec3 vitesseM = noeud->obtenirVitesse();
	double masseM = 10;	// Masse du maillet estime a 2.5kg
	
	// Calcul de la collision
	DetailsCollision collision = calculerCollisionSphere(positionM, rayonM, rondelle_->obtenirPositionRelative(), rayonR_);

	// Calcul de la direction de reflection
	if (collision.type != COLLISION_AUCUNE) {

		if (!noeud->enCollision())
		{
			// Son
			float volume = glm::length(vitesseR_ - vitesseM) / (2 * VITESSE_MAX);
			volume = max(0.05f, volume);
			Audio::obtenirInstance()->playSound(SOUND_MAILLET, volume);
			noeud->setEnCollision(true);	
		}
			
		nombreCollisions_++;
		
		// Affichage de la collision
		Logger::obtenirInstance()->afficherCollision(std::string(noeud->obtenirType()));
		
		// Axe de collision
		glm::dvec3 normal = glm::normalize(rondelle_->obtenirPositionRelative() - glm::vec3(positionM));
		// Composante de la vitesse sur l'axe de collision
		float vri = glm::dot(vitesseR_, normal);
		float vmi = glm::dot(vitesseM, normal);

		// Vitesse finale sur l'axe de collision de la rondelle
		float vrf = (vri * (masseR_ - masseM) + 2 * masseM * vmi) / (masseR_ + masseM);

		// Remplacement de la vitesse sur l'axe de collision
		glm::vec3 force = double(vrf - vri) * normal;

		ajouterCollision(noeud->obtenirType(), ResultatsCollision{ noeud, collision, force });
	}
	else
	{
		noeud->setEnCollision(false);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurCollisionRondelle::visiterTable(NoeudTable* noeud)
///
/// Cette fonction permet de visiter une table pour la collision.
///
///	@param[in] noeud : Une table
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::visiterTable(NoeudTable* noeud) {

	// Iterer sur tous les segments de la table
	std::vector<glm::vec3> bordures = obtenirBordureTable(noeud);
	
	DetailsCollision collision; glm::vec3 force(0);
	std::vector<ResultatsCollision> resultats;
	for (int i = 0; i < bordures.size(); ++i) {
	
		force = calculerCollisionSegment(bordures[i],
			bordures[(i + 1) % bordures.size()],
			rondelle_->obtenirPositionRelative(),
			rayonR_,
			true,
			0, 2,
			vitesseR_,
			&collision);

		if (collision.type != COLLISION_AUCUNE) {
			resultats.push_back(ResultatsCollision{ noeud, collision, force });
		}
	}

	if (resultats.size() > 0) {
		nombreCollisions_++;
		// Affichage de la collision
		Logger::obtenirInstance()->afficherCollision(std::string(noeud->obtenirType()));
		// Son
		float volume = glm::length(vitesseR_) / (2 * VITESSE_MAX);
		volume = max(0.05f, volume);
		Audio::obtenirInstance()->playSound(SOUND_MUR, volume);
	}

	// Vérification supplémentaire pour mieux gérer les coins
	for (int i = 0; i < resultats.size(); ++i) {
		switch (resultats[i].details.type)
		{
			case COLLISION_SEGMENT :
				ajouterCollision(noeud->obtenirType(), resultats[i]);
				break;
			case COLLISION_SEGMENT_PREMIERPOINT :
				if (resultats[(i-1) % resultats.size()].details.type == COLLISION_SEGMENT_DEUXIEMEPOINT )
				ajouterCollision(noeud->obtenirType(), resultats[i]);
				break;
			case COLLISION_SEGMENT_DEUXIEMEPOINT:
				if (resultats[(i + 1) % resultats.size()].details.type == COLLISION_SEGMENT_PREMIERPOINT)
					ajouterCollision(noeud->obtenirType(), resultats[i]);
				break;
			default:
				break;
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurCollisionRondelle::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiter un mur pour la collision.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::visiterMur(NoeudMur* noeud) {
	// Calcul des collisions	
	std::vector<ResultatsCollision> resultats;
	std::vector<int> segmentsEnCollision;
	resultatVisiterMur(noeud, resultats, segmentsEnCollision);

	// Vérification supplémentaire pour mieux gérer les coins
	for (int i = 0; i < resultats.size(); ++i) {
		switch (resultats[i].details.type)
		{
		case COLLISION_SEGMENT:
			ajouterCollision(noeud->obtenirType(), resultats[i]);
			break;
		case COLLISION_SEGMENT_PREMIERPOINT:
			if (resultats[(i - 1) % resultats.size()].details.type == COLLISION_SEGMENT_DEUXIEMEPOINT)
				ajouterCollision(noeud->obtenirType(), resultats[i]);
			break;
		case COLLISION_SEGMENT_DEUXIEMEPOINT:
			if (resultats[(i + 1) % resultats.size()].details.type == COLLISION_SEGMENT_PREMIERPOINT)
				ajouterCollision(noeud->obtenirType(), resultats[i]);
			break;
		default:
			break;
		}
	}

	if (segmentsEnCollision.size() > 0) {
		nombreCollisions_++;
		// Affichage de la collision	
		Logger::obtenirInstance()->afficherCollision(std::string(noeud->obtenirType()));
		// Son
		float volume = glm::length(vitesseR_) / (2 * VITESSE_MAX);
		volume = max(0.05f, volume);
		Audio::obtenirInstance()->playSound(SOUND_MUR, volume);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurCollisionRondelle::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiter un portail pour la collision.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::visiterPortail(NoeudPortail* noeud) {

	// Collision avec le portail lui-meme
	float rayonP = noeud->obtenirCollider().rayon;
	DetailsCollision details = calculerCollisionSphere(noeud->obtenirPositionRelative(),
		rayonP,
		rondelle_->obtenirPositionRelative(),
		rayonR_);

	// Calcul de la force additionnelle
	glm::vec3 direction = glm::normalize(glm::vec3(rondelle_->obtenirPositionRelative()) - noeud->obtenirPositionRelative());
	float enfoncement = 3 * rayonP - glm::length(glm::vec3(rondelle_->obtenirPositionRelative()) - noeud->obtenirPositionRelative());
	
	if (details.type != COLLISION_AUCUNE) {
		if (!noeud->estDesactiver()) {
			nombreCollisions_++;
			noeud->obtenirOppose()->assignerDesactiver(true);
			rondelle_->deplacer(noeud->obtenirOppose()->obtenirPositionRelative());

			// Affichage de la collision
			Logger::obtenirInstance()->afficherCollision(std::string(noeud->obtenirType()));
			// Son
			Audio::obtenirInstance()->playSound(SOUND_PORTAIL, DEFAULT_SOUND_VOLUME);
		}
	}
	else {
		if (enfoncement > 0) {
			if (!noeud->estDesactiver())
				force_ += direction * enfoncement * -ATTRACTION_PORTAIL;
		}
		else
			noeud->assignerDesactiver(false);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurCollisionRondelle::visiterBut(NoeudBut* noeud)
///
/// Cette fonction permet de visiter un but pour la collision.
///
///	@param[in] noeud : Un but
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::visiterBut(NoeudBut * noeud) {
	// Obtention des points dans le fond du but
	std::vector<glm::vec3> collider = noeud->obtenirCollider();
	glm::vec3 bissect = glm::normalize(collider[1] - collider[0]) * float(rayonR_) * 2.0f;
	collider[1] = collider[0] + bissect;
	collider[2] = collider[5] + bissect;
	collider[3] = collider[4] + bissect;

	DetailsCollision details;
	for (int i = 1; i < 3; ++i) {
		details = calculerCollisionSegment(collider[i],
			collider[(i+1) % collider.size()],
			rondelle_->obtenirPositionRelative(),
			rayonR_);

		if (details.type != COLLISION_AUCUNE) {
			++nombreCollisions_;
			ajouterCollision(noeud->obtenirType(), ResultatsCollision{ noeud, details, glm::vec3(0) });
		
			// Son
			Audio::obtenirInstance()->playSound(SOUND_BUT, DEFAULT_SOUND_VOLUME);
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 VisiteurCollisionRondelle::calculerVitesseFinale()
///
/// Calcule la vitesse finale de la rondelle selon les résultats des
/// collisions.
///
/// @return La vitesse finale de la rondelle
///
////////////////////////////////////////////////////////////////////////
glm::vec3 VisiteurCollisionRondelle::calculerVitesseFinale() {

	// S'il y a un but
	if (enCollisionAvec(ArbreRenduINF2990::NOM_BUT)) {
		vitesseFinale_ = glm::vec3(0);
		force_ = glm::vec3(0);

		NoeudBut* but = (NoeudBut*)collisions_[ArbreRenduINF2990::NOM_BUT].front().noeud;
		but->obtenirCollider()[0].z < 0 ? ModeleEtatJeu::obtenirInstance()->player2Goal() : ModeleEtatJeu::obtenirInstance()->player1Goal();
		ModeleEtatJeu::obtenirInstance()->miseAuJeu();

		return vitesseFinale_;
	}

	// Concaténation des collisions des murs et des bordures
	std::vector<ResultatsCollision> collisions;
	// Collisions avec la table
	bool tableContact = false;
	if (enCollisionAvec(ArbreRenduINF2990::NOM_TABLE)) {
		tableContact = true;
		collisions = collisions_[ArbreRenduINF2990::NOM_TABLE];
	}

	// Collisions avec les murs
	bool murContact = false;
    if (enCollisionAvec(ArbreRenduINF2990::NOM_MUR)) {
		murContact = true;
		auto collisionsMur = collisions_[ArbreRenduINF2990::NOM_MUR];
		collisions.insert(collisions.end(), collisionsMur.begin(), collisionsMur.end());
	}
	
	// Collision avec la table et les murs
	glm::vec3 enfoncementTable(0);
	if (collisions.size()) {		
		// Calcul de l'enfoncement
		enfoncementTable = calculerEnfoncement(collisions);
		rondelle_->appliquerDeplacement(enfoncementTable);

		// Calcule de la vitesse
		if (collisions.size() == 1) // Une seule collision
			vitesseFinale_ += collisions[0].force;
		else { // Multiple collisions
			glm::vec3 direction(0);
			for (auto& collision : collisions) {
				direction += glm::normalize(collision.details.direction);
			}

			if (glm::length(direction) > 0.01) {
				if (glm::length(vitesseFinale_) > 0.01)
					vitesseFinale_ = glm::reflect(glm::normalize(vitesseFinale_), glm::normalize(direction))  * glm::length(vitesseFinale_);
			}
		}

		// Ajout du rebondissement
		if (glm::length(vitesseFinale_) > 0.5 && glm::length(enfoncementTable) > 0.01) {
			glm::vec3 forceRebond = calculerForceRebondissement3D(DetailsCollision{ COLLISION_SEGMENT, glm::normalize(enfoncementTable), glm::length(enfoncementTable) }, PhysProperties::obtenirInstance()->coefficientRebondissement());
			vitesseFinale_ = vitesseFinale_ + (glm::length(forceRebond) * glm::normalize(vitesseFinale_));
		}
	}


	// Contact avec les maillets
	bool contactMaillet = false;
	if (enCollisionAvec(ArbreRenduINF2990::NOM_MAILLET)) {
		contactMaillet = true;
		auto collisionMaillet = collisions_[ArbreRenduINF2990::NOM_MAILLET];
		for (auto& collision : collisionMaillet) {
			NoeudMaillet* maillet = (NoeudMaillet*)(collision.noeud);

			// On recalcule l'enfoncement suite a l'élimination de l'enfoncement dans la table
			glm::vec3 direction = maillet->obtenirPositionRelative() - rondelle_->obtenirPositionRelative();
			float enfoncement = rayonR_ + maillet->obtenirCollider().rayon - glm::length(direction);
			
			if (glm::length(direction) < 0.1) {
					direction = glm::vec3(1, 0, 0);
			}

			if (enfoncement >= rayonR_ * 2)
				maillet->appliquerDeplacement(enfoncement * glm::normalize(direction));
			else {
				// On appliquer d'abord l'enfoncement sur la rondelle
				rondelle_->savePosition();
				rondelle_->appliquerDeplacement(enfoncement * glm::normalize(-direction));

				// On vérifie que le déplacement est valide, sinon on revert
				VisiteurSurTable visiteurSurTable;
				visiteurSurTable.visiterRondelle(rondelle_);
				if (!visiteurSurTable.sontSurTable() || tableContact || murContact || estDansMur()) {
					rondelle_->revertPosition();
					maillet->appliquerDeplacement(enfoncement * glm::normalize(direction));
				}
			}
			
			// Si la rondelle est en contact avec un mur/bordure
			if (tableContact || murContact) {
				// Elimination de la vitesse de la rondelle vers le maillet. Cela
				// permet de créer un ralentissement qui évite les glitch lorsque
				// la puck est proche d'un mur. De plus, on n'applique pas la force
				// du maillet.
				vitesseFinale_ -= glm::proj(vitesseFinale_, glm::normalize(direction));
			}
			else {
				// On peut appliquer la force du maillet
				vitesseFinale_ += collision.force;
			}
		}
	}

	// Si la puck est immobile sur un mur/bordure, on applique une vitesse vers le centre de la table
	if ((murContact || tableContact) && !contactMaillet && glm::length(vitesseFinale_) < 0.1f ) {
		glm::vec3 positionRondelle = rondelle_->obtenirPositionRelative();
		// on s'assure d'abord que le puck n'est pas déjà au centre
		if (glm::length(positionRondelle) < 0.01) 
			vitesseFinale_ = glm::vec3(0, 0, -20.0f);
		else
			vitesseFinale_ = glm::normalize(rondelle_->obtenirPositionRelative()) * -20.0f;
	}

	// Cas accelerateur
	if (enCollisionAvec(ArbreRenduINF2990::NOM_ACCELERATEUR)) {
		auto collisionAccelerateur = collisions_[ArbreRenduINF2990::NOM_ACCELERATEUR];
		if (glm::length(vitesseFinale_) > 0.01)
			vitesseFinale_ = (glm::length(vitesseFinale_) + collisionAccelerateur.size() * PhysProperties::obtenirInstance()->coefficientAcceleration()) * glm::normalize(vitesseFinale_);
		else
			vitesseFinale_ += glm::vec3(collisionAccelerateur.size() * PhysProperties::obtenirInstance()->coefficientAcceleration(), 0.0f, 0.0f);
	}
	
	return vitesseFinale_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 VisiteurCollisionRondelle::calculerEnfoncement(std::vector<ResultatsCollision> collisions)
///
/// Calcule l'enfoncement résultante pour plusieurs collisions
///
/// @return L'enfoncement résultant
///
////////////////////////////////////////////////////////////////////////
glm::vec3 VisiteurCollisionRondelle::calculerEnfoncement(std::vector<ResultatsCollision> collisions)
{
	glm::dvec3 direction(0);
	for (auto& collision : collisions) {
		direction += collision.details.direction;
	}

	glm::dvec3 enfoncementTotal(0);
	if (glm::length(direction) > 0.001) {
		for (auto& collision : collisions) {
			enfoncementTotal += glm::proj(collision.details.enfoncement * glm::normalize(collision.details.direction), direction);
		}
	}

	return enfoncementTotal;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool VisiteurCollisionRondelle::rondelleSurTable(const glm::vec3& sommet) const
///
/// Vérifie si la rondelle est sur la table ou dans les buts
///
/// @param[in] sommet : Le centre de la puck
///
/// @return Vrai si la rondelle est sur la table ou dans les buts
///
////////////////////////////////////////////////////////////////////////
bool VisiteurCollisionRondelle::rondelleSurTable(const glm::vec3& sommet) const {
	NoeudTable* table = (NoeudTable*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->chercher(ArbreRenduINF2990::NOM_TABLE);
	std::vector<glm::vec3> bordures = obtenirBordureTable(table);
	bordures.insert(bordures.begin(), glm::vec3(0));

	// Si le point est dans l'un des triangles formant la table, on retourne vrai
	for (int i = 1; i < bordures.size(); ++i) {
		if (aidecollision::pointDansTriangle(sommet, bordures[0], bordures[i], bordures[(i % (bordures.size() - 1)) + 1]))
			return true;
	}

	return false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool VisiteurCollisionRondelle::estDansMur()
///
/// Vérifie si la rondelle est dans un mur
///
/// @return Vrai si la rondelle est rendu dans un mur
///
////////////////////////////////////////////////////////////////////////
bool VisiteurCollisionRondelle::estDansMur() {
	dansMur_ = false;
	std::vector<NoeudMur*> murs;
	auto arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	
	for (auto it = arbre->obtenirIterateurBegin(); it != arbre->obtenirIterateurEnd(); ++it) {
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_MUR)
			murs.push_back((NoeudMur*)(*it));
	}

	Logger::obtenirInstance()->activer(false);
	std::vector<ResultatsCollision> resultats;
	std::vector<int> segmentsEnCollision;

	for (auto& mur : murs) {
		resultatVisiterMur(mur, resultats, segmentsEnCollision);
		// On détecte si la rondelle est dans le mur
		if (segmentsEnCollision.size() == 2) {
			auto pair = [](int x) { return (x % 2) == 0; };
			if (std::all_of(segmentsEnCollision.begin(), segmentsEnCollision.end(), pair))
				dansMur_ = true;
		}
	}

	Logger::obtenirInstance()->activer(true);
	
	return dansMur_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurCollisionRondelle::ajouterCollision(std::string type, ResultatsCollision resultat)
///
/// Ajoute une collision dans la map des collisions
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::ajouterCollision(std::string type, ResultatsCollision resultat) {
	collisions_[type].push_back(resultat);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool VisiteurCollisionRondelle::enCollisionAvec(const std::string & type) const
///
/// Vérifie s'il y a eu un collision avec un type particulier de noeud
///
/// @param[in] type : Type de noeud
///
/// @return Vrai, si la rondelle est rentrée en contact avec le type de noeud
///
////////////////////////////////////////////////////////////////////////
bool VisiteurCollisionRondelle::enCollisionAvec(const std::string & type) const
{
	return collisions_.find(type) != collisions_.end();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurCollisionRondelle::resultatVisiterMur(NoeudMur* noeud, std::vector<ResultatsCollision>& resultats, std::vector<int>& segmentsEnCollision)
///
/// Calcule les collisions avec un mur. Les résultats seront analysés
/// Par la suite par le visiteur du mur. Ces résultats sont aussi utiliser
/// dans la fonction est dans mur pour déterminer si la rondelle est dans un
/// mur.
///
/// @param[in] noeud : Un mur
/// @param[out] resultats : Les resultats de la collision
/// @param[out] segmentsEnCollision : Les segments en collision
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurCollisionRondelle::resultatVisiterMur(NoeudMur* noeud, std::vector<ResultatsCollision>& resultats, std::vector<int>& segmentsEnCollision)
{
	utilitaire::BoiteEnglobante c = noeud->obtenirCollider();
	std::vector<glm::vec3> sommets;
	auto matrModel = noeud->obtenirMatriceTransformation();
	sommets.push_back(glm::vec3(matrModel * glm::vec4(c.coinMin.x, 0, c.coinMin.z, 1)));
	sommets.push_back(glm::vec3(matrModel * glm::vec4(c.coinMin.x, 0, c.coinMax.z, 1)));
	sommets.push_back(glm::vec3(matrModel * glm::vec4(c.coinMax.x, 0, c.coinMax.z, 1)));
	sommets.push_back(glm::vec3(matrModel * glm::vec4(c.coinMax.x, 0, c.coinMin.z, 1)));

	DetailsCollision collision;
	glm::vec3 force(0);

	for (int i = 0; i < sommets.size(); ++i) {
		force = calculerCollisionSegment(sommets[i],
			sommets[(i + 1) % sommets.size()],
			rondelle_->obtenirPositionRelative(),
			rayonR_,
			true,
			0, 2,
			vitesseR_,
			&collision);

		if (collision.type != COLLISION_AUCUNE) {
			segmentsEnCollision.push_back(i);
			resultats.push_back(ResultatsCollision{ noeud, collision, force });
		}
	}

	if (aidecollision::pointDansTriangle(rondelle_->obtenirPositionRelative(), sommets[0], sommets[1], sommets[2]))
		dansMur_ = true;

	if (aidecollision::pointDansTriangle(rondelle_->obtenirPositionRelative(), sommets[2], sommets[3], sommets[0]))
		dansMur_ = true;

}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool VisiteurCollisionRondelle::areParallel(glm::vec3 v1, glm::vec3 v2)
///
/// Indique si deux vecteurs sont parallèles
///
/// @return Vrai si les vectuers sont parallèles, sinon faux
///
////////////////////////////////////////////////////////////////////////
bool VisiteurCollisionRondelle::areParallel(glm::vec3 v1, glm::vec3 v2) {
	return glm::dot(glm::normalize(v1), glm::normalize(v2)) == 1;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn std::vector<glm::vec3> VisiteurCollisionRondelle::obtenirBordureTable(NoeudTable* table) const
///
/// Obtient le contour de la patinoire. Cela inclut l'intérieur des buts
///
/// @param[in] table : Pointeur vers la table
///
/// @return Les points sur la bordure
///
////////////////////////////////////////////////////////////////////////
std::vector<glm::vec3> VisiteurCollisionRondelle::obtenirBordureTable(NoeudTable* table) const {
	float diametre = rayonR_ * 2.0f;

	std::vector<glm::vec3> sommets;
	std::vector<glm::vec3> patinoire = table->obtenirSommetsPatinoire();
	patinoire.erase(patinoire.begin());
	patinoire.erase(patinoire.begin());
	patinoire.erase(patinoire.begin() + 3);

	sommets.insert(sommets.begin(), patinoire.begin(), patinoire.end());
	
	// Obtenir les buts
	std::vector<NoeudBut*> buts = table->obtenirButs();
	std::vector<glm::vec3> collider1 = buts[0]->obtenirCollider();

	glm::vec3 bissect = glm::normalize(collider1[1] - collider1[0]) * diametre;
	collider1[1] = collider1[0] + bissect;
	collider1[2] = collider1[5] + bissect;
	collider1[3] = collider1[4] + bissect;
	sommets.insert(sommets.begin() + 3, collider1.begin(), collider1.begin() + 5);

	std::vector<glm::vec3> collider2 = buts[1]->obtenirCollider();
	collider2[1] = collider2[0] - bissect;
	collider2[2] = collider2[5] - bissect;
	collider2[3] = collider2[4] - bissect;
	sommets.insert(sommets.begin(), collider2.begin(), collider2.begin() + 5);

	return sommets;
}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////