using InterfaceGraphique.CommunicationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using System.Windows;
using System.Windows.Forms;
using InterfaceGraphique.Exceptions;
using InterfaceGraphique.Services;

namespace InterfaceGraphique.Managers
{
    public class GameRequestManager
    {
        protected GameRequestEntity PendingRequest { get; set; }

        public GameRequestManager(FriendsHub friendsHub, UserService userService)
        {
            FriendsHub = friendsHub;
            UserService = userService;
            InitializeEvents();
        }

        public FriendsHub FriendsHub { get; }
        public UserService UserService { get; }

        private void InitializeEvents()
        {
            FriendsHub.AcceptedGameRequestEvent += OnGameRequestAccepted;
            FriendsHub.DeclinedGameRequestEvent += OnGameRequestDeclined;
            FriendsHub.GameRequestEvent += OnGameRequest;
        }

        private void OnGameRequest(GameRequestEntity request)
        {
            PendingRequest = request;
            Program.FormManager.ShowGameRequestPopup();
        }

        private void OnGameRequestDeclined(GameRequestEntity request)
        {
            PendingRequest = null;
            Program.FormManager.Invoke(new MethodInvoker(() =>
            {
                Program.FormManager.CurrentForm = Program.MainMenu;
            }));

            System.Windows.Forms.MessageBox.Show(
                @"Votre ami a refusé votre demande de partie",
                @"Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnGameRequestAccepted(GameRequestEntity request)
        {
            //
        }
        
        public async Task SendGameRequest(int recipientId)
        {
            var users = await UserService.GetAllUsers();
            GameRequestEntity gameRequest = new GameRequestEntity()
            {
                Recipient = users.Find(x => x.Id == recipientId),
                Sender = users.Find(x => x.Id == User.Instance.UserEntity.Id),
            };

            bool isAvailable = await FriendsHub.SendGameRequest(gameRequest);

            PendingRequest = gameRequest;
            Program.QuickPlayMenu.LoadOnlineGameSettings();
        }

        public async Task AcceptGameRequest()
        {
            PendingRequest.IsAccept = true;
            await FriendsHub.AcceptGameRequest(PendingRequest);

            PendingRequest = null;
            
            Program.QuickPlayMenu.LoadOnlineGameSettings();
        }

        public async Task DeclineGameRequest()
        {
            PendingRequest.IsAccept = false;
            await FriendsHub.DeclineGameRequest(PendingRequest);

            PendingRequest = null;
        }
    }
}
