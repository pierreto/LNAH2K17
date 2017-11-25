using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using System.Collections.Generic;
using System.Text;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub : BaseHub, IBaseHub
    {

        public event Action<ChatMessage> NewMessage;
        public event Action<ChatMessage, String> NewMessageFromChannel;
        public event Action<ChatMessage, int> NewPrivateMessage;
        public event Action<string> NewJoinableChannel;
        public event Action<string, int, string> NewPrivateChannel;
        public event Action<string> ChannelDeleted;
        private IHubProxy chatHubProxy;

        public void InitializeHub(HubConnection connection)
        {
            chatHubProxy = connection.CreateHubProxy("ChatHub");
        }

        public async Task InitializeChat()
        {
            List<string> items = new List<string>();
            try
            {
                items = await chatHubProxy.Invoke<List<string>>("Subscribe", User.Instance.UserEntity.Id);
            }
            catch(Exception e)
            {

            }

            foreach (var item in items)
            {
                Program.unityContainer.Resolve<JoinChannelListViewModel>().Items.Add(new ChatListItemViewModel(new ChannelEntity { Name = item, IsJoinable = true }));
            }

            // Inscription à l'event "ChatMessageReceived". Quand l'event est lancé du serveur on veut print le message:
            //Message envoye pour le canal principal
            chatHubProxy.On<ChatMessage>("ChatMessageReceived", message =>
            {
                message.MessageValue = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageValue));
                NewMessage?.Invoke(message);
            });
            //On distingue un message recu d'un canal
            chatHubProxy.On<ChatMessage, String>("ChatMessageReceivedChannel", (message, channelName) =>
            {
                message.MessageValue = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageValue));
                NewMessageFromChannel?.Invoke(message, channelName);
            });
            chatHubProxy.On<ChatMessage, int>("ChatMessageReceivedPrivate", (message, senderId) =>
            {
                message.MessageValue = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageValue));
                NewPrivateMessage?.Invoke(message, senderId);
            });
            chatHubProxy.On<string, int, string>("PrivateChannelCreated", (privateName, othersId, othersProfile) =>
            {
                NewPrivateChannel?.Invoke(privateName, othersId, othersProfile);
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
            message.MessageValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(message.MessageValue));
            message.TimeStamp = DateTime.Now;
            try
            {
                await chatHubProxy.Invoke("SendBroadcast", message);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> SendMessage");
            }
        }

        public async Task<string> CreateChannel(string channelName)
        {
            bool res = false;
            try
            {
                res = await chatHubProxy.Invoke<Boolean>("CreateChannel", channelName);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> CreateChannel");
            }

            if (res)
            {
                return null;
            }
            else
            {
                return "Canal déjà crée";
            }
        }

        public async Task<bool> CreatePrivateChannel(string othersName, int myId, int othersId, string othersProfile)
        {
            try
            {
                return await chatHubProxy.Invoke<Boolean>("CreatePrivateChannel", othersName, User.Instance.UserEntity.Id, othersId, othersProfile);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> CreatePrivateChannel");
            }

            return false;
        }

        public async Task JoinChannel(string channelName)
        {
            try
            {
                await chatHubProxy.Invoke<String>("JoinChannel", channelName);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> JoinChannel");
            }
        }

        public async void SendPrivateMessage(ChatMessage message, int senderId, int receptorId)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.MessageValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(message.MessageValue));
            message.TimeStamp = DateTime.Now;

            try
            {
                await chatHubProxy.Invoke("SendPrivateMessage", message, senderId, receptorId);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> SendPrivateMessage");
            }
        }

        public async void SendChannel(ChatMessage message, string channelName)
        {
            message.Sender = User.Instance.UserEntity.Username;
            message.MessageValue = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageValue));
            message.TimeStamp = DateTime.Now;
            try
            {
                await chatHubProxy.Invoke("SendChannel", channelName, message);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> SendChannel");
            }
        }

        public async void LeaveRoom(String roomName)
        {
            try
            {
                await chatHubProxy.Invoke("LeaveRoom", roomName, User.Instance.UserEntity.Id);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> LeaveRoom");
            }
        }

        public async Task Logout()
        {
            var roomNames = Program.unityContainer.Resolve<ChatListViewModel>().Items.Where(x => x.Name != "Principal" && !x.ChannelEntity.IsPrivate).Select(x => x.Name);
            try
            {
                await chatHubProxy.Invoke("Disconnect", roomNames, User.Instance.UserEntity.Id);
            }
            catch (Exception e)
            {
                HandleError("ChatHub -> Logout");
            }
        }

        public async Task LeaveRoom()
        {
            // do nothing
            return;
        }
    }
}