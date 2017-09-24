using AirHockeyServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;

namespace AirHockeyServer.Controllers
{
    public class ChatController : ApiController
    {

        public ChatController(IChatService chatService)
        {
            ChatService = chatService;
        }

        public IChatService ChatService { get; }

        [Route("api/chat")]
        public HttpResponseMessage Post([FromBody]ChatMessageEntity chatMessage)
        {
            ChatService.SendPrivateMessage(chatMessage);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
    }
}