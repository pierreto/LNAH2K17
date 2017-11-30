namespace InterfaceGraphique.Editor
{
    partial class SaveMapForm
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
            this.Button_SaveOnline = new System.Windows.Forms.Button();
            this.Button_SaveLocally = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button_SaveOnline
            // 
            this.Button_SaveOnline.Location = new System.Drawing.Point(350, 82);
            this.Button_SaveOnline.Name = "Button_SaveOnline";
            this.Button_SaveOnline.Size = new System.Drawing.Size(186, 37);
            this.Button_SaveOnline.TabIndex = 0;
            this.Button_SaveOnline.Text = "Enregistrer en ligne";
            this.Button_SaveOnline.UseVisualStyleBackColor = true;
            this.Button_SaveOnline.Click += new System.EventHandler(this.Button_SaveOnline_Click);
            // 
            // Button_SaveLocally
            // 
            this.Button_SaveLocally.Location = new System.Drawing.Point(70, 82);
            this.Button_SaveLocally.Name = "Button_SaveLocally";
            this.Button_SaveLocally.Size = new System.Drawing.Size(186, 37);
            this.Button_SaveLocally.TabIndex = 1;
            this.Button_SaveLocally.Text = "Enregistrer sur l\'ordinateur";
            this.Button_SaveLocally.UseVisualStyleBackColor = true;
            this.Button_SaveLocally.Click += new System.EventHandler(this.Button_SaveLocally_Click);
            // 
            // SaveMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 185);
            this.Controls.Add(this.Button_SaveLocally);
            this.Controls.Add(this.Button_SaveOnline);
            this.Name = "SaveMap";
            this.Text = "SaveMap";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button_SaveOnline;
        private System.Windows.Forms.Button Button_SaveLocally;
    }
}