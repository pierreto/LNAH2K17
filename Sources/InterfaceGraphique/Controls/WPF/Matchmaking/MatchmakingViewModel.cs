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

            InitializeEvents();

            LoadData();
        }

        private void InitializeEvents()
        {
            waitingRoomHub.RemainingTimeEvent += (sender, args) => { OnRemainingTimeEvent(args);  };

            waitingRoomHub.OpponentFoundEvent += (sender, args) => { OpponentName = args.Name; };

            waitingRoomHub.MapUpdatedEvent += (sender, args) => { SelectedMap = args.Map; };
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
                    Name = "map1"
                },
                new MapEntity
                {
                    Name = "map2"
                }
            };

            RemainingTime = 15;
        }

        private ICommand matchmakingCommand;
        public ICommand MatchmakingCommand
        {
            get
            {
                return matchmakingCommand ??
                       (matchmakingCommand = new RelayCommandAsync(Matchmaking, (o) => CanStart()));
            }
        }
        private async Task Matchmaking()
        {
            if (this.isStarted)
            {
                this.waitingRoomHub.Cancel();
                this.IsStarted = false;

            }
            else
            {
                this.waitingRoomHub.JoinGame();
                this.IsStarted = true;
            }

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
            Program.QuickPlayMenu.ShowDialog();
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
    }
}
