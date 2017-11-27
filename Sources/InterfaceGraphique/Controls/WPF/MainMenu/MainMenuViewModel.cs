using InterfaceGraphique.CommunicationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Tutorial;
using System.Windows.Forms;
using InterfaceGraphique.Controls.WPF.UserProfile;
using InterfaceGraphique.Controls.WPF.Store;
using InterfaceGraphique.Controls.WPF.Home;
using InterfaceGraphique.Controls.WPF.Chat;

namespace InterfaceGraphique.Controls.WPF.MainMenu
{
    public class MainMenuViewModel : ViewModelBase
    {

        #region Private Properties
        private bool notLoading;
        private bool onlineMode;
        #endregion

        #region Public Properties
        public bool NotLoading
        {
            get
            {
                return notLoading;
            }
            set
            {
                notLoading = value;
                OnPropertyChanged(nameof(NotLoading));
                OnPropertyChanged(nameof(Loading));
            }
        }

        public bool Loading
        {
            get
            {
                return !notLoading;
            }
        }

        public bool OnlineMode
        {
            get
            {
                return onlineMode;
            }
            set
            {
                onlineMode = value;
                OnPropertyChanged(nameof(OnlineMode));
            }
        }
        #endregion

        #region Constructor
        public MainMenuViewModel()
        {
            Row = 0;
            RowSpan = 5;
        }
        #endregion

        #region Commands
        //Section Grise
        private ICommand partieRapideCommand;
        public ICommand PartieRapideCommand
        {
            get
            {
                if (partieRapideCommand == null)
                {
                    partieRapideCommand = new RelayCommandAsync(PartieRapide);
                }
                return partieRapideCommand;
            }
        }

        private ICommand partieRapideOnlineCommand;
        public ICommand PartieRapideOnlineCommand
        {
            get
            {
                if(partieRapideOnlineCommand == null)
                {
                    partieRapideOnlineCommand = new RelayCommandAsync(PartieRapideOnline);
                }
                return partieRapideOnlineCommand;
            }
        }
        
        private ICommand tournoiCommand;
        public ICommand TournoiCommand
        {
            get
            {
                if (tournoiCommand == null)
                {
                    tournoiCommand = new RelayCommand(Tournoi);
                }
                return tournoiCommand;
            }
        }

        private ICommand tournoiEnLigneCommand;
        public ICommand TournoiEnLigneCommand
        {
            get
            {
                if (tournoiEnLigneCommand == null)
                {
                    tournoiEnLigneCommand = new RelayCommand(TournoiEnLigne);
                }
                return tournoiEnLigneCommand;
            }
        }

        private ICommand configurationCommand;
        public ICommand ConfigurationCommand
        {
            get
            {
                if (configurationCommand == null)
                {
                    configurationCommand = new RelayCommand(Configuration);
                }
                return configurationCommand;
            }
        }

        //Section Bleue
        private ICommand monProfilCommand;
        public ICommand MonProfilCommand
        {
            get
            {
                if (monProfilCommand == null)
                {
                    monProfilCommand = new RelayCommandAsync(MonProfil);
                }
                return monProfilCommand;
            }
        }

        private ICommand magasinCommand;
        public ICommand MagasinCommand
        {
            get
            {
                if (magasinCommand == null)
                {
                    magasinCommand = new RelayCommandAsync(Magasin);
                }
                return magasinCommand;
            }
        }

        //Section Jaune
        private ICommand tutorielEditionCommand;
        public ICommand TutorielEditionCommand
        {
            get
            {
                if (tutorielEditionCommand == null)
                {
                    tutorielEditionCommand = new RelayCommandAsync(TutorielEdition);
                }
                return tutorielEditionCommand;
            }
        }

        private ICommand tutorielPartieCommand;
        public ICommand TutorielPartieCommand
        {
            get
            {
                if (tutorielPartieCommand == null)
                {
                    tutorielPartieCommand = new RelayCommandAsync(TutorielPartie);
                }
                return tutorielPartieCommand;
            }
        }

        //Section Verte
        private ICommand editionCommand;
        public ICommand EditionCommand
        {
            get
            {
                if (editionCommand == null)
                {
                    editionCommand = new RelayCommandAsync(Edition);
                }
                return editionCommand;
            }
        }

        //Section Rouge
        private ICommand deconnexionCommand;
        public ICommand DeconnexionCommand
        {
            get
            {
                if (deconnexionCommand == null)
                {
                    deconnexionCommand = new RelayCommandAsync(Deconnexion);
                }
                return deconnexionCommand;
            }
        }

        private ICommand quitterCommand;
        public ICommand QuitterCommand
        {
            get
            {
                if (quitterCommand == null)
                {
                    quitterCommand = new RelayCommandAsync(Quitter);
                }
                return quitterCommand;
            }
        }

        private ICommand homeCommand;
        public ICommand HomeCommand
        {
            get
            {
                if (homeCommand == null)
                {
                    homeCommand = new RelayCommand(Home);
                }
                return homeCommand;
            }
        }


        #endregion

        #region Command Methods
        //Section Grise
        public async Task PartieRapide()
        {
            await CheckIfNeedToShowMatchTutoriel();
            Program.QuickPlayMenu.ShowDialog();
            CommandManager.InvalidateRequerySuggested();
        }

        public async Task PartieRapideOnline()
        {
            await CheckIfNeedToShowMatchTutoriel();
            Program.FormManager.CurrentForm = Program.LobbyHost;

            var vm = Program.unityContainer.Resolve<Matchmaking.MatchmakingViewModel>();
            vm.Initialize();
            vm.SetOnlineGame();
        }

        public void Tournoi()
        {
            Program.FormManager.CurrentForm = Program.TournementMenu;
            CommandManager.InvalidateRequerySuggested();
        }

        public void TournoiEnLigne()
        {
            Program.FormManager.CurrentForm = Program.OnlineTournamentMenu;
            CommandManager.InvalidateRequerySuggested();
        }

        public void Configuration()
        {
            Program.ConfigurationMenu.ShowDialog();
            CommandManager.InvalidateRequerySuggested();
        }

        //Section Bleue
        public async Task MonProfil()
        {
            Program.FormManager.CurrentForm = Program.UserProfileMenu;
            await Program.unityContainer.Resolve<UserProfileViewModel>().Initialize();
            CommandManager.InvalidateRequerySuggested();
        }

        public async Task Magasin()
        {
            Program.FormManager.CurrentForm = Program.StoreMenu;
            await Program.unityContainer.Resolve<StoreViewModel>().Initialize();
            CommandManager.InvalidateRequerySuggested();
        }

        //Section Jaune
        public async Task TutorielEdition()
        {
            await ShowTutorialEditor();
            CommandManager.InvalidateRequerySuggested();
        }

        public async Task TutorielPartie()
        {
            await ShowTutorialGame();
            CommandManager.InvalidateRequerySuggested();
        }

        //Section Verte
        public async Task Edition()
        {
            Program.Editeur.ResetDefaultTable();
            Program.FormManager.CurrentForm = Program.Editeur;
            await CheckIfNeedToShowEditorTutoriel();
            CommandManager.InvalidateRequerySuggested();
        }

        //Section Rouge
        public async Task Deconnexion()
        {
            Program.unityContainer.Resolve<ChatViewModel>().UndockedChat?.Close();
            await CrashKillExitAllApplication();
            User.Instance.UserEntity = null;
            User.Instance.IsConnected = false;
            Program.unityContainer.Resolve<MainMenuViewModel>().OnlineMode = false;
            //TODO: KILL HUB CONNECTIONS
            Program.FormManager.CurrentForm = Program.HomeMenu;
            Program.InitializeUnityDependencyInjection();
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<HomeViewModel>());
            CommandManager.InvalidateRequerySuggested();
        }

        public async Task Quitter()
        {
            if(User.Instance.IsConnected)
            {
                Program.unityContainer.Resolve<ChatViewModel>().UndockedChat?.Close();
            }
            System.Windows.Forms.Application.Exit();
        }

        public async Task CrashKillExitAllApplication()
        {
            var response = await Program.client.PostAsJsonAsync(Program.client.BaseAddress + "api/logout", User.Instance.UserEntity);
            await HubManager.Instance.LeaveHubs();
            await HubManager.Instance.Logout();
        }

        public void Home()
        {
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<HomeViewModel>());
        }
        #endregion

        #region Private Methods
        private async Task CheckIfNeedToShowEditorTutoriel()
        {
            if (!User.Instance.IsConnected)
            {
                await ShowTutorialEditor();
            }
            else if (!User.Instance.UserEntity.AlreadyUsedFatEditor)
            {
                await ShowTutorialEditor();
                User.Instance.UserEntity.AlreadyUsedFatEditor = true;
                await Program.client.PutAsJsonAsync(Program.client.BaseAddress + "api/user/" + User.Instance.UserEntity.Id.ToString(), User.Instance.UserEntity);
            }
        }
        private async Task CheckIfNeedToShowMatchTutoriel()
        {
            if (!User.Instance.IsConnected)
            {
                await ShowTutorialGame();
            }
            else if (!User.Instance.UserEntity.AlreadyPlayedGame)
            {
                await ShowTutorialGame();
                User.Instance.UserEntity.AlreadyPlayedGame = true;
                await Program.client.PutAsJsonAsync(Program.client.BaseAddress + "api/user/" + User.Instance.UserEntity.Id.ToString(), User.Instance.UserEntity);
            }
        }

        public static async Task ShowTutorialEditor()
        {
            await Program.unityContainer.Resolve<TutorialViewModel>().SwitchToEditorSlides();
            Form fc = Application.OpenForms["TutorialHost"];
            if (fc == null)
            {
                Program.TutorialHost.ShowDialog();
            }
        }
        public static async Task ShowTutorialGame()
        {
            await Program.unityContainer.Resolve<TutorialViewModel>().SwitchToMatchSlides();

            Form fc = Application.OpenForms["TutorialHost"];
            if (fc == null)
            {
                Program.TutorialHost.ShowDialog();
            }
        }
        #endregion


        #region Overwritten Methods
        public override void InitializeViewModel()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
