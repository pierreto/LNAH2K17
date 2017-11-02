using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat.Channel;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub : IBaseHub
    {

        public event Action<ChatMessage> NewMessage;
        public event Action<ChatMessage,ChannelEntity> NewMessageFromChannel;
        public event Action<string> NewJoinableChannel;
        private IHubProxy chatHubProxy;

        public void InitializeHub(HubConnection connection)
        {
            chatHubProxy = connection.CreateHubProxy("ChatHub");
        }

        public async Task InitializeChat()
        {
            var items = await chatHubProxy.Invoke<ObservableCollection<string>>("Subscribe", User.Instance.UserEntity.Id);
            foreach (var item in items)
            {
                Program.unityContainer.Resolve<JoinChannelListViewModel>().Items.Add(new ChatListItemViewModel(new ChannelEntity { Name = item }));
            }

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
            //Reception de l'evenement de la creation d'un nouveau canal
            chatHubProxy.On<string>("NewJoinableChannel", (channelName) =>
            {
                NewJoinableChannel?.Invoke(channelName);
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
            return null;
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
            var roomNames = Program.unityContainer.Resolve<ChatListViewModel>().Items.Where(x=> x.Name != "Principal").Select(x => x.Name);
            await chatHubProxy.Invoke("Disconnect", roomNames, User.Instance.UserEntity.Id);
        }
    }
}