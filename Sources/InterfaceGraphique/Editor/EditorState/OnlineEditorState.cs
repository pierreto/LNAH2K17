using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Entities.EditonCommand;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Editor.EditorState
{
    public class OnlineEditorState : AbstractEditorState
    {
        private EditionHub editionHub;
        private FonctionsNatives.PortalCreationCallback portalCreationCallback;
        public OnlineEditorState(EditionHub editionHub)
        {
            this.editionHub = editionHub;
            this.editionHub.NewCommand += OnNewCommand;
            this.portalCreationCallback = CurrentUserCreatedPortal;

        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            FonctionsNatives.modifierKeys((Control.ModifierKeys == Keys.Alt), (Control.ModifierKeys == Keys.Control));
            if (e.Button == MouseButtons.Left)
            {
                FonctionsNatives.mouseUpL();
                Program.Editeur.EditionSupprimer.Enabled = FonctionsNatives.verifierSelection();
                Program.Editeur.resetProprietesPanel(null, null);
            }
            if (e.Button == MouseButtons.Right)
            {
                FonctionsNatives.mouseUpR();
            }
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            FonctionsNatives.modifierKeys((Control.ModifierKeys == Keys.Alt), (Control.ModifierKeys == Keys.Control));
            if (e.Button == MouseButtons.Left)
            {
                FonctionsNatives.mouseDownL();
            }
            if (e.Button == MouseButtons.Right)
            {
                FonctionsNatives.mouseDownR();
            }
        }

        public override void JoinEdition(MapEntity mapEntity)
        {
            this.editionHub.JoinPublicRoom(mapEntity);
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.ONLINE_EDITION);
            FonctionsNatives.setPortalCreationCallback(this.portalCreationCallback);

        }

        private void OnNewCommand(AbstractEditionCommand editionCommand)
        {
            editionCommand.ExecuteCommand();
        }

        private void CurrentUserCreatedPortal(char[] startUuid, float[] startPos, char[] endUuid, float[] endPos)
        {
            PortalCommand portalCommand = new PortalCommand(startUuid.ToString())
            {
                StartPosition = startPos,
                EndPosition = endPos
            };
            this.editionHub.SendEditorCommand(portalCommand);

        }
    }
}
