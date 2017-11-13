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
            this.StoreItems = new List<StoreItemEntity>()
            {
                new StoreItemEntity
                {
                    Name = "Name1",
                    Description = "Desc1",
                    Price = 5
                },
                new StoreItemEntity
                {
                    Name = "Name2",
                    Description = "Desc2",
                    Price = 4
                },
                new StoreItemEntity
                {
                    Name = "Name3",
                    Description = "Desc3",
                    Price = 8
                },
                new StoreItemEntity
                {
                    Name = "Name4",
                    Description = "Desc4",
                    Price = 9
                },
                new StoreItemEntity
                {
                    Name = "Name5",
                    Description = "Desc5",
                    Price = 15
                }
            };
            StoreService = storeService;
        }

        public override void InitializeViewModel()
        {
            //
        }

        public void Initialize()
        {
            
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
            await StoreService.BuyElements(StoreItems.Where(x => x.IsChecked).ToList());
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
