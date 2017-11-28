using AirHockeyServer.Core;
using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AirHockeyServer.Manager
{
    public class PlayOnlineManager : IPlayOnlineManager
    {
        protected const int GAME_WON_POINTS = 20;

        protected const int FINAL_DELAI = 4000;

        public IPlayerStatsService PlayerStatsService { get; }

        public IGameRepository GameRepository { get; }

        public ITournamentRepository TournamentRepository { get; }

        public ConnectionMapper ConnectionMapper { get; }


        protected Dictionary<int, int> ElapsedTime { get; set; }

        public PlayOnlineManager(IPlayerStatsService playerStatsService, IGameRepository gameRepository,
            ITournamentRepository tournamentRepository, ConnectionMapper connectionMapper)
        {
            ElapsedTime = new Dictionary<int, int>();
            PlayerStatsService = playerStatsService;
            GameRepository = gameRepository;
            TournamentRepository = tournamentRepository;
            ConnectionMapper = connectionMapper;
        }

        public void AddGame(GameEntity game)
        {
            if (game != null && !Cache.Games.ContainsKey(game.GameId))
            {
                Cache.Games[game.GameId] = game;
            }
        }

        public void AddTournament(TournamentEntity tournament)
        {
            if (tournament != null && !Cache.Tournaments.ContainsKey(tournament.Id))
            {
                Cache.Tournaments[tournament.Id] = tournament;
            }
        }

        public void GoalScored(Guid gameId, int playerId)
        {
            if (Cache.Games.ContainsKey(gameId))
            {
                //if (playerId == 1)
                //{
                //    //MASTER SCORED
                //    Cache.Games[gameId].Score[0] += 1;
                //}
                //else
                //{
                //    // SLAVE SCORED
                //    Cache.Games[gameId].Score[1] += 1;
                //}

                for (int i = 0; i < Cache.Games[gameId].Players.Count(); i++)
                {
                    if (Cache.Games[gameId].Players[i].Id == playerId)
                    {
                        Cache.Games[gameId].Score[i] += 1;
                        return;
                    }
                }
            }
        }

        public async Task GameEnded(Guid gameId)
        {
            if (!Cache.Games.ContainsKey(gameId))
            {
                return;
            }

            var game = Cache.Games[gameId];
            game.GameState = GameState.Ended;
            game.Winner = game.Score[0] > game.Score[1] ? game.Players[0] : game.Players[1];

            if (game.TournamentId > -1)
            {
                await UpdateTournamentState(game.TournamentId, game);
            }
            else
            {
                await UpdateGame(game);
            }

            Cache.Games.Remove(gameId);
        }

        private async Task UpdateGame(GameEntity game)
        {
            await GameRepository.CreateGame(game);

            await SendGameStats(game);

            await RemoveConnection<GameWaitingRoomHub>(game.Players[0].Id, game.GameId.ToString());
            await RemoveConnection<GameWaitingRoomHub>(game.Players[1].Id, game.GameId.ToString());
        }

        private async Task SendGameStats(GameEntity game)
        {
            List<PlayerEndOfGameStatsEntity> stats = new List<PlayerEndOfGameStatsEntity>();
            foreach (var player in game.Players)
            {
                PlayerEndOfGameStatsEntity playerStats = new PlayerEndOfGameStatsEntity();
                playerStats.Id = player.Id;

                if (game.Winner.Id == player.Id)
                {
                    playerStats.PointsWon = GAME_WON_POINTS;
                    await PlayerStatsService.AddPoints(game.Winner.Id, GAME_WON_POINTS);
                    await PlayerStatsService.IncrementGamesWon(game.Winner.Id);
                }

                playerStats.UnlockedAchievements = await PlayerStatsService.GetAchievementsToUpdate(player.Id);

                stats.Add(playerStats);
            }

            stats.ForEach(x =>
            {
                GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>()
                                .Clients.Group(game.GameId.ToString()).EndOfGameInfo(x);
            });

            foreach (var stat in stats)
            {
                await PlayerStatsService.CreateAchievements(stat.Id, stat.UnlockedAchievements.Select(x => x.AchivementType).ToList());
            }
        }

        private async Task RemoveConnection<T>(int userId, string group) where T : IHub
        {
            var connection = ConnectionMapper.GetConnection(userId);
            if (connection != null)
            {
                await GlobalHost.ConnectionManager.GetHubContext<T>().Groups.Remove(connection, group);
            }
            Cache.RemovePlayer(userId);
        }

        public async Task UpdateTournamentState(int tournamentId, GameEntity gameUpdated)
        {
            if (!Cache.Tournaments.ContainsKey(tournamentId))
            {
                return;
            }

            UpdateTournamentGames(gameUpdated, tournamentId);

            var tournament = Cache.Tournaments[tournamentId];

            if (!tournament.SemiFinals.All(game => game.GameState == GameState.Ended))
            {
                // only 1 semi final is done
                GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>()
                    .Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);

                Cache.Tournaments[tournament.Id] = tournament;
                return;
            }

            if (!(tournament.Final?.GameState == GameState.Ended))
            {
                // create the final game
                await CreateTournamentFinalGame(tournament);
                return;
            }

            // end of tournament
            await ManageEndOfTournament(tournament);

        }

        private async Task CreateTournamentFinalGame(TournamentEntity tournament)
        {
            GameEntity finalGame = CreateTournamentGame(tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Winner, tournament);

            finalGame.SelectedMap = tournament.SelectedMap;
            tournament.State = TournamentState.Final;
            tournament.Final = finalGame;

            GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>()
                .Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);

            AddGame(finalGame);
            Cache.Tournaments[tournament.Id] = tournament;
            
            if (finalGame.Players.All(x => x.IsAi))
            {
                await GameEnded(finalGame.GameId);
                return;
            }

            var gameHub = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
            foreach (var player in finalGame.Players)
            {
                if (!player.IsAi)
                {
                    await gameHub.Groups.Add(ConnectionMapper.GetConnection(player.Id), finalGame.GameId.ToString());
                }
            }

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => FinalCountdown(timerSender, e, tournament, timer);

            this.ElapsedTime.Add(tournament.Id, 0);

            timer.Start();
        }

        private async Task ManageEndOfTournament(TournamentEntity tournament)
        {
            tournament.State = TournamentState.Done;
            tournament.Winner = tournament.Final.Winner;

            GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>()
                .Clients.Group(tournament.Id.ToString()).TournamentFinalResult(tournament);

            if (!tournament.Winner.IsAi)
            {
                await PlayerStatsService.IncrementTournamentsWon(tournament.Winner.Id);
                await PlayerStatsService.AddPoints(tournament.Winner.Id, 80);
            }
            await TournamentRepository.CreateTournament(tournament);

            List<AchievementEntity> achievementsToUpdate = new List<AchievementEntity>();
            foreach (var player in tournament.Players)
            {
                if (!player.IsAi)
                {
                    achievementsToUpdate = await PlayerStatsService.GetAchievementsToUpdate(player.Id);
                    await PlayerStatsService.CreateAchievements(player.Id, achievementsToUpdate.Select(x => x.AchivementType).ToList());
                    await RemoveConnection<TournamentWaitingRoomHub>(player.Id, tournament.Id.ToString());
                }
            }

            Cache.Tournaments.Remove(tournament.Id);
        }

        private void UpdateTournamentGames(GameEntity gameUpdated, int tournamentId)
        {
            for (int i = 0; i < 2; i++)
            {
                if (Cache.Tournaments[tournamentId].SemiFinals[i].GameId == gameUpdated.GameId)
                {
                    Cache.Tournaments[tournamentId].SemiFinals[i] = gameUpdated;
                    return;
                }
            }

            if (Cache.Tournaments[tournamentId].Final?.GameId == gameUpdated.GameId)
            {
                Cache.Tournaments[tournamentId].Final = gameUpdated;
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

        public GameEntity CreateTournamentGame(GamePlayerEntity player1, GamePlayerEntity player2, TournamentEntity tournament)
        {
            GameEntity game = new GameEntity()
            {
                GameId = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                Players = new GamePlayerEntity[2] { player1, player2 },
                Master = player1,
                Slave = player2,
                TournamentId = tournament.Id,
            };

            if (player1.IsAi && player2.IsAi)
            {
                game.Score = new int[2] { 2, 0 };
            }
            if (game.Players.Any(x => x.IsAi))
            {
                game.Master = game.Players[0].IsAi ? game.Players[1] : game.Players[0];
                game.Slave = game.Players[0].IsAi ? game.Players[0] : game.Players[1];
            }

            else
            {
                var stringGameId = game.GameId.ToString();
                foreach (var player in game.Players)
                {
                    if (!player.IsAi)
                    {
                        var connection = ConnectionMapper.GetConnection(player.Id);
                        GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Groups.Add(connection, stringGameId).Wait();
                    }
                }
            }

            return game;
        }

        public async Task PlayerLeaveLiveGame(int userId)
        {
            GameEntity liveGame = null;
            foreach (var game in Cache.Games.Values)
            {
                if ((game.Players[0].Id == userId || game.Players[1].Id == userId) && game.TournamentId == 0)
                {
                    liveGame = game;
                    break;
                }
            }

            if (liveGame == null)
            {
                return;
            }

            liveGame.GameState = GameState.Ended;
            liveGame.Winner = userId == liveGame.Players[0].Id ? liveGame.Players[1] : liveGame.Players[2];

            await UpdateGame(liveGame);

            Cache.Games.Remove(liveGame.GameId);
        }

        public async Task PlayerLeaveLiveTournament(int userId)
        {
            GameEntity tournamentGame = null;
            foreach (var game in Cache.Games.Values)
            {
                if ((game.Players[0].Id == userId || game.Players[1].Id == userId) && game.TournamentId > 0)
                {
                    tournamentGame = game;
                    break;
                }
            }

            if (tournamentGame == null)
            {
                return;
            }

            tournamentGame.GameState = GameState.Ended;
            if (userId == tournamentGame.Players[0].Id)
            {
                tournamentGame.Winner = tournamentGame.Players[1];
            }
            else
            {
                tournamentGame.Winner = tournamentGame.Players[0];
            }

            await UpdateTournamentState(tournamentGame.TournamentId, tournamentGame);
            Cache.Games.Remove(tournamentGame.GameId);
        }
    }
}