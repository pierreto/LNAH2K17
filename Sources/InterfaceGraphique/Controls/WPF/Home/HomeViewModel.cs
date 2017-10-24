using InterfaceGraphique.Controls.WPF.ConnectServer;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.Home
{
    public class HomeViewModel : ViewModelBase
    {
        

        public HomeViewModel()
        {
            Title = "LNAH 2K17";
            Hidden = "Hidden";
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
            Program.FormManager.CurrentForm = Program.MainMenu;
        }
    }
}
