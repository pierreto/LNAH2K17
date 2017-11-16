using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using System.Windows.Forms;

namespace InterfaceGraphique.CommunicationInterface
{
    public class GameHub : IBaseHub
    {
        public event Action<GameDataMessage> NewPositions;
        public event Action<GoalMessage> NewGoal;
        public event Action NewGameOver;

        private Guid gameGuid;

        private IHubProxy gameHubProxy;

        public string[] Textures { get; set; }
        public void InitializeHub(HubConnection connection)
        {
            //   gameHubProxy = connection.CreateHubProxy("GameWaitingRoomHub");
            gameHubProxy = GameWaitingRoomHub.WaitingRoomProxy;
        }

        private void ManagePlayerDisconnection()
        {
            Program.QuickPlay.Invoke(new MethodInvoker(() =>
            {
                Program.QuickPlay.ReplacePlayerByAI();
            }));

            MessageBox.Show(
                @"Votre adversaire n'est plus en ligne et vient d'être remplacé par un joueur virtuel. Vous pouvez reprendre la partie en appuyant sur Esc.",
                @"Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //For the slave
        public void InitializeSlaveGameHub(Guid gameGuid)
        {
            this.gameGuid = gameGuid;

            // We need to give a mapping master<->gameId to the server hub so we can handle
            // disconnections properly:
            Task.Run(() => gameHubProxy.Invoke("RegisterPlayer", gameGuid));

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

            gameHubProxy.On("ReceivedGamePauseOrResume", () =>
            {
                Program.QuickPlay.Invoke(new MethodInvoker(() =>
                {
                    Program.QuickPlay.ApplyEsc();
                }));
            });

            gameHubProxy.On("DisconnectedOpponent", () =>
            {
                ManagePlayerDisconnection();
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

            // We need to give a mapping master<->gameId to the server hub so we can handle
            // disconnections properly:
            Task.Run(() => gameHubProxy.Invoke("RegisterPlayer", gameGuid));

            gameHubProxy.On<GameDataMessage>("ReceivedGameData", message =>
            {
                NewPositions?.Invoke(message);
            });

            gameHubProxy.On("ReceivedGamePauseOrResume", () =>
            {
                Program.QuickPlay.Invoke(new MethodInvoker(() =>
                {
                    Program.QuickPlay.ApplyEsc();
                }));
            });

            gameHubProxy.On("DisconnectedOpponent", () =>
            {
                ManagePlayerDisconnection();
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

        public async Task SendGamePauseOrResume()
        {
            await gameHubProxy.Invoke("GamePauseOrResume", gameGuid);
        }

        public async Task Logout()
        {
            //gameHubProxy.Invoke("LeaveRoom", gameGuid);

            //gameHubProxy?.Invoke("Disconnect", User.Instance.UserEntity.Username).Wait();
        }
    }
}
