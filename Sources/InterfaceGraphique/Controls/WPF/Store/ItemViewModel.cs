using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Controls.WPF.Store
{
    public class ItemViewModel : ViewModelBase
    {
        public ItemViewModel(StoreItemEntity item, bool available)
        {
            Name = item.Name;
            IsChecked = false;
            Price = item.Price;
            Description = item.Description;
            ImageUrl = item.ImageUrl;
            StoreItem = item;
            CanBuy = available;
            Id = item.Id;
            IsGameEnabled = item.IsGameEnabled;
        }
        
        public StoreItemEntity StoreItem { get; set; }

        private string name;
        public string Name
        {
            get => name ?? "";
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }

        private int price;
        public int Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get => imageUrl ?? "";
            set
            {
                imageUrl = value;
                OnPropertyChanged();
            }
        }

        private string description;
        public string Description
        {
            get => description ?? "";
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        private bool canBuy;
        public bool CanBuy
        {
            get => canBuy;
            set
            {
                canBuy = value;
                OnPropertyChanged();
            }
        }

        private bool isGameEnabled;
        public bool IsGameEnabled
        {
            get => isGameEnabled;
            set
            {
                isGameEnabled = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }

        public override void InitializeViewModel()
        {
        }
    }
}
