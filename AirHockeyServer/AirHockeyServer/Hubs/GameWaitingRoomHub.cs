﻿using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web;
using AirHockeyServer.Services;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Events;
using System.Diagnostics;
using AirHockeyServer.Entities.Messages;

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

        // Is set via the constructor on each creation
        private Groupcaster _broadcaster;

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn GameWaitingRoomHub(IGameService gameService)
        ///
        /// Constructeur.
        ///
        /// @return Aucune (Constructeur).
        ///
        ////////////////////////////////////////////////////////////////////////
        public GameWaitingRoomHub()
            //: this(Groupcaster.Instance)
        {
            GameService = new GameService();
        }

        //public GameWaitingRoomHub(Groupcaster broadcaster)
        //{
        //    _broadcaster = broadcaster;
        //    GameService = new GameService();
        //}

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
            Clients.Group(gameEntity.GameId.ToString()).GameMapUpdatedEvent(gameEntity);
            return await GameService.UpdateGame(gameEntity);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void JoinGame(UserEntity user)
        ///
        /// Cette fonction permet de gérer la demande d'un utilisateur de se joindre à une partie. 
        /// On appel simplement la classe GameService
        ///
        ////////////////////////////////////////////////////////////////////////
        public void Join(UserEntity user)
        {
            // TO REMOVE, WAITING FOR AUTHENTIFICATION
            ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);

            GameService.JoinGame(user);
        }

        public void LeaveGame(UserEntity user)
        {
            GameService.LeaveGame(user);
        }

        public void SendGameData(int gameId, GameMasterData gameData)
        {
            Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedMasterData(gameData);

            //Groupcaster.Instance.SetGame(gameId);
            //Groupcaster.Instance.MasterUpdated(gameData);
        }

        public void SendSlaveGameData(int gameId, GameSlaveData gameData)
        {
            Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedSlaveData(gameData);

            //Groupcaster.Instance.SetGame(gameId);
            //Groupcaster.Instance.SlaveUpdated(gameData);
        }

        public void SendGoal(int gameId, GoalMessage goal)
        {
            GameService.GoalScored(gameId, goal.PlayerNumber);
            Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedGoal(goal);
        }

        public void GameOver(int gameId)
        {
            GameService.GameOver(gameId);
            Clients.Group(gameId.ToString(), Context.ConnectionId).ReceivedGameOver();
        }

    }
}
