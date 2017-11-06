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
using InterfaceGraphique.Entities.Editor.EditonCommand;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Editor.EditorState
{
    public class OnlineEditorState : AbstractEditorState
    {
        private EditionHub editionHub;
        private FonctionsNatives.PortalCreationCallback portalCreationCallback;
        private FonctionsNatives.WallCreationCallback wallCreationCallback;
        private FonctionsNatives.BoostCreationCallback boostCreationCallback;
        private FonctionsNatives.TransformEventCallback _transformEventCallback;
        private FonctionsNatives.SelectionEventCallback selectionEventCallback;
        private FonctionsNatives.ControlPointEventCallback controlPoinEventCallback;



        public OnlineEditorState(EditionHub editionHub)
        {
            this.editionHub = editionHub;

            this.editionHub.NewCommand += OnNewCommand;
            this.editionHub.NewUser += OnNewUser;
            this.editionHub.UserLeft += OnUserLeft;



            this.portalCreationCallback = CurrentUserCreatedPortal;
            this.wallCreationCallback = CurrentUserCreatedWall;
            this.boostCreationCallback = CurrentUserCreatedBoost;
            this._transformEventCallback = CurrentUserObjectTransformChanged;
            this.selectionEventCallback = CurrentUserSelectedObject;
            this.controlPoinEventCallback = CurrentUserChangedControlPoint;

        }




        private void OnUserLeft(string username)
        {
            FonctionsNatives.removeUser(username);

        }

        private void OnNewUser(OnlineUser user)
        {
            FonctionsNatives.addNewUser(user.Username,user.HexColor);
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

        public override async void JoinEdition(MapEntity mapEntity)
        {
            FonctionsNatives.clearUsers();

            FonctionsNatives.setOnlineClientType((int)OnlineClientType.ONLINE_EDITION);
            FonctionsNatives.setPortalCreationCallback(this.portalCreationCallback);
            FonctionsNatives.setWallCreationCallback(this.wallCreationCallback);
            FonctionsNatives.setBoostCreationCallback(this.boostCreationCallback);
            FonctionsNatives.setTransformEventCallback(this._transformEventCallback);
            FonctionsNatives.setSelectionEventCallback(this.selectionEventCallback);
            FonctionsNatives.setControlPointEventCallback(this.controlPoinEventCallback);

            List<OnlineUser> usersInTheGame = await this.editionHub.JoinPublicRoom(mapEntity);
            foreach (OnlineUser user in usersInTheGame)
            {
                if (user.Username.Equals(User.Instance.UserEntity.Username))
                {
                    FonctionsNatives.setCurrentPlayerSelectionColor(user.HexColor);
                }
                else
                {
                    FonctionsNatives.addNewUser(user.Username,user.HexColor);
                }
            }
        }
        public override async Task LeaveEdition()
        {
            await this.editionHub.LeaveRoom();
        }

        private void OnNewCommand(AbstractEditionCommand editionCommand)
        {
            editionCommand.ExecuteCommand();
        }

        private void CurrentUserCreatedPortal(string startUuid, IntPtr startPos, float startRotation, IntPtr startScale, string endUuid, IntPtr endPosition, float endRotation, IntPtr endScale)
        {

            PortalCommand portalCommand = new PortalCommand(startUuid)
            {
                EndUuid = endUuid,

                StartPosition = getVec3FromIntptr(startPos),
                StartRotation = startRotation,
                StartScale = getVec3FromIntptr(startScale),

                EndPosition = getVec3FromIntptr(endPosition),
                EndRotation = endRotation,
                EndScale = getVec3FromIntptr(endScale)
            };
            this.editionHub.SendEditorCommand(portalCommand);
        }

     
        private void CurrentUserCreatedWall(string uuid,IntPtr pos, float rotation, IntPtr scale)
        {
            float[] posVec = getVec3FromIntptr(pos);
            float[] scaleVec = getVec3FromIntptr(scale);

            WallCommand wallCommand = new WallCommand(uuid)
            {
                Position = posVec,
                Rotation = rotation,
                Scale = scaleVec
            };

            this.editionHub.SendEditorCommand(wallCommand);
        }
        private void CurrentUserCreatedBoost(string uuid, IntPtr startpos, float rotation, IntPtr scale)
        {

            BoostCommand boostCommand = new BoostCommand(uuid)
            {
                Position = getVec3FromIntptr(startpos),
                Rotation = rotation,
                Scale = getVec3FromIntptr(scale)

            };

            this.editionHub.SendEditorCommand(boostCommand);
        }
        private void CurrentUserObjectTransformChanged(string uuid, IntPtr pos, float rotation, IntPtr scale)
        {
            float[] posVec = getVec3FromIntptr(pos);

            float[] scaleVec = getVec3FromIntptr(scale);


            this.editionHub.SendEditorCommand(new TransformCommand(uuid)
            {
                Username = User.Instance.UserEntity.Username,
                Position = posVec,
                Rotation = rotation,
                Scale = scaleVec
            });

        }
        private void CurrentUserSelectedObject(string uuidselected, bool isSelected, bool deselectAll)
        {
            this.editionHub.SendEditorCommand(new SelectionCommand(uuidselected)
            {
                Username = User.Instance.UserEntity.Username,
                IsSelected = isSelected,
                DeselectAll = deselectAll 
            });
        }

        private void CurrentUserChangedControlPoint(string uuid, IntPtr position)
        {
            float[] positionVec = getVec3FromIntptr(position);
            this.editionHub.SendEditorCommand(new ControlPointCommand(uuid)
            {
                Username = User.Instance.UserEntity.Username,
                Position = positionVec
            });
        }
        private float[] getVec3FromIntptr(IntPtr ptr)
        {
            float[] vec3 = new float[3];
            Marshal.Copy(ptr, vec3, 0, 3);
            return vec3;
        }

        private float[] getTransformMatrixFromIntptr(IntPtr ptr)
        {
            float[] vec3 = new float[16];
            Marshal.Copy(ptr, vec3, 0, 16);
            return vec3;
        }



    }
}
