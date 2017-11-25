using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Authenticate;
using InterfaceGraphique.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Home;

namespace InterfaceGraphique.Controls.WPF.ConnectServer
{
    public class ConnectServerViewModel : ViewModelBase
    {
        #region Private Properties
        private readonly string LOCALHOST = "localhost";
        private readonly int TIMEOUT = 5000;
        private HubManager hubManager;
        private string ipAddress;
        private string ipAddressErrMsg;
        private bool ipAddressInputEnabled;
        #endregion

        #region Public Properties
        public string IpAddress
        {
            get => ipAddress;
            set
            {
                if (IpAddress != value && value != "")
                {
                    IpAddressErrMsg = "";
                    ipAddress = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    ipAddress = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string IpAddressErrMsg
        {
            get => ipAddressErrMsg;
            set
            {
                ipAddressErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        public bool IpAddressInputEnabled
        {
            get { return ipAddressInputEnabled; }

            set
            {
                if (ipAddressInputEnabled == value)
                {
                    return;
                }
                ipAddressInputEnabled = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public ConnectServerViewModel()
        {
            Title = "Connexion";
            BackText = "LNAH 2K17";
            this.hubManager = HubManager.Instance;
            this.IpAddressInputEnabled = true;
        }
        #endregion

        #region Commands
        private ICommand connectServerCommand;
        public ICommand ConnectServerCommand
        {
            get
            {
                if (connectServerCommand == null)
                {
                    connectServerCommand = new RelayCommandAsync(ConnectServer);
                }
                return connectServerCommand;
            }
        }
        #endregion

        #region Command Methods
        private async Task ConnectServer()
        {
            try
            {
                Loading();
                ValidateIpAddress();
                int timeout = TIMEOUT;
                var task = hubManager.EstablishConnection(IpAddress);
                await task;
                // Permet de voir si le task est realise dans le bon delai
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    //Task resolved within delay
                    Program.client.BaseAddress = new System.Uri("http://" + IpAddress + ":63056/");
                    IpAddress = "";
                    Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<AuthenticateViewModel>());
                }
                else
                {
                    // timeout logic
                    throw new Exception("Too long to connect to IP address: " + IpAddress + ". Make sure it is a valid IP address.");
                }
            }
            catch (ConnectServerException e)
            {
                System.Diagnostics.Debug.WriteLine("[ConnectServerViewModel.ConnectToServer] " + e.ToString());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[ConnectServerViewModel.ConnectToServer] " + e.ToString());
                IpAddressErrMsg = "Adresse IP non rejoignable";
            }
            finally
            {
                LoadingDone();
            }
        }
        #endregion

        #region Private Methods
        private void ValidateIpAddress()
        {
            if (!LOCALHOST.Equals(IpAddress) && !ValidateIP())
            {
                throw new ConnectServerException("Invalid Ip");
            }
        }

        private bool ValidateIP()
        {
            if (String.IsNullOrWhiteSpace(IpAddress))
            {
                IpAddressErrMsg = "Adresse IP requise";
                return false;
            }

            string[] splitValues = IpAddress.Split('.');
            if (splitValues.Length != 4)
            {
                IpAddressErrMsg = "Adresse IP invalide";
                return false;
            }

            if (splitValues.All(r => byte.TryParse(r, out byte tempForParsing)))
            {
                IpAddressErrMsg = "";
                return true;
            }
            else
            {
                IpAddressErrMsg = "Adresse IP invalide";
                return false;
            }
        }

        private void Loading()
        {
            IpAddressInputEnabled = false;
        }

        private void LoadingDone()
        {
            IpAddressInputEnabled = true;
            CommandManager.InvalidateRequerySuggested();
        }
        #endregion

        #region Overwritten Methods
        protected override void GoBack()
        {
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<HomeViewModel>());
        }
        public override void InitializeViewModel()
        {
            // throw new NotImplementedException();
        }
        #endregion

    }
}
