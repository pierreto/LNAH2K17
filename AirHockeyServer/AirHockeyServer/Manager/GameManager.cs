using AirHockeyServer.Core;
using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace AirHockeyServer.Manager
{
    public class GameManager : IGameManager
    {
        public IPlayerStatsService PlayerStatsService { get; }
        public IGameRepository GameRepository { get; }

        public ITournamentRepository TournamentRepository { get; }

        public EventHandler<GameEntity> TournamentUpdateNeeded { get; set; }

        private const int FINAL_DELAI = 10000;

        protected Dictionary<int, int> ElapsedTime { get; set; }

        public GameManager(IPlayerStatsService playerStatsService, IGameRepository gameRepository, ITournamentRepository tournamentRepository)
        {
            ElapsedTime = new Dictionary<int, int>();
            PlayerStatsService = playerStatsService;
            GameRepository = gameRepository;
            TournamentRepository = tournamentRepository;
        }

        public void AddGame(GameEntity game)
        {
            if(game != null && !Cache.Games.ContainsKey(game.GameId))
            {
                Cache.Games[game.GameId] = game;
            }
        }

        public void GoalScored(Guid gameId, int playerId)
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

        public async Task GameEnded(Guid gameId)
        {
            if(Cache.Games.ContainsKey(gameId))
            {
                var game = Cache.Games[gameId];
                game.GameState = GameState.Ended;
                game.Winner = game.Score[0] > game.Score[1] ? game.Players[0] : game.Players[1];

                await GameRepository.CreateGame(game);

                if(game.TournamentId > -1)
                {
                    await UpdateTournamentState(game.TournamentId, game);
                }
                else
                {
                    int points = CaculateGamePoints(Cache.Games[gameId]);
                    await PlayerStatsService.AddPoints(game.Winner.Id, points);
                    await PlayerStatsService.IncrementGamesWon(game.Winner.Id);

                    await PlayerStatsService.SetPlayerAchievements(game.Players[0].Id);
                    await PlayerStatsService.SetPlayerAchievements(game.Players[1].Id);
                }
                
                Cache.Games.Remove(gameId);

                var connectionP1 = ConnectionMapper.GetConnection(game.Players[0].Id);
                var connectionP2 = ConnectionMapper.GetConnection(game.Players[1].Id);
                await GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Groups.Remove(connectionP1, game.GameId.ToString());
                await GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Groups.Remove(connectionP2, game.GameId.ToString());
            }
        }

        private int CaculateGamePoints(GameEntity gameEntity)
        {
            return 20;
        }

        public async Task UpdateTournamentState(int tournamentId, GameEntity gameUpdated)
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
                        tournament.Winner = tournament.Final.Winner;

                        hub.Clients.Group(tournament.Id.ToString()).TournamentFinalResult(tournament);
                        
                        await PlayerStatsService.IncrementTournamentsWon(tournament.Winner.Id);
                        await PlayerStatsService.AddPoints(tournament.Winner.Id, 80);
                        await TournamentRepository.CreateTournament(tournament);

                        await PlayerStatsService.SetPlayerAchievements(tournament.Players[0].Id);
                        await PlayerStatsService.SetPlayerAchievements(tournament.Players[1].Id);
                        await PlayerStatsService.SetPlayerAchievements(tournament.Players[2].Id);
                        await PlayerStatsService.SetPlayerAchievements(tournament.Players[3].Id);

                        Cache.Tournaments.Remove(tournamentId);
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
                        
                        AddGame(finalGame);
                        Cache.Tournaments[tournament.Id] = tournament;

                        var gameHub = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
                        await gameHub.Groups.Add(ConnectionMapper.GetConnection(finalGame.Players[0].Id), finalGame.GameId.ToString());
                        await gameHub.Groups.Add(ConnectionMapper.GetConnection(finalGame.Players[1].Id), finalGame.GameId.ToString());

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