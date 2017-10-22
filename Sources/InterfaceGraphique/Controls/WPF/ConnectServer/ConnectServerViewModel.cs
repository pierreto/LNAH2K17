using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.ConnectServer
{
    public class ConnectServerViewModel: ViewModelBase
    {
        private readonly string LOCALHOST = "localhost";

        private HubManager hubManager;
        public ConnectServerViewModel()
        {
            this.hubManager = HubManager.Instance;
            this.IpAddressInputEnabled = true;
        }

        private ICommand enterKeyCommand;
        public ICommand EnterKeyCommand
        {
            get
            {
                if (enterKeyCommand == null)
                {
                    enterKeyCommand = new RelayCommandAsync(ConnectToServer);
                }
                return enterKeyCommand;
            }
        }

        private async Task ConnectToServer()
        {
            try
            {
                Loading();
                ValidateIpAddress();
                int timeout = 5000;
                var task = hubManager.EstablishConnection(IpAddress);
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    // task completed within timeout
                    Program.FormManager.CurrentForm = Program.AuthenticateMenu;
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
                IpAddressErrMsg = "Adresse IP non rejoinable";
            }
            finally
            {
                LoadingDone();
            }
        }

        private void ValidateIpAddress()
        {
            if (!LOCALHOST.Equals(IpAddress) && !ValidateIP())
            {
                throw new ConnectServerException("Invalid Ip");
            }
        }

        public bool ValidateIP()
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

        private string ipAddress;
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

        private string ipAddressErrMsg;
        public string IpAddressErrMsg
        {
            get => ipAddressErrMsg;
            set
            {
                ipAddressErrMsg = value;
                this.OnPropertyChanged();
            }
        }

        private bool ipAddressInputEnabled;
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
    }
}
