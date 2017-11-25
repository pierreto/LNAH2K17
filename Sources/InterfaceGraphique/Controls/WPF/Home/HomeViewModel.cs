using InterfaceGraphique.Controls.WPF.ConnectServer;
using InterfaceGraphique.Controls.WPF.MainMenu;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Home
{
    public class HomeViewModel : ViewModelBase
    {
        

        public HomeViewModel()
        {
            Title = "LNAH 2K17";
            Row = 0;
            RowSpan = 5;
            Visibility = "Hidden";
        }

        private ICommand onlineCommand;
        public ICommand OnlineCommand
        {
            get
            {
                if (onlineCommand == null)
                {
                    onlineCommand = new RelayCommand(GoConnectServerMenu);
                }
                return onlineCommand;
            }
        }

        private void GoConnectServerMenu()
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
                    offlineCommand = new RelayCommand(GoOfflineMenu);
                }
                return offlineCommand;
            }
        }

        private void GoOfflineMenu()
        {
            Program.InitAfterConnection();
            Program.unityContainer.Resolve<MainMenuViewModel>().NotLoading = true;
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
            //Program.FormManager.CurrentForm = Program.MainMenu;
        }

        public override void InitializeViewModel()
        {
           // throw new System.NotImplementedException();
        }
    }
}
