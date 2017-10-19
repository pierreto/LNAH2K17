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

            waitingRoomHub.MapUpdatedEvent += (sender, args) => { selectedMap = args; OnPropertyChanged(); };
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
            MapsAvailable = new List<MapEntity>
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

            SelectedMap = mapsAvailable[0];

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

        private List<MapEntity> mapsAvailable;
        public List<MapEntity> MapsAvailable
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
                if (!string.Equals(selectedMap?.MapName, value?.MapName))
                {
                    selectedMap = value;
                    waitingRoomHub.UpdateSelectedMap(value);
                    this.OnPropertyChanged();
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
