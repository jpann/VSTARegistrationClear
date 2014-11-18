namespace VSTARegistrationClear
{
    partial class Main
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpRegistry = new System.Windows.Forms.GroupBox();
            this.btnRegistryBackup = new System.Windows.Forms.Button();
            this.btnRegistryDelete = new System.Windows.Forms.Button();
            this.lstRegistry = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.statusStrip.SuspendLayout();
            this.grpRegistry.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 291);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1017, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1002, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // grpRegistry
            // 
            this.grpRegistry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRegistry.Controls.Add(this.btnRegistryBackup);
            this.grpRegistry.Controls.Add(this.btnRegistryDelete);
            this.grpRegistry.Controls.Add(this.lstRegistry);
            this.grpRegistry.Location = new System.Drawing.Point(12, 27);
            this.grpRegistry.Name = "grpRegistry";
            this.grpRegistry.Size = new System.Drawing.Size(993, 250);
            this.grpRegistry.TabIndex = 2;
            this.grpRegistry.TabStop = false;
            this.grpRegistry.Text = "Registry Keys";
            // 
            // btnRegistryBackup
            // 
            this.btnRegistryBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegistryBackup.Location = new System.Drawing.Point(821, 218);
            this.btnRegistryBackup.Name = "btnRegistryBackup";
            this.btnRegistryBackup.Size = new System.Drawing.Size(75, 23);
            this.btnRegistryBackup.TabIndex = 3;
            this.btnRegistryBackup.Text = "Backup";
            this.btnRegistryBackup.UseVisualStyleBackColor = true;
            this.btnRegistryBackup.Click += new System.EventHandler(this.btnRegistryBackup_Click);
            // 
            // btnRegistryDelete
            // 
            this.btnRegistryDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegistryDelete.Enabled = false;
            this.btnRegistryDelete.Location = new System.Drawing.Point(902, 218);
            this.btnRegistryDelete.Name = "btnRegistryDelete";
            this.btnRegistryDelete.Size = new System.Drawing.Size(75, 23);
            this.btnRegistryDelete.TabIndex = 1;
            this.btnRegistryDelete.Text = "Delete";
            this.btnRegistryDelete.UseVisualStyleBackColor = true;
            this.btnRegistryDelete.Click += new System.EventHandler(this.btnRegistryDelete_Click);
            // 
            // lstRegistry
            // 
            this.lstRegistry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRegistry.CheckBoxes = true;
            this.lstRegistry.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstRegistry.FullRowSelect = true;
            this.lstRegistry.GridLines = true;
            this.lstRegistry.Location = new System.Drawing.Point(16, 19);
            this.lstRegistry.Name = "lstRegistry";
            this.lstRegistry.Size = new System.Drawing.Size(961, 193);
            this.lstRegistry.TabIndex = 0;
            this.lstRegistry.UseCompatibleStateImageBehavior = false;
            this.lstRegistry.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 292;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Full Path";
            this.columnHeader2.Width = 689;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAbout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1017, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAbout
            // 
            this.btnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAbout.Image = global::VSTARegistrationClear.Properties.Resources.help;
            this.btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(23, 22);
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 313);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.grpRegistry);
            this.Controls.Add(this.statusStrip);
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "VSTA Registration Clear";
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.grpRegistry.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.GroupBox grpRegistry;
        private System.Windows.Forms.Button btnRegistryBackup;
        private System.Windows.Forms.Button btnRegistryDelete;
        private System.Windows.Forms.ListView lstRegistry;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAbout;
    }
}

