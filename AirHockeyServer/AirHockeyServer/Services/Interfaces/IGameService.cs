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

        void JoinGame(GamePlayerEntity gamePlayer);

        void UpdateGame(Guid gameId, MapEntity map);

        Task LeaveGame(UserEntity user);

        void GoalScored(Guid gameId, int playerId);

        Task GameOver(Guid gameId);

        Task SaveGame(GameEntity game);

        void LeaveGame(int userId);
    }
}