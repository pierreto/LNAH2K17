using AirHockeyServer.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirHockeyServer.Services.ChatServiceServer
{
    public class ChatHub : Hub
    {
        // Should be thread-safe?
        private readonly static Dictionary<int, string> ConnectionsMapping = new Dictionary<int, string>();
        // Should be thread-safe?
        private static HashSet<string> usernames = new HashSet<string>();

        public IChannelService ChannelService { get; }

        public ChatHub()
        {
            ChannelService = new ChannelService();
        }

        public bool Authenticate(string username)
        {
            if (usernames.Contains(username))
            {
                return false;
            }
            else
            {
                usernames.Add(username);
                return true;
            }
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
            await Clients.Group(channelName).ChatMessageReceived(message);
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
            ChannelEntity channelCreated = await this.ChannelService.CreateChannel(channel);
            await Groups.Add(Context.ConnectionId, channel.Name);
            return channel;
        }

        public async Task JoinChannel(string channelName)
        {
            //Channel channelCreated = await this.ChannelService.JoinChannel(channelName);
            await Groups.Add(Context.ConnectionId, channelName);
        }

        public void Disconnect(string username)
        {
            if (usernames.Contains(username))
            {
                usernames.Remove(username);
            }
        }
        
    }
}