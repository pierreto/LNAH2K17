using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows.Forms;
using System.Diagnostics;

namespace InterfaceGraphique.CommunicationInterface
{
    public class BaseHub
    {
        public virtual void HandleError(string className = "")
        {
            Program.unityContainer.Resolve<HubManager>().HandleDisconnection();
        }
    }
}
