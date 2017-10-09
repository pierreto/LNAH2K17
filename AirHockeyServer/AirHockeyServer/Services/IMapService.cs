using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services
{
    public interface IMapService
    {
        IEnumerable<MapEntity> GetMaps(); 
    }
}