using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;

namespace InterfaceGraphique.CommunicationInterface
{
    public class GameHub : IBaseHub
    {
        public event Action<GameDataMessage> NewPositions;
        public event Action<GoalMessage> NewGoal;
        public event Action NewGameOver;

        private Guid gameGuid;

        private IHubProxy gameHubProxy;
        public void InitializeHub(HubConnection connection)
        {
            //   gameHubProxy = connection.CreateHubProxy("GameWaitingRoomHub");
            gameHubProxy = GameWaitingRoomHub.WaitingRoomProxy;
        }

        //For the slave
        public  void InitializeSlaveGameHub(Guid gameGuid)
        {
            this.gameGuid = gameGuid;

            // Étape necessaire pour que le serveur sache que la connexion est reliée au bon userId:
            // await gameHubProxy.Invoke("JoinRoom", gameGuid);
            try
            {
                gameHubProxy.On<GameDataMessage>("ReceivedGameData", message =>
                {
                    NewPositions?.Invoke(message);
                });
            }catch(Exception e)
            {

            }

            gameHubProxy.On<GoalMessage>("ReceivedGoal", message =>
            {
                NewGoal?.Invoke(message);
            });
            
            gameHubProxy.On("ReceivedGameOver", () =>
            {
                NewGameOver?.Invoke();
            });
        }

        public async Task SendSlavePosition(float[] slavePosition)
        {
            GameDataMessage gameDataMessage = new GameDataMessage(slavePosition);

            await gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
        }


        //For the master
        public void InitializeMasterGameHub(Guid gameId)
        {
            this.gameGuid = gameId;
            // Étape necessaire pour que le serveur sache que la connexion est reliée au bon userId:
            //await gameHubProxy.Invoke("JoinRoom", gameGuid);

            gameHubProxy.On<GameDataMessage>("ReceivedGameData", message =>
            {
                NewPositions?.Invoke(message);
            });
        }

        public async Task SendGameData(float[] slavePosition, float[] masterPosition, float[] puckPosition)
        {
            GameDataMessage gameDataMessage = new GameDataMessage(slavePosition, masterPosition,puckPosition);

            await gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
        }

        public async Task SendGameOver()
        {
            await gameHubProxy.Invoke("GameOver", gameGuid);
        }

        public async Task SendGoal(int player)
        {
            GoalMessage goalMessage = new GoalMessage(player);
            await gameHubProxy.Invoke("SendGoal", gameGuid, goalMessage);

        }


        public async Task Logout()
        {
            //gameHubProxy.Invoke("LeaveRoom", gameGuid);

            //gameHubProxy?.Invoke("Disconnect", User.Instance.UserEntity.Username).Wait();
        }
    }
}
