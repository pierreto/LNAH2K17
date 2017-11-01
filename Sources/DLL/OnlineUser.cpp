#include "OnlineUser.h"
#include "VisitorByUUID.h"


OnlineUser::OnlineUser(std::string name, std::string hexColor)
{
	name_ = name;
	color_ = hexadecimalToRGB(hexColor);
	nodesSelected_ = std::list<NoeudAbstrait*>();
}


OnlineUser::~OnlineUser()
{
}

void OnlineUser::select(std::string uuid)
{
	NoeudAbstrait* node = getNodeFromRenderTree(uuid);
	if(node)
	{
		node->useOtherColor(true, color_);
		nodesSelected_.push_back(node);
	}
}

void OnlineUser::deselect(std::string uuid)
{
	nodesSelected_.remove(getNodeFromRenderTree(uuid));
}

void OnlineUser::deselectAll()
{
	nodesSelected_.clear();
}

NoeudAbstrait* OnlineUser::getNodeFromRenderTree(std::string uuid)
{
	VisitorByUUID visitorWrapper = VisitorByUUID(uuid.c_str());
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visitorWrapper);
	return  visitorWrapper.getNode();
}


glm::vec4 OnlineUser::hexadecimalToRGB(std::string hex) const
{
	if (hex[0] == '#')
		hex = hex.erase(0, 1);

	int r = hexadecimalToDecimal(hex.substr(0, 2));
	int g = hexadecimalToDecimal(hex.substr(2, 2));
	int b = hexadecimalToDecimal(hex.substr(4, 2));

	return glm::vec4(r, g, b, 1);
}

int OnlineUser::hexadecimalToDecimal(std::string hex) const
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
