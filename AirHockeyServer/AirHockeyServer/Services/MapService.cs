using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Core;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Services.Interfaces;

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
        private IMapRepository MapRepository;

        public MapService(IMapRepository mapRepository)
        {
            MapRepository = mapRepository;
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
        /// Cette fonction asynchrone met à jour une carte dans la DB.
        /// Retourn true en cas de succès, false sinon.
        /// 
        /// @return Task<bool>
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<bool> SaveMap(MapEntity map)
        {
            return await MapRepository.UpdateMap(map);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn Task SaveMap(MapEntity map)
        ///
        /// Cette fonction asynchrone crée une nouvelle entrée dans la db pour map.
        /// Retourne la MapEntity qui contient l'ID auto-généré par la db.
        /// 
        /// @return Task<MapEntity>
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<int?> SaveNewMap(MapEntity map)
        {
            return await MapRepository.CreateNewMap(map);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn Task SaveMap(MapEntity map)
        ///
        /// Cette fonction asynchrone crée une nouvelle entrée dans la db pour map.
        /// Retourne la MapEntity qui contient l'ID auto-généré par la db.
        /// 
        /// @return Task<MapEntity>
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<bool> RemoveMap(int id)
        {
            return await MapRepository.RemoveMap(id);
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

        public async Task<IEnumerable<MapEntity>> GetFullMaps()
        {
            return await MapRepository.GetFullMaps();
        }
        
        public async Task<bool> SyncMap(MapEntity map)
        {
            return await MapRepository.SyncMap(map);
        }
    }
}