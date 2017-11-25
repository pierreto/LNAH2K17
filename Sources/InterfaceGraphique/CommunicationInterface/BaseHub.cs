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
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var cls = mth.ReflectedType.Name;
            Program.FormManager.CurrentForm = Program.HomeMenu;
            MessageBox.Show(
               @"Le lien entre vous et le serveur s'est brisé. Classe: "+ className + ". Vérifiez votre connection internet. Sinon ce peut être dû à une catastrophe naturelle, des chargés de laboratoires ou autre",
               @"Catastrophe",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
