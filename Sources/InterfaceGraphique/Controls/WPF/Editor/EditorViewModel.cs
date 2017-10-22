using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.Entities.Model;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class EditorViewModel
    {

        private ObservableCollection<OnlineEditedMapInfo> onlineEditedMapInfos;
        private ICommand serverListViewCommand;
        private ICommand modeViewCommand;
        private ICommand joinEditionCommand;

        public EditorViewModel()
        {
            this.onlineEditedMapInfos = new ObservableCollection<OnlineEditedMapInfo>();
            OnlineEditedMapInfos = new ObservableCollection<OnlineEditedMapInfo>
            {
                new OnlineEditedMapInfo() {MapName = "test text 1", NumberOfPlayer = 2},
                new OnlineEditedMapInfo() {MapName = "test text 3", NumberOfPlayer = 2}
            };
        }

        public ICommand ServerListViewCommand
        {
            get
            {
                return serverListViewCommand ??
                       (serverListViewCommand = new RelayCommandAsync(SwitchToServerBrowser, (o) => true));
            }
        }

        public async Task SwitchToServerBrowser()
        {
            Program.EditorHost.SwitchViewToServerBrowser();
        }

        public ICommand PrivateModeCommand
        {
            get
            {
                return modeViewCommand ??
                       (modeViewCommand = new RelayCommandAsync(SwitchToCreationMode, (o) => true));
            }
        }

        public async Task SwitchToCreationMode()
        {
            Program.EditorHost.SwitchViewToMapModeView();
        }
        public ObservableCollection<OnlineEditedMapInfo> OnlineEditedMapInfos
        {
            get => onlineEditedMapInfos;
            set => onlineEditedMapInfos = value;
        }

        public ICommand JoinEditionCommand
        {
            get
            {
                return joinEditionCommand ??
                       (joinEditionCommand = new RelayCommandAsync(JoinEdition, (o) => CanJoinEdition()));
            }

        }

        private async Task JoinEdition()
        {
            
        }

        private bool CanJoinEdition()
        {
            return true;
        }
    }
}
