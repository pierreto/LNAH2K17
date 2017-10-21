using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using System.Collections.ObjectModel;

namespace InterfaceGraphique.Controls.WPF.Matchmaking
{
    public class MatchmakingViewModel : ViewModelBase
    {
        private WaitingRoomHub waitingRoomHub;
        private bool isStarted;

        protected MapsRepository MapsRepository { get; set; }

        public MatchmakingViewModel(WaitingRoomHub matchmakingHub)
        {

            this.waitingRoomHub = matchmakingHub;
            this.isStarted = false;
            this.MapsRepository = new MapsRepository();
        }

        public void Initialize()
        {
            LoadData();
            InitializeEvents();
            this.waitingRoomHub.JoinGame();
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

            waitingRoomHub.MapUpdatedEvent += (sender, args) => { SelectedMap = args; };
            //waitingRoomHub.MapUpdatedEvent += (sender, args) => { SelectedMap = args; };

        }

        private void SetVisibility(bool isWaitingForOpponentValue)
        {
            IsWaitingForOpponent = isWaitingForOpponentValue ? "Visible" : "Hidden";
            OpponentFound = isWaitingForOpponentValue ? "Hidden" : "Visible";
        }

        private void OnRemainingTimeEvent(int remainingTime)
        {
            this.RemainingTime = remainingTime;
        }

        private async void LoadData()
        {
            GameEntity gg = new GameEntity();
            //MapsAvailable = await MapsRepository.GetMaps();
            MapsAvailable = new ObservableCollection<MapEntity>
            {
                new MapEntity
                {
                    MapName = "map1"
                },
                new MapEntity
                {
                    MapName = "map2"
                }
            };

            SelectedMap = mapsAvailable[1];

            RemainingTime = 30;
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
                if (selectedMap==null || !string.Equals(selectedMap.MapName, value.MapName))
                {
                    foreach(MapEntity map in mapsAvailable)
                    {

                        if(string.Equals(map.MapName, value.MapName))
                        {
                            selectedMap = map ;
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

    }
}
