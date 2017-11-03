using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Manager
{
    public interface IGameManager
    {
        void AddGame(GameEntity game);

        void GoalScored(int gameId, int playerId);

        void GameEnded(int gameId);
    }
}
