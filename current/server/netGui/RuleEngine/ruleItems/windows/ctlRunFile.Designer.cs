namespace netGui.RuleEngine.ruleItems.itemControls
{
    partial class ctlRunFile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseToexecutableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.testByRunningSpecifiedApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureIcon = new System.Windows.Forms.PictureBox();
            this.lbl = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.browseToexecutableToolStripMenuItem,
            this.toolStripMenuItem1,
            this.testByRunningSpecifiedApplicationToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(260, 76);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.optionsToolStripMenuItem.Text = "&Options..";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // browseToexecutableToolStripMenuItem
            // 
            this.browseToexecutableToolStripMenuItem.Name = "browseToexecutableToolStripMenuItem";
            this.browseToexecutableToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.browseToexecutableToolStripMenuItem.Text = "&Browse to executable";
            this.browseToexecutableToolStripMenuItem.Click += new System.EventHandler(this.browseToexecutableToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(256, 6);
            // 
            // testByRunningSpecifiedApplicationToolStripMenuItem
            // 
            this.testByRunningSpecifiedApplicationToolStripMenuItem.Name = "testByRunningSpecifiedApplicationToolStripMenuItem";
            this.testByRunningSpecifiedApplicationToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.testByRunningSpecifiedApplicationToolStripMenuItem.Text = "&Test by running specified application";
            this.testByRunningSpecifiedApplicationToolStripMenuItem.Click += new System.EventHandler(this.testByRunningSpecifiedApplicationToolStripMenuItem_Click);
            // 
            // pictureIcon
            // 
            this.pictureIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureIcon.Location = new System.Drawing.Point(19, 3);
            this.pictureIcon.Name = "pictureIcon";
            this.pictureIcon.Size = new System.Drawing.Size(39, 33);
            this.pictureIcon.TabIndex = 1;
            this.pictureIcon.TabStop = false;
            // 
            // lbl
            // 
            this.lbl.Location = new System.Drawing.Point(3, 39);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(73, 37);
            this.lbl.TabIndex = 2;
            this.lbl.Text = "Run an executable";
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctlRunFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.pictureIcon);
            this.Name = "ctlRunFile";
            this.Size = new System.Drawing.Size(76, 76);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pictureIcon;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.ToolStripMenuItem browseToexecutableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem testByRunningSpecifiedApplicationToolStripMenuItem;
    }
}
