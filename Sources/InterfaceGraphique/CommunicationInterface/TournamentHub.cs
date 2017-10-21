using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    class TournamentHub : IBaseHub
    {
        private string username;
        private IHubProxy tournamentHubProxy;
        public void InitializeHub(HubConnection connection, string username)
        {
            this.username = username;
            tournamentHubProxy = connection.CreateHubProxy("TournamentHub");
        }
        public async void JoinTournament()
        {
            InitializeEvents();
            Random random = new Random();
            UserEntity user = new UserEntity
            {
                Id = random.Next(),
                Username = username
            };
            await tournamentHubProxy.Invoke("JoinGame", user);
        }
        private void InitializeEvents()
        {
            tournamentHubProxy.On<TournamentOpponentMessage>("OpponentFoundEvent", (opponent) =>
            {

                tournamentHubProxy.On<GameEntity>("GameStartingEvent", officialGame =>
                {
                    
                });
            });
        }

        private void FillTree(TournamentOpponentMessage m)
        {
            switch (m)
            {
                    
            }
        }

        public void Logout()
        {
            tournamentHubProxy?.Invoke("Disconnect", this.username).Wait();
        }
    }
}
