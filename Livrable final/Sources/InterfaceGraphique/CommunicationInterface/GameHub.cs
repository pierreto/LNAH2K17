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
    public class GameHub : BaseHub, IBaseHub
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
            try
            {
                Task.Run(() => gameHubProxy.Invoke("RegisterPlayer", gameGuid));
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task SendSlavePosition(float[] slavePosition)
        {
            GameDataMessage gameDataMessage = new GameDataMessage(slavePosition);
            try
            {
                await gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task SendGameData(float[] slavePosition, float[] masterPosition, float[] puckPosition)
        {
            GameDataMessage gameDataMessage = new GameDataMessage(slavePosition, masterPosition, puckPosition);
            try
            {
                await gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task SendGameOver()
        {
            try
            {
                await gameHubProxy.Invoke("GameOver", gameGuid);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task SendGoal(int player)
        {
            try
            {
                GoalMessage goalMessage = new GoalMessage(player);
                await gameHubProxy.Invoke("SendGoal", gameGuid, goalMessage);
            }
            catch (Exception e)
            {
                HandleError();
            }

        }

        public async Task SendGamePauseOrResume()
        {
            try
            {
                await gameHubProxy.Invoke("GamePauseOrResume", gameGuid);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task Logout()
        {
        }

        public async Task LeaveRoom()
        {

        }
    }
}
