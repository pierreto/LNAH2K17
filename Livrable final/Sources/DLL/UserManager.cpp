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
	std::map<std::string, OnlineUser*>::iterator it = usersMap_.find(name);
	if (it != usersMap_.end())
	{
		//element found;
		OnlineUser* user = usersMap_.at(name);
		user->deselectAll();
		delete user;
		usersMap_.erase(name);
	}
}

bool UserManager::userExist(std::string name)
{
	return usersMap_.count(name);
}

void UserManager::clearUsers()
{
	std::map<std::string, OnlineUser*>::iterator itr = usersMap_.begin();
	if (itr != usersMap_.end())
	{
		delete itr->second;
	}
	usersMap_.clear();
}
