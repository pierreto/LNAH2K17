#include "UserManager.h"



UserManager::UserManager()
{
	usersMap_ = std::map<std::string, OnlineUser*>();
}


UserManager::~UserManager()
{
	std::map<std::string, OnlineUser*>::iterator itr = usersMap_.begin();
	if (itr != usersMap_.end())
	{
		delete itr->second;
	}
}

void UserManager::addNewUser(std::string name, std::string hexColor)
{
	usersMap_.insert_or_assign(name,new OnlineUser(name,hexColor));
}

void UserManager::removeUser(std::string name)
{
	delete usersMap_.at(name);
	usersMap_.erase(name);
}
