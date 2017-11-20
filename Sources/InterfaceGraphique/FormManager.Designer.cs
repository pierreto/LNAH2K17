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
            this.playerName = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonRefus = new System.Windows.Forms.Button();
            this.gameRequestPopup.SuspendLayout();
            this.SuspendLayout();
            // 
            // elementHost2
            // 
            this.elementHost2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.elementHost2.Location = new System.Drawing.Point(1172, 410);
            this.elementHost2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(400, 840);
            this.elementHost2.TabIndex = 12;
            this.elementHost2.Text = "elementHost2";
            this.elementHost2.Child = null;
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.elementHost1.Location = new System.Drawing.Point(0, 673);
            this.elementHost1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1160, 577);
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
            this.gameRequestPopup.Location = new System.Drawing.Point(805, 230);
            this.gameRequestPopup.Name = "gameRequestPopup";
            this.gameRequestPopup.Size = new System.Drawing.Size(573, 413);
            this.gameRequestPopup.TabIndex = 14;
            // 
            // playerName
            // 
            this.playerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.playerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playerName.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.playerName.ForeColor = System.Drawing.Color.White;
            this.playerName.Location = new System.Drawing.Point(179, 68);
            this.playerName.Name = "playerName";
            this.playerName.Size = new System.Drawing.Size(204, 47);
            this.playerName.TabIndex = 0;
            this.playerName.Text = "Joueur";
            this.playerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(69, 135);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(447, 102);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "Demande à jouer contre vous!";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonAccept
            // 
            this.buttonAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.buttonAccept.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.buttonAccept.Location = new System.Drawing.Point(69, 273);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(203, 59);
            this.buttonAccept.TabIndex = 2;
            this.buttonAccept.Text = "Accepter";
            this.buttonAccept.UseVisualStyleBackColor = false;
            // 
            // buttonRefus
            // 
            this.buttonRefus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonRefus.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.buttonRefus.Location = new System.Drawing.Point(277, 273);
            this.buttonRefus.Name = "buttonRefus";
            this.buttonRefus.Size = new System.Drawing.Size(215, 59);
            this.buttonRefus.TabIndex = 3;
            this.buttonRefus.Text = "Refuser";
            this.buttonRefus.UseVisualStyleBackColor = false;
            // 
            // FormManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2368, 1246);
            this.Controls.Add(this.gameRequestPopup);
            this.Controls.Add(this.elementHost2);
            this.Controls.Add(this.elementHost1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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