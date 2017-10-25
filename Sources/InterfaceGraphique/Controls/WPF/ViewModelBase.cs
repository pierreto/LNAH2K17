using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Controls.WPF
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public abstract  void InitializeViewModel();
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName()] string name = null)
        {
            if (name != null) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
