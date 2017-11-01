using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Authenticate;
using InterfaceGraphique.Entities;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Exceptions;

namespace InterfaceGraphique.Controls.WPF.Signup
{
    public class SignupViewModel : ViewModelBase
    {
        //TODO: Mettre ailleurs?
        static HttpClient client = new HttpClient();

        #region Private Properties
        private SignupEntity signupEntity;
        private HubManager hubManager;
        private ChatHub chatHub;
        private string usernameErrMsg;
        private string passwordErrMsg;
        private string confirmPasswordErrMsg;
        private bool inputsEnabled;
        #endregion

        #region Public Properties
        public string Username
        {
            get => signupEntity.Username;
            set
            {
                if (signupEntity.Username != value && value != "")
                {
                    UsernameErrMsg = "";
                    signupEntity.Username = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.Username = value;
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
            get => signupEntity.Password;
            set
            {
                if (signupEntity.Password != value && value != "")
                {
                    PasswordErrMsg = "";
                    signupEntity.Password = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.Password = value;
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

        public string ConfirmPassword
        {
            get => signupEntity.ConfirmPassword;
            set
            {
                if (signupEntity.ConfirmPassword != value && value != "")
                {
                    ConfirmPasswordErrMsg = "";
                    signupEntity.ConfirmPassword = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.ConfirmPassword = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string ConfirmPasswordErrMsg
        {
            get => confirmPasswordErrMsg;
            set
            {
                confirmPasswordErrMsg = value;
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
        #endregion

        #region Constructor
        public SignupViewModel(SignupEntity signupEntity, ChatHub chatHub)
        {
            Title = "Créer un compte";
            this.chatHub = chatHub;
            this.signupEntity = signupEntity;
            this.hubManager = HubManager.Instance;
            this.inputsEnabled = true;
        }
        #endregion

        #region Commands
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
        #endregion

        #region Command Methods
        private async Task<Uri> Signup()
        {
            try
            {
                Loading();
                ResetErrMsg();
                ValidateFields();
                var response = await client.PostAsJsonAsync(Program.client.BaseAddress + "api/signup", signupEntity);
                if (response.IsSuccessStatusCode)
                {
                    int userId = response.Content.ReadAsAsync<int>().Result;
                    User.Instance.UserEntity = new UserEntity { Id = userId, Username = signupEntity.Username };
                    User.Instance.IsConnected = true;
                    await chatHub.InitializeChat();
                    Program.InitAfterConnection();
                    Username = Password = ConfirmPassword = "";
                    Program.FormManager.CurrentForm = Program.MainMenu;
                }
                else
                {
                    var res = response.Content.ReadAsAsync<string>().Result;
                    //response.EnsureSuccessStatusCode();
                    // return URI of the created resource.
                    UsernameErrMsg = res;
                }
                return response.Headers.Location;
            }
            catch(SignupException e)
            {
                System.Diagnostics.Debug.WriteLine("[SignupViewModel.Signup] " + e.ToString());
                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[SignupViewModel.Signup] " + e.ToString());
                return null;
            }
            finally
            {
                LoadingDone();
            }
        }
        #endregion

        #region Private Methods
        private void ValidateFields()
        {
            if (Username == "")
            {
                UsernameErrMsg = "Nom d'usager requis";
                throw new SignupException(UsernameErrMsg);
            }
            else if (Username.Length < 2)
            {
                UsernameErrMsg = "Minimum 2 charactères requis";
                throw new SignupException(UsernameErrMsg);
            }
            else
            {
                Regex rgx = new Regex(@"^[a-zA-Z0-9_.-]*$");
                if (!rgx.IsMatch(Username))
                {
                    UsernameErrMsg = "Seul les charactères alphanumériqus et les tirets sont acceptés";
                    throw new SignupException(UsernameErrMsg);
                }
            }
            if (Password == "")
            {
                PasswordErrMsg = "Mot de passe requis";
                throw new SignupException(PasswordErrMsg);
            }
            else if (Password.Length < 8)
            {
                PasswordErrMsg = "Minimum 8 charactères requis";
                throw new SignupException(PasswordErrMsg);
            }
            else
            {
                Regex rgx = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]");
                if (!rgx.IsMatch(Password))
                {
                    PasswordErrMsg = "Minimum une lettre minuscule, une lettre majuscule et un chiffre";
                    throw new SignupException(PasswordErrMsg);
                }
            }
            if (ConfirmPassword == "")
            {
                ConfirmPasswordErrMsg = "Confirmation mot de passe requise";
                throw new SignupException(ConfirmPasswordErrMsg);
            }
            if (Password != ConfirmPassword)
            {
                ConfirmPasswordErrMsg = "Mots de passes pas identiques";
                throw new SignupException(ConfirmPasswordErrMsg);
            }
        }

        private void Loading()
        {
            InputsEnabled = false;
        }

        private void LoadingDone()
        {
            InputsEnabled = true;
            CommandManager.InvalidateRequerySuggested();
        }

        private void ResetErrMsg()
        {
            UsernameErrMsg = "";
            PasswordErrMsg = "";
        }

        #endregion

        #region Overwritten Methods
        protected override void GoBack()
        {
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<AuthenticateViewModel>());
        }

        public override void InitializeViewModel()
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
