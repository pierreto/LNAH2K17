using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
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

        private MapEntity selectedMap;

        public EditorViewModel(MapService mapService)
        {
            this.mapService = mapService;
            this.onlineEditedMapInfos = new ObservableCollection<MapEntity>();

        }
        public override async void  InitializeViewModel()
        {
            this.onlineEditedMapInfos.Clear();
            //TODO:Not optimized should use a list here but for testing purpose i'll leave it this way
            List<MapEntity> list = await this.mapService.GetMaps(); 
            try
            {
                if (list != null)
                {
                    list.ForEach(map => this.onlineEditedMapInfos.Add(map));

                }

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
            await Program.Editeur.JoinEdition(this.selectedMap);
            Program.EditorHost.Close();
        }

        private bool CanJoinEdition()
        {
            return this.selectedMap!=null;
        }

        public MapEntity SelectedMap
        {
            get => selectedMap;
            set => selectedMap = value;
        }
    }
}
