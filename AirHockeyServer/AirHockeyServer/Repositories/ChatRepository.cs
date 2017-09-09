using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;

namespace AirHockeyServer.Repositories
{
    public class ChatRepository : IChatRepository, IRepository
    {
        public ChatRepository(IChatService chatService, IChannelRepository channelRepository)
        {
            ChatService = chatService;
            ChannelRepository = channelRepository;
        }

        public IChatService ChatService { get; }
        public IChannelRepository ChannelRepository { get; }

        public void SendMessage(ChatMessage chatMessage)
        {

            // Todo : Check in DB if there is a channel using ChannelRepository
            //if(chatMessage.Recipient == IsChannel)
            //{
            //    ChatService.SendMessageToChannel(chatMessage, new Channel());
            //}
            ChatService.SendPrivateMessage(chatMessage);
        }

        
    }
}