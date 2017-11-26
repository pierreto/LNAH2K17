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
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Controls.WPF.Friends;
using InterfaceGraphique.Controls.WPF.MainMenu;
using InterfaceGraphique.Services;
using InterfaceGraphique.Controls.WPF.Chat;

namespace InterfaceGraphique.Controls.WPF.Signup
{
    public class SignupViewModel : ViewModelBase
    {
        #region Private Properties
        private SignupEntity signupEntity;
        private HubManager hubManager;
        private ChatHub chatHub;
        private string usernameErrMsg;
        private string nameErrMsg;
        private string emailErrMsg;
        private string passwordErrMsg;
        private string confirmPasswordErrMsg;
        private bool inputsEnabled;
        private bool notLoading = true;
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

        public string Name
        {
            get => signupEntity.Name;
            set
            {
                if (signupEntity.Name != value && value != "")
                {
                    NameErrMsg = "";
                    signupEntity.Name = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.Name = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string NameErrMsg
        {
            get => nameErrMsg;
            set
            {
                nameErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        public string Email
        {
            get => signupEntity.Email;
            set
            {
                if (signupEntity.Name != value && value != "")
                {
                    EmailErrMsg = "";
                    signupEntity.Email = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    signupEntity.Email = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string EmailErrMsg
        {
            get => emailErrMsg;
            set
            {
                emailErrMsg = value;
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
        public SignupViewModel(SignupEntity signupEntity, ChatHub chatHub)
        {
            Title = "Créer un compte";
            BackText = "S'authentifier";
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
                Load();
                ResetErrMsg();
                if (ValidateFields())
                {
                    var response = await Program.client.PostAsJsonAsync(Program.client.BaseAddress + "api/signup", signupEntity);
                    if (response.IsSuccessStatusCode)
                    {
                        int userId = response.Content.ReadAsAsync<int>().Result;

                        //On set l'instance statique du user.
                        HttpResponseMessage uEResponse = await Program.client.GetAsync(Program.client.BaseAddress + "api/user/" + userId);
                        User.Instance.UserEntity = await HttpResponseParser.ParseResponse<UserEntity>(uEResponse);
                        User.Instance.IsConnected = true;
                        Program.unityContainer.Resolve<MainMenuViewModel>().OnlineMode = true;

                        await chatHub.InitializeChat();

                        //On reset le nom d'usager et le mot de passe (Au cas ou il fait un retour a l'arriere ou deconnexion)
                        Username = Password = "";

                        //On initie tous les formes qui on besoin de savoir si on est en mode en ligne 
                        Program.InitAfterConnection();

                        Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());

                        //Should show loading spinner
                        Program.unityContainer.Resolve<MainMenuViewModel>().NotLoading = false;
                        await Program.unityContainer.Resolve<FriendsHub>().InitializeFriendsHub();
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
                        var res = response.Content.ReadAsAsync<string>().Result;
                        UsernameErrMsg = res;
                    }
                    return response.Headers.Location;
                }
                else
                {
                    return null;
                }
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
        private bool ValidateUsername()
        {
            bool valid = true;
            if (Username == "")
            {
                UsernameErrMsg = "Nom d'usager requis";
                valid = false;
            }
            else if (Username.Length < 2)
            {
                UsernameErrMsg = "Minimum 2 charactères requis";
                valid = false;
            }
            else
            {
                Regex rgx = new Regex(@"^[a-zA-Z0-9_.-]*$");
                if (!rgx.IsMatch(Username))
                {
                    UsernameErrMsg = "Seul les charactères alphanumériqus et les tirets sont acceptés";
                    valid = false;
                }
            }
            return valid;
        }

        private bool ValidateName()
        {
            bool valid = true;
            if (Name == "")
            {
                NameErrMsg = "Nom requis";
                valid = false;
            }
            else if (Name.Length < 2)
            {
                NameErrMsg = "Minimum 2 charactères requis";
                valid = false;
            }
            else
            {
                Regex rgx = new Regex(@"^[a-zA-Z0-9_.-]*$");
                if (!rgx.IsMatch(Name))
                {
                    NameErrMsg = "Seul les charactères alphanumériqus et les tirets sont acceptés";
                    valid = false;
                }
            }
            return valid;
        }

        private bool ValidateEmail()
        {
            bool valid = true;
            if (Email == "")
            {
                EmailErrMsg = "Courriel requis";
                valid = false;
            }
            else if (Email.Length > 64)
            {
                EmailErrMsg = "Maximum 64 charactères permis";
                valid = false;
            }
            else
            {
                Regex rgx = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                if (!rgx.IsMatch(Email))
                {
                    EmailErrMsg = "Format de courriel invalide";
                    valid = false;
                }
            }
            return valid;
        }

        private bool ValidatePassword()
        {
            bool valid = true;
            if (Password == "")
            {
                PasswordErrMsg = "Mot de passe requis";
                valid = false;
            }
            else if (Password.Length < 8)
            {
                PasswordErrMsg = "Minimum 8 charactères requis";
                valid = false;
            }
            else
            {
                Regex rgx = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]");
                if (!rgx.IsMatch(Password))
                {
                    PasswordErrMsg = "Minimum une lettre minuscule, une lettre majuscule et un chiffre";
                    valid = false;
                }
            }
            return valid;
        }
        
        private bool ValidateConfirmPassword()
        {
            bool valid = true;
            if (ConfirmPassword == "")
            {
                ConfirmPasswordErrMsg = "Confirmation mot de passe requise";
                valid = false;
            }
            if (Password != ConfirmPassword)
            {
                ConfirmPasswordErrMsg = "Mots de passes pas identiques";
                valid = false;
            }
            return valid;
        }
        private bool ValidateFields()
        {
            bool valid = true;
            if (!ValidateUsername())
            {
                valid = false;
            }
            if (!ValidateName())
            {
                valid = false;
            }
            if (!ValidateEmail())
            {
                valid = false;
            }
            if (!ValidatePassword())
            {
                valid = false;
            }
            if (!ValidateConfirmPassword())
            {
                valid = false;
            }
            return valid;
        }

        private void Load()
        {
            InputsEnabled = false;
            NotLoading = false;
        }

        private void LoadingDone()
        {
            NotLoading = true;
            InputsEnabled = true;
            CommandManager.InvalidateRequerySuggested();
        }

        private void ResetErrMsg()
        {
            UsernameErrMsg = "";
            NameErrMsg = "";
            EmailErrMsg = "";
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
