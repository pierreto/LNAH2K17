﻿using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Store;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.UserProfile
{
    public class UserProfileViewModel : ViewModelBase
    {
        public override void InitializeViewModel()
        {

        }


        public UserProfileViewModel(PlayerStatsService playerStatsService, StoreService storeService, UserService userService)
        {
            PlayerStatsService = playerStatsService;
            StoreService = storeService;
            UserService = userService;
            Reset();
        }

        public void Reset()
        {
            this.Achievements = new ObservableCollection<Achievement>();
            Items = new List<ItemViewModel>();
            Name = "myName";
            UserName = "myUsername";
            Email = "email@love.com";
            CreationDate = DateTime.Now.ToLongDateString();
            PointsNb = 0;
            GameWon = 0;
            TournamentWon = 0;
            ProfilePictureUrl = Directory.GetCurrentDirectory() + "\\media\\image\\default_profile_picture.png";
        }

        public async Task Initialize(int userId = 0)
        {
            isFriendProfile = userId != 0;
            OnPropertyChanged("IsFriendProfile");

            int profileId = isFriendProfile ? userId : User.Instance.UserEntity.Id;

            if (!isFriendProfile)
            {
                UserName = User.Instance.UserEntity.Username;
                Name = User.Instance.UserEntity.Name;
                Email = User.Instance.UserEntity.Email;
                //TODO : GET USER BY ID for name,email,date of account creation, profile pic
                var items = await StoreService.GetUserStoreItems(User.Instance.UserEntity.Id);

                Items = new List<ItemViewModel>();
                foreach (var item in items)
                {
                    Items.Add(new ItemViewModel(item, false));
                }
                OnPropertyChanged("Items");

                //Items = await StoreService.GetUserStoreItems(User.Instance.UserEntity.Id);
            }
            else
            {
                var users = await UserService.GetAllUsers();
                var friend = users.Find(x => x.Id == userId);

                UserName = friend.Username;
                Name = friend.Name;
                Email = friend.Email;
                ProfilePicture = friend.Profile;
            }

            var achievements = await PlayerStatsService.GetPlayerAchivements(profileId);
            Achievements = new ObservableCollection<Achievement>(achievements.OrderBy(x => x.Category).ThenBy(x => x.Order));

            var playerStats = await PlayerStatsService.GetPlayerStats(profileId);
            if (playerStats != null)
            {
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

        private string profilePicture;
        public string ProfilePicture
        {
            get => profilePicture;
            set
            {
                profilePicture = value;
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
                       (mainMenuCommand = new DelegateCommand(MainMenu));
            }
        }

        public PlayerStatsService PlayerStatsService { get; }

        public StoreService StoreService { get; }

        public UserService UserService { get; }

        private void MainMenu()
        {
            Program.FormManager.CurrentForm = Program.MainMenu;
        }

        private List<ItemViewModel> items;
        public List<ItemViewModel> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }

        private ICommand _selectCommand;
        public ICommand SelectCommand
        {
            get { return _selectCommand ?? (_selectCommand = new DelegateCommand<ItemViewModel>(DoSelect)); }
        }

        private async void DoSelect(ItemViewModel item)
        {
            if (item.IsGameEnabled)
            {
                item.StoreItem.IsGameEnabled = true;
                foreach (var element in Items)
                {
                    if (element.Id != item.Id && element.IsGameEnabled)
                    {
                        element.IsGameEnabled = false;
                    }
                }

                await StoreService.UpdateItemEnable(User.Instance.UserEntity.Id, item.StoreItem);
            }
            else
            {
                item.StoreItem.IsGameEnabled = false;
                await StoreService.UpdateItemEnable(User.Instance.UserEntity.Id, item.StoreItem);
            }

            User.Instance.Inventory = await StoreService.GetUserStoreItems(User.Instance.UserEntity.Id);
        }

        private int friendId;
        public int FriendId
        {
            get => friendId;
            set
            {
                friendId = value;
                OnPropertyChanged();
            }
        }

        private ICommand searchFriend;
        public ICommand SearchFriend
        {
            get
            {
                return searchFriend ??
                                       (searchFriend = new RelayCommandAsync(SearchFriendAction, (o) => true));
            }
        }

        private async Task SearchFriendAction()
        {
            if (friendId > -1)
            {
                Reset();
                await Initialize(friendId);
            }
        }

        private ICommand backProfile;
        public ICommand BackProfile
        {
            get
            {
                return backProfile ??
                                       (backProfile = new RelayCommandAsync(BackToUserProfile, (o) => true));
            }
        }

        public async Task BackToUserProfile()
        {
            Reset();
            await Initialize();
        }

        private bool isFriendProfile;
        public string IsFriendProfile
        {
            get => isFriendProfile ? "Visible" : "Hidden";
        }

        private ICommand changeProfilePictureCommand;
        public ICommand ChangeProfilePictureCommand
        {
            get
            {
                return changeProfilePictureCommand ?? (changeProfilePictureCommand = new RelayCommandAsync(ChangeProfilePicture));
            }
        }

        private async Task ChangeProfilePicture()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    System.Diagnostics.Debug.WriteLine(dlg.FileName);
                    Image img = Image.FromFile(dlg.FileName);
                    using (MemoryStream m = new MemoryStream())
                    {
                        img.Save(m, img.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        if (base64String.Length <= 65535)
                        {
                            UserEntity uE = new UserEntity { Profile = base64String };
                            var response = await Program.client.PutAsJsonAsync(Program.client.BaseAddress + "api/user/" + User.Instance.UserEntity.Id.ToString(), uE);
                            if (response.IsSuccessStatusCode)
                            {
                                ProfilePicture = base64String;
                            }
                        }
                        else
                        {
                            MessageBox.Show("La taille maximale de l'image est dépassée!", "Image", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
        }
    }
}
