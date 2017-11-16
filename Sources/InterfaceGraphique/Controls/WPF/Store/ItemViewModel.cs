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
        public ItemViewModel(StoreItemEntity item)
        {
            Name = item.Name;
            IsChecked = false;
            Price = item.Price;
            Description = item.Description;
            ImageUrl = Directory.GetCurrentDirectory() + "\\media\\image\\default_profile_picture.png";
            StoreItem = item;
            CanBuy = true;
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

        public override void InitializeViewModel()
        {
        }
    }
}
