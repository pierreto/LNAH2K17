using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using Microsoft.AspNet.SignalR;

namespace AirHockeyServer.Hubs
{
    public class GameHub : Hub
    {
        public void CreateGame()
        {
            Clients.Group(Guid.NewGuid().ToString());
        }


        public async Task SendGameData(Guid gameId, GameDataMessage gameData)
        {
            await Clients.Group(gameId.ToString()).ReceivedGameData(gameData);
        }

        public async Task SendGoal(Guid gameId, GoalMessage goal)
        {
            await Clients.Group(gameId.ToString()).ReceivedGoal(goal);
        }

        public async Task GameOver(Guid gameId)
        {
            await Clients.Group(gameId.ToString()).ReceivedGameOver();
        }
    }
}
