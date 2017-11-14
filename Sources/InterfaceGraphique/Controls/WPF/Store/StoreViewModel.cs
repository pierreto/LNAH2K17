using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
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
            this.StoreItems = new List<StoreItemEntity>();
            StoreService = storeService;
        }

        public override void InitializeViewModel()
        {
            //
        }

        public async Task Initialize()
        {
            StoreItems = await StoreService.GetStoreItems(); 

        }

        private List<StoreItemEntity> storeItems;
        public List<StoreItemEntity> StoreItems
        {
            get => storeItems;
            set
            {
                storeItems = value;
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
            await StoreService.BuyElements(StoreItems.Where(x => x.IsChecked).ToList(), User.Instance.UserEntity.Id);
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
    }
}
