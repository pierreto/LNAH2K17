using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Services;
using System.IO;

namespace InterfaceGraphique.Controls.WPF.Tournament
{
    public class TournamentViewModel : ViewModelBase
    {
        private const string DEFAULT_PLAYER_NAME = "En attente";

        public TournamentWaitingRoomHub WaitingRoomHub { get; set; }

        protected MapService MapService { get; set; }

        public TournamentViewModel(TournamentWaitingRoomHub waitingRoomHub, MapService mapService)
        {
            this.WaitingRoomHub = waitingRoomHub;
            MapService = mapService;
            this.Players = new List<UserEntity>();
        }

        public void Initialize()
        {
            SetDefaultValues();
            WaitingRoomHub.Join();
            InitializeEvents();
            InitializeData();
        }

        public void SetDefaultValues()
        {
            MapsAvailable = new ObservableCollection<MapEntity>();
            RemainingTime = 30;
            Players = new List<UserEntity>();
            Winner = string.Empty;
            SemiFinal1 = string.Empty;
            SemiFinal2 = string.Empty;

            OnPropertyChanged("OpponentsFound");
            OnPropertyChanged("EnabledMaps");
            OnPropertyChanged("Player1");
            OnPropertyChanged("Player2");
            OnPropertyChanged("Player3");
            OnPropertyChanged("Player4");
        }

        private async void InitializeData()
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
            SelectedMap = mapsAvailable[1];
            

        }

        private void InitializeEvents()
        {
            this.WaitingRoomHub.OpponentFoundEvent += (e, args) => OnOpponentFount(e, args);

            this.WaitingRoomHub.TournamentAllOpponentsFound += (e, args) => { OnPropertyChanged("OpponentsFound"); OnPropertyChanged("EnabledMaps"); };

            this.WaitingRoomHub.RemainingTimeEvent += (e, args) => { RemainingTime = args; };

            this.WaitingRoomHub.WinnerResultEvent += (e, args) => { Winner = args.Username; };

            this.WaitingRoomHub.SemiFinalResultEvent += (e, args) => OnSemiFinalResult(e, args);

            this.WaitingRoomHub.MapUpdatedEvent += (e, args) => OnMapUpdated(e, args);
        }

        private void OnMapUpdated(object e, MapEntity args)
        {
            if (args != null)
            {
                if (args != null && (selectedMap == null || selectedMap.Id != args.Id))
                {
                    foreach (MapEntity map in mapsAvailable)
                    {
                        if (map.Id == args.Id)
                        {
                            selectedMap = map;
                        }
                    }
                }
                OnPropertyChanged("SelectedMap");
            }
        }

        private void OnSemiFinalResult(object e, List<UserEntity> users)
        {
            SemiFinal1 = users[0]?.Username;
            SemiFinal2 = users[1]?.Username;
        }

        private void OnOpponentFount(object e, List<UserEntity> users)
        {
            Players = users;
            for (int i = 0; i <= Players.Count; i++)
            {
                OnPropertyChanged("Player" + i);
            }
        }

        private int remainingTime = 0;
        public int RemainingTime
        {
            get => remainingTime;
            set
            {
                remainingTime = value;
                this.OnPropertyChanged();
            }
        }

        private List<UserEntity> Players { get; set; }

        public string Player1
        {
            get => Players.Count > 0 ? Players[0].Username : DEFAULT_PLAYER_NAME;
        }

        public string Player2
        {
            get => Players.Count > 1 ? Players[1].Username : DEFAULT_PLAYER_NAME;
        }

        public string Player3
        {
            get => Players.Count > 2 ? Players[2].Username : DEFAULT_PLAYER_NAME;
        }

        public string Player4
        {
            get => Players.Count > 3 ? Players[3].Username : DEFAULT_PLAYER_NAME;
        }

        private string winner;
        public string Winner
        {
            get => winner;
            set
            {
                winner = value;
                OnPropertyChanged();
            }
        }

        private string semiFinal1;
        public string SemiFinal1
        {
            get => semiFinal1;
            set
            {
                semiFinal1 = value;
                OnPropertyChanged();
            }
        }

        private string semiFinal2;
        public string SemiFinal2
        {
            get => semiFinal2;
            set
            {
                semiFinal2 = value;
                OnPropertyChanged();
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
                    WaitingRoomHub.UpdateSelectedMap(value);
                }
            }
        }

        public string OpponentsFound
        {
            get => Players.Count == 4 ? "Enabled" : "Disabled";
        }

        private ICommand cancel;
        public ICommand Cancel
        {
            get
            {
                return cancel ?? (cancel = new RelayCommandAsync(Leave, (o) => true));
            }
        }

        private async Task Leave()
        {
            await this.WaitingRoomHub.LeaveTournament();
            SetDefaultValues();
            Program.FormManager.CurrentForm = Program.MainMenu;
        }

        public override void InitializeViewModel()
        {
            //  throw new NotImplementedException();
        }

        public bool EnabledMaps
        {
            get => Players.Count == 4;
        }

        private string imageSrc = "";
        public string ImageSrc
        {
            get => Directory.GetCurrentDirectory() + "\\media\\image\\No_image_available.png";
            set
            {
                imageSrc = value;
                OnPropertyChanged();
            }
        }
    }
}
