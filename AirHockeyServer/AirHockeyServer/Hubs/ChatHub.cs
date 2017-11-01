using AirHockeyServer.Entities;
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
        private readonly static Dictionary<int, string> ConnectionsMapping = new Dictionary<int, string>();
        // Should be thread-safe?
        private static HashSet<string> channelNames = new HashSet<string>();

        private static ObservableCollection<ChannelEntity> channels = new ObservableCollection<ChannelEntity>();

        public IChannelService ChannelService { get; }

        public ChatHub(IChannelService channelService)
        {
            ChannelService = channelService;
        }

        public void Subscribe(int userId)
        {
            if(!ConnectionsMapping.ContainsKey(userId))
            {
                ConnectionsMapping.Add(userId, Context.ConnectionId);
            }
        }

        public void SendBroadcast(ChatMessageEntity chatMessage)
        {
            Clients.All.ChatMessageReceived(chatMessage);
        }

        public async Task SendChannel(string channelName, ChatMessageEntity message)
        {
            ChannelEntity cE = channels.Where(s => s.Name == channelName).First();
            await Clients.Group(channelName).ChatMessageReceivedChannel(message, cE);
        }

        public void SendPrivateMessage(int userId, ChatMessageEntity message)
        {
            if(ConnectionsMapping.ContainsKey(userId))
            {
                Clients.Client(ConnectionsMapping[userId]).ChatMessageReceived(message);
            }
        }

        public async Task<ChannelEntity> CreateChannel(ChannelEntity channel)
        {
            if (channelNames.Contains(channel.Name))
            {
                return null;
            }
            else
            {
                //TODO: Peut-etre pas necessaire d'avoir les nom et les canaux... map? 
                channelNames.Add(channel.Name);
                ChannelEntity channelCreated = await this.ChannelService.CreateChannel(channel);
                channels.Add(channelCreated);
                await Groups.Add(Context.ConnectionId, channel.Name);
                return channel;
            }
        }

        public async Task<ChannelEntity> JoinChannel(string channelName)
        {
            ChannelEntity channelJoined = channels.Where(s => s.Name == channelName).First();
            await Groups.Add(Context.ConnectionId, channelName);
            return channelJoined;
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
    }
}