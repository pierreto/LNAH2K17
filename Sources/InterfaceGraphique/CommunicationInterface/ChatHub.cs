using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InterfaceGraphique.CommunicationInterface
{
    public class ChatHub
    {
        public delegate void UpdateChatBoxDelegate(ChatMessage message);

        private IPAddress targetServerIp;
        private UpdateChatBoxDelegate updateChatBoxDelegate;
        private IHubProxy chatHubProxy;

        public ChatHub(IPAddress targetServerIp, UpdateChatBoxDelegate updateChatBoxDelegate)
        {
            this.targetServerIp = targetServerIp;
            this.updateChatBoxDelegate = updateChatBoxDelegate;
        }

        public async void EstablishConnection()
        {
            // TODO : À TRANSFORMER EN INTERFACE POUR ACCEDER AUX FONCTIONS 

            // Connection au serveur
            var hubConnection = new HubConnection("http://localhost:63056/signalr");
            // Ouverture d'une connexion au Hub "ChatHub" (classe du côté serveur)
            chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
            hubConnection.Start().Wait();

            // etape necessaire pour que le serveur sache la connection est reliee au bon userId
            var userId = Guid.NewGuid();
            await chatHubProxy.Invoke("Subscribe", userId);

            // Inscription à l'event "ChatMessageReceived". Quand l'event est lancée du serveur on veut print le message
            chatHubProxy.On<string>("ChatMessageReceived", message =>
            {
                Console.WriteLine("ChatMessageReceived : " + message);
                ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage>(message);
                updateChatBoxDelegate(chatMessage);
            });
/*
            // Appel d'une methode "SendBroadcast" sur le serveur
            await chatHubProxy.Invoke("SendBroadcast", "", "BroadcastMessage");

            // Création d'un channel
            ChannelEntity channel = new ChannelEntity()
            {
                Name = "MySuperChannel"
            };

            var channelCreated = await chatHubProxy.Invoke<ChannelEntity>("CreateChannel", channel);
            Console.WriteLine("Channel Created : " + channelCreated.Name);

            // envoyer un message à un channel
            await chatHubProxy.Invoke("SendChannel", "MySuperChannel", "MySuperChannelMessage");

            // envoyer un private message 
            await chatHubProxy.Invoke("SendPrivateMessage", userId, "hello dear friend");   */
        }

        public async void SendMessage(ChatMessage message)
        {
            await chatHubProxy.Invoke("SendBroadcast", "", JsonConvert.SerializeObject(message));

        }


    }
}
