using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        public OnlineEditorState(EditionHub editionHub)
        {
            this.editionHub = editionHub;

            this.InitializeCallbacks();

            this.editionHub.NewCommand += OnNewCommand;
            this.editionHub.NewUser += OnNewUser;
            this.editionHub.UserLeft += OnUserLeft;
        }

        private void OnUserLeft(string username)
        {
            FonctionsNatives.removeUser(username);
            Task.Run(() => Editeur.mapManager.SaveMap());
        }

        private void OnNewUser(OnlineUser user)
        {
            FonctionsNatives.addNewUser(user.Username,user.HexColor);
        }

        public override async void JoinEdition(MapEntity mapEntity)
        {
            FonctionsNatives.clearUsers();
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.ONLINE_EDITION);
            this.SetCallbacks();

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
                    if (user.UuidsSelected != null)
                    {
                        foreach (string uuidSelected in user.UuidsSelected)
                        {
                            FonctionsNatives.setElementSelection(user.Username, uuidSelected, true, false);
                        }
                    }
                   
                }
            }
        }

        public override async Task LeaveEdition()
        {
            await this.editionHub.LeaveRoom();
        }

        public override void HandleCoefficientChanges(float coefficientFriction, float coefficientAcceleration, float coefficientRebond)
        {
            this.editionHub.SendEditorCommand(new CoefficientCommand()
            {
                accCoeff = coefficientAcceleration,
                frictionCoeff = coefficientFriction,
                reboundCoeff = coefficientRebond
            });
        }

        private void OnNewCommand(AbstractEditionCommand editionCommand)
        {
            editionCommand.ExecuteCommand();
        }

        protected override void SaveMap()
        {
            Task.Run(() =>
                Editeur.mapManager.SaveMap()
            );
        }

        protected override void CurrentUserCreatedPortal(string startUuid, IntPtr startPos, float startRotation, IntPtr startScale, string endUuid, IntPtr endPosition, float endRotation, IntPtr endScale)
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
            this.SaveMap();
        }
     
        protected override void CurrentUserCreatedWall(string uuid,IntPtr pos, float rotation, IntPtr scale)
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
            this.SaveMap();
        }

        protected override void CurrentUserCreatedBoost(string uuid, IntPtr startpos, float rotation, IntPtr scale)
        {

            BoostCommand boostCommand = new BoostCommand(uuid)
            {
                Position = getVec3FromIntptr(startpos),
                Rotation = rotation,
                Scale = getVec3FromIntptr(scale)

            };

            this.editionHub.SendEditorCommand(boostCommand);
            this.SaveMap();
        }

        protected override void CurrentUserObjectTransformChanged(string uuid, IntPtr pos, float rotation, IntPtr scale)
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
            this.inTransformation = true;
        }

        protected override void CurrentUserSelectedObject(string uuidselected, bool isSelected, bool deselectAll)
        {
            this.editionHub.SendSelectionCommand(new SelectionCommand(uuidselected)
            {
                Username = User.Instance.UserEntity.Username,
                IsSelected = isSelected,
                DeselectAll = deselectAll 
            });
        }

        protected override void CurrentUserChangedControlPoint(string uuid, IntPtr position)
        {
            float[] positionVec = getVec3FromIntptr(position);

            this.editionHub.SendEditorCommand(new ControlPointCommand(uuid)
            {
                Username = User.Instance.UserEntity.Username,
                Position = positionVec
            });
            this.SaveMap();
        }

        private float[] getVec3FromIntptr(IntPtr ptr)
        {
            float[] vec3 = new float[3];
            Marshal.Copy(ptr, vec3, 0, 3);
            return vec3;
        }

        protected override void CurrentUserDeletedNode(string uuid)
        {
            this.editionHub.SendEditorCommand(new DeleteCommand(uuid)
            {
                Username = User.Instance.UserEntity.Username,
            });
            this.SaveMap();
        }

    }
}