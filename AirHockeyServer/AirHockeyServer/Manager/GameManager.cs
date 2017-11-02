using AirHockeyServer.Core;
using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace AirHockeyServer.Manager
{
    public class GameManager : IGameManager
    {
        //public IPlayerStatsService PlayerStatsService { get; }

        public EventHandler<GameEntity> TournamentUpdateNeeded { get; set; }

        private const int FINAL_DELAI = 10000;

        protected Dictionary<int, int> ElapsedTime { get; set; }

        public GameManager()
        {
            ElapsedTime = new Dictionary<int, int>();
            //PlayerStatsService = new PlayerStatsService();
        }

        public void AddGame(GameEntity game)
        {
            if(!Cache.Games.ContainsKey(game.GameId))
            {
                Cache.Games[game.GameId] = game;
            }
        }

        public void GoalScored(int gameId, int playerId)
        {
            if (Cache.Games.ContainsKey(gameId))
            {
                if (Cache.Games[gameId].Players[0].Id == playerId)
                {
                    Cache.Games[gameId].Score[0] += 1;
                }
                else
                {
                    Cache.Games[gameId].Score[1] += 1;
                }
            }
        }

        public void GameEnded(int gameId)
        {
            if(Cache.Games.ContainsKey(gameId))
            {
                var game = Cache.Games[gameId];
                game.GameState = GameState.Ended;
                game.Winner = game.Score[0] > game.Score[1] ? game.Players[0] : game.Players[1];
                if(game.TournamentId > -1)
                {
                    //TournamentUpdateNeeded?.Invoke(this, game);
                    //TournamentManager.UpdateTournamentState(game.TournamentId, game);
                    UpdateTournamentState(game.TournamentId, game);
                }
                else
                {
                    int points = CaculateGamePoints(Cache.Games[gameId]);
                    //PlayerStatsService.AddPoints(game.Winner.Id, points);
                    //PlayerStatsService.IncrementGamesWon(game.Winner.Id);
                }
                // TODO Save in DB
                Cache.Games.Remove(gameId);
            }
        }

        private int CaculateGamePoints(GameEntity gameEntity)
        {
            return 20;
        }

        public void UpdateTournamentState(int tournamentId, GameEntity gameUpdated)
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
                        //await PlayerStatsService.IncrementTournamentsWon(tournament.Winner.Id);
                        Cache.Tournaments.Remove(tournamentId);
                    }
                    else
                    {
                        // do final
                        GameEntity finalGame = new GameEntity
                        {
                            Players = new UserEntity[] { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Winner },
                            GameState = GameState.InProgress,
                            GameId = new Random().Next(),
                            CreationDate = DateTime.Now,
                            TournamentId = tournament.Id
                        };
                        finalGame.Master = finalGame.Players[0];
                        finalGame.Slave = finalGame.Players[1];

                        tournament.State = TournamentState.Final;
                        tournament.Final = finalGame;


                        AddGame(finalGame);
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