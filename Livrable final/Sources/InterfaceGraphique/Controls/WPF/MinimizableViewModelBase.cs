using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF
{
    public abstract class MinimizableViewModelBase : ViewModelBase
    {
        private Visibility collapsed;
        private string tabIcon;

        private ICommand minimizeCommand;

        public abstract override void InitializeViewModel();


        public ICommand MinimizeCommand
        {
            get
            {
                if (minimizeCommand == null)
                {
                    minimizeCommand = new RelayCommand(Minimize);

                }
                return minimizeCommand;
            }
        }
        public Visibility Collapsed
        {
            get
            {
                return collapsed;
            }
            set
            {
                collapsed = value;
                OnPropertyChanged(nameof(Collapsed));
            }
        }
   
        public string TabIcon
        {
            get
            {
                return tabIcon;
            }
            set
            {
                tabIcon = value;
                OnPropertyChanged(nameof(TabIcon));
            }
        }

        public abstract void Minimize();
    }
}
