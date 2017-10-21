using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using AirHockeyServer.Services.MatchMaking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirHockeyServer.Events.EventManagers
{
    public class TournamentWaitingRoomEventManager : WaitingRoomEventManager
    {
        protected TournamentEntity Tournament { get; set; }

        public TournamentWaitingRoomEventManager(IGameService gameService) : base(gameService)
        {
            TournamentMatchMakerService.Instance().OpponentFound += OnOpponentFound;
            HubContext = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
        }

        protected async void OnOpponentFound(object sender, UserEntity user)
        {
            if (Tournament == null)
            {
                Tournament = new TournamentEntity
                {
                    Id = new Random().Next()
                };
            }


            var connection = ConnectionMapper.GetConnection(user.UserId);
            await HubContext.Groups.Add(connection, Tournament.Id.ToString());

            Tournament.Players.Add(user);

            HubContext.Clients.Group(Tournament.Id.ToString()).OpponentFoundEvent(user);

            if (Tournament.Players.Count == 4)
            {
                GameEntity game1 = await CreateGame(Tournament.Players[0], Tournament.Players[1]);
                GameEntity game2 = await CreateGame(Tournament.Players[2], Tournament.Players[3]);

                Tournament.Games.Add(game1);
                Tournament.Games.Add(game2);

                this.RemainingTime[Tournament.Id] = 0;

                HubContext.Clients.Group(Tournament.Id.ToString()).TournamentAllOpponentsFound(Tournament);

                System.Timers.Timer timer = CreateTimeoutTimer(Tournament.Id);
                timer.Start();
            }

        }

        private async Task<GameEntity> CreateGame(UserEntity player1, UserEntity player2)
        {
            GameEntity game = new GameEntity()
            {
                CreationDate = DateTime.Now,
                Players = new UserEntity[2] { player1, player2 },
                Master = player1,
                Slave = player2
            };

            GameEntity gameCreated = await GameService.CreateGame(game);

            var stringGameId = gameCreated.GameId.ToString();

            foreach (var player in gameCreated.Players)
            {
                var connection = ConnectionMapper.GetConnection(player.UserId);
                await HubContext.Groups.Add(connection, stringGameId);
            }

            return gameCreated;
        }

        protected override void SendRemainingTimeEvent(int remainingTime)
        {
            var Hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
            Hub.Clients.Group(Tournament.Id.ToString()).WaitingRoomRemainingTime(remainingTime);
        }

        protected override void SendEndOfTimer()
        {
            var Hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
            Hub.Clients.Group(Tournament.Id.ToString()).TournamentStarting(Tournament);
        }
    }
}