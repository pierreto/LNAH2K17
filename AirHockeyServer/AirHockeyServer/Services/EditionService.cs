using System.Collections.Generic;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;

public class EditionService : IService
{
    private List<OnlineEditedMapInfo> _availableMapInfos;

    public EditionService()
    {
       
    }

    public List<OnlineEditedMapInfo> AvailableMapInfos
    {
        get => _availableMapInfos;
        set => _availableMapInfos = value;
    }
}