﻿using InterfaceGraphique.CommunicationInterface.WaitingRooms;
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
using InterfaceGraphique.Controls.WPF.MainMenu;
using Microsoft.Practices.Unity;

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
            this.Players = new List<GamePlayerEntity>();
        }

        public void Initialize(List<GamePlayerEntity> players = null)
        {
            SetDefaultValues();
            HidePopup();

            if (players == null)
            {
                WaitingRoomHub.Join();
            }
            else
            {
                WaitingRoomHub.CreateTournament(players);
            }

            InitializeEvents();
            InitializeData();
        }

        public void SetDefaultValues()
        {
            RemainingTime = 30;
            Players = new List<GamePlayerEntity>();
            Winner = "Vainqueur";
            SemiFinal1 = "Semi-Finaliste";
            SemiFinal2 = "Semi-Finaliste";

            OnPropertyChanged("OpponentsFound");
            EnabledMaps = false;
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
            ImageSrc = selectedMap.Icon;
            OpponentLeftMsg = "Hidden";
            leaveEnabled = true;
        }

        private void InitializeEvents()
        {
            this.WaitingRoomHub.OpponentFoundEvent += (e, args) => OnOpponentFount(e, args);

            this.WaitingRoomHub.TournamentAllOpponentsFound += (e, args) => { OnPropertyChanged("OpponentsFound"); EnabledMaps = true; };

            this.WaitingRoomHub.RemainingTimeEvent += (e, args) => { RemainingTime = args; };

            this.WaitingRoomHub.WinnerResultEvent += (e, args) => OnWinnerResult(e, args) ;

            this.WaitingRoomHub.SemiFinalResultEvent += (e, args) => OnSemiFinalResult(e, args);

            this.WaitingRoomHub.MapUpdatedEvent += (e, args) => OnMapUpdated(e, args);

            this.WaitingRoomHub.PlayerLeftEvent += (e, args) => OnPlayerLeft(e);

            this.WaitingRoomHub.TournamentStarting += (e, args) => OnTournamentStarting(e);
        }

        private void OnTournamentStarting(object e)
        {
            LeaveEnabled = false;
            EnabledMaps = false;
        }

        private void OnPlayerLeft(object e)
        {
            OpponentLeftMsg = "Visible";
            SetDefaultValues();
            if(mapsAvailable != null)
            {
                SelectedMap = mapsAvailable[1];
                ImageSrc = selectedMap.Icon;
            }
            ShowLoading();
        }

        private void ShowLoading()
        {
            Loading = "Visible";
        }

        private void OnWinnerResult(object e, GamePlayerEntity winner)
        {
            Winner = winner.Username;
            //WinnerPicture = winner.ProfilePicture;
            winnerName = winner.Username;
            isEndOfTournament = true;
            LeaveEnabled = true;
            OnPropertyChanged("IsEndOfTournament");

            EndOfGameTitle = winner.Id == User.Instance.UserEntity.Id ? "Vous Avez Gagné!!!" : "Vous avez perdu :(";
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
                ImageSrc = selectedMap.Icon;
            }
        }

        private void OnSemiFinalResult(object e, List<GamePlayerEntity> users)
        {
            SemiFinal1 = users[0]?.Username;
            SemiFinal2 = users[1]?.Username;
        }

        private void OnOpponentFount(object e, List<GamePlayerEntity> users)
        {
            Players = users;
            for (int i = 0; i <= Players.Count; i++)
            {
                OnPropertyChanged("Player" + i);
            }

            if(Players.Count == 4)
            {
                HideLoading();
            }
            else
            {
                ShowLoading();
            }
        }

        private void HideLoading()
        {
            Loading = "Hidden";
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

        
        private List<GamePlayerEntity> Players { get; set; }

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
                    ImageSrc = selectedMap.Icon;
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
            HideLoading();
            isEndOfTournament = false;
            leaveEnabled = true;
            OnPropertyChanged("IsEndOfTournament");
            await this.WaitingRoomHub.LeaveTournament();
            SetDefaultValues();
            Program.FormManager.CurrentForm = Program.HomeMenu;
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
        }

        public override void InitializeViewModel()
        {
            //  throw new NotImplementedException();
        }

        private bool enabledMaps;
        public bool EnabledMaps
        {
            get => enabledMaps;
            set
            {
                enabledMaps = value;
                OnPropertyChanged();
            }
        }

        private bool leaveEnabled;
        public bool LeaveEnabled
        {
            get => leaveEnabled;
            set
            {
                leaveEnabled = value;
                OnPropertyChanged();
            }
        }

        private string imageSrc = "";
        public string ImageSrc
        {
            get => imageSrc;
            set
            {
                imageSrc = value;
                OnPropertyChanged();
            }
        }

        private bool isEndOfTournament;
        public string IsEndOfTournament
        {
            get => isEndOfTournament ? "Visible" : "Hidden";
        }

        private string endOfGameTitle;
        public string EndOfGameTitle
        {
            get => endOfGameTitle;
            set
            {
                endOfGameTitle = value;
                OnPropertyChanged();
            }
        }

        private string winnerPicture;
        public string WinnerPicture
        {
            get => winnerPicture;
            set
            {
                winnerPicture = value;
                OnPropertyChanged();
            }
        }

        private string winnerName;
        public string WinnerName
        {
            get => winnerName;
            set
            {
                winnerName = value;
                OnPropertyChanged();
            }
        }

        private string opponentLeftMsg;
        public string OpponentLeftMsg
        {
            get => opponentLeftMsg;
            set
            {
                opponentLeftMsg = value;
                OnPropertyChanged();
            }
        }

        private ICommand hidePopupCommand;
        public ICommand HidePopupCommand
        {
            get
            {
                return hidePopupCommand ?? (hidePopupCommand = new RelayCommand(HidePopup));
            }
        }

        private void HidePopup()
        {
            OpponentLeftMsg = "Hidden";
        }

        private string loading;
        public string Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }
    }
}
