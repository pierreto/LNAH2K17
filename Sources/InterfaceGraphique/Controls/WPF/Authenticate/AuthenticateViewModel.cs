using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Signup;
using InterfaceGraphique.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.ConnectServer;
using InterfaceGraphique.Controls.WPF.Friends;
using InterfaceGraphique.Exceptions;
using InterfaceGraphique.Services;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.MainMenu;

namespace InterfaceGraphique.Controls.WPF.Authenticate
{
    public class AuthenticateViewModel : ViewModelBase
    {
        //TODO: Mettre ailleurs?
        #region Private Properties
        private LoginEntity loginEntity;
        private HubManager hubManager;
        private ChatHub chatHub;
        private string usernameErrMsg;
        private string passwordErrMsg;
        private bool inputsEnabled;
        private bool notLoading = true;

        #endregion

        #region Public Properties
        public string Username
        {
            get => loginEntity.Username;
            set
            {
                if (loginEntity.Username != value && value != "")
                {
                    UsernameErrMsg = "";
                    loginEntity.Username = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    loginEntity.Username = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string UsernameErrMsg
        {
            get => usernameErrMsg;
            set
            {
                usernameErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        public string Password
        {
            get => loginEntity.Password;
            set
            {
                if (loginEntity.Password != value && value != "")
                {
                    PasswordErrMsg = "";
                    loginEntity.Password = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    loginEntity.Password = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string PasswordErrMsg
        {
            get => passwordErrMsg;
            set
            {
                passwordErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        public bool InputsEnabled
        {
            get { return inputsEnabled; }

            set
            {
                if (inputsEnabled == value)
                {
                    return;
                }
                inputsEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public bool NotLoading
        {
            get { return notLoading; }

            set
            {
                if (notLoading == value)
                {
                    return;
                }
                notLoading = value;
                this.OnPropertyChanged(nameof(NotLoading));
                this.OnPropertyChanged(nameof(Loading));
            }
        }

        public bool Loading
        {
            get { return !notLoading; }
        }
        #endregion

        #region Constructor
        public AuthenticateViewModel(LoginEntity loginEntity, ChatHub chatHub, StoreService storeService, FriendsHub friendsHub)
        {
            Title = "Authentification";
            BackText = "Se Connecter";
            this.loginEntity = loginEntity;
            this.chatHub = chatHub;
            StoreService = storeService;
            FriendsHub = friendsHub;
            this.hubManager = HubManager.Instance;
            this.inputsEnabled = true;
        }
        #endregion

        #region Commands
        private ICommand authenticateCommand;
        public ICommand AuthenticateCommand
        {
            get
            {
                if (authenticateCommand == null)
                {
                    authenticateCommand = new RelayCommandAsync(Authenticate, (o) => true);
                }
                return authenticateCommand;
            }
        }

        private ICommand signupCommand;
        public ICommand SignupCommand
        {
            get
            {
                if (signupCommand == null)
                {
                    signupCommand = new RelayCommand(Signup);
                }
                return signupCommand;
            }
        }

        public StoreService StoreService { get; set; }
        public FriendsHub FriendsHub { get; }
        #endregion

        #region Command Methods
        private async Task<Uri> Authenticate()
        {
            try
            {
                Load();
                if (ValidateLoginEntity())
                {
                    var response = await Program.client.PostAsJsonAsync(Program.client.BaseAddress + "api/login", loginEntity);
                    if (response.IsSuccessStatusCode)
                    {
                        int userId = response.Content.ReadAsAsync<int>().Result;
                    
                        //On set l'instance statique du user.
                        HttpResponseMessage uEResponse = await Program.client.GetAsync(Program.client.BaseAddress + "api/user/" + userId);
                        User.Instance.UserEntity = await HttpResponseParser.ParseResponse<UserEntity>(uEResponse);
                        User.Instance.IsConnected = true;
                        Program.unityContainer.Resolve<MainMenuViewModel>().OnlineMode = true;

                        User.Instance.Inventory = await StoreService.GetUserStoreItems(User.Instance.UserEntity.Id);

                        await chatHub.InitializeChat();

                        //On reset le nom d'usager et le mot de passe (Au cas ou il fait un retour a l'arriere ou deconnexion)
                        Username = Password = "";

                        //On initie tous les formes qui on besoin de savoir si on est en mode en ligne 
                        Program.InitAfterConnection();

                        Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());

                        //Should show loading spinner
                        Program.unityContainer.Resolve<MainMenuViewModel>().NotLoading = false;
                        await FriendsHub.InitializeFriendsHub();
                        //await FriendsHub.AcceptGameRequest(new GameRequestEntity() { Recipient = new UserEntity() { Id = 0 }, Sender = new UserEntity() { Id = 0 } });
                        await Program.unityContainer.Resolve<FriendListViewModel>().Init();
                        await Program.unityContainer.Resolve<AddUserViewModel>().Init();
                        await Program.unityContainer.Resolve<FriendRequestListViewModel>().Init();
                        await Program.unityContainer.Resolve<AddFriendListViewModel>().InitAddFriends();
                        Program.unityContainer.Resolve<ChatViewModel>().Init();
                        Program.unityContainer.Resolve<FriendListViewModel>().Minimize();
                        //Hide loading spinner
                        Program.unityContainer.Resolve<MainMenuViewModel>().NotLoading = true;

                    }
                    else
                    {
                        //Du cote serveur, on retourne un message d'erreur
                        LoadingDone();
                        var res = response.Content.ReadAsAsync<string>().Result;

                        //On met une erreur seulement sur le password pour indiquer que soit le mdp, soit nom d'usager invalide
                        PasswordErrMsg = res;
                    }
                    return response.Headers.Location;
                }
                else
                {
                    return null;
                }
            }
            catch(LoginException e)
            {
                System.Diagnostics.Debug.WriteLine("[AuthenticateViewModel.Authenticate] " + e.ToString());
                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[AuthenticateViewModel.Authenticate] " + e.ToString());
                return null;
            }
            finally
            {
                LoadingDone();
            }
        }

        private void Signup()
        {
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<SignupViewModel>());
        }
        #endregion

        #region Private Methods
        private bool ValidateLoginEntity()
        {
            bool valid = true;
            if (loginEntity.Username == null || loginEntity.Username == "")
            {
                UsernameErrMsg = "Nom d'usager requis";
                valid = false;
            }
            if (loginEntity.Password == null || loginEntity.Password == "")
            {
                PasswordErrMsg = "Mot de passe requis";
                valid = false;
            }
            return valid;
        }

        private void Load()
        {
            UsernameErrMsg = "";
            PasswordErrMsg = "";
            InputsEnabled = false;
            NotLoading = false;
        }

        private void LoadingDone()
        {
            InputsEnabled = true;
            NotLoading = true;
            CommandManager.InvalidateRequerySuggested();
        }
        #endregion

        #region Overwritten Methods
        protected override void GoBack()
        {
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<ConnectServerViewModel>());
        }

        public override void InitializeViewModel()
        {
            //throw new NotImplementedException();
        }
        #endregion 
    }
}
