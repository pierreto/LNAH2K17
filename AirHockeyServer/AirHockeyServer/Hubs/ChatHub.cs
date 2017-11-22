using AirHockeyServer.Entities;
using AirHockeyServer.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AirHockeyServer.Services.ChatServiceServer
{
    public class ChatHub : Hub
    {
        // Should be thread-safe?
        private static Dictionary<int, string> ConnectionsMapping = new Dictionary<int, string>();
        // Should be thread-safe?
        private static Dictionary<string, int> roomPpl = new Dictionary<string, int>(); 

        private static List<String> channels = new List<String>();

        public IChannelService ChannelService { get; }

        public ChatHub(IChannelService channelService)
        {
            ChannelService = channelService;
        }

        public List<string> Subscribe(int userId)
        {
            if(!ConnectionsMapping.ContainsKey(userId))
            {
                ConnectionsMapping.Add(userId, Context.ConnectionId);
                //Fetch all the channels 
                return channels;
            }
            return new List<string>();
        }

        public void SendBroadcast(ChatMessageEntity chatMessage)
        {
            Clients.All.ChatMessageReceived(chatMessage);
        }

        public async Task SendChannel(string channelName, ChatMessageEntity message)
        {
            await Clients.Group(channelName).ChatMessageReceivedChannel(message, channelName);
        }

        public void SendPrivateMessage(ChatMessageEntity message, int senderId, int receptorId)
        {
            //Envoi le message au destinataire
            if(ConnectionsMapping.ContainsKey(receptorId) && ConnectionsMapping.ContainsKey(senderId))
            {
                Clients.Client(ConnectionsMapping[receptorId]).ChatMessageReceivedPrivate(message, senderId);
                //Envoi le message a l'emetteur
                Clients.Client(ConnectionsMapping[senderId]).ChatMessageReceivedPrivate(message, receptorId);
            }
        }

        public async Task<Boolean> CreateChannel(string channelName)
        {
            if (roomPpl.ContainsKey(channelName) && roomPpl[channelName] > 0)
            {
                return false;
            }
            else
            {   
                roomPpl[channelName] = 1;
                //ChannelEntity channelCreated = await this.ChannelService.CreateChannel(channel);
                channels.Add(channelName);
                await Groups.Add(Context.ConnectionId, channelName);
                //BroadCastChannelToAll
                Clients.Others.NewJoinableChannel(channelName);
                return true;
            }
        }

        public async Task<bool> CreatePrivateChannel(string name, int myId, int othersId)
        {
            if (ConnectionsMapping.ContainsKey(othersId))
            {
                await Clients.Client(ConnectionsMapping[othersId]).PrivateChannelCreated(name, myId);
                return true;
            } 
            else
            {
                return false;
            }
        }

        public async Task JoinChannel(string channelName)
        {
            await Groups.Add(Context.ConnectionId, channelName);
            roomPpl[channelName]++;
        }

        public Task LeaveRoom(string roomName, int userId)
        {
            if (--roomPpl[roomName] == 0)
            {
                Clients.Others.ChannelDeleted(roomName);
            } else
            {
                Clients.Client(ConnectionsMapping[userId]).NewJoinableChannel(roomName);
            }

            return Groups.Remove(Context.ConnectionId, roomName);
        }

        public void Disconnect(ObservableCollection<string> roomNames, int userId)
        {
            foreach(var roomName in roomNames)
            {
                LeaveRoom(roomName, userId);
            }
            ConnectionsMapping.Remove(userId);
        }
    }
}