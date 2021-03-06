#pragma once
#include <list>
#include "NoeudAbstrait.h"

class OnlineUser
{
public:
	OnlineUser(std::string name, std::string color);
	~OnlineUser();

	void select(std::string uuid);
	void deselect(std::string uuid);
	NoeudAbstrait* findNode(std::string uuid);
	void deselectAll();
	glm::vec4 static hexadecimalToRGB(std::string hex);

private:
	glm::vec4 color_;
	std::string name_;
	std::vector<NoeudAbstrait*> nodesSelected_;

	NoeudAbstrait* getNodeFromRenderTree(std::string uuid);
	int static hexadecimalToDecimal(std::string hex);
};

