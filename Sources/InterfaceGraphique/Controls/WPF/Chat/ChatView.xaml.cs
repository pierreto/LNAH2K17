using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Menus;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;
using UserControl = System.Windows.Controls.UserControl;

namespace InterfaceGraphique.Controls
{
    /// <summary>
    /// Logique d'interaction pour WPFChatView.xaml
    /// </summary>
    public partial class WPFChatView : UserControl
    {


        public WPFChatView()
        {
            InitializeComponent();


        }





    }
}
public class User
{
    public string Name { get; set; }
    public string ID { get; set; }
    public byte[] Photo { get; set; }
}


public class Messages
{
    public IList<ChatMessage> ListOfItems { get; set; }

    public Messages()
    {
        ListOfItems = new List<ChatMessage>();
        ListOfItems.Add(new ChatMessage("asdf","asdfasd",System.DateTime.Now));
        //ListOfItems.Add("Two");
       // ListOfItems.Add("Three");
    }

}
