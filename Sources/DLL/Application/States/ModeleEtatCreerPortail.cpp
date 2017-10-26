///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerPortail.cpp
/// @author Nam Lesage
/// @date 2016-09-20
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatCreerPortail.h"

#include "Vue.h"
#include "FacadeModele.h"
#include "ArbreRenduINF2990.h"

#include <vector>
#include "ModeleEtatJeu.h"
/// Pointeur vers l'instance unique de la classe.
ModeleEtatCreerPortail* ModeleEtatCreerPortail::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerPortail* ModeleEtatCreerPortail::obtenirInstance()
///
/// Cette fonction retourne un pointeur vers l'instance unique de la
/// classe.  Si cette instance n'a pas �t� cr��e, elle la cr�e.  Cette
/// cr�ation n'est toutefois pas n�cessairement "thread-safe", car
/// aucun verrou n'est pris entre le test pour savoir si l'instance
/// existe et le moment de sa cr�ation.
///
/// @return Un pointeur vers l'instance unique de cette classe.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatCreerPortail* ModeleEtatCreerPortail::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatCreerPortail();

	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->deselectionnerTout();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerPortail::libererInstance()
///
/// Cette fonction lib�re l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerPortail::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerPortail::ModeleEtatCreerPortail()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatCreerPortail::ModeleEtatCreerPortail()
	: ModeleEtat()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerPortail::~ModeleEtatCreerPortail()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatCreerPortail::~ModeleEtatCreerPortail() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerPortail::mouseUpL()
///
/// �v�nement appel� lorsque le bouton gauche de la souris est lev�.
/// Cette fonction cr�e les noeuds.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerPortail::mouseUpL() {
	if (mouseDownL_ && isAClick() && mouseOverTable()) {
		// Cr�ation du noeud
		NoeudPortail* noeud = (NoeudPortail*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL);

		// D�placement du noeud
		// Transformation du point dans l'esapce virtuelle
		glm::dvec3 point;
		obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, point);

		noeud->assignerPositionRelative(point);

		// Ajout du noeud � l'arbre de rendu
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud);

		// Etat de l'operation	
		if (premierNoeud_ == nullptr) {
			premierNoeud_ = noeud;
			premierNoeud_->effetFantome(true);
		}
		else {
			// Verification que les noeuds soient sur la table
			if (noeudsSurLaTable()) {
				// Link portal together
				noeud->assignerOppose(premierNoeud_);
				premierNoeud_->assignerOppose(noeud);
				premierNoeud_->effetFantome(false);
				premierNoeud_ = nullptr;
				if(ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
				{
					portalCreationCallback_(premierNoeud_->getUUID(),glm::value_ptr(premierNoeud_->obtenirPositionRelative()), noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()));
				}
			}
			else { // Annulation de la commande
				escape();
				FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->effacer(noeud);
			}
		}
	}

	mouseDownL_ = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerPortail::escape()
///
/// Cette fonction annule l'action
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerPortail::escape()  {
	if (premierNoeud_ != nullptr) {
		// Supprimer le noeud ajout�
		 FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->effacer(premierNoeud_);
		 premierNoeud_->effetFantome(false);
		 premierNoeud_ = nullptr;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerPortail::nettoyerEtat()
///
/// Cette fonction nettoie l'etat. Plus pr�cisement, elle annule
/// l'action en cours lors d'un changement d'�tat.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerPortail::nettoyerEtat()
{
	escape();
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////