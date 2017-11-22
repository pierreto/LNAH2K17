using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using System.Collections.Generic;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub : IBaseHub
    {

        public event Action<ChatMessage> NewMessage;
        public event Action<ChatMessage, String> NewMessageFromChannel;
        public event Action<ChatMessage, int> NewPrivateMessage;
        public event Action<string> NewJoinableChannel;
        public event Action<string> NewPrivateChannel;
        public event Action<string> ChannelDeleted;
        private IHubProxy chatHubProxy;

        public void InitializeHub(HubConnection connection)
        {
            chatHubProxy = connection.CreateHubProxy("ChatHub");
        }

        public async Task InitializeChat()
        {
            var items = await chatHubProxy.Invoke<List<string>>("Subscribe", User.Instance.UserEntity.Id);
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
            chatHubProxy.On<ChatMessage, String>("ChatMessageReceivedChannel", (message, channelName) =>
            {
                NewMessageFromChannel?.Invoke(message, channelName);
            });
            chatHubProxy.On<ChatMessage, int>("ChatMessageReceivedPrivate", (message, senderId) =>
            {
                NewPrivateMessage?.Invoke(message, senderId);
            });
            chatHubProxy.On<string>("PrivateChannelCreated", (privateName) =>
            {
                NewPrivateChannel?.Invoke(privateName);
            });
            //Reception de l'evenement de la creation d'un nouveau canal
            chatHubProxy.On<string>("NewJoinableChannel", (channelName) =>
            {
                NewJoinableChannel?.Invoke(channelName);
            });
            //Reception de l'evenement de la supression canal
            chatHubProxy.On<string>("ChannelDeleted", (channelName) =>
            {
                ChannelDeleted?.Invoke(channelName);
            });
        }

        public async void SendMessage(ChatMessage message)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.TimeStamp = DateTime.Now;
            await chatHubProxy.Invoke("SendBroadcast", message);
        }

        public async Task<string> CreateChannel(string channelName)
        {
            bool res = await chatHubProxy.Invoke<Boolean>("CreateChannel", channelName);
            if(res) {
                return null;
            } else
            {
                return "Canal déjà crée";
            }
        }

        public async Task<bool> CreatePrivateChannel(string myName, int othersId)
        {
            bool res = await chatHubProxy.Invoke<Boolean>("CreatePrivateChannel", myName, othersId);
            return res;
        }

        public async Task JoinChannel(string channelName)
        {
            await chatHubProxy.Invoke<String>("JoinChannel", channelName);
        }

        public async Task SendPrivateMessage(ChatMessage message, int senderId, int receptorId)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.TimeStamp = DateTime.Now;
            await chatHubProxy.Invoke("SendPrivateMessage", message, senderId, receptorId);
        }

        public async void SendChannel(ChatMessage message, string channelName)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.TimeStamp = DateTime.Now;
            await chatHubProxy.Invoke("SendChannel", channelName, message);
        }

        public async void LeaveRoom(String roomName)
        {
            await chatHubProxy.Invoke("LeaveRoom", roomName, User.Instance.UserEntity.Id);
        }

        public async Task Logout()
        {
            var roomNames = Program.unityContainer.Resolve<ChatListViewModel>().Items.Where(x=> x.Name != "Principal").Select(x => x.Name);
            await chatHubProxy.Invoke("Disconnect", roomNames, User.Instance.UserEntity.Id);
        }
    }
}