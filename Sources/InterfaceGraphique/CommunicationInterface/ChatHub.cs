using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub
    {
        public ChatHub()
        {
            
        }

        public async void EstablishConnection()
        {
            var hubConnection = new HubConnection("http://localhost:63056/signalr");
            IHubProxy chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
            hubConnection.Start().Wait();

            await chatHubProxy.Invoke("Subscribe", "ariane");

            chatHubProxy.On<string>("ChatMessageReceived", message =>
                Console.WriteLine("ChatMessageReceived : " + message));

            await chatHubProxy.Invoke("SendBroadcast", "", "BroadcastMessage");

            ChannelEntity channel = new ChannelEntity()
            {
                Name = "MySuperChannel"
            };

            var channelCreated = await chatHubProxy.Invoke<ChannelEntity>("CreateChannel", channel);
            Console.WriteLine("Channel Created : " + channelCreated.Name);

            await chatHubProxy.Invoke("SendChannel", "MySuperChannel", "MySuperChannelMessage");


        }
    }
}
