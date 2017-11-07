using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using AirHockeyServer.Services;
using AirHockeyServer.Core;
using AirHockeyServer.Services.Interfaces;

namespace AirHockeyServer.Manager
{
    public class TournamentManager : ITournamentManager
    {
        private const int FINAL_DELAI = 10000;

        protected Dictionary<int, int> ElapsedTime { get; set; }

        public IPlayerStatsService PlayerStatsService { get; }

        public IGameManager GameManager { get; private set; }

        public TournamentManager(IPlayerStatsService playerStatsService, IGameManager gameManager)
        {
            this.ElapsedTime = new Dictionary<int, int>();
            PlayerStatsService = playerStatsService;
            GameManager = gameManager;

            //GameManager.TournamentUpdateNeeded += (sender, game) => UpdateTournamentGames(game, game.TournamentId);
        }

        public void AddTournament(TournamentEntity tournament)
        {
            if (tournament != null && !Cache.Tournaments.ContainsKey(tournament.Id))
            {
                Cache.Tournaments[tournament.Id] = tournament;

                GameManager.AddGame(tournament.SemiFinals[0]);
                GameManager.AddGame(tournament.SemiFinals[1]);
            }
        }

        public async void UpdateTournamentState(int tournamentId, GameEntity gameUpdated)
        {
            UpdateTournamentGames(gameUpdated, tournamentId);
            if (Cache.Tournaments.ContainsKey(tournamentId))
            {
                var tournament = Cache.Tournaments[tournamentId];
                var hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();

                if (tournament.SemiFinals.All(game => game.GameState == GameState.Ended))
                {
                    if (tournament.Final?.GameState == GameState.Ended)
                    {
                        // end of tournament
                        tournament.State = TournamentState.Done;

                        hub.Clients.Group(tournament.Id.ToString()).TournamentFinalResult(tournament);

                        // save to DB
                        await PlayerStatsService.IncrementTournamentsWon(tournament.Winner.Id);
                        Cache.Tournaments.Remove(tournamentId);

                        return;
                    }
                    else
                    {
                        // do final
                        GameEntity finalGame = new GameEntity
                        {
                            Players = new UserEntity[] { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Winner },
                            GameState = GameState.InProgress,
                            GameId = Guid.NewGuid(),
                            CreationDate = DateTime.Now,
                            TournamentId = tournament.Id
                        };
                        finalGame.Master = finalGame.Players[0];
                        finalGame.Slave = finalGame.Players[1];

                        tournament.State = TournamentState.Final;
                        tournament.Final = finalGame;


                        GameManager.AddGame(finalGame);
                        Cache.Tournaments[tournament.Id] = tournament;

                        hub.Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);

                        Timer timer = new Timer();
                        timer.Interval = 1000;
                        timer.Elapsed += (timerSender, e) => FinalCountdown(timerSender, e, tournament, timer);

                        this.ElapsedTime.Add(tournament.Id, 0);

                        timer.Start();
                    }
                }
                else
                {
                    hub.Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);
                    //testFlow(tournament.Id);
                }

                Cache.Tournaments[tournament.Id] = tournament;
            }
        }

        private void testFlow(int tournamentId)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
            var tournament = Cache.Tournaments[tournamentId];

            tournament.SemiFinals[1].Winner = tournament.SemiFinals[1].Players[0];
            tournament.SemiFinals[1].GameState = GameState.Ended;

            GameEntity finalGame = new GameEntity
            {
                Players = new UserEntity[] { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Players[0] },
                GameState = GameState.InProgress,
                GameId = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                TournamentId = tournament.Id
            };
            finalGame.Master = finalGame.Players[0];
            finalGame.Slave = finalGame.Players[1];

            GameManager.AddGame(finalGame);

            tournament.State = TournamentState.Final;
            tournament.Final = finalGame;

            Cache.Tournaments[tournament.Id] = tournament;

            hub.Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => FinalCountdown(timerSender, e, tournament, timer);

            this.ElapsedTime.Add(tournament.Id, 0);

            timer.Start();
        }

        private void UpdateTournamentGames(GameEntity gameUpdated, int tournamentId)
        {
            if (Cache.Tournaments.ContainsKey(tournamentId))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (Cache.Tournaments[tournamentId].SemiFinals[i].GameId == gameUpdated.GameId)
                    {
                        Cache.Tournaments[tournamentId].SemiFinals[i] = gameUpdated;
                        break;
                    }
                }

                if (Cache.Tournaments[tournamentId].Final?.GameId == gameUpdated.GameId)
                {
                    Cache.Tournaments[tournamentId].Final = gameUpdated;
                }
            }
        }

        private void FinalCountdown(object timerSender, ElapsedEventArgs e, TournamentEntity tournament, Timer timer)
        {
            this.ElapsedTime[tournament.Id] += 1000;
            if (ElapsedTime[tournament.Id] >= FINAL_DELAI)
            {
                timer.Stop();
                ElapsedTime.Remove(tournament.Id);

                var hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
                hub.Clients.Group(tournament.Id.ToString()).StartFinal(tournament);
            }
        }

    }
}