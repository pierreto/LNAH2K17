using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.UserProfile
{
    public class UserProfileViewModel : ViewModelBase
    {
        public override void InitializeViewModel()
        {

        }


        public UserProfileViewModel(PlayerStatsService playerStatsService)
        {
            this.Achievements = new ObservableCollection<Achievement>();
            PlayerStatsService = playerStatsService;

            Name = "myName";
            UserName = "myUsername";
            Email = "email@love.com";
            CreationDate = DateTime.Now.ToLongDateString();
            PointsNb = 0;
            GameWon = 0;
            TournamentWon = 0;
            ProfilePictureUrl = Directory.GetCurrentDirectory() + "\\media\\image\\default_profile_picture.png";

            //Achievements = new ObservableCollection<Achievement>()
            //{
            //    new Achievement(Directory.GetCurrentDirectory() + "\\media\\image\\coins.png", Directory.GetCurrentDirectory() + "\\media\\image\\piggy-bank.png")
            //    {
            //        Name = "Test point"
            //    },
            //    new Achievement
            //    {
            //        Name = "Test rere"
            //    }
            //};
        }

        public async void Initialize()
        {
            UserName = User.Instance.UserEntity.Username;
            
            if(User.Instance.IsConnected)
            {
                var achievements = await PlayerStatsService.GetPlayerAchivements(User.Instance.UserEntity.Id);
                Achievements = new ObservableCollection<Achievement>(achievements);

                var playerStats = await PlayerStatsService.GetPlayerStats(User.Instance.UserEntity.Id);
                PointsNb = playerStats.Points;
                TournamentWon = playerStats.TournamentsWon;
                GameWon = playerStats.GamesWon;
            }
        }

        private ObservableCollection<Achievement> achievements;
        public ObservableCollection<Achievement> Achievements
        {
            get => achievements;
            set
            {
                achievements = value;
                OnPropertyChanged();
            }
        }

        private string profilePictureUrl;
        public string ProfilePictureUrl
        {
            get => profilePictureUrl;
            set
            {
                profilePictureUrl = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string userName;
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string creationDate;
        public string CreationDate
        {
            get => creationDate;
            set
            {
                creationDate = value;
                OnPropertyChanged();
            }
        }

        private int pointsNb;
        public int PointsNb
        {
            get => pointsNb;
            set
            {
                pointsNb = value;
                OnPropertyChanged();
            }
        }

        private int gameWon;
        public int GameWon
        {
            get => gameWon;
            set
            {
                gameWon = value;
                OnPropertyChanged();
            }
        }

        private int tournamentWon;
        public int TournamentWon
        {
            get => tournamentWon;
            set
            {
                tournamentWon = value;
                OnPropertyChanged();
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

        public PlayerStatsService PlayerStatsService { get; }

        private async Task MainMenu()
        {
            Program.FormManager.CurrentForm = Program.MainMenu;
        }

    }
}
