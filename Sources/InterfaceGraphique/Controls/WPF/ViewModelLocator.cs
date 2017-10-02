using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Chat;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF
{
    public class ViewModelLocator
    {
        private static ViewModelBase instance;

        private UnityContainer container;

        public ViewModelLocator()
        {
            container = Program.unityContainer;
        }

        public ChatViewModel ChatViewModel
        {
            get { return container.Resolve<ChatViewModel>(); }
        }
    }
}
