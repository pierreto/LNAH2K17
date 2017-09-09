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

namespace AirHockeyServer.Controllers
{
    public class ChatController : ApiController
    {

        public ChatController(IChatRepository chatRepository)
        {
            ChatRepository = chatRepository;
        }

        public IChatRepository ChatRepository { get; }

        [Route("api/chat")]
        public HttpResponseMessage Post([FromBody]ChatMessage chatMessage)
        {
            ChatRepository.SendMessage(chatMessage);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
    }
}