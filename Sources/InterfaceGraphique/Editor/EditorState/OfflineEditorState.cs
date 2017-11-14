using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Editor.EditorState 
{
    public class OfflineEditorState : AbstractEditorState
    {
        public OfflineEditorState()
        {
            this.InitializeCallbacks();
        }

        public override void JoinEdition(MapEntity mapEntity)
        {
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.OFFLINE_EDITION);
            FonctionsNatives.setCurrentPlayerSelectionColorToDefault();
            //FonctionsNatives.clearUsers();

            this.SetCallbacks();
        }

        protected override void SaveMap()
        {
            if (Editeur.mapManager.CurrentMapAlreadySaved())
                Task.Run(() =>
                    Editeur.mapManager.SaveMap()
                    );
        }

        protected override void CurrentUserCreatedPortal(string startUuid, IntPtr startPos, float startRotation, IntPtr startScale, string endUuid, IntPtr endPosition, float endRotation, IntPtr endScale)
        {
            SaveMap();
        }

        protected override void CurrentUserCreatedWall(string uuid, IntPtr pos, float rotation, IntPtr scale)
        {
            SaveMap();
        }

        protected override void CurrentUserCreatedBoost(string uuid, IntPtr startpos, float rotation, IntPtr scale)
        {
            SaveMap();
        }

        protected override void CurrentUserObjectTransformChanged(string uuid, IntPtr pos, float rotation, IntPtr scale)
        {
            this.inTransformation = true;
        }

        protected override void CurrentUserSelectedObject(string uuidselected, bool isSelected, bool deselectAll)
        {
            // Do nothing.
        }

        protected override void CurrentUserChangedControlPoint(string uuid, IntPtr position)
        {
            SaveMap();
        }

        protected override void CurrentUserDeletedNode(string uuid)
        {
            SaveMap();
        }

        public override Task LeaveEdition()
        {
            // Nothing
            return Task.FromResult(0);
        }

        public override void HandleCoefficientChanges(float coefficientFriction, float coefficientAcceleration, float coefficientRebond)
        {
            // Do nothing.
        }
    }
}
