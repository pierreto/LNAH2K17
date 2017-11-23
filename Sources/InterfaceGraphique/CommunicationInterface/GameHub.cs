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
        public event Action<PlayerEndOfGameStatsEntity> EndOfGameStatsEvent;
        public event Action GameOver;
        public event Action DisconnectedEvent;

        private Guid gameGuid;

        private IHubProxy gameHubProxy;
        
        public void InitializeHub(HubConnection connection)
        {
            gameHubProxy = GameWaitingRoomHub.WaitingRoomProxy;
            InitializeEvents();
        }

        private void ManagePlayerDisconnection()
        {
            DisconnectedEvent?.Invoke();
        }

        public void InitializeEvents()
        {
            gameHubProxy.On<GameDataMessage>("ReceivedGameData", message => OnGameData(message));

            gameHubProxy.On<GoalMessage>("ReceivedGoal", message => OnGoalMessage(message));

            gameHubProxy.On("ReceivedGameOver", () => OnGameOver());

            gameHubProxy.On("ReceivedGamePauseOrResume", () => OnGamePauseOrResume());

            gameHubProxy.On<PlayerEndOfGameStatsEntity>("EndOfGameInfo", infos => OnEndOfGameInfo(infos));

            gameHubProxy.On("DisconnectedOpponent", () => OnDisconnectedOpponent());
        }

        public void OnEndOfGameInfo(PlayerEndOfGameStatsEntity infos)
        {
            EndOfGameStatsEvent?.Invoke(infos);
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
            await gameHubProxy.Invoke("LeaveRoom", gameGuid);

            //gameHubProxy?.Invoke("Disconnect", User.Instance.UserEntity.Username).Wait();
        }

        public async Task LeaveRoom()
        {
            // do nothing
            await Logout();
        }
    }
}
