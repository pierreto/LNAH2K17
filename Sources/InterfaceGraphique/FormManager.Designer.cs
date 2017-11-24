namespace InterfaceGraphique {
    partial class FormManager {
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
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.gameRequestPopup = new System.Windows.Forms.Panel();
            this.buttonRefus = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.playerName = new System.Windows.Forms.TextBox();
            this.gameRequestPopup.SuspendLayout();
            this.SuspendLayout();
            // 
            // elementHost2
            // 
            this.elementHost2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.elementHost2.Location = new System.Drawing.Point(586, 213);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(200, 437);
            this.elementHost2.TabIndex = 12;
            this.elementHost2.Text = "elementHost2";
            this.elementHost2.Child = null;
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.elementHost1.Location = new System.Drawing.Point(0, 350);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(580, 437);
            this.elementHost1.TabIndex = 13;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // gameRequestPopup
            // 
            this.gameRequestPopup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gameRequestPopup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gameRequestPopup.Controls.Add(this.buttonRefus);
            this.gameRequestPopup.Controls.Add(this.buttonAccept);
            this.gameRequestPopup.Controls.Add(this.textBox2);
            this.gameRequestPopup.Controls.Add(this.playerName);
            this.gameRequestPopup.ForeColor = System.Drawing.Color.White;
            this.gameRequestPopup.Location = new System.Drawing.Point(402, 120);
            this.gameRequestPopup.Margin = new System.Windows.Forms.Padding(2);
            this.gameRequestPopup.Name = "gameRequestPopup";
            this.gameRequestPopup.Size = new System.Drawing.Size(288, 217);
            this.gameRequestPopup.TabIndex = 14;
            // 
            // buttonRefus
            // 
            this.buttonRefus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonRefus.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.buttonRefus.Location = new System.Drawing.Point(138, 142);
            this.buttonRefus.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRefus.Name = "buttonRefus";
            this.buttonRefus.Size = new System.Drawing.Size(108, 31);
            this.buttonRefus.TabIndex = 3;
            this.buttonRefus.Text = "Refuser";
            this.buttonRefus.UseVisualStyleBackColor = false;
            // 
            // buttonAccept
            // 
            this.buttonAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.buttonAccept.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.buttonAccept.Location = new System.Drawing.Point(34, 142);
            this.buttonAccept.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(102, 31);
            this.buttonAccept.TabIndex = 2;
            this.buttonAccept.Text = "Accepter";
            this.buttonAccept.UseVisualStyleBackColor = false;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(34, 70);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(224, 53);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "Demande à jouer contre vous!";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // playerName
            // 
            this.playerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.playerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playerName.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.playerName.ForeColor = System.Drawing.Color.White;
            this.playerName.Location = new System.Drawing.Point(90, 35);
            this.playerName.Margin = new System.Windows.Forms.Padding(2);
            this.playerName.Name = "playerName";
            this.playerName.Size = new System.Drawing.Size(102, 24);
            this.playerName.TabIndex = 0;
            this.playerName.Text = "Joueur";
            this.playerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 548);
            this.Controls.Add(this.gameRequestPopup);
            this.Controls.Add(this.elementHost2);
            this.Controls.Add(this.elementHost1);
            this.Name = "FormManager";
            this.Text = "Air Hockey";
            this.gameRequestPopup.ResumeLayout(false);
            this.gameRequestPopup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.Panel gameRequestPopup;
        private System.Windows.Forms.TextBox playerName;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonRefus;
        private System.Windows.Forms.Button buttonAccept;
    }
}