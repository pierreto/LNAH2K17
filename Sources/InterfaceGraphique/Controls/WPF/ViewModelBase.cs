using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Visibility { get; set; }

        public ViewModelBase()
        {
            Visibility = "Visible";
            Title = "";
        }
        private ICommand backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new RelayCommand(GoBack);
                }
                return backCommand;
            }
        }

        protected virtual void GoBack() { }

        public abstract void InitializeViewModel();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName()] string name = null)
        {
            if (name != null) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
