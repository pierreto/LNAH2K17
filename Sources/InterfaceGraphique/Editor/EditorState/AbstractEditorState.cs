using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique.Editor.EditorState
{
    public abstract class AbstractEditorState
    {
        public abstract void mouseUp(object sender, MouseEventArgs e);
        public abstract void mouseDown(object sender, MouseEventArgs e);
    }
}
