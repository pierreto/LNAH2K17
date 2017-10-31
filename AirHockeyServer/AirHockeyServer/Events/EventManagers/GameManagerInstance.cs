using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Events.EventManagers
{
    public class GameManagerInstance
    {

        protected Dictionary<int, GameEntity> Games { get; set; }

        public GameManagerInstance()
        {
            this.Games = new Dictionary<int, GameEntity>();
        }

        public void AddGame(GameEntity game)
        {
            if(!Games.ContainsKey(game.GameId))
            {
                Games[game.GameId] = game;
            }
        }

        public void GoalScored(int gameId, int playerId)
        {
            if (Games.ContainsKey(gameId))
            {
                if (Games[gameId].Players[0].Id == playerId)
                {
                    Games[gameId].Score[0] += 1;
                }
                else
                {
                    Games[gameId].Score[1] += 1;
                }
            }
        }

        public void GameEnded(int gameId)
        {
            if(Games.ContainsKey(gameId))
            {
                var game = Games[gameId];
                game.GameState = GameState.Ended;
                game.Winner = game.Score[0] > game.Score[1] ? game.Players[0] : game.Players[1];

                if(game.TournamentId > -1)
                {
                    TournamentsManager.Instance().UpdateTournamentState(game.TournamentId, game);
                }
                else
                {
                    int points = CaculateGamePoints(Games[gameId]);

                    // SAVE POINTS IN BD
                }

                // TODO Save in DB

                Games.Remove(gameId);
            }
        }

        private int CaculateGamePoints(GameEntity gameEntity)
        {
            return 20;
        }
    }
}