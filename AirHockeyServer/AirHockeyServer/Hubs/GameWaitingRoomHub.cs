using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web;
using AirHockeyServer.Services;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Events;

namespace AirHockeyServer.Hubs
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file GameWaitingRoomHub.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe permet de gérer la connection entre les clients et le serveur
    /// lorsqu'un match en ligne est en préparation
    ///////////////////////////////////////////////////////////////////////////////
    public class GameWaitingRoomHub : Hub 
    {
        protected IGameService GameService { get; }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn GameWaitingRoomHub(IGameService gameService)
        ///
        /// Constructeur.
        ///
        /// @return Aucune (Constructeur).
        ///
        ////////////////////////////////////////////////////////////////////////
        public GameWaitingRoomHub(IGameService gameService)
        {
            GameService = gameService;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn async Task<Guid> CreateGame(GameEntity gameEntity)
        ///
        /// Cette fonction permet la création d'un match. Il crée la "room" contenant les
        /// utilisateurs (opposants et spectacteurs) interessé à recevoir les notifications 
        /// de la partie.
        ///
        /// @return id du gameEntity créé et enregistré dans
        /// la bd. 
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<Guid> CreateGame(GameEntity gameEntity)
        {
            gameEntity.CreationDate = DateTime.Now;
            gameEntity.Players[0] = gameEntity.Creator;

            // TO REMOVE, WAITING FOR AUTHENTIFICATION
            ConnectionMapper.AddConnection(gameEntity.Creator.Id, Context.ConnectionId);
            
            Guid gameCreatedId = await GameService.CreateGame(gameEntity);

            await Groups.Add(Context.ConnectionId, gameCreatedId.ToString());

            return gameCreatedId;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void JoinGame(UserEntity user)
        ///
        /// Cette fonction permet de gérer la demande d'un utilisateur de se joindre à une partie. 
        /// On appel simplement la classe GameService
        ///
        ////////////////////////////////////////////////////////////////////////
        public void JoinGame(UserEntity user)
        {
            // TO REMOVE, WAITING FOR AUTHENTIFICATION
            ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);

            GameService.JoinGame(user);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn async Task<GameEntity> UpdateMap(GameEntity gameEntity)
        ///
        /// Cette fonction permet d'updater la carte de 
        /// partie et d'avertir les autres clients
        /// 
        /// @return la partie mise à jour
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<GameEntity> UpdateMap(GameEntity gameEntity)
        {
            var updatedGame = await GameService.UpdateGame(gameEntity);

            Clients.Group(gameEntity.GameId.ToString()).GameMapUpdatedEvent(updatedGame);

            return updatedGame;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn async Task<GameEntity> UpdateConfiguration(GameEntity gameEntity)
        ///
        /// Cette fonction permet d'updater les paramètres de parties et
        /// d'avertir les autres clients
        /// 
        /// @return la partie mise à jour
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<GameEntity> UpdateConfiguration(GameEntity gameEntity)
        {
            var updatedGame = await GameService.UpdateGame(gameEntity);

            Clients.Group(gameEntity.GameId.ToString()).GameConfigurationUpdatedEvent(updatedGame);

            return updatedGame;
        }
        
    }
}