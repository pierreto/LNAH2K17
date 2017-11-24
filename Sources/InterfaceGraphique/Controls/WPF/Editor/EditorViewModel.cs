using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.ObjectBuilder2;
using System.Security.Cryptography;
using InterfaceGraphique.Editor;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class EditorViewModel : ViewModelBase
    {
        private MapService mapService;
        private MapManager mapManager;

        private ObservableCollection<MapEntity> onlineEditedMapInfos;
        private ICommand serverListViewCommand;
        private ICommand modeViewCommand;
        private ICommand joinEditionCommand;

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

        public EditorViewModel(MapService mapService,MapManager mapManager)
        {
            this.mapService = mapService;
            this.onlineEditedMapInfos = new ObservableCollection<MapEntity>();
            this.mapManager = mapManager;
        }


        public async Task InitializeViewModelAsync()
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
                Program.EditorHost.Close();
            }

        }


        public ICommand checkPrivatePasswordCommand;

        public ICommand CheckPrivatePasswordCommand
        {
            get
            {
                return checkPrivatePasswordCommand ??
                       (checkPrivatePasswordCommand =
                           new RelayCommandAsync(CheckPrivatePassword, (o) => CanCheckPrivatePassword()));
            }


        }

        public ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                       (refreshCommand =
                           new RelayCommandAsync(RefreshMapList, (o) => true));
            }


        }

        public async Task RefreshMapList()
        {
            await InitializeViewModelAsync();
        }

        private bool CanCheckPrivatePassword()
        {
            return Password?.Length >= 5;
        }

        private async Task CheckPrivatePassword()
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var encryptedPassword =
                    Convert.ToBase64String(
                        sha1.ComputeHash(
                            Encoding.UTF8.GetBytes(Password)));

            if (selectedMap.Password.Equals(encryptedPassword))
            {
                await Program.Editeur.JoinEdition(this.selectedMap);
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
            MapMetaData currentMap = mapManager.CurrentMapInfo;
            return this.selectedMap != null && this.selectedMap.CurrentNumberOfPlayer < 4 &&
                   (currentMap == null || currentMap.Id != this.selectedMap.Id);
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

        public override void InitializeViewModel()
        {
        }
    }

}
