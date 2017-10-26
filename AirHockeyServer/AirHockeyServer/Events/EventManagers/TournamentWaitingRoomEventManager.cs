using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using AirHockeyServer.Services.MatchMaking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace AirHockeyServer.Events.EventManagers
{
    public class TournamentWaitingRoomEventManager
    {
        protected const int WAITING_TIMEOUT = 2000;

        protected ConcurrentDictionary<int, int> RemainingTime { get; set; }

        protected TournamentEntity Tournament { get; set; }

        protected IHubContext HubContext { get; set; }
        public IGameService GameService { get; }

        public TournamentWaitingRoomEventManager(IGameService gameService)
        {
            this.RemainingTime = new ConcurrentDictionary<int, int>();
            TournamentMatchMakerService.Instance().OpponentFound += OnOpponentFound;
            HubContext = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
            GameService = gameService;
        }

        protected async void OnOpponentFound(object sender, UserEntity user)
        {
            if(Tournament?.Players?.Count >= 4)
            {
                Tournament = null;
            }

            if (Tournament == null)
            {
                Tournament = new TournamentEntity
                {
                    Id = new Random().Next(),
                    State = TournamentState.WaitingForPlayers
                };
            }



            var connection = ConnectionMapper.GetConnection(user.Id);
            await HubContext.Groups.Add(connection, Tournament.Id.ToString());

            Tournament.Players.Add(user);

            HubContext.Clients.Group(Tournament.Id.ToString()).OpponentFoundEvent(Tournament.Players);

            if (Tournament.Players.Count == 4)
            {
                GameEntity game1 = await CreateGame(Tournament.Players[0], Tournament.Players[1]);
                GameEntity game2 = await CreateGame(Tournament.Players[2], Tournament.Players[3]);

                Tournament.SemiFinals.Add(game1);
                Tournament.SemiFinals.Add(game2);
                Tournament.State = TournamentState.TournamentConfiguration;

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
                Slave = player2,
                TournamentId = Tournament.Id
            };

            GameEntity gameCreated = await GameService.CreateGame(game);

            var stringGameId = gameCreated.GameId.ToString();

            foreach (var player in gameCreated.Players)
            {
                var connection = ConnectionMapper.GetConnection(player.Id);
                await GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Groups.Add(connection, stringGameId);
            }

            return gameCreated;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void WaitingRoomTimeOut(object source, ElapsedEventArgs e, Guid gameId, Timer timer)
        ///
        /// Fonction appellé à chaque seconde. Vérifie si le temps est échoué pour la modification
        /// des paramètre de parties. Si ce n'est pas le cas, avertis les clients du temps restants. 
        /// Sinon, il vérifier s'il mettre par defaut la carte et la configuration et avertis
        /// les clients du démarrage de la partie
        ///
        ////////////////////////////////////////////////////////////////////////
        protected void WaitingRoomTimeOut(object source, ElapsedEventArgs e, int tournamentId, System.Timers.Timer timer)
        {
            if (RemainingTime[tournamentId] < WAITING_TIMEOUT)
            {
                RemainingTime[tournamentId] += 1000;
                
                HubContext.Clients.Group(tournamentId.ToString()).WaitingRoomRemainingTime((WAITING_TIMEOUT - RemainingTime[tournamentId]) / 1000);
            }
            else
            {
                timer.Stop();
                
                Tournament.State = TournamentState.SemiFinals;
                TournamentsManager.Instance().AddTournament(Tournament);
                HubContext.Clients.Group(tournamentId.ToString()).TournamentStarting(Tournament);
            }
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn Timer CreateTimeoutTimer(Guid gameId)
        ///
        /// Cette fonction crée un timer qui appellera la fonction WaitingRoomTimeOut
        /// à chaque seconde
        /// 
        /// @return le timer créé
        ///
        ////////////////////////////////////////////////////////////////////////
        protected System.Timers.Timer CreateTimeoutTimer(int gameId)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => WaitingRoomTimeOut(timerSender, e, gameId, timer);

            return timer;
        }
    }
}