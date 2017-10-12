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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom de la carte :";
            // 
            // Text_MapName
            // 
            this.Text_MapName.Location = new System.Drawing.Point(202, 85);
            this.Text_MapName.Name = "Text_MapName";
            this.Text_MapName.Size = new System.Drawing.Size(155, 22);
            this.Text_MapName.TabIndex = 1;
            // 
            // Button_SaveOnline
            // 
            this.Button_SaveOnline.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button_SaveOnline.Location = new System.Drawing.Point(202, 132);
            this.Button_SaveOnline.Name = "Button_SaveOnline";
            this.Button_SaveOnline.Size = new System.Drawing.Size(155, 30);
            this.Button_SaveOnline.TabIndex = 2;
            this.Button_SaveOnline.Text = "Enregistrer en ligne";
            this.Button_SaveOnline.UseVisualStyleBackColor = true;
            // 
            // SaveMapOnlineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 222);
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
    }
}