﻿using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.CommunicationInterface.RestInterface;

namespace InterfaceGraphique.Controls.WPF.Tournament
{
    public class TournamentViewModel : ViewModelBase
    {
        private const string DEFAULT_PLAYER_NAME = "En attente";

        private TournamentWaitingRoomHub waitingRoomHub;

        public TournamentViewModel(TournamentWaitingRoomHub waitingRoomHub)
        {
            this.waitingRoomHub = waitingRoomHub;
            this.Players = new List<UserEntity>();
        }

        public void Initialize()
        {
            waitingRoomHub.Join();
            InitializeEvents();
            InitializeData();
        }

        private void InitializeData()
        {
            var mapsRepo = new MapsRepository();

            //MapsAvailable = new ObservableCollection<MapEntity>(Task.Run(() => mapsRepo.GetMaps()).Result);
            MapsAvailable = new ObservableCollection<MapEntity>
            {
                new MapEntity
                {
                    MapName = "foret enchantee",
                    Id = 1
                },
                new MapEntity
                {
                    MapName = "loup garou",
                    Id = 2
                },
                new MapEntity
                {
                    MapName = "New york",
                    Id = 3
                }
            };

            SelectedMap = mapsAvailable[1];
        }

        private void InitializeEvents()
        {
            this.waitingRoomHub.OpponentFoundEvent += (e, args) => OnOpponentFount(e, args);

            this.waitingRoomHub.TournamentAllOpponentsFound += (e, args) => { OnPropertyChanged("OpponentsFound"); OnPropertyChanged("EnabledMaps"); };

            this.waitingRoomHub.RemainingTimeEvent += (e, args) => { RemainingTime = args; };

            this.waitingRoomHub.WinnerResultEvent += (e, args) => { Winner = args.Username; };

            this.waitingRoomHub.SemiFinalResultEvent += (e, args) => OnSemiFinalResult(e, args);

            this.waitingRoomHub.MapUpdatedEvent += (e, args) => OnMapUpdated(e, args);
        }

        private void OnMapUpdated(object e, MapEntity args)
        {
            if (args != null)
            {
                SelectedMap = args;
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
                    waitingRoomHub.UpdateSelectedMap(value);
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
                return cancel ?? (cancel = new RelayCommandAsync(Join, (o) => true));
            }
        }

        private async Task Join()
        {
            this.waitingRoomHub.Join();
        }

        public override void InitializeViewModel()
        {
          //  throw new NotImplementedException();
        }

        public bool EnabledMaps
        {
            get => Players.Count == 4;
        }
}
}
