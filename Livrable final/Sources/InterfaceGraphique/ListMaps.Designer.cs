namespace InterfaceGraphique
{
    partial class ListMaps
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
            this.DataGridView_Maps = new System.Windows.Forms.DataGridView();
            this.MapName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Creator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Confidentiality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Maps)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridView_Maps
            // 
            this.DataGridView_Maps.AllowUserToAddRows = false;
            this.DataGridView_Maps.AllowUserToDeleteRows = false;
            this.DataGridView_Maps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_Maps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MapName,
            this.Creator,
            this.CreationDate,
            this.Confidentiality});
            this.DataGridView_Maps.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DataGridView_Maps.Location = new System.Drawing.Point(142, 67);
            this.DataGridView_Maps.MultiSelect = false;
            this.DataGridView_Maps.Name = "DataGridView_Maps";
            this.DataGridView_Maps.ReadOnly = true;
            this.DataGridView_Maps.RowTemplate.Height = 24;
            this.DataGridView_Maps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView_Maps.Size = new System.Drawing.Size(1093, 648);
            this.DataGridView_Maps.TabIndex = 0;
            this.DataGridView_Maps.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_Maps_CellContentDoubleClick);
            this.DataGridView_Maps.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_Maps_CellContentDoubleClick);
            // 
            // MapName
            // 
            this.MapName.HeaderText = "Name";
            this.MapName.Name = "MapName";
            this.MapName.ReadOnly = true;
            this.MapName.Width = 300;
            // 
            // Creator
            // 
            this.Creator.HeaderText = "Auteur";
            this.Creator.Name = "Creator";
            this.Creator.ReadOnly = true;
            this.Creator.Width = 300;
            // 
            // CreationDate
            // 
            this.CreationDate.HeaderText = "Date de création";
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            this.CreationDate.Width = 300;
            // 
            // Confidentiality
            // 
            this.Confidentiality.HeaderText = "Type";
            this.Confidentiality.Name = "Confidentiality";
            this.Confidentiality.ReadOnly = true;
            this.Confidentiality.Width = 150;
            // 
            // ListMaps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 776);
            this.Controls.Add(this.DataGridView_Maps);
            this.Name = "ListMaps";
            this.Text = "Cartes disponibles";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Maps)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.DataGridView DataGridView_Maps;
        private System.Windows.Forms.DataGridViewTextBoxColumn MapName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Creator;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Confidentiality;
    }
}