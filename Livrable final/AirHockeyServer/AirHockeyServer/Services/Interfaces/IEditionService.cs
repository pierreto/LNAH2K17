using System.Collections.Generic;
using AirHockeyServer.Entities;
using AirHockeyServer.Entities.EditionCommand;

public interface IEditionService
{
    List<OnlineEditedMapInfo> AvailableMapInfos { get; set; }
    Dictionary<string, EditionGroup> UsersPerGame { get; set; }
}