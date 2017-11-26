using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Timers;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.MainMenu;

namespace InterfaceGraphique.Controls.WPF.Store
{
    public class StoreViewModel : ViewModelBase
    {
        public StoreService StoreService { get; }
        public PlayerStatsService PlayerStatsService { get; }

        public StoreViewModel(StoreService storeService, PlayerStatsService playerStatsService)
        {
            this.StoreItems = new ObservableCollection<ItemViewModel>();
            StoreService = storeService;
            PlayerStatsService = playerStatsService;
            notEnoughPoints = false;

            OnPropertyChanged("NotEnoughtPoints");
        }

        public override void InitializeViewModel()
        {
            //
        }

        public async Task Initialize()
        {
            Load();
            var items = await StoreService.GetStoreItems();
            foreach(var item in items)
            {
                StoreItems.Add(new ItemViewModel(item, User.Instance.Inventory.Find(x => x.Id == item.Id) == null));
            }

            var stats = await PlayerStatsService.GetPlayerStats(User.Instance.UserEntity.Id);
            if(stats == null)
            {
                Points = 0;
            }
            else
            {
                Points = stats.Points;
            }

            PlayerName = User.Instance.UserEntity.Username;
            LoadingDone();
        }

        private void LoadingDone()
        {
            NotLoading = true;
        }

        private void Load()
        {
            NotLoading = false;
        }

        private bool notLoading;
        public bool NotLoading
        {
            get { return notLoading; }

            set
            {
                if (notLoading == value)
                {
                    return;
                }
                notLoading = value;
                this.OnPropertyChanged(nameof(NotLoading));
                this.OnPropertyChanged(nameof(Loading));
            }
        }

        public bool Loading
        {
            get { return !notLoading; }
        }

        private ObservableCollection<ItemViewModel> storeItems;
        public ObservableCollection<ItemViewModel> StoreItems
        {
            get => storeItems;
            set
            {
                storeItems = value;
                OnPropertyChanged();
            }
        }

        private ItemViewModel focusItem;
        public ItemViewModel FocusItem
        {
            get => focusItem;
            set
            {
                focusItem = value;
                OnPropertyChanged();
            }
        }
        
        private ICommand buy;
        public ICommand Buy
        {
            get
            {
                return buy ??
                       (buy = new RelayCommandAsync(BuyItems, (o) => true));
            }
        }

        private int points;
        public int Points
        {
            get => points;
            set
            {
                points = value;
                OnPropertyChanged();
            }
        }

        private async Task BuyItems()
        {
            if(TotalPrice <= Points)
            {
                await StoreService.BuyElements(StoreItems.Where(x => x.IsChecked && x.CanBuy).Select(x => x.StoreItem).ToList(), User.Instance.UserEntity.Id);
                Points -= StoreItems.Where(x => x.IsChecked).Sum(x => x.Price);

                User.Instance.Inventory = await StoreService.GetUserStoreItems(User.Instance.UserEntity.Id);

                foreach(var item in StoreItems)
                {
                    if(User.Instance.Inventory.Any(x => x.Id == item.Id))
                    {
                        item.CanBuy = false;
                    }
                }

                EmptyCart();

                purchaseMsg = true;
                OnPropertyChanged("purchaseMsg");
                OnPropertyChanged("DisableStore");
            }
            else
            {
                notEnoughPoints = true;

                Timer timer = new Timer();
                timer.Interval = 3000;
                timer.Elapsed += (timerSender, e) => ErrorNotice(timerSender, e, timer, "NotEnoughPoints");

                OnPropertyChanged("NotEnoughPoints");
            }
        }

        private void ErrorNotice(object timerSender, ElapsedEventArgs e, System.Timers.Timer timer, string property)
        {
            timer.Stop();
            notEnoughPoints = false;
            OnPropertyChanged(property);
        }

        private ICommand reset;
        public ICommand Reset
        {
            get
            {
                return reset ??
                       (reset = new RelayCommand(EmptyCart));
            }
        }

        private void EmptyCart()
        {
            foreach(var item in StoreItems)
            {
                item.IsChecked = false;
            }
            OnPropertyChanged("StoreItems");
            TotalPrice = 0;
        }

        private ICommand leave;
        public ICommand Leave
        {
            get
            {
                return leave ??
                       (leave = new RelayCommand(BackMainMenu));
            }
        }
        
        private void BackMainMenu()
        {
            EmptyCart();
            Program.FormManager.CurrentForm = Program.HomeMenu;
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
        }

        private ICommand clickCommand;
        public ICommand ClickCommand
        {
            get { return clickCommand ?? (clickCommand = new DelegateCommand<ItemViewModel>(DoSelect)); }
        }

        private void DoSelect(ItemViewModel item)
        {
            if(item.CanBuy)
            {
                item.IsChecked = item.IsChecked ? false : true;
                TotalPrice = item.IsChecked ? TotalPrice + item.Price : TotalPrice - item.Price;
            }
        }

        private bool notEnoughPoints;
        public string NotEnoughPointsError
        {
            get => notEnoughPoints ? "Visible" : "Hidden";
        }

        private int totalPrice;
        public int TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                notEnoughPoints = TotalPrice > points ? true : false;
                OnPropertyChanged("NotEnoughPointsError");
                CartItemsNumber = storeItems.Count(x => x.IsChecked);
                OnPropertyChanged();
            }
        }

        private int cartItemsNumber;
        public int CartItemsNumber
        {
            get => cartItemsNumber;
            set
            {
                cartItemsNumber = value;
                OnPropertyChanged();
            }
        }

        private string playerName;
        public string PlayerName
        {
            get => playerName;
            set
            {
                playerName = value;
                OnPropertyChanged();
            }
        }

        public bool purchaseMsg = false;
        public string PurchaseMsg
        {
            get => purchaseMsg ? "Visible" : "Hidden";
        }

        public bool DisableStore
        {
            get => !purchaseMsg;
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
            purchaseMsg = false;
            OnPropertyChanged("purchaseMsg");
            OnPropertyChanged("DisableStore");
        }

        private ICommand shareOnFacebookCommand;
        public ICommand ShareOnFacebookCommand
        {
            get
            {
                return shareOnFacebookCommand ?? (shareOnFacebookCommand = new RelayCommand(ShareOnFacebook));
            }
        }
        private void ShareOnFacebook()
        {
            Browser browser = new Browser();
            browser.Show();
            browser.webBrowser.Navigate("" +
                "https://www.facebook.com/dialog/feed?" +
                "app_id=143581339623947" +
                "&display=popup" +
                "&link=http://tcpc.isomorphis.me/store.html");

            HidePopup();
        }
    }
}
