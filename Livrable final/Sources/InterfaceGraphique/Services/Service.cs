using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.MainMenu;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Services
{
    public class Service
    {
        protected async Task OnException()
        {
            Program.FormManager.CurrentForm = Program.HomeMenu;
            await HubManager.Instance.LeaveHubs();
        }
    }
}
