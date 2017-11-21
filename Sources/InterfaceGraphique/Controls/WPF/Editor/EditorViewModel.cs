using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.ObjectBuilder2;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class EditorViewModel : ViewModelBase
    {
        private MapService mapService;

        private ObservableCollection<MapEntity> onlineEditedMapInfos;
        private ICommand serverListViewCommand;
        private ICommand modeViewCommand;
        private ICommand joinEditionCommand;

        private MapEntity currentMap;
        private MapEntity selectedMap;

        private string password;
        private bool passwordFailed = false;

        public bool PasswordFailed
        {
            get => passwordFailed;
            set
            {
                passwordFailed = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                PasswordFailed = false;

                OnPropertyChanged();
            }
        }

        public EditorViewModel(MapService mapService)
        {
            this.mapService = mapService;
            this.onlineEditedMapInfos = new ObservableCollection<MapEntity>();

        }

        public void ClearCurrentMap()
        {
            currentMap = null;
        }
        public override async void  InitializeViewModel()
        {
            this.onlineEditedMapInfos.Clear();
            //TODO:Not optimized should use a list here but for testing purpose i'll leave it this way
            try
            {
                List<MapEntity> list = await this.mapService.GetMaps();

                list?.ForEach(map => this.onlineEditedMapInfos.Add(map));
            }
            catch (Exception e)
            {
                
            }
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
        public ObservableCollection<MapEntity> OnlineEditedMapInfos
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
            if (selectedMap.Private)
            {

                //show the dialog
                await DialogHost.Show(Program.EditorHost.PasswordDialog, "RootDialog", ExtendedOpenedEventHandler,
                    ExtendedClosingEventHandler);
            }
            else
            {
                await Program.Editeur.JoinEdition(this.selectedMap);
                currentMap = this.selectedMap;
                Program.EditorHost.Close();
            }

        }


        public ICommand checkPrivatePasswordCommand;
        public ICommand CheckPrivatePasswordCommand
        {
            get
            {
                return checkPrivatePasswordCommand ??
                       (checkPrivatePasswordCommand = new RelayCommandAsync(CheckPrivatePassword, (o) => CanCheckPrivatePassword()));
            }


        }

        private bool CanCheckPrivatePassword()
        {
            return Password?.Length >= 5;
        }

        private async Task CheckPrivatePassword()
        {
            if (selectedMap.Password.Equals(Password))
            {
                await Program.Editeur.JoinEdition(this.selectedMap);
                currentMap = this.selectedMap;
                Program.EditorHost.Close();
            }
            else
            {
                PasswordFailed = true;
            }
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventargs)
        {

        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            PasswordFailed = false;
            Password = "";
        }


        private bool CanJoinEdition()
        {

            return this.selectedMap!=null && this.selectedMap.CurrentNumberOfPlayer<4 && (currentMap==null || currentMap.Id!=this.selectedMap.Id);
        }

        public MapEntity SelectedMap
        {
            get => selectedMap;
            set
            {
                selectedMap = value;
                OnPropertyChanged();
            }
        }
    }

}
