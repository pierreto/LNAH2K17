using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.opponentName = "test";
        }

        public void Initialize()
        {
            this.OpponentName = "lal";
            LoadData();
            InitializeEvents();
            this.waitingRoomHub.JoinGame();
        }

        private void InitializeEvents()
        {
            waitingRoomHub.RemainingTimeEvent += (sender, args) => { OnRemainingTimeEvent(args);  };

            waitingRoomHub.OpponentFoundEvent += (sender, args) =>
            {
                OpponentName = args.Players[0].Name;
                PlayerName = args.Players[1].Name;
            };

            waitingRoomHub.MapUpdatedEvent += (sender, args) => { SelectedMap = args; };
        }

        private void OnRemainingTimeEvent(int remainingTime)
        {
            this.RemainingTime = remainingTime;
        }

        private async void LoadData()
        {
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

            RemainingTime = 15;
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
                selectedMap = value;
                if(string.Equals(selectedMap.MapName, value.MapName))
                {
                    waitingRoomHub.MapUpdated(value);
                }
                this.OnPropertyChanged();
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
    }
}
