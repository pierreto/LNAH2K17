using System.Collections.Generic;
using AirHockeyServer.Entities;
using AirHockeyServer.Entities.EditionCommand;
using AirHockeyServer.Services;

public class EditionService : IService
{
    private List<OnlineEditedMapInfo> _availableMapInfos;
    private Dictionary<string, List<OnlineUser>> usersPerGame;

    public EditionService()
    {
            usersPerGame = new Dictionary<string, List<OnlineUser>>();
    }

    public List<OnlineEditedMapInfo> AvailableMapInfos
    {
        get => _availableMapInfos;
        set => _availableMapInfos = value;
    }

    public Dictionary<string, List<OnlineUser>> UsersPerGame
    {
        get => usersPerGame;
        set => usersPerGame = value;
    }
}