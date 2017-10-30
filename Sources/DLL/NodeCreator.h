#pragma once
#include <glm/detail/type_vec3.hpp>

class NodeCreator
{
public:
		/// Obtient l'instance unique de la classe.

	static NodeCreator* obtenirInstance();
	static void libererInstance();


	virtual void createPortal(const char* startUuid, const glm::vec3 portal1Pos, const char* endUuid, const glm::vec3 portal2Pos);
	void createWall(const char* uuid, glm::vec3 startPos, glm::vec3 endPos);
	void createBoost(const char* uuid, glm::vec3 pos);

private:
	NodeCreator();
	~NodeCreator();


	static NodeCreator* instance_;
	
};

