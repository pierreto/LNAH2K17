﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Entities.Message;

namespace InterfaceGraphique.CommunicationInterface
{
    public class GameHub : IBaseHub
    {
        public event Action<GameSlaveData> NewSlavePositions;
        public event Action<GameMasterData> NewMasterPositions;

        public event Action<GoalMessage> NewGoal;
        public event Action NewGameOver;

        private int gameGuid;

        private IHubProxy gameHubProxy;
        public void InitializeHub(HubConnection connection)
        {
            //   gameHubProxy = connection.CreateHubProxy("GameWaitingRoomHub");
            gameHubProxy = GameWaitingRoomHub.WaitingRoomProxy;
        }

        //For the slave
        public  void InitializeSlaveGameHub(int gameGuid)
        {
            this.gameGuid = gameGuid;

            // Étape necessaire pour que le serveur sache que la connexion est reliée au bon userId:
           // await gameHubProxy.Invoke("JoinRoom", gameGuid);

            gameHubProxy.On<GameMasterData>("ReceivedMasterData", message =>
            {
                NewMasterPositions?.Invoke(message);
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

        //public void SendSlavePosition(GameDataMessage gameDataMessage)
        //{
        //    gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
        //}


        //For the master
        public void InitializeMasterGameHub(int gameId)
        {
            this.gameGuid = gameId;
            // Étape necessaire pour que le serveur sache que la connexion est reliée au bon userId:
            //await gameHubProxy.Invoke("JoinRoom", gameGuid);

            gameHubProxy.On<GameSlaveData>("ReceivedSlaveData", message =>
            {
                NewSlavePositions?.Invoke(message);
            });
        }

        public void SendGameData(GameMasterData gameDataMessage)
        {
            gameHubProxy.Invoke("SendGameData", gameGuid, gameDataMessage);
        }

        public void SendGameData(GameSlaveData gameDataMessage)
        {
            gameHubProxy.Invoke("SendSlaveGameData", gameGuid, gameDataMessage);
        }

        public void SendGameOver()
        {
            gameHubProxy.Invoke("GameOver", gameGuid);
        }

        public void SendGoal(int player)
        {
            GoalMessage goalMessage = new GoalMessage(player);
            gameHubProxy.Invoke("SendGoal", gameGuid, goalMessage);

        }


        public void Logout()
        {
            gameHubProxy.Invoke("LeaveRoom", gameGuid);

            gameHubProxy?.Invoke("Disconnect", User.Instance.UserEntity.Username).Wait();
        }
    }
}
