﻿using System;
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
        public event Action GameOver;

        private Guid gameGuid;

        private IHubProxy gameHubProxy;
        
        public void InitializeHub(HubConnection connection)
        {
            gameHubProxy = GameWaitingRoomHub.WaitingRoomProxy;
            InitializeEvents();
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

        public void InitializeEvents()
        {
            gameHubProxy.On<GameDataMessage>("ReceivedGameData", message => OnGameData(message));

            gameHubProxy.On<GoalMessage>("ReceivedGoal", message => OnGoalMessage(message));

            gameHubProxy.On("ReceivedGameOver", () => OnGameOver());

            gameHubProxy.On("ReceivedGamePauseOrResume", () => OnGamePauseOrResume());

            gameHubProxy.On("DisconnectedOpponent", () => OnDisconnectedOpponent());
        }

        public void OnGameData(GameDataMessage gameData)
        {
            NewPositions?.Invoke(gameData);
        }

        public void OnGoalMessage(GoalMessage goal)
        {
            NewGoal?.Invoke(goal);
        }

        public void OnGameOver()
        {
            GameOver?.Invoke();
        }

        public void OnGamePauseOrResume()
        {
            Program.QuickPlay.Invoke(new MethodInvoker(() =>
            {
                Program.QuickPlay.ApplyEsc();
            }));
        }

        public void OnDisconnectedOpponent()
        {
            ManagePlayerDisconnection();
        }

        public void InitialiseGame(Guid gameGuid)
        {
            this.gameGuid = gameGuid;

            // We need to give a mapping master<->gameId to the server hub so we can handle
            // disconnections properly:
            Task.Run(() => gameHubProxy.Invoke("RegisterPlayer", gameGuid));
        }

        public async Task SendSlavePosition(float[] slavePosition)
        {
            GameDataMessage gameDataMessage = new GameDataMessage(slavePosition);

            await gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
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
