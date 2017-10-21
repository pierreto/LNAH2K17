using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace InterfaceGraphique.Controls.WPF.Tournament
{
    public class TournamentViewModel : ViewModelBase
    {
        private const string DEFAULT_PLAYER_NAME = "En attente d'un joueur";

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
            GameEntity gg = new GameEntity();
            //MapsAvailable = await MapsRepository.GetMaps();
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

            SelectedMap = mapsAvailable[1];
            Player1 = waitingRoomHub.Username;
        }

        private void InitializeEvents()
        {
            this.waitingRoomHub.OpponentFoundEvent += (e, args) => OnOpponentFount(e, args);

            this.waitingRoomHub.TournamentAllOpponentsFound += (e, args) => { OnPropertyChanged("OpponentsFound"); };
        }

        private void OnOpponentFount(object e, UserEntity user)
        {
            Players.Add(user);
            for (int i = 0; i < Players.Count; i++)
            {
                OnPropertyChanged("Player" + i);
            }
        }

        private void LoadData()
        {
            Players.Add(
                new UserEntity
                {
                    Username = this.waitingRoomHub.Username
                }
            );

            Player1 = Players[0].Username;
        }

        private int remainingTime = 30;
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

        private string player1;
        public string Player1
        {
            get => player1;
            set
            {
                player1 = value;
                this.OnPropertyChanged();
            }
        }

        public string Player2
        {
            get => Players.Count > 0 ? Players[1].Username : DEFAULT_PLAYER_NAME;
        }

        public string Player3
        {
            get => Players.Count > 1 ? Players[2].Username : DEFAULT_PLAYER_NAME;
        }

        public string Player4
        {
            get => Players.Count > 2 ? Players[3].Username : DEFAULT_PLAYER_NAME;
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
                if (selectedMap == null || !string.Equals(selectedMap.MapName, value.MapName))
                {
                    foreach (MapEntity map in mapsAvailable)
                    {

                        if (string.Equals(map.MapName, value.MapName))
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
            get => Players.Count == 4 ? "Visible" : "Hidden";
        }


    }
}
