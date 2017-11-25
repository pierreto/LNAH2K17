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
using InterfaceGraphique.Controls.WPF.MainMenu;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Managers;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace InterfaceGraphique.Controls.WPF.Matchmaking
{
    public class MatchmakingViewModel : ViewModelBase
    {
        public GameWaitingRoomHub WaitingRoomHub { get; set; }
        private bool isStarted;

        protected MapService MapService { get; }
        public GameRequestManager GameRequestManager { get; }
        protected MapsRepository MapsRepository { get; set; }

        public MatchmakingViewModel(GameWaitingRoomHub matchmakingHub, MapService mapService, GameRequestManager gameRequestManager)
        {
            this.WaitingRoomHub = matchmakingHub;
            MapService = mapService;
            GameRequestManager = gameRequestManager;
            this.isStarted = false;
            this.MapsRepository = new MapsRepository();

            WaitingRoomHub.RemainingTimeEvent += (sender, args) => { OnRemainingTimeEvent(args); };

            WaitingRoomHub.OpponentFoundEvent += (sender, args) =>
            {
                OpponentName = args.Players[0].Username;
                OpponentPicture = args.Players[0].ProfilePicture;
                PlayerName = args.Players[1].Username;
                PlayerPicture = args.Players[1].ProfilePicture;
                SetVisibility(false);
            };

            WaitingRoomHub.MapUpdatedEvent += (sender, args) => OnMapUpdated(sender, args);

            WaitingRoomHub.OpponentLeftEvent += (sender, args) => OnOpponentLeft(sender, args);
        }

        private void OnOpponentLeft(object sender, int args)
        {
            SetDefaultValues();

            selectedMap = mapsAvailable[1];
            ImageSrc = mapsAvailable[1].Icon;
            this.OnPropertyChanged("SelectedMap");

            opponentLeftMsg = true;
            OnPropertyChanged("OpponentLeftMsg");
        }

        public override void InitializeViewModel()
        {

        }

        public void SetOnlineGame()
        {
            Program.QuickPlay.CurrentGameState.IsTournementMode = false;

            string baseName = "Joueur";
            StringBuilder player1Name = new StringBuilder(6);
            StringBuilder player2Name = new StringBuilder(6);
            player1Name.Append(baseName);
            player2Name.Append(baseName);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);

            float[] playerColor = new float[4] { Color.White.R, Color.White.G, Color.White.B, Color.White.A };
            FonctionsNatives.setPlayerColors(playerColor, playerColor);

            OpponentType opponentType = opponentType = OpponentType.ONLINE_PLAYER;
            FonctionsNatives.setCurrentOpponentType((int)opponentType);
        }

        public void Initialize(bool isGameRequest = false)
        {
            LoadData();
            if (!isGameRequest)
            {
                SetDefaultValues();
                this.WaitingRoomHub.Join();
            }
        }

        public void SetDefaultValues()
        {
            RemainingTime = 30;
            SetVisibility(true);
            OpponentName = string.Empty;
            playerName = string.Empty;
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
            OnPropertyChanged("SelectedMap");
            ImageSrc = selectedMap.Icon;
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
            ImageSrc = mapsAvailable[1].Icon;
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

        public async Task LeaveGame()
        {
            await this.WaitingRoomHub.LeaveGame();
            await GameRequestManager.CancelGameRequest();
            SetDefaultValues();
            Program.FormManager.CurrentForm = Program.HomeMenu;
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
        }

        private async Task MainMenu()
        {
            await LeaveGame();
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
                            ImageSrc = selectedMap.Icon;
                        }
                    }
                    this.OnPropertyChanged();
                    WaitingRoomHub.UpdateSelectedMap(value);
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

        private string opponentPicture;
        public string OpponentPicture
        {
            get => opponentPicture;
            set
            {
                opponentPicture = value;
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

        private string playerPicture;
        public string PlayerPicture
        {
            get => playerPicture;
            set
            {
                playerPicture = value;
                this.OnPropertyChanged();
            }
        }

        private string imageSrc;
        public string ImageSrc
        {
            get
            {
                return imageSrc;
            }
            set
            {
                imageSrc = value;
                OnPropertyChanged();
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

        public bool opponentLeftMsg = false;
        public string OpponentLeftMsg
        {
            get => opponentLeftMsg ? "Visible" : "Hidden";
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
            opponentLeftMsg = false;
            OnPropertyChanged("OpponentLeftMsg");
        }
    }
}
