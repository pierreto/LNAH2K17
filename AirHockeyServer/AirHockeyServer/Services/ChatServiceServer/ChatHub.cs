using AirHockeyServer.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services.ChatServiceServer
{
    public class ChatHub : Hub
    {
        public IChannelService ChannelService { get; }

        public ChatHub(IChannelService channelService)
        {
            ChannelService = channelService;
        }

        public void Subscribe(string userName)
        {
            // TODO
        }

        public void SendBroadcast(string name, string message)
        {
            Clients.All.ChatMessageReceived(message);
        }

        public async Task SendChannel(string channelName, string message)
        {
            await Clients.Group(channelName).ChatMessageReceived(message);
        }

        public async void SendPrivateMessage(string message)
        {
            // TODO
        }

        public async Task<Channel> CreateChannel(Channel channel)
        {
            Channel channelCreated = await this.ChannelService.CreateChannel(channel);
            await Groups.Add(Context.ConnectionId, channel.Name);
            return channel;
        }

        public async Task JoinChannel(string channelName)
        {
            //Channel channelCreated = await this.ChannelService.JoinChannel(channelName);
            await Groups.Add(Context.ConnectionId, channelName);
        }
    }
}