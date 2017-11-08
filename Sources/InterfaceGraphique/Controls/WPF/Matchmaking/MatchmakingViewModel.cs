using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using System.Collections.ObjectModel;
using InterfaceGraphique.Services;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Controls.WPF.Matchmaking
{
    public class MatchmakingViewModel : ViewModelBase
    {
        private GameWaitingRoomHub waitingRoomHub;
        private bool isStarted;

        protected MapService MapService { get; }

        protected MapsRepository MapsRepository { get; set; }

        public MatchmakingViewModel(GameWaitingRoomHub matchmakingHub, MapService mapService)
        {

            this.waitingRoomHub = matchmakingHub;
            MapService = mapService;
            this.isStarted = false;
            this.MapsRepository = new MapsRepository();
        }
        public override void InitializeViewModel()
        {
            LoadData();
            InitializeEvents();
            this.waitingRoomHub.Join();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            RemainingTime = 30;
        }


        private void InitializeEvents()
        {
            waitingRoomHub.RemainingTimeEvent += (sender, args) => { OnRemainingTimeEvent(args);  };

            waitingRoomHub.OpponentFoundEvent += (sender, args) =>
            {
                OpponentName = args.Players[0].Username;
                PlayerName = args.Players[1].Username;
                SetVisibility(false);
            };

            waitingRoomHub.MapUpdatedEvent += (sender, args) => OnMapUpdated(sender, args);

        }

        private void OnMapUpdated(object sender, MapEntity args)
        {
            foreach (MapEntity map in mapsAvailable)
            {
                if (map.Id == args.Id)
                {
                    selectedMap = map;
                }
            }
            OnPropertyChanged("SelectedMap")
        }

        private void SetVisibility(bool isWaitingForOpponentValue)
        {
            IsWaitingForOpponent = isWaitingForOpponentValue ? "Visible" : "Hidden";
            OpponentFound = isWaitingForOpponentValue ? "Hidden" : "Visible";
            EnabledMap = !isWaitingForOpponentValue;
        }

        private void OnRemainingTimeEvent(int remainingTime)
        {
            this.RemainingTime = remainingTime;
        }

        private async void LoadData()
        {
            if (User.Instance.IsConnected)
            {
                var maps = await MapService.GetMaps();
                MapsAvailable = new ObservableCollection<MapEntity>(maps);
            }
            else
            {
                MapsAvailable = new ObservableCollection<MapEntity>
                {
                    new MapEntity
                    {
                        MapName = "foret enchantee"
                    },
                    new MapEntity
                    {
                        MapName = "loup garou"
                    },
                    new MapEntity
                    {
                        MapName = "New york"
                    }
                };

            }
            selectedMap = mapsAvailable[1];
            this.OnPropertyChanged("SelectedMap");

        }

        private ICommand mainMenuCommand;
        public ICommand MainMenuCommand
        {
            get
            {
                return mainMenuCommand ??
                       (mainMenuCommand = new RelayCommandAsync(MainMenu, (o) => true));
            }
        }

        private ICommand cancel;
        public ICommand Cancel
        {
            get
            {
                return cancel ?? (cancel = new RelayCommandAsync(LeaveGame, (o) => true));
            }
        }

        private async Task LeaveGame()
        {
            await this.waitingRoomHub.LeaveGame();
            SetDefaultValues();
            Program.FormManager.CurrentForm = Program.MainMenu;
        }

        private async Task MainMenu()
        {
            Program.FormManager.CurrentForm=Program.MainMenu;
        }

        private void StartGame()
        {
            Program.FormManager.CurrentForm = Program.QuickPlay;
        }

        private bool CanStart()
        {
            return true;
        }

        public bool IsStarted
        {
            get => isStarted;
            set
            {
                isStarted = value;
                this.OnPropertyChanged();
            }
        }

        private ObservableCollection<MapEntity> mapsAvailable;
        public ObservableCollection<MapEntity> MapsAvailable
        {
            get => mapsAvailable;
            set
            {
                mapsAvailable = value;
                this.OnPropertyChanged();
            }
        }

        private MapEntity selectedMap;
        public MapEntity SelectedMap
        {
            get => selectedMap;
            set
            {
                if (value != null && (selectedMap == null || selectedMap.Id != value.Id))
                {
                    foreach (MapEntity map in mapsAvailable)
                    {
                        if (map.Id == value.Id)
                        {
                            selectedMap = map;
                        }
                    }
                    this.OnPropertyChanged();
                    waitingRoomHub.UpdateSelectedMap(value);
                }
            }
        }

        private int remainingTime;
        public int RemainingTime
        {
            get => remainingTime;
            set
            {
                remainingTime = value;
                this.OnPropertyChanged();
            }
        }

        private string opponentName;
        public string OpponentName
        {
            get => opponentName;
            set
            {
                opponentName = value;
                this.OnPropertyChanged();
            }
        }

        private string playerName;
        public string PlayerName
        {
            get => playerName;
            set
            {
                playerName = value;
                this.OnPropertyChanged();
            }
        }

        public string ImageSrc
        {
            get
            {
                var test = Directory.GetCurrentDirectory() + "\\media\\image\\No_image_available.png";
                return test;
            }
        }

        private string isWaitingForOpponent = "Visible";
        public string IsWaitingForOpponent
        {
            get
            {
                return isWaitingForOpponent;
            }
            set
            {
                isWaitingForOpponent = value;
                this.OnPropertyChanged();
            }
        }

        private string opponentFound = "Hidden";
        public string OpponentFound
        {
            get => opponentFound;

            set
            {
                opponentFound = value;
                this.OnPropertyChanged();
            }
        }

        public bool enabledMap = false;

        public bool EnabledMap
        {
            get => enabledMap;
            set
            {
                enabledMap = value;
                OnPropertyChanged();
            }
        }

    }
}
