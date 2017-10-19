using InterfaceGraphique.Controls.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace InterfaceGraphique.Entities
{
    public class MapEntity : BindableBase
    {
        public string Creator { get; set; }
        public string MapName { get; set; }
        public DateTime LastBackup { get; set; }
        public string Json { get; set; }
    }
}
