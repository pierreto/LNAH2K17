using System.Windows.Forms;
using InterfaceGraphique.Controls.WPF.Tutorial;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls
{
    public partial class TutorialHost : Form
    {
        public TutorialHost()
        {
            InitializeComponent();
            //this.elementHost1.Child = Program.unityContainer.Resolve<TutorialView>();
        }
    }
}
