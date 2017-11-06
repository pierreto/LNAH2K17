#pragma once
#include <glm/detail/type_vec3.hpp>

class NodeCreator
{
public:
		/// Obtient l'instance unique de la classe.

	static NodeCreator* obtenirInstance();
	static void libererInstance();


	virtual void createPortal(const char* startUuid, glm::vec3 startPos, const float  startRotation, glm::vec3 startScale, const char* endUuid, glm::vec3 endPos, const  float endRotation, glm::vec3 endScale);
	void createWall(const char* uuid, glm::vec3 position, float angle, glm::vec3 scale);
	void createBoost(const char* uuid, glm::vec3 pos, const float angle, glm::vec3 scale);

private:
	NodeCreator();
	~NodeCreator();


	static NodeCreator* instance_;
	
};

