using InterfaceGraphique.Controls.WPF.ConnectServer;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.Home
{
    public class HomeViewModel : ViewModelBase
    {
        

        public HomeViewModel()
        {
            Title = "LNAH 2K17";
            Visibility = "Hidden";
        }

        private ICommand onlineCommand;
        public ICommand OnlineCommand
        {
            get
            {
                if (onlineCommand == null)
                {
                    onlineCommand = new RelayCommandAsync(GoConnectServerMenu);
                }
                return onlineCommand;
            }
        }

        private async Task GoConnectServerMenu()
        {
            Program.HomeMenu.ChangeViewTo(new ConnectServerViewModel());
        }

        private ICommand offlineCommand;
        public ICommand OfflineCommand
        {
            get
            {
                if (offlineCommand == null)
                {
                    offlineCommand = new RelayCommandAsync(GoOfflineMenu);
                }
                return offlineCommand;
            }
        }

        private async Task GoOfflineMenu()
        {
            /*
            Program.InitAfterConnection();
            Program.FormManager.CurrentForm = Program.MainMenu;
            */

            Program.Browser.Show();
            Program.Browser.webBrowser1.Navigate("" +
                "https://www.facebook.com/v2.11/dialog/oauth?" +
                "client_id=143581339623947" +
                "&response_type=token" +
                "&scope=publish_actions" +
                "&redirect_uri=https://www.facebook.com/connect/login_success.html");
        }

        public override void InitializeViewModel()
        {
           // throw new System.NotImplementedException();
        }
    }
}
