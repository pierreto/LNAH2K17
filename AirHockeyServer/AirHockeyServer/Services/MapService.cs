using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Core;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;
using AirHockeyServer.DatabaseCore;

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
        private MapRepository MapRepository;

        public MapService()
        {
            MapRepository = new MapRepository();
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn Task<MapEntity> GetMapByName(string creator, string name)
        ///
        /// Cette fonction asynchrone retourne la carte (si elle existe) créée par l'auteur `creator`
        /// et portant le nom `name`.
        /// 
        /// @return une carte
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<MapEntity> GetMap(int id)
        {
            return await MapRepository.GetMap(id);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn Task SaveMap(MapEntity map)
        ///
        /// Cette fonction asynchrone enregistre une carte dans la DB (en créant une entrée
        /// si elle n'existe pas déjà ou en la mettant à jour, dans le cas contraire).
        /// 
        /// @return Task
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task SaveMap(MapEntity map)
        {
            if (map.Id == null)
            {
                await MapRepository.CreateNewMap(map);
            }
            else
            {
                await MapRepository.UpdateMap(map);
            }
        }

        public async Task<int?> GetMapID(MapEntity map)
        {
            return await MapRepository.GetMapID(map);
        }

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
        public async Task<IEnumerable<MapEntity>> GetMaps()
        {
            return await MapRepository.GetMaps();
        }
    }
}