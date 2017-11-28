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
using Microsoft.Practices.Unity;
using InterfaceGraphique.Services;
using InterfaceGraphique.Controls.WPF.MainMenu;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using InterfaceGraphique.Menus;

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
            FriendsHub.GameRequestCanceledEvent += OnCanceledGameRequest;
        }

        private void OnCanceledGameRequest(GameRequestEntity gameRequest)
        {
            PendingRequest = null;
            Program.FormManager.CancelGameRequest();
        }

        private void OnGameRequest(GameRequestEntity request)
        {
            PendingRequest = request;
            if (Program.FormManager.CurrentForm.GetType() == typeof(QuickPlay))
            {
                // Program.QuickPlay.ProcessCmdKey(keyData);
                DeclineGameRequest();

            }
            else if (Program.FormManager.CurrentForm.GetType() == typeof(TestMode))
            {
                // Program.TestMode.ProcessCmdKey(keyData);
                DeclineGameRequest();

            }
            else if (Program.FormManager.CurrentForm.GetType() == typeof(Editeur))
            {
                DeclineGameRequest();

            }
            else if (Program.FormManager.CurrentForm.GetType() == typeof(TournementTree))
            {
                DeclineGameRequest();
            }
            else if (Program.FormManager.CurrentForm.GetType() == typeof(TournementMenu))
            {
                DeclineGameRequest();
            }
            else if (Program.FormManager.CurrentForm.GetType() == typeof(OnlineTournementMenu))
            {
                DeclineGameRequest();
            }
            else
            {
                Program.unityContainer.Resolve<MatchmakingViewModel>().SetDefaultValues();
                Program.unityContainer.Resolve<MatchmakingViewModel>().Initialize(true);

                Program.FormManager.ShowGameRequestPopup(request.Sender.Username);
            }

        }

        private void OnGameRequestDeclined(GameRequestEntity request)
        {
            PendingRequest = null;
            Program.FormManager.Invoke(new MethodInvoker(() =>
            {
                Program.FormManager.CurrentForm = Program.HomeMenu;
                Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
            }));

            System.Windows.Forms.MessageBox.Show(
                @"Votre ami a refusé votre demande de partie",
                @"Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnGameRequestAccepted(GameRequestEntity request)
        {
            PendingRequest = null;
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

            bool isAvailable = await FriendsHub.FriendIsAvailable(recipientId);
            if (!isAvailable)
            {
                System.Windows.Forms.MessageBox.Show(
                @"Votre ami est en cours de partie (ou tournoi). Veuillez ressayer plus tard",
                @"Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await FriendsHub.SendGameRequest(gameRequest);

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

        public async Task CancelGameRequest()
        {
            if(PendingRequest != null)
            {
                await FriendsHub.CancelGameRequest(PendingRequest);
                PendingRequest = null;
            }
        }

    }
}
