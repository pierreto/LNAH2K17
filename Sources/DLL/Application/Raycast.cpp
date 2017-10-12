///////////////////////////////////////////////////////////////////////////////
/// @file Raycast.cpp
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "Raycast.h"
#include "FacadeModele.h"
#include "Vue.h"
#include "Projection.h"

#define RAYLENGHT 1000.0


////////////////////////////////////////////////////////////////////////
///
/// @fn Raycast::Raycast(const int& mousePosX, const int& mousePosY)
///
/// Calcule le point initial et final du raycast à partir des 
/// coordonnées d'écran. Va du near plane au far plane.
///
/// @param[in] mousePosX : Position de la souris en X
/// @param[in] mousePosY : Position de la souris en Y
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
Raycast::Raycast(const int& mousePosX, const int& mousePosY) {
	glm::dvec3 camPos, camNormal;
	camPos = FacadeModele::obtenirInstance()->obtenirVue()->obtenirCamera().obtenirPosition();
	camNormal = glm::normalize(FacadeModele::obtenirInstance()->obtenirVue()->obtenirCamera().obtenirPointVise() - camPos);
	
	glm::dvec2 zMinMax = FacadeModele::obtenirInstance()->obtenirVue()->obtenirProjection().obtenirZMinMax();

	math::Plan3D nearPlane(camNormal, camPos + zMinMax[1] * camNormal);
	math::Plan3D farPlane(camNormal, camPos + zMinMax[0] * camNormal);

	FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX, mousePosY, nearPlane, rayStart_);
	FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX, mousePosY, farPlane, rayEnd_);
}


