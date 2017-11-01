using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub : IBaseHub
    {

        public event Action<ChatMessage> NewMessage;
        public event Action<ChatMessage,ChannelEntity> NewMessageFromChannel;
        private IHubProxy chatHubProxy;

        public void InitializeHub(HubConnection connection)
        {
            chatHubProxy = connection.CreateHubProxy("ChatHub");
        }

        public async Task InitializeChat()
        {
            await chatHubProxy.Invoke("Subscribe", User.Instance.UserEntity.Id);

            // Inscription à l'event "ChatMessageReceived". Quand l'event est lancé du serveur on veut print le message:
            //Message envoye pour le canal principal
            chatHubProxy.On<ChatMessage>("ChatMessageReceived", message =>
            {
                NewMessage?.Invoke(message);
            });
            //On distingue un message recu d'un canal
            chatHubProxy.On<ChatMessage, ChannelEntity>("ChatMessageReceivedChannel", (message, cE) =>
            {
                NewMessageFromChannel?.Invoke(message, cE);
            });
        }

        public async void SendMessage(ChatMessage message)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.TimeStamp = DateTime.Now;
            await chatHubProxy.Invoke("SendBroadcast", message);
        }

        public async Task<string> CreateChannel(ChannelEntity channelEntity)
        {
            ChannelEntity cE = await chatHubProxy.Invoke<ChannelEntity>("CreateChannel", channelEntity);
            if(cE == null) { return "Canal déjà crée"; }
            return "";
        }

        public async Task<ChannelEntity> JoinChannel(string channelName)
        {
            var cE = chatHubProxy.Invoke<ChannelEntity>("JoinChannel", channelName);
            await cE;
            return cE.Result;
        }

        public async void SendChannel(ChatMessage message, string channelName)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.TimeStamp = DateTime.Now;
            await chatHubProxy.Invoke("SendChannel", channelName, message);
        }

        public async void LeaveRoom(String roomName)
        {
            await chatHubProxy.Invoke("LeaveRoom", roomName);
        }

        public async Task Logout()
        {
            //
        }

    }
}