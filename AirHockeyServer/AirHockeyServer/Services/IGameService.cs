using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services
{
    public interface IGameService
    {
        Guid CreateGame(GameEntity gameEntity);

        void JoinGame(UserEntity userEntity);
    }
}