using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services
{
    public interface IGameService
    {
        Task<GameEntity> CreateGame(GameEntity gameEntity);

        void JoinGame(UserEntity userEntity);

        Task<GameEntity> UpdateGame(GameEntity gameEntity);

        GameEntity GetGameEntityById(Guid id);
    }
}