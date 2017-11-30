#pragma once
#include "OnlineUser.h"
#include <map>

class UserManager
{
public:
	UserManager();
	~UserManager();

	void addNewUser(std::string name, std::string hexColor);
	void removeUser(std::string name);
	bool userExist(std::string name);
	OnlineUser* getUser(std::string name) { return usersMap_.at(name); }
	void clearUsers();

private:
	std::map<std::string, OnlineUser*> usersMap_;
};

