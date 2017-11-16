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

namespace InterfaceGraphique.Controls.WPF.Store
{
    public class StoreViewModel : ViewModelBase
    {
        public StoreService StoreService { get; }

        public StoreViewModel(StoreService storeService)
        {
            this.StoreItems = new ObservableCollection<ItemViewModel>();
            StoreService = storeService;
        }

        public override void InitializeViewModel()
        {
            //
        }

        public async Task Initialize()
        {
            var items = await StoreService.GetStoreItems();
            foreach(var item in items)
            {
                StoreItems.Add(new ItemViewModel(item));
            }
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

        private async Task BuyItems()
        {
            await StoreService.BuyElements(StoreItems.Where(x => x.IsChecked && x.CanBuy).Select(x => x.StoreItem).ToList(), User.Instance.UserEntity.Id);
            EmptyCart();
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
            Program.FormManager.CurrentForm = Program.MainMenu;
        }

        private ICommand clickCommand;
        public ICommand ClickCommand
        {
            get { return clickCommand ?? (clickCommand = new DelegateCommand<ItemViewModel>(DoSelect)); }
        }

        private void DoSelect(ItemViewModel item)
        {
            FocusItem = item;
        }
    }
}
