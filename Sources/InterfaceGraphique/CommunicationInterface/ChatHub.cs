﻿using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub
    {
        public delegate void UpdateChatBoxDelegate(ChatMessage message);

        private string targetServerIp;
        private UpdateChatBoxDelegate updateChatBoxDelegate;
        private IHubProxy chatHubProxy;

        public IHubProxy GameWaitingRoomProxy { get; private set; }

        private HubConnection connection;

        public ChatHub(UpdateChatBoxDelegate chatBoxDelegate)
        {
            this.updateChatBoxDelegate = chatBoxDelegate;
        }

        // TODO: retourner un résultat pour savoir si la connexion a échouée ou pas.
        public async Task EstablishConnection(string targetServerIp)
        {
            this.targetServerIp = targetServerIp;

            this.connection = new HubConnection("http://"+targetServerIp+":63056/signalr");
            chatHubProxy = this.connection.CreateHubProxy("ChatHub");
            GameWaitingRoomProxy = this.connection.CreateHubProxy("GameWaitingRoomHub");
            await this.connection.Start();
        }

        public void test()
        {
            GameEntity game = new GameEntity
            {
                Creator = new UserEntity
                {
                    Id = Guid.NewGuid()
                }
            };

            GameWaitingRoomProxy.Invoke("CreateGame", game);
            
            GameWaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame =>
            {
                Console.WriteLine("Opponent found");
            });
            
            Thread.Sleep(5000);

            UserEntity user = new UserEntity
            {
                Id = Guid.NewGuid()
            };

            GameWaitingRoomProxy.Invoke("JoinGame", user);
        }

        public async Task<bool> AuthenticateUser(string username)
        {
            var authentication = chatHubProxy.Invoke<bool>("Authenticate", username);
            await authentication;
            return authentication.Result;
        }

        public async Task InitializeChat()
        {
            // Étape necessaire pour que le serveur sache que la connexion est reliée au bon userId:
            var userId = Guid.NewGuid();
            await chatHubProxy.Invoke("Subscribe", userId);

            // Inscription à l'event "ChatMessageReceived". Quand l'event est lancé du serveur on veut print le message:
            chatHubProxy.On<ChatMessage>("ChatMessageReceived", message =>
            {
                Console.WriteLine("ChatMessageReceived : " + message.MessageValue);
                updateChatBoxDelegate(message);
            });
        }

        public void Logout(string username)
        {
            if(chatHubProxy != null)
            {
                chatHubProxy.Invoke("Disconnect", username).Wait();
                this.connection.Stop();
            }
        }

        public async void SendMessage(ChatMessage message)
        {
            await chatHubProxy.Invoke("SendBroadcast", message);
        }
    }
}