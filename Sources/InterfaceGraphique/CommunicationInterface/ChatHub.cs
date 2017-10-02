using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Threading;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub : IChatHub
    {
        public delegate void UpdateChatBoxDelegate(ChatMessage message);

        public event Action<ChatMessage> NewMessage;
        private string targetServerIp;

        private string username;
        //private UpdateChatBoxDelegate updateChatBoxDelegate;
        private IHubProxy chatHubProxy;

        public IHubProxy GameWaitingRoomProxy { get; private set; }

        private HubConnection connection;

        private ChannelEntity mainChannel;
        private ObservableCollection<ChannelEntity> channels;

        // TODO: retourner un résultat pour savoir si la connexion a échouée ou pas.
        public async Task EstablishConnection(string targetServerIp)
        {
            this.targetServerIp = targetServerIp;

            this.connection = new HubConnection("http://"+targetServerIp+":63056/signalr");
            chatHubProxy = this.connection.CreateHubProxy("ChatHub");
            GameWaitingRoomProxy = this.connection.CreateHubProxy("GameWaitingRoomHub");
            await this.connection.Start();
        }

        public async void test()
        {
            GameEntity game = new GameEntity
            {
                Creator = new UserEntity
                {
                    Id = Guid.NewGuid()
                }
            };

            Console.WriteLine("Game created");
            await GameWaitingRoomProxy.Invoke("CreateGame", game);
            
            GameWaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame =>
            {
                Console.WriteLine("Opponent found");
                GameWaitingRoomProxy.On<GameEntity>("GameStartingEvent", officialGame =>
                {
                    Console.WriteLine("Game is starting!");
                });

                GameWaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime =>
                {
                    Console.WriteLine(remainingTime);
                });

                GameWaitingRoomProxy.On<GameEntity>("GameConfigurationUpdatedEvent", gameUpdated2 =>
                {
                    Console.WriteLine("configuration updated");
                });

                GameWaitingRoomProxy.On<GameEntity>("GameMapUpdatedEvent", mapUpdated =>
                {
                    Console.WriteLine("map updated");
                });
            });
            
            Thread.Sleep(5000);

            UserEntity user = new UserEntity
            {
                Id = Guid.NewGuid()
            };

            await GameWaitingRoomProxy.Invoke("JoinGame", user);

            Thread.Sleep(5000);

            var gameUpdated = await GameWaitingRoomProxy.Invoke<GameEntity>("UpdateConfiguration", game);

            Thread.Sleep(2000);

            gameUpdated = await GameWaitingRoomProxy.Invoke<GameEntity>("UpdateMap", game);
        }

        public async Task<bool> AuthenticateUser(string username)
        {
            this.username = username;
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
                NewMessage?.Invoke(message);
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
            message.Sender = this.username;
            message.TimeStamp=DateTime.Now;
            await chatHubProxy.Invoke("SendBroadcast", message);
        }
    }
}