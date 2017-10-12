///////////////////////////////////////////////////////////////////////////////
/// @file Table.cpp
/// @author Nam Lesage
/// @date 2015-10-13
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "utilitaire.h"
#include "Table.h"
#include "GL/glew.h"

/// Couleurs prédéfinies
#define White glm::vec3(1)
#define Gray glm::vec3(0.25,0.25,0.25)

namespace table
{

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Sommets obtenirSommets()
	///
	/// Cette fonction obtient tous les sommets de la table
	///
	/// @return Les sommets.
	///
	////////////////////////////////////////////////////////////////////////
	Sommets obtenirSommets()
	{
		Sommets table;

		// Middle
		table.push_back(glm::vec3(0));						// 0

		// Right
		table.push_back(glm::vec3(0, 0, 125));											// 1
		table.push_back(glm::vec3(0, TABLE_BORDER_HEIGHT, 125));						// 2
		table.push_back(glm::vec3(0, TABLE_BORDER_HEIGHT, 125 + TABLE_BORDER_WIDTH));	// 3
		table.push_back(glm::vec3(0, 0, 125 + TABLE_BORDER_WIDTH));						// 4
		table.push_back(glm::vec3(0, -TABLE_BASE_HEIGHT, 115));						    // 5

		// Top Right
		table.push_back(glm::vec3(60, 0, 125));																	// 6
		table.push_back(glm::vec3(60, TABLE_BORDER_HEIGHT, 125));												// 7
		table.push_back(glm::vec3(60 + TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 125 + TABLE_BORDER_WIDTH));		// 8
		table.push_back(glm::vec3(60 + TABLE_BORDER_WIDTH, 0, 125 + TABLE_BORDER_WIDTH));						// 9
		table.push_back(glm::vec3(50, -TABLE_BASE_HEIGHT, 115));																// 10

		// Top
		table.push_back(glm::vec3(60, 0, 0));											// 11
		table.push_back(glm::vec3(60, TABLE_BORDER_HEIGHT, 0));							// 12
		table.push_back(glm::vec3(60 + TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 0));	// 13
		table.push_back(glm::vec3(60 + TABLE_BORDER_WIDTH, 0, 0));						// 14
		table.push_back(glm::vec3(50, -TABLE_BASE_HEIGHT, 0));							// 15

		// Top Left
		table.push_back(glm::vec3(60, 0, -125));																// 16
		table.push_back(glm::vec3(60, TABLE_BORDER_HEIGHT, -125));												// 17
		table.push_back(glm::vec3(60 + TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, -125 - TABLE_BORDER_WIDTH));	// 18
		table.push_back(glm::vec3(60 + TABLE_BORDER_WIDTH, 0, -125 - +TABLE_BORDER_WIDTH));						// 19
		table.push_back(glm::vec3(50, -TABLE_BASE_HEIGHT, -115));																// 20

		// Right
		table.push_back(glm::vec3(0, 0, -125));											// 21
		table.push_back(glm::vec3(0, TABLE_BORDER_HEIGHT, -125));						// 22
		table.push_back(glm::vec3(0, TABLE_BORDER_HEIGHT, -125 - TABLE_BORDER_WIDTH));	// 23
		table.push_back(glm::vec3(0, 0, -125 - TABLE_BORDER_WIDTH));					// 24
		table.push_back(glm::vec3(0, -TABLE_BASE_HEIGHT, -115));						// 25

		// Bottom Left
		table.push_back(glm::vec3(-60, 0, -125));																// 26
		table.push_back(glm::vec3(-60, TABLE_BORDER_HEIGHT, -125));												// 27
		table.push_back(glm::vec3(-60 - TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, -125 - TABLE_BORDER_WIDTH));   // 28
		table.push_back(glm::vec3(-60 - TABLE_BORDER_WIDTH, 0, -125 - TABLE_BORDER_WIDTH));						// 29
		table.push_back(glm::vec3(-50, -TABLE_BASE_HEIGHT, -115));												// 30

		// Bottom
		table.push_back(glm::vec3(-60, 0, 0));											// 31
		table.push_back(glm::vec3(-60, TABLE_BORDER_HEIGHT, 0));						// 32
		table.push_back(glm::vec3(-60 - TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 0));	// 33
		table.push_back(glm::vec3(-60 - TABLE_BORDER_WIDTH, 0, 0));						// 34
		table.push_back(glm::vec3(-50, -TABLE_BASE_HEIGHT, 0));							// 35

		// Bottom Right
		table.push_back(glm::vec3(-60, 0, 125));																// 36
		table.push_back(glm::vec3(-60, TABLE_BORDER_HEIGHT, 125));												// 37
		table.push_back(glm::vec3(-60 - TABLE_BORDER_WIDTH, TABLE_BORDER_HEIGHT, 125 + TABLE_BORDER_WIDTH));	// 38
		table.push_back(glm::vec3(-60 - TABLE_BORDER_WIDTH, 0, 125 + TABLE_BORDER_WIDTH));						// 39
		table.push_back(glm::vec3(-50, -TABLE_BASE_HEIGHT, 115));												// 40

		// Middle
		table.push_back(glm::vec3(0, -TABLE_BASE_HEIGHT, 0));			    // 41

		return table;
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn std::vector<GLuint> obtenirFaces()
	///
	/// Cette fonction obtient toutes les faces de la table
	///
	/// @return Les faces.
	///
	////////////////////////////////////////////////////////////////////////
	std::vector<GLuint> obtenirFaces()
	{
		GLuint faces[] = {
			0,1,6,
			0,6,11,
			0,11,16,
			0,16,21,
			0,21,26,
			0,26,31,
			0,31,36,
			0,36,1,

			41,10,5,
			41,15,10,
			41,20,15,
			41,25,20,
			41,30,25,
			41,35,30,
			41,40,35,
			41,5,40,

			1,7,6,
			1,2,7,
			2,3,7,
			3,8,7,
			3,9,8,
			4,9,3,
			5,10,4,
			4,10,9,

			7,12,11,
			6,7,11,
			7,8,12,
			8,13,12,
			8,14,13,
			9,14,8,
			10,15,9,
			9,15,14,

			11,17,16,
			11,12,17,
			12,13,17,
			13,18,17,
			13,19,18,
			14,19,13,
			15,20,14,
			14,20,19,

			16,22,21,
			16,17,22,
			17,18,22,
			18,23,22,
			18,24,23,
			19,24,18,
			20,25,19,
			19,25,24,

			21,27,26,
			21,22,27,
			22,23,27,
			23,28,27,
			23,29,28,
			24,29,23,
			25,30,24,
			24,30,29,

			26,32,31,
			26,27,32,
			27,28,32,
			28,33,32,
			28,34,33,
			29,34,28,
			30,35,29,
			29,35,34,

			31,37,36,
			31,32,37,
			32,33,37,
			33,38,37,
			33,39,38,
			34,39,33,
			35,40,34,
			34,40,39,

			36,2,1,
			36,37,2,
			37,38,2,
			38,3,2,
			38,4,3,
			39,4,38,
			40,5,39,
			39,5,4,
		};

		return std::vector<GLuint>(std::begin(faces), std::end(faces));;
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn modele::Materiau obtenirMateriau()
	///
	/// Cette fonction obtient le matériau de la table
	///
	/// @return Le matériau de la table.
	///
	////////////////////////////////////////////////////////////////////////
	modele::Materiau obtenirMateriau()
	{	
		modele::Materiau materiau;
		materiau.ambiant_ = glm::vec3(0.1f, 0.1f, 0.1f);
		materiau.diffuse_ = glm::vec3(1.0, 1.0, 1.0);
		materiau.speculaire_ = glm::vec3(0.0f);
		materiau.shininess_ = 0;
		materiau.shininessStrength_ = 0;
		materiau.opacite_ = 1;
		materiau.emission_ = glm::vec3(0.0f, 0.0f, 0.0f);
		return materiau;
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn std::vector<glm::vec2> obtenirTexCoords()
	///
	/// Cette fonction obtient les coordonnées de texture de la table
	///
	/// @return les coordonnées de texture de la table.
	///
	////////////////////////////////////////////////////////////////////////
	std::vector<glm::vec2> obtenirTexCoords()
	{
		std::vector<glm::vec2> texCoords;
		// Middle
		texCoords.push_back(glm::vec2(0.5f, 0.5f));			    // 0

																// Right
		texCoords.push_back(glm::vec2(0.8f, 0.5f));				// 1
		texCoords.push_back(glm::vec2(0.8f, 0.5f));				// 2
		texCoords.push_back(glm::vec2(1.0f, 0.5f));			    // 3
		texCoords.push_back(glm::vec2(1.0f, 0.5f));		        // 4
		texCoords.push_back(glm::vec2(1.0f, 0.5f));			    // 5

																// Top Right
		texCoords.push_back(glm::vec2(0.8f, 0.8f));				// 6
		texCoords.push_back(glm::vec2(0.8f, 0.8f));				// 7
		texCoords.push_back(glm::vec2(1.0f, 1.0f));			    // 8
		texCoords.push_back(glm::vec2(1.0f, 1.0f));		        // 9
		texCoords.push_back(glm::vec2(1.0f, 1.0f));			    // 10

																// Top
		texCoords.push_back(glm::vec2(0.5f, 0.8f));				// 11
		texCoords.push_back(glm::vec2(0.5f, 0.8f));				// 12
		texCoords.push_back(glm::vec2(0.5f, 1.0f));			    // 13
		texCoords.push_back(glm::vec2(0.5f, 1.0f));		        // 14
		texCoords.push_back(glm::vec2(0.5f, 1.0f));			    // 15

																// Top Left
		texCoords.push_back(glm::vec2(0.2f, 0.8f));				// 16
		texCoords.push_back(glm::vec2(0.2f, 0.8f));				// 17
		texCoords.push_back(glm::vec2(0.0f, 1.0f));			    // 18
		texCoords.push_back(glm::vec2(0.0f, 1.0f));		        // 19
		texCoords.push_back(glm::vec2(0.0f, 1.0f));			    // 20

																// Right
		texCoords.push_back(glm::vec2(0.2f, 0.5f));				// 21
		texCoords.push_back(glm::vec2(0.2f, 0.5f));				// 22
		texCoords.push_back(glm::vec2(0.0f, 0.5f));			    // 23
		texCoords.push_back(glm::vec2(0.0f, 0.5f));		        // 24
		texCoords.push_back(glm::vec2(0.0f, 0.5f));			    // 25

																// Bottom Left
		texCoords.push_back(glm::vec2(0.2f, 0.2f));				// 26
		texCoords.push_back(glm::vec2(0.2f, 0.2f));				// 27
		texCoords.push_back(glm::vec2(0.0f, 0.0f));			    // 28
		texCoords.push_back(glm::vec2(0.0f, 0.0f));		        // 29
		texCoords.push_back(glm::vec2(0.0f, 0.0f));			    // 30

																// Bottom
		texCoords.push_back(glm::vec2(0.5f, 0.2f));				// 31
		texCoords.push_back(glm::vec2(0.5f, 0.2f));				// 32
		texCoords.push_back(glm::vec2(0.5f, 0.0f));			    // 33
		texCoords.push_back(glm::vec2(0.5f, 0.0f));		        // 34
		texCoords.push_back(glm::vec2(0.5f, 0.0f));			    // 35

																// Bottom Right
		texCoords.push_back(glm::vec2(0.8f, 0.2f));				// 36
		texCoords.push_back(glm::vec2(0.8f, 0.2f));				// 37
		texCoords.push_back(glm::vec2(1.0f, 0.0f));			    // 38
		texCoords.push_back(glm::vec2(1.0f, 0.0f));		        // 39
		texCoords.push_back(glm::vec2(1.0f, 0.0f));			    // 40

																// Middle
		texCoords.push_back(glm::vec2(1.0f, 0.0f));			    // 41

		return texCoords;
	}

} // namespace table
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
