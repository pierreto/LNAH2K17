using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Editor.EditorState
{
    public abstract class AbstractEditorState
    {
        public abstract void MouseUp(object sender, MouseEventArgs e);
        public abstract void MouseDown(object sender, MouseEventArgs e);

        public abstract void JoinEdition(MapEntity mapEntity);
        public abstract Task LeaveEdition();
    }
}
