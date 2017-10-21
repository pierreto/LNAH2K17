using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class EditorViewModel
    {
        private ICommand publicModeCommand;
        private ICommand privateModeCommand;

        public ICommand PublicModeCommand
        {
            get
            {
                return publicModeCommand ??
                       (publicModeCommand = new RelayCommandAsync(ChangeViewToPublicEditor, (o) => true));
            }
        }
        private async Task ChangeViewToPublicEditor()
        {
            //Program.FormManager.CurrentForm = Program.Editeur;
        }




        public ICommand PrivateModeCommand
        {
            get
            {
                return privateModeCommand ??
                       (privateModeCommand = new RelayCommandAsync(ChangeViewToPrivateEditor, (o) => true));
            }
        }
   
        private async Task ChangeViewToPrivateEditor()
        {
            Program.FormManager.CurrentForm = Program.Editeur;
        }

    }
}
