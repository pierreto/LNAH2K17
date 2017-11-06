#include "OnlineUser.h"
#include "VisitorByUUID.h"
#include "ModeleEtatPointControl.h"


OnlineUser::OnlineUser(std::string name, std::string hexColor)
{
	name_ = name;
	color_ = hexadecimalToRGB(hexColor);
	nodesSelected_ = std::vector<NoeudAbstrait*>();
}


OnlineUser::~OnlineUser()
{
}

void OnlineUser::select(std::string uuid)
{
	NoeudAbstrait* node = getNodeFromRenderTree(uuid);
	if(node)
	{
		node->setSelectedByAnotherUser(true);
		node->useOtherColor(true, color_);

		nodesSelected_.push_back(node);
	}
}
void OnlineUser::deselect(std::string uuid)
{

	std::vector<NoeudAbstrait*>::iterator it = nodesSelected_.begin();;  // declare an iterator to a vector of strings

	while (it != nodesSelected_.end()) 
	{
		// found nth element..print and break.
		if (std::string((*it)->getUUID()) == uuid)
		{
			(*it)->setSelectedByAnotherUser(false);
			(*it)->useOtherColor(false, color_);
			nodesSelected_.erase(it);
			break;
		}
		++it;
	}
}


NoeudAbstrait* OnlineUser::findNode(std::string uuid)
{
	for(NoeudAbstrait* node : nodesSelected_)
	{
		if (node->getUUID() == uuid.c_str())
		{
			return node;
		}
	}
	return nullptr;
}


void OnlineUser::deselectAll()
{
	for(NoeudAbstrait* node : nodesSelected_)
	{
		node->setSelectedByAnotherUser(false);
		if(dynamic_cast<NoeudPointControl*>(node))
		{
			if(dynamic_cast<ModeleEtatPointControl*>(FacadeModele::obtenirInstance()->getEtat()))
			{
				node->useOtherColor(true, glm::vec4(1, 85.0f / 255, 82.0f / 255, 1));
			}
			else
			{
				node->useOtherColor(false, color_);
			}

		}else
		{
			node->useOtherColor(false, color_);
		}
	}
	nodesSelected_.clear();
}

NoeudAbstrait* OnlineUser::getNodeFromRenderTree(std::string uuid)
{
	VisitorByUUID visitorWrapper = VisitorByUUID(uuid.c_str());
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visitorWrapper);
	if(visitorWrapper.hasFound)
	{
		return  visitorWrapper.getNode();
	}else
	{
		return nullptr;
	}
}


glm::vec4 OnlineUser::hexadecimalToRGB(std::string hex)
{
	if (hex[0] == '#')
		hex = hex.erase(0, 1);

	int r = hexadecimalToDecimal(hex.substr(0, 2));
	int g = hexadecimalToDecimal(hex.substr(2, 2));
	int b = hexadecimalToDecimal(hex.substr(4, 2));

	return glm::vec4(r, g, b, 0.5);
}


int OnlineUser::hexadecimalToDecimal(std::string hex)
{
	int hexLength = hex.length();
	double dec = 0;

	for (int i = 0; i < hexLength; ++i)
	{
		char b = hex[i];

		if (b >= 48 && b <= 57)
			b -= 48;
		else if (b >= 65 && b <= 70)
			b -= 55;

		dec += b * pow(16, ((hexLength - i) - 1));
	}

	return (int)dec;
}
