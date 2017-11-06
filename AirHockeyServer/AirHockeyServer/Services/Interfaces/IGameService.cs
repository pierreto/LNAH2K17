using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IGameService
    {
        Task<GameEntity> CreateGame(GameEntity gameEntity);

        void JoinGame(UserEntity userEntity);

        Task<GameEntity> UpdateGame(GameEntity gameEntity);

        GameEntity GetGameEntityById(Guid id);

        void LeaveGame(UserEntity user);

        void GoalScored(Guid gameId, int playerId);

        Task GameOver(Guid gameId);

        Task SaveGame(GameEntity game);
    }
}