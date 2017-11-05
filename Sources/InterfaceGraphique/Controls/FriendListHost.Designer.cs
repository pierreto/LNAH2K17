namespace InterfaceGraphique.Controls
{
    partial class FriendListHost
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
            this.FriendListWPF = new System.Windows.Forms.Integration.ElementHost();
            this.friendList1 = new InterfaceGraphique.Controls.WPF.Friends.FriendList();
            this.SuspendLayout();
            // 
            // FriendListWPF
            // 
            this.FriendListWPF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FriendListWPF.Location = new System.Drawing.Point(0, 0);
            this.FriendListWPF.Name = "FriendListWPF";
            this.FriendListWPF.Size = new System.Drawing.Size(1433, 773);
            this.FriendListWPF.TabIndex = 0;
            this.FriendListWPF.Text = "FriendListWPF";
            this.FriendListWPF.Child = this.friendList1;
            // 
            // FriendListHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1433, 773);
            this.Controls.Add(this.FriendListWPF);
            this.Name = "FriendListHost";
            this.Text = "FriendListHost";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost FriendListWPF;
        private WPF.Friends.FriendList friendList1;
    }
}