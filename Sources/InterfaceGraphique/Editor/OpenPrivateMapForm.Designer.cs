namespace InterfaceGraphique.Editor
{
    partial class OpenPrivateMapForm
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
            this.Text_MapPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_OpenPrivateMap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Text_MapPassword
            // 
            this.Text_MapPassword.Location = new System.Drawing.Point(239, 94);
            this.Text_MapPassword.MaxLength = 255;
            this.Text_MapPassword.Name = "Text_MapPassword";
            this.Text_MapPassword.PasswordChar = '*';
            this.Text_MapPassword.Size = new System.Drawing.Size(222, 22);
            this.Text_MapPassword.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mot de passe de la carte :";
            // 
            // Button_OpenPrivateMap
            // 
            this.Button_OpenPrivateMap.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button_OpenPrivateMap.Location = new System.Drawing.Point(239, 161);
            this.Button_OpenPrivateMap.Name = "Button_OpenPrivateMap";
            this.Button_OpenPrivateMap.Size = new System.Drawing.Size(178, 23);
            this.Button_OpenPrivateMap.TabIndex = 2;
            this.Button_OpenPrivateMap.Text = "Charger la carte";
            this.Button_OpenPrivateMap.UseVisualStyleBackColor = true;
            // 
            // OpenPrivateMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 255);
            this.Controls.Add(this.Button_OpenPrivateMap);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Text_MapPassword);
            this.Name = "OpenPrivateMapForm";
            this.Text = "Ouvrir une carte";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Button_OpenPrivateMap;
        public System.Windows.Forms.TextBox Text_MapPassword;
    }
}