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
	void deselectAll();

private:
	glm::vec4 color_;
	std::string name_;
	std::list<NoeudAbstrait*> nodesSelected_;

	NoeudAbstrait* getNodeFromRenderTree(std::string uuid);
	glm::vec4 hexadecimalToRGB(std::string hex) const;
	int hexadecimalToDecimal(std::string hex) const;
};

