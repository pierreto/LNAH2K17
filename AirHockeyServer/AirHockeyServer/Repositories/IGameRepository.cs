using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Repositories
{
    public interface IGameRepository
    {
        Task<GameEntity> CreateGame(GameEntity game);

        Task<GameEntity> GetGame(int gameId);
    }
}
