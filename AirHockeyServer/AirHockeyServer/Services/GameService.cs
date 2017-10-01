using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public class GameService : IGameService
    {
        public Guid CreateGame(GameEntity gameEntity)
        {
            // call bd
            Guid gameCreatedId = new Guid();

            // start match making

            return gameCreatedId;
        }

        public void JoinGame(UserEntity userEntity)
        {
            // start match making
        }
        
    }
}