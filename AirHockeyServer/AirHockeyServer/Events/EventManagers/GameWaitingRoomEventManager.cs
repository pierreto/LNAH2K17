using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using AirHockeyServer.Services.MatchMaking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;

namespace AirHockeyServer.Events.EventManagers
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file GameWaitingRoomEventManager.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe permet de gérer les évènements relatifs à la préparation
    /// d'une partie en ligne
    ///////////////////////////////////////////////////////////////////////////////
    public class GameWaitingRoomEventManager : WaitingRoomEventManager
    {
        GameEntity game;

        public GameWaitingRoomEventManager(IGameService gameService) : base(gameService)
        {
            GameMatchMakerService.Instance().MatchFoundEvent += OnMatchFound;
            HubContext = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void OnMatchFound(object sender, MatchFoundArgs args)
        ///
        /// Cette fonction est appelé lorsqu'un évènement du type "MatchFoundEvent"
        /// est lancé. Il doit donc récupérer les opposants d'un match et les 
        /// avertir qu'un adversaire leur a été attribué.
        ///
        ////////////////////////////////////////////////////////////////////////
        private async void OnMatchFound(object sender, MatchFoundArgs args)
        {
            GameEntity game = new GameEntity()
            {
                CreationDate = DateTime.Now,
                Players = new UserEntity[2] { args.PlayersMatch.PlayersMatch[0], args.PlayersMatch.PlayersMatch[1] },
                Master = args.PlayersMatch.PlayersMatch[0],
                Slave = args.PlayersMatch.PlayersMatch[1]
            };

            GameEntity gameCreated = await GameService.CreateGame(game);

            var stringGameId = gameCreated.GameId.ToString();
            
            foreach(var player in gameCreated.Players)
            {
                var connection = ConnectionMapper.GetConnection(player.UserId);
                await HubContext.Groups.Add(connection, stringGameId);
            }

            game = gameCreated;
            HubContext.Clients.Group(stringGameId).OpponentFoundEvent(gameCreated);
            
            this.RemainingTime[gameCreated.GameId] = 0;

            System.Timers.Timer timer = CreateTimeoutTimer(gameCreated.GameId);
            timer.Start();
        }

        protected override void SendRemainingTimeEvent(int remainingTime)
        {
            var Hub = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
            Hub.Clients.Group(game.GameId.ToString()).WaitingRoomRemainingTime(remainingTime);
        }

        protected override void SendEndOfTimer()
        {
            var Hub = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
            Hub.Clients.Group(game.GameId.ToString()).TournamentStarting(game);
        }
    }
}
