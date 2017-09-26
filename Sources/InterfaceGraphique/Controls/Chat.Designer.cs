namespace InterfaceGraphique.Menus
{
    partial class Chat
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
            this.chatViewRichTextBox = new System.Windows.Forms.RichTextBox();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // chatViewRichTextBox
            // 
            this.chatViewRichTextBox.BackColor = System.Drawing.Color.White;
            this.chatViewRichTextBox.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatViewRichTextBox.Location = new System.Drawing.Point(6, 6);
            this.chatViewRichTextBox.Name = "chatViewRichTextBox";
            this.chatViewRichTextBox.Size = new System.Drawing.Size(402, 293);
            this.chatViewRichTextBox.TabIndex = 0;
            this.chatViewRichTextBox.Text = "";
            // 
            // InputTextBox
            // 
            this.InputTextBox.BackColor = System.Drawing.Color.White;
            this.InputTextBox.Font = new System.Drawing.Font("Arial Narrow", 14F);
            this.InputTextBox.Location = new System.Drawing.Point(6, 309);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(304, 29);
            this.InputTextBox.TabIndex = 1;
            // 
            // SendButton
            // 
            this.SendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SendButton.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.SendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendButton.Font = new System.Drawing.Font("Arial Narrow", 15F);
            this.SendButton.ForeColor = System.Drawing.Color.Yellow;
            this.SendButton.Location = new System.Drawing.Point(313, 306);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(95, 33);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Envoyer";
            this.SendButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SendButton.UseVisualStyleBackColor = false;
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.InputTextBox);
            this.Controls.Add(this.chatViewRichTextBox);
            this.Name = "Chat";
            this.Size = new System.Drawing.Size(413, 345);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox chatViewRichTextBox;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button SendButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}