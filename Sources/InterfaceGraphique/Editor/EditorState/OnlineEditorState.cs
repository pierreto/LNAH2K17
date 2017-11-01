using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Entities.EditonCommand;
using InterfaceGraphique.Entities.Editor;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Editor.EditorState
{
    public class OnlineEditorState : AbstractEditorState
    {
        private EditionHub editionHub;
        private FonctionsNatives.PortalCreationCallback portalCreationCallback;
        private FonctionsNatives.WallCreationCallback wallCreationCallback;
        private FonctionsNatives.BoostCreationCallback boostCreationCallback;
        private FonctionsNatives.MoveEventCallback moveEventCallback;
        private FonctionsNatives.SelectionEventCallback selectionEventCallback;


        public OnlineEditorState(EditionHub editionHub)
        {
            this.editionHub = editionHub;

            this.editionHub.NewCommand += OnNewCommand;
            this.editionHub.NewUser += OnNewUser;
            this.editionHub.UserLeft += OnUserLeft;



            this.portalCreationCallback = CurrentUserCreatedPortal;
            this.wallCreationCallback = CurrentUserCreatedWall;
            this.boostCreationCallback = CurrentUserCreatedBoost;
            this.moveEventCallback = CurrentUserMovedObject;
            this.selectionEventCallback = CurrentUserSelectedObject;

        }


        private void OnUserLeft(string username)
        {
            FonctionsNatives.removeUser(username.ToCharArray());

        }

        private void OnNewUser(OnlineUser user)
        {
            FonctionsNatives.addNewUser(user.Username.ToCharArray(),user.HexColor.ToCharArray());
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
            FonctionsNatives.setWallCreationCallback(this.wallCreationCallback);
            FonctionsNatives.setBoostCreationCallback(this.boostCreationCallback);
            FonctionsNatives.setMoveEventCallback(this.moveEventCallback);
            FonctionsNatives.setSelectionEventCallback(this.selectionEventCallback);

        }
        public override void LeaveEdition()
        {
            this.editionHub.LeaveRoom();
        }

        private void OnNewCommand(AbstractEditionCommand editionCommand)
        {
            editionCommand.ExecuteCommand();
        }

        private void CurrentUserCreatedPortal(string startUuid, IntPtr startPos, string endUuid, IntPtr endPos)
        {
            float[] startVec= getVec3FromIntptr(startPos);

            float[] endVec = getVec3FromIntptr(endPos);

            PortalCommand portalCommand = new PortalCommand(startUuid)
            {
                EndUuid = endUuid,
                StartPosition = startVec,
                EndPosition = endVec
            };
            this.editionHub.SendEditorCommand(portalCommand);
        }

     
        private void CurrentUserCreatedWall(string uuid,IntPtr startPos, IntPtr endPos)
        {
            float[] startVec = getVec3FromIntptr(startPos);
            float[] endVec = getVec3FromIntptr(endPos);

            WallCommand wallCommand = new WallCommand(uuid)
            {
                StartPosition = startVec,
                EndPosition = endVec
            };

            this.editionHub.SendEditorCommand(wallCommand);
        }
        private void CurrentUserCreatedBoost(string uuid, IntPtr startpos)
        {
            float[] vec = getVec3FromIntptr(startpos);

            BoostCommand boostCommand = new BoostCommand(uuid)
            {
                Position = vec,
            };

            this.editionHub.SendEditorCommand(boostCommand);
        }
        private void CurrentUserMovedObject(string uuid, IntPtr pos)
        {
            float[] vec = getVec3FromIntptr(pos);
         
            this.editionHub.SendEditorCommand(new MoveCommand(uuid)
            {
                Position = vec
            });

        }
        private void CurrentUserSelectedObject(string username, string uuidselected)
        {
            this.editionHub.SendEditorCommand(new SelectionCommand(uuidselected)
            {
                Username = username
            });
        }
        private float[] getVec3FromIntptr(IntPtr ptr)
        {
            float[] vec3 = new float[3];
            Marshal.Copy(ptr, vec3, 0, 3);
            return vec3;
        }


    
    }
}
