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
    public class ChatHub : IBaseHub
    {

        public event Action<ChatMessage> NewMessage;
        private string username;
        private IHubProxy chatHubProxy;

        private ChannelEntity mainChannel;
        private ObservableCollection<ChannelEntity> channels;

     
        public void InitializeHub(HubConnection connection, string username)
        {
            this.username = username;
            chatHubProxy = connection.CreateHubProxy("ChatHub");
        }

        public async Task<bool> AuthenticateUser()
        {
            var authentication = chatHubProxy.Invoke<bool>("Authenticate", this.username);
            await authentication;
           return authentication.Result;
        }

        public async Task InitializeChat()
        {
            // Étape necessaire pour que le serveur sache que la connexion est reliée au bon userId:


            var userId = Guid.NewGuid();
            Program.user = new UserEntity { Id = userId, Name = this.username };

            await chatHubProxy.Invoke("Subscribe", userId);

            // Inscription à l'event "ChatMessageReceived". Quand l'event est lancé du serveur on veut print le message:
            chatHubProxy.On<ChatMessage>("ChatMessageReceived", message =>
            {
                NewMessage?.Invoke(message);
            });
        }

        public void Logout()
        {
            chatHubProxy?.Invoke("Disconnect", this.username).Wait();
        }

        public async void SendMessage(ChatMessage message)
        {
            message.Sender = this.username;
            message.TimeStamp=DateTime.Now;
            await chatHubProxy.Invoke("SendBroadcast", message);
        }
    }
}