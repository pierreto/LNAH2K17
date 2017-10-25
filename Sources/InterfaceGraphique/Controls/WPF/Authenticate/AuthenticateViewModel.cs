using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Signup;
using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.ConnectServer;

namespace InterfaceGraphique.Controls.WPF.Authenticate
{
    public class AuthenticateViewModel : ViewModelBase
    {
        static HttpClient client = new HttpClient();

        private LoginEntity loginEntity;
        private HubManager hubManager;
        private ChatHub chatHub;
        public AuthenticateViewModel(LoginEntity loginEntity, ChatHub chatHub)
        {
            Title = "Authentification";
            this.loginEntity = loginEntity;
            this.chatHub = chatHub;
            this.hubManager = HubManager.Instance;
            this.inputsEnabled = true;
        }

        protected override async Task GoBack()
        {
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<ConnectServerViewModel>());
        }

        private ICommand authenticateCommand;
        public ICommand AuthenticateCommand
        {
            get
            {
                if (authenticateCommand == null)
                {
                    authenticateCommand = new RelayCommandAsync(Authenticate);
                }
                return authenticateCommand;
            }
        }

        private async Task<Uri> Authenticate()
        {
            try
            {
                Loading();
                if (!ValidateLoginEntity())
                {
                    return null;
                }
                var response = await client.PostAsJsonAsync("http://localhost:63056/api/login", loginEntity);
                if (response.IsSuccessStatusCode)
                {
                    int userId = response.Content.ReadAsAsync<int>().Result;
                    User.Instance.UserEntity = new UserEntity { Id = userId, Username = loginEntity.Username };
                    await chatHub.InitializeChat();
                    Program.FormManager.CurrentForm = Program.MainMenu;
                }
                else
                {
                    var res = response.Content.ReadAsAsync<string>().Result;
                    //response.EnsureSuccessStatusCode();
                    // return URI of the created resource.
                    UsernameErrMsg = res;
                    PasswordErrMsg = res;
                }
                return response.Headers.Location;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
            finally
            {
                LoadingDone();
            }
        }

        private ICommand signupCommand;
        public ICommand SignupCommand
        {
            get
            {
                if (signupCommand == null)
                {
                    signupCommand = new RelayCommandAsync(Signup);
                }
                return signupCommand;
            }
        }

        private async Task Signup()
        {
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<SignupViewModel>());
        }

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

        private void Loading()
        {
            UsernameErrMsg = "";
            PasswordErrMsg = "";
            InputsEnabled = false;
        }

        private void LoadingDone()
        {
            InputsEnabled = true;
            CommandManager.InvalidateRequerySuggested();
        }

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

        private string usernameErrMsg;
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

        private string passwordErrMsg;
        public string PasswordErrMsg
        {
            get => passwordErrMsg;
            set
            {
                passwordErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        private bool inputsEnabled;
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
    }
}
