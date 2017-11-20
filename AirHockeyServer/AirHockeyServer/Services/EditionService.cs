using System.Collections.Generic;
using AirHockeyServer.Entities;
using AirHockeyServer.Entities.EditionCommand;
using AirHockeyServer.Services;

public class EditionService : IEditionService
{
    private List<OnlineEditedMapInfo> _availableMapInfos;
    private Dictionary<string, EditionGroup> usersPerGame;

    public EditionService()
    {
            usersPerGame = new Dictionary<string, EditionGroup>();
    }

    public List<OnlineEditedMapInfo> AvailableMapInfos
    {
        get => _availableMapInfos;
        set => _availableMapInfos = value;
    }

    public Dictionary<string, EditionGroup> UsersPerGame
    {
        get => usersPerGame;
        set => usersPerGame = value;
    }
}