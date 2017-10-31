#pragma once
#include <glm/detail/type_vec3.hpp>

class NodeCreator
{
public:
		/// Obtient l'instance unique de la classe.

	static NodeCreator* obtenirInstance();
	static void libererInstance();


	virtual void createPortal(glm::vec3  portal1Pos, glm::vec3 portal2Pos);

private:
	NodeCreator();
	~NodeCreator();


	static NodeCreator* instance_;
	
};

