namespace InterfaceGraphique.Editor
{
    partial class SaveMapOnlineForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.Text_MapName = new System.Windows.Forms.TextBox();
            this.Button_SaveOnline = new System.Windows.Forms.Button();
            this.Button_PublicMap = new System.Windows.Forms.RadioButton();
            this.Button_PrivateMap = new System.Windows.Forms.RadioButton();
            this.Text_PwdMap = new System.Windows.Forms.TextBox();
            this.Label_PwdMap = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom de la carte :";
            // 
            // Text_MapName
            // 
            this.Text_MapName.Location = new System.Drawing.Point(200, 32);
            this.Text_MapName.Name = "Text_MapName";
            this.Text_MapName.Size = new System.Drawing.Size(169, 22);
            this.Text_MapName.TabIndex = 1;
            // 
            // Button_SaveOnline
            // 
            this.Button_SaveOnline.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button_SaveOnline.Location = new System.Drawing.Point(200, 215);
            this.Button_SaveOnline.Name = "Button_SaveOnline";
            this.Button_SaveOnline.Size = new System.Drawing.Size(155, 30);
            this.Button_SaveOnline.TabIndex = 2;
            this.Button_SaveOnline.Text = "Enregistrer en ligne";
            this.Button_SaveOnline.UseVisualStyleBackColor = true;
            // 
            // Button_PublicMap
            // 
            this.Button_PublicMap.AutoSize = true;
            this.Button_PublicMap.Location = new System.Drawing.Point(200, 82);
            this.Button_PublicMap.Name = "Button_PublicMap";
            this.Button_PublicMap.Size = new System.Drawing.Size(84, 21);
            this.Button_PublicMap.TabIndex = 3;
            this.Button_PublicMap.TabStop = true;
            this.Button_PublicMap.Text = "Publique";
            this.Button_PublicMap.UseVisualStyleBackColor = true;
            // 
            // Button_PrivateMap
            // 
            this.Button_PrivateMap.AutoSize = true;
            this.Button_PrivateMap.Location = new System.Drawing.Point(300, 82);
            this.Button_PrivateMap.Name = "Button_PrivateMap";
            this.Button_PrivateMap.Size = new System.Drawing.Size(69, 21);
            this.Button_PrivateMap.TabIndex = 4;
            this.Button_PrivateMap.TabStop = true;
            this.Button_PrivateMap.Text = "Privée";
            this.Button_PrivateMap.UseVisualStyleBackColor = true;
            this.Button_PrivateMap.CheckedChanged += new System.EventHandler(this.Button_PrivateMap_CheckedChanged);
            // 
            // Text_PwdMap
            // 
            this.Text_PwdMap.Location = new System.Drawing.Point(200, 135);
            this.Text_PwdMap.Name = "Text_PwdMap";
            this.Text_PwdMap.Size = new System.Drawing.Size(169, 22);
            this.Text_PwdMap.TabIndex = 5;
            // 
            // Label_PwdMap
            // 
            this.Label_PwdMap.AutoSize = true;
            this.Label_PwdMap.Location = new System.Drawing.Point(12, 135);
            this.Label_PwdMap.Name = "Label_PwdMap";
            this.Label_PwdMap.Size = new System.Drawing.Size(172, 17);
            this.Label_PwdMap.TabIndex = 6;
            this.Label_PwdMap.Text = "Mot de passe de la carte :";
            // 
            // SaveMapOnlineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 257);
            this.Controls.Add(this.Label_PwdMap);
            this.Controls.Add(this.Text_PwdMap);
            this.Controls.Add(this.Button_PrivateMap);
            this.Controls.Add(this.Button_PublicMap);
            this.Controls.Add(this.Button_SaveOnline);
            this.Controls.Add(this.Text_MapName);
            this.Controls.Add(this.label1);
            this.Name = "SaveMapOnlineForm";
            this.Text = "Enregistrement d\'une carte";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Button_SaveOnline;
        public System.Windows.Forms.TextBox Text_MapName;
        private System.Windows.Forms.Label Label_PwdMap;
        public System.Windows.Forms.RadioButton Button_PublicMap;
        public System.Windows.Forms.RadioButton Button_PrivateMap;
        public System.Windows.Forms.TextBox Text_PwdMap;
    }
}