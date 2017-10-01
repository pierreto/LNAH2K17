using System;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface
{
    public interface IChatHub
    {
        event Action<ChatMessage> NewMessage;

        Task<bool> AuthenticateUser(string username);
        Task EstablishConnection(string targetServerIp);
        Task InitializeChat();
        void Logout(string username);
        void SendMessage(ChatMessage message);
    }
}