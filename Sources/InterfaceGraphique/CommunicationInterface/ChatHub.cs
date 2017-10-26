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
        private IHubProxy chatHubProxy;

        private ChannelEntity mainChannel;
        private ObservableCollection<ChannelEntity> channels;


        public void InitializeHub(HubConnection connection)
        {
            chatHubProxy = connection.CreateHubProxy("ChatHub");
        }

        public async Task<bool> AuthenticateUser()
        {
            var authentication = chatHubProxy.Invoke<bool>("Authenticate", User.Instance.UserEntity.Username);
            await authentication;
            return authentication.Result;
        }

        public async Task InitializeChat()
        {
            await chatHubProxy.Invoke("Subscribe", User.Instance.UserEntity.Id);

            // Inscription à l'event "ChatMessageReceived". Quand l'event est lancé du serveur on veut print le message:
            chatHubProxy.On<ChatMessage>("ChatMessageReceived", message =>
            {
                NewMessage?.Invoke(message);
            });
        }

        public async Task Logout()
        {
           await chatHubProxy?.Invoke("Disconnect", User.Instance.UserEntity.Username);
        }

        public async void SendMessage(ChatMessage message)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.TimeStamp = DateTime.Now;
            await chatHubProxy.Invoke("SendBroadcast", message);
        }
    }
}