﻿namespace InterfaceGraphique.Controls
{
    partial class OnlineTournament
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.onlineTournamentView1 = new InterfaceGraphique.Controls.WPF.Tournament.TournamentView();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.elementHost1.Location = new System.Drawing.Point(2, -2);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1181, 649);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.onlineTournamentView1;
            // 
            // LobbyHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1184, 648);
            this.Controls.Add(this.elementHost1);
            this.Name = "LobbyHost";
            this.Text = "LobbyHost";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private WPF.Tournament.TournamentView onlineTournamentView1;
    }
}