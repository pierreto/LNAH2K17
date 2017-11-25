using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web;
using AirHockeyServer.Services;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Events;
using System.Diagnostics;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Core;

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
    public class GameWaitingRoomHub : BaseHub 
    {
        protected IGameService GameService { get; }
        protected FriendService FriendService { get; }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn GameWaitingRoomHub(IGameService gameService)
        ///
        /// Constructeur.
        ///
        /// @return Aucune (Constructeur).
        ///
        ////////////////////////////////////////////////////////////////////////
        public GameWaitingRoomHub(IGameService gameService, ConnectionMapper connectionMapper, FriendService friendService)
            : base(connectionMapper)
        {
            GameService = gameService;
            FriendService = friendService;
        }
        
        /// @fn void JoinGame(UserEntity user)
        ///
        /// Cette fonction permet de gérer la demande d'un utilisateur de se joindre à une partie. 
        /// On appel simplement la classe GameService
        ///
        ////////////////////////////////////////////////////////////////////////
        public void JoinGame(GamePlayerEntity user)
        {
            ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);
            Cache.AddPlayer(FriendService.UsersIdConnected.Find(x => x.Id == user.Id));

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
        public void UpdateMap(Guid gameId, MapEntity map)
        {
            Clients.Group(gameId.ToString()).GameMapUpdatedEvent(map);
            GameService.UpdateGame(gameId, map);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void JoinGame(UserEntity user)
        ///
        /// Cette fonction permet de gérer la demande d'un utilisateur de se joindre à une partie. 
        /// On appel simplement la classe GameService
        ///
        ////////////////////////////////////////////////////////////////////////
        public void Join(GamePlayerEntity user)
        {
            // TO REMOVE, WAITING FOR AUTHENTIFICATION
            ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);
            Cache.AddPlayer(FriendService.UsersIdConnected.Find(x => x.Id == user.Id));
            GameService.JoinGame(user);
        }

        public async Task LeaveGame(UserEntity user, Guid gameId)
        {
            string connectionid = ConnectionMapper.GetConnection(user.Id);
            if(!string.IsNullOrEmpty(connectionid))
            {
                await Groups.Remove(connectionid, gameId.ToString());
            }
            await GameService.LeaveGame(user);
            Cache.RemovePlayer(user);
        }

        public void SendGameData(Guid gameId, GameDataMessage gameData)
        {
             Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedGameData(gameData);
        }

        public void SendGoal(Guid gameId, GoalMessage goal)
        {
            Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedGoal(goal);
            GameService.GoalScored(gameId, goal.PlayerNumber);
        }

        public async Task GameOver(Guid gameId)
        {
            Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedGameOver();
            await GameService.GameOver(gameId);
        }

        public void GamePauseOrResume(Guid gameId)
        {
            Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedGamePauseOrResume();
        }

        public void RegisterPlayer(Guid gameId)
        {
            ConnectionMapper.AddGameID(Context.ConnectionId, gameId);
        }

        public override void Disconnect()
        {
            var userId = ConnectionMapper.GetIdFromConnection(Context.ConnectionId);
            GameService.LeaveGame(userId);

            base.Disconnect();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Disconnect();

            return base.OnDisconnected(stopCalled);
        }
    }
}