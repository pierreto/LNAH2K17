﻿using System;
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

            /*if (Editeur.mapManager.CurrentMapAlreadySaved())
                Task.Run(() => Editeur.mapManager.SaveMap());*/
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
            //Online is not working in offline mode so we do nothing
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.OFFLINE_EDITION);
            FonctionsNatives.setCurrentPlayerSelectionColorToDefault();
        }

        public override void Escape()
        {

        }

        public override Task LeaveEdition()
        {
            //Nothing
            return Task.FromResult(0);
        }

        public override void HandleCoefficientChanges(float coefficientFriction, float coefficientAcceleration, float coefficientRebond)
        {
        }
    }
}
