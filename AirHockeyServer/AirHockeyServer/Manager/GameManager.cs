using AirHockeyServer.Core;
using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Manager
{
    public class GameManager
    {
        //public IPlayerStatsService PlayerStatsService { get; }

        public EventHandler<GameEntity> TournamentUpdateNeeded { get; set; }

        public GameManager()
        {
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
                    TournamentUpdateNeeded.Invoke(this, game);
                    //TournamentManager.UpdateTournamentState(game.TournamentId, game);
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
    }
}