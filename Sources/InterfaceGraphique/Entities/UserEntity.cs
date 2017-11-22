using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Controls.WPF;
using InterfaceGraphique.Controls.WPF.Friends;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Date { get; set; }

        public string Profile { get; set; }

        public bool IsSelected { get; set; }

        public bool AlreadyPlayedGame { get; set; }

        public bool AlreadyUsedFatEditor { get; set; }

        public bool AlreadyUsedLightEditor { get; set; }

        private ICommand sendFriendRequestCommand;
        public ICommand SendFriendRequestCommand
        {
            get
            {
                return sendFriendRequestCommand ??
                       (sendFriendRequestCommand = new RelayCommandAsync(SendFriendRequest));
            }
        }
        private async Task SendFriendRequest()
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/user/u/" + Username);
            UserEntity friend = await HttpResponseParser.ParseResponse<UserEntity>(response);
            await Program.unityContainer.Resolve<FriendsHub>().SendFriendRequest(friend);
            var item = Program.unityContainer.Resolve<AddFriendListViewModel>().Items;
            //Retire de notre liste de personnes ajoutables la personne qu'on vien d'envoyer une demande d'amis
            item.Remove(item.Single(x => x.Id == friend.Id));
        }
    }
}
