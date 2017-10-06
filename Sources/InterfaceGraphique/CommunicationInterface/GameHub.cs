using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    public class GameHub : IBaseHub
    {
        public event Action<GameDataMessage> NewPositions;
        public event Action<GoalMessage> NewGoal;
        public event Action NewGameOver;

        private Guid gameGuid;


        private string username;
        private IHubProxy gameHubProxy;
        public void InitializeHub(HubConnection connection, string username)
        {
            this.username = username;
            gameHubProxy = connection.CreateHubProxy("GameHub");
        }
        //For the slave
        public void InitializeSlaveGameHub(Guid gameGuid)
        {
            this.gameGuid = gameGuid;

            gameHubProxy.On<GameDataMessage>("ReceivedGameData", message =>
            {
                NewPositions?.Invoke(message);
            });

            gameHubProxy.On<GoalMessage>("ReceivedGoal", message =>
            {
                NewGoal?.Invoke(message);
            });
            gameHubProxy.On("ReceivedGameOver", () =>
            {
                NewGameOver?.Invoke();
            });
        }

        public void SendSlavePosition(float[] slavePosition)
        {
            GameDataMessage gameDataMessage = new GameDataMessage(slavePosition);

            gameHubProxy.Invoke("SendGameData", gameGuid,gameDataMessage);
        }


        //For the master
        public void InitializeMasterGameHub(Guid gameGuid)
        {
            this.gameGuid = gameGuid;

            gameHubProxy.On<GameDataMessage>("ReceivedGameData", message =>
            {
                NewPositions?.Invoke(message);
            });
        }

        public void SendMasterPosition(float[] slavePosition, float[] masterPosition, float[] puckPosition)
        {
            GameDataMessage gameDataMessage = new GameDataMessage(slavePosition, masterPosition,puckPosition);

            gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
        }

        public void SendGameOver()
        {
            gameHubProxy.Invoke("SendGameOver", gameGuid);

        }


        public void Logout()
        {
            gameHubProxy?.Invoke("Disconnect", this.username).Wait();
        }
    }
}
