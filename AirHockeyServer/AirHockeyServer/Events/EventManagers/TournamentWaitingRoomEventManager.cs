using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Manager;
using AirHockeyServer.Services;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Services.MatchMaking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AirHockeyServer.Events.EventManagers
{
    public class TournamentWaitingRoomEventManager
    {
        protected const int WAITING_TIMEOUT = 3000;

        protected ConcurrentDictionary<int, int> RemainingTime { get; set; }

        protected ConcurrentDictionary<int, TournamentEntity> Tournaments { get; set; }

        public PlayOnlineManager GameManager { get; }

        public IMapService MapService { get; set; }
        public ConnectionMapper ConnectionMapper { get; set; }

        public TournamentWaitingRoomEventManager(PlayOnlineManager gameManager,
            IMapService mapService, ConnectionMapper connectionMapper)
        {
            this.RemainingTime = new ConcurrentDictionary<int, int>();
            TournamentMatchMakerService.Instance().OpponentFound += OnOpponentFound;

            GameManager = gameManager;
            MapService = mapService;
            ConnectionMapper = connectionMapper;
            Tournaments = new ConcurrentDictionary<int, TournamentEntity>();
        }

        protected void OnOpponentFound(object sender, TournamentEntity tournament)
        {

            //if (Tournament == null)
            //{
            //    Tournament = new TournamentEntity
            //    {
            //        Id = new Random().Next(),
            //        State = TournamentState.WaitingForPlayers
            //    };
            //}

            //foreach(var user in users)
            //{
            //    if(!user.IsAi)
            //    {
            //        var connection = ConnectionMapper.GetConnection(user.Id);
            //        string tournamentIdString = Tournament.Id.ToString();
            //        GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Groups.Add(connection, tournamentIdString).Wait();

            //        GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(Tournament.Id.ToString()).OpponentFoundEvent(Tournament.Players);
            //    }

            //    Tournament.Players.Add(user);
            //}

            if (tournament.Players.Count == 4)
            {
                GameEntity game1 = GameManager.CreateTournamentGame(tournament.Players[0], tournament.Players[1], tournament);
                GameEntity game2 = GameManager.CreateTournamentGame(tournament.Players[2], tournament.Players[3], tournament);

                tournament.SemiFinals.Add(game1);
                tournament.SemiFinals.Add(game2);
                tournament.State = TournamentState.TournamentConfiguration;

                Tournaments[tournament.Id] = tournament;
                this.RemainingTime[tournament.Id] = 0;
                
                GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(tournament.Id.ToString()).TournamentAllOpponentsFound(tournament);

                System.Timers.Timer timer = CreateTimeoutTimer(tournament);

                timer.Start();
            }

        }

        //private GameEntity CreateGame(GamePlayerEntity player1, GamePlayerEntity player2, int tournamentId)
        //{
        //    GameEntity game = new GameEntity()
        //    {
        //        GameId = Guid.NewGuid(),
        //        CreationDate = DateTime.Now,
        //        Players = new GamePlayerEntity[2] { player1, player2 },
        //        Master = player1,
        //        Slave = player2,
        //        TournamentId = tournamentId
        //    };

        //    var stringGameId = game.GameId.ToString();

        //    foreach (var player in game.Players)
        //    {
        //        if (!player.IsAi)
        //        {
        //            var connection = ConnectionMapper.GetConnection(player.Id);
        //            GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Groups.Add(connection, stringGameId).Wait();
        //        }
        //    }

        //    if(game.Players.All(x=> x.IsAi))
        //    {
        //        game.Score = new int[2] { 0, 3 };
        //        game.Winner = player2;
        //    }

        //    return game;
        //}

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
        protected async Task WaitingRoomTimeOutAsync(object source, ElapsedEventArgs e, int tournamentId, System.Timers.Timer timer)
        {
            if (RemainingTime[tournamentId] < WAITING_TIMEOUT)
            {
                RemainingTime[tournamentId] += 1000;

                GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(tournamentId.ToString()).WaitingRoomRemainingTime((WAITING_TIMEOUT - RemainingTime[tournamentId]) / 1000);
            }
            else
            {
                timer.Stop();

                int mapId = 0;
                if (Tournaments[tournamentId].SelectedMap == null)
                {
                    var maps = await MapService.GetMaps();
                    mapId = maps.First().Id.Value;
                }
                else
                {
                    mapId = Tournaments[tournamentId].SelectedMap.Id.Value;
                }

                Tournaments[tournamentId].SelectedMap = await MapService.GetMap(mapId);

                Tournaments[tournamentId].SemiFinals[0].SelectedMap = Tournaments[tournamentId].SelectedMap;
                Tournaments[tournamentId].SemiFinals[1].SelectedMap = Tournaments[tournamentId].SelectedMap;

                Tournaments[tournamentId].State = TournamentState.SemiFinals;

                GameManager.AddGame(Tournaments[tournamentId].SemiFinals[0]);
                GameManager.AddGame(Tournaments[tournamentId].SemiFinals[1]);
                GameManager.AddTournament(Tournaments[tournamentId]);

                GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(tournamentId.ToString()).TournamentStarting(Tournaments[tournamentId]);

                Tournaments[tournamentId].SemiFinals.ForEach(async semiFinal =>
                {
                    if (semiFinal.Players.All(x => x.IsAi))
                    {
                        await GameManager.GameEnded(semiFinal.GameId);
                    }
                });
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
        protected System.Timers.Timer CreateTimeoutTimer(TournamentEntity tournament)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => WaitingRoomTimeOutAsync(timerSender, e, tournament.Id, timer);

            return timer;
        }

        public void SetMap(int tournamentId, MapEntity map)
        {
            Tournaments[tournamentId].SelectedMap = map;
        }
    }
}