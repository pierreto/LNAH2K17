﻿using System;
using System.Collections.Generic;
using System.Linq;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Repositories;
using AirHockeyServer.Services.MatchMaking;
using AirHockeyServer.Manager;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Events.EventManagers;

namespace AirHockeyServer.Services
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file GameService.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe gère les actions relatives à un match en ligne
    ///////////////////////////////////////////////////////////////////////////////
    public class GameService : IGameService
    {
        private List<GameEntity> games;

        private IGameRepository GameRepository { get; set; }

        public GameWaitingRoomEventManager GameWaitingRoomEventManager { get; set; }

        public IPlayOnlineManager PlayOnlineManager { get; }

        public GameService(IPlayOnlineManager playOnlineManager, IGameRepository gameRepository
            , GameWaitingRoomEventManager gameWaitingRoomEventManager
            )
        {
            this.games = new List<GameEntity>();
            PlayOnlineManager = playOnlineManager;
            GameRepository = gameRepository;
            GameWaitingRoomEventManager = gameWaitingRoomEventManager;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void JoinGame(UserEntity userEntity)
        ///
        /// Cette fonction permet d'ajouter l'utilisateur pour que le "MatchMakerService"
        /// puisse trouver un (des) adversaire(s)
        ///
        ////////////////////////////////////////////////////////////////////////
        public void JoinGame(GamePlayerEntity gamePlayer)
        {
            GameMatchMakerService.Instance().AddOpponent(gamePlayer);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn async Task<GameEntity> UpdateGame(GameEntity gameEntity)
        ///
        /// Cette fonction permet d'updater une partie dans la base de données
        /// 
        /// @return la partie mise à jour
        ///
        ////////////////////////////////////////////////////////////////////////
        public void UpdateGame(Guid gameId, MapEntity map)
        {
            GameWaitingRoomEventManager.SetMap(gameId, map);
        }

        public async Task LeaveGame(UserEntity user)
        {
            await LeaveGame(user.Id);
        }

        public async Task LeaveGame(int userId)
        {
            GameMatchMakerService.Instance().RemoveUser(userId);
            GameWaitingRoomEventManager.PlayerLeft(userId);
            await PlayOnlineManager.PlayerLeaveLiveGame(userId);
            await PlayOnlineManager.PlayerLeaveLiveTournament(userId);
        }

        public void GoalScored(Guid gameId, int playerId)
        {
            PlayOnlineManager.GoalScored(gameId, playerId);
        }

        public async Task GameOver(Guid gameId)
        {
            await PlayOnlineManager.GameEnded(gameId);
        }

        public async Task SaveGame(GameEntity game)
        {
            await GameRepository.CreateGame(game);
        }

        public void CreateGame(GameRequestEntity gameRequest)
        {
            GameMatchMakerService.Instance().CreateMatch(gameRequest.Recipient, gameRequest.Sender);
        }
    }
}