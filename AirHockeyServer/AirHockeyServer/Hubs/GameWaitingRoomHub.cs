using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web;
using AirHockeyServer.Services;
using AirHockeyServer.Entities;
using System.Threading.Tasks;

namespace AirHockeyServer.Hubs
{
    public class GameWaitingRoomHub : Hub 
    {

        public GameWaitingRoomHub(IGameService gameService)
        {
            GameService = gameService;
        }

        public IGameService GameService { get; }

        public async Task<Guid> CreateGame(GameEntity gameEntity)
        {
            gameEntity.CreationDate = DateTime.Now;

            Guid gameCreatedId = GameService.CreateGame(gameEntity);
            await Groups.Add(Context.ConnectionId, gameCreatedId.ToString());

            return gameCreatedId;
        }

        public void JoinGame(UserEntity user)
        {
            GameService.JoinGame(user);
        }

        public static void OpponentMatch()
        {

        }
    }
}