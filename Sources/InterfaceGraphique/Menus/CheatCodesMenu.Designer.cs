namespace InterfaceGraphique {
    partial class CheatCodesMenu {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheatCodesMenu));
            this.MediaPlayer_Player = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer_Player)).BeginInit();
            this.SuspendLayout();
            // 
            // MediaPlayer_Player
            // 
            this.MediaPlayer_Player.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MediaPlayer_Player.Enabled = true;
            this.MediaPlayer_Player.Location = new System.Drawing.Point(0, 0);
            this.MediaPlayer_Player.Name = "MediaPlayer_Player";
            this.MediaPlayer_Player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MediaPlayer_Player.OcxState")));
            this.MediaPlayer_Player.Size = new System.Drawing.Size(944, 501);
            this.MediaPlayer_Player.TabIndex = 0;
            // 
            // CheatCodesMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Controls.Add(this.MediaPlayer_Player);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheatCodesMenu";
            this.Text = "Cheat Codes";
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayer_Player)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer MediaPlayer_Player;
    }
}