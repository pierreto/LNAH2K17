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
        protected const int WAITING_TIMEOUT = 30000;

        protected ConcurrentDictionary<int, int> RemainingTime { get; set; }

        protected ConcurrentDictionary<int, TournamentEntity> Tournaments { get; set; }

        public IPlayOnlineManager PlayOnlineManager { get; }

        public IMapService MapService { get; set; }

        public ConnectionMapper ConnectionMapper { get; set; }

        protected List<int> LeftPlayers { get; set; }

        public TournamentWaitingRoomEventManager(IPlayOnlineManager gameManager,
            IMapService mapService, ConnectionMapper connectionMapper)
        {
            this.RemainingTime = new ConcurrentDictionary<int, int>();
            TournamentMatchMakerService.Instance().OpponentFound += OnOpponentFound;

            PlayOnlineManager = gameManager;
            MapService = mapService;
            ConnectionMapper = connectionMapper;
            Tournaments = new ConcurrentDictionary<int, TournamentEntity>();
            LeftPlayers = new List<int>();
        }

        protected void OnOpponentFound(object sender, TournamentEntity tournament)
        {
            if (!(tournament.Players.Count == 4))
            {
                return;
            }

            GameEntity game1 = PlayOnlineManager.CreateTournamentGame(tournament.Players[0], tournament.Players[1], tournament);
            GameEntity game2 = PlayOnlineManager.CreateTournamentGame(tournament.Players[2], tournament.Players[3], tournament);

            tournament.SemiFinals.Add(game1);
            tournament.SemiFinals.Add(game2);
            tournament.State = TournamentState.TournamentConfiguration;

            Tournaments[tournament.Id] = tournament;
            this.RemainingTime[tournament.Id] = 0;

            GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(tournament.Id.ToString()).TournamentAllOpponentsFound(tournament);

            System.Timers.Timer timer = CreateTimeoutTimer(tournament);

            timer.Start();

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
        protected async Task WaitingRoomTimeOutAsync(object source, ElapsedEventArgs e, int tournamentId, System.Timers.Timer timer)
        {
            if (LeftPlayers.Count > 0 && LeftPlayers.Any(x => Tournaments[tournamentId].Players.Find(w => w.Id == x) != null))
            {
                timer.Stop();

                GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(tournamentId.ToString()).PlayerLeft();

                var playerToRemove = LeftPlayers.Find(x => Tournaments[tournamentId].Players.Find(w => w.Id == x) != null);
                LeftPlayers.Remove(playerToRemove);

                TournamentMatchMakerService.Instance().AddOpponent(Tournaments[tournamentId].Players.Where(x => x.Id != playerToRemove).ToList());

                TournamentEntity removed = new TournamentEntity();
                Tournaments.TryRemove(tournamentId, out removed);

                return;
            }

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

                PlayOnlineManager.AddGame(Tournaments[tournamentId].SemiFinals[0]);
                PlayOnlineManager.AddGame(Tournaments[tournamentId].SemiFinals[1]);
                PlayOnlineManager.AddTournament(Tournaments[tournamentId]);

                GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(tournamentId.ToString()).TournamentStarting(Tournaments[tournamentId]);

                var tournament = Tournaments[tournamentId];

                TournamentEntity removed = new TournamentEntity();
                Tournaments.TryRemove(tournamentId, out removed);

                foreach (var game in tournament.SemiFinals)
                {
                    if (game.Players.All(x => x.IsAi))
                    {
                        await PlayOnlineManager.GameEnded(game.GameId);
                    }
                }

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

        public void PlayerLeft(int userId)
        {
            if (!LeftPlayers.Exists(x => userId == x) && IsInvolvedInTournament(userId))
            {
                LeftPlayers.Add(userId);
            }
        }

        private bool IsInvolvedInTournament(int userId)
        {
            foreach (var game in Tournaments.Values)
            {
                if (game.Players.Exists(x => x.Id == userId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}