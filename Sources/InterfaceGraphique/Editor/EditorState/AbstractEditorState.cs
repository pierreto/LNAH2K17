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
        protected FonctionsNatives.PortalCreationCallback portalCreationCallback;
        protected FonctionsNatives.WallCreationCallback wallCreationCallback;
        protected FonctionsNatives.BoostCreationCallback boostCreationCallback;
        protected FonctionsNatives.TransformEventCallback _transformEventCallback;
        protected FonctionsNatives.SelectionEventCallback selectionEventCallback;
        protected FonctionsNatives.ControlPointEventCallback controlPoinEventCallback;
        protected FonctionsNatives.DeleteEventCallback deleteEventCallback;

        protected void InitializeCallbacks()
        {
            this.portalCreationCallback = this.CurrentUserCreatedPortal;
            this.wallCreationCallback = this.CurrentUserCreatedWall;
            this.boostCreationCallback = this.CurrentUserCreatedBoost;
            this._transformEventCallback = this.CurrentUserObjectTransformChanged;
            this.selectionEventCallback = this.CurrentUserSelectedObject;
            this.controlPoinEventCallback = this.CurrentUserChangedControlPoint;
            this.deleteEventCallback = this.CurrentUserDeletedNode;
        }

        protected void SetCallbacks()
        {
            FonctionsNatives.setPortalCreationCallback(this.portalCreationCallback);
            FonctionsNatives.setWallCreationCallback(this.wallCreationCallback);
            FonctionsNatives.setBoostCreationCallback(this.boostCreationCallback);
            FonctionsNatives.setTransformEventCallback(this._transformEventCallback);
            FonctionsNatives.setSelectionEventCallback(this.selectionEventCallback);
            FonctionsNatives.setControlPointEventCallback(this.controlPoinEventCallback);
            FonctionsNatives.setDeleteEventCallback(this.deleteEventCallback);
        }

        protected abstract void CurrentUserCreatedPortal(string startUuid, IntPtr startPos, float startRotation, IntPtr startScale, string endUuid, IntPtr endPosition, float endRotation, IntPtr endScale);
        protected abstract void CurrentUserCreatedWall(string uuid, IntPtr pos, float rotation, IntPtr scale);
        protected abstract void CurrentUserCreatedBoost(string uuid, IntPtr startpos, float rotation, IntPtr scale);
        protected abstract void CurrentUserObjectTransformChanged(string uuid, IntPtr pos, float rotation, IntPtr scale);
        protected abstract void CurrentUserSelectedObject(string uuidselected, bool isSelected, bool deselectAll);
        protected abstract void CurrentUserChangedControlPoint(string uuid, IntPtr position);
        protected abstract void CurrentUserDeletedNode(string uuid);

        protected bool inTransformation;
        protected abstract void SaveMap();

        public void MouseUp(object sender, MouseEventArgs e)
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

            if (this.inTransformation)
            {
                this.SaveMap();
                this.inTransformation = false;
            }
        }

        public void MouseDown(object sender, MouseEventArgs e)
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

        public void Escape()
        {
            this.inTransformation = false;
        }

        public abstract void JoinEdition(MapEntity mapEntity=null);
        public abstract Task LeaveEdition();

        public abstract void HandleCoefficientChanges(float coefficientFriction, float coefficientAcceleration, float coefficientRebond);
    }
}
