using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Core;
using AirHockeyServer.Repositories;

namespace AirHockeyServer.Services
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file MapService.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe gère les actions relatives aux cartes de jeu
    ///////////////////////////////////////////////////////////////////////////////
    public class MapService : IMapService
    {
        private static IMapRepository MapRepository = new MapRepository();

        public MapService()
        {}

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn IEnumerable<MapEntity> GetMaps()
        ///
        /// Cette fonction récupere les cartes de jeu dans la bd
        /// et la retourne
        /// 
        /// @return une liste de carte
        ///
        ////////////////////////////////////////////////////////////////////////
        public IEnumerable<MapEntity> GetMaps()
        {
            // get maps from bd
            return new List<MapEntity>();
        }
    }
}