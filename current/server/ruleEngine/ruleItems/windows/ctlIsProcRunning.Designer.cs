namespace ruleEngine.ruleItems.windows
{
    partial class ctlIsProcRunning
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
            this.setprocessNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbl = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setprocessNameToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 48);
            // 
            // setprocessNameToolStripMenuItem
            // 
            this.setprocessNameToolStripMenuItem.Name = "setprocessNameToolStripMenuItem";
            this.setprocessNameToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.setprocessNameToolStripMenuItem.Text = "Set &process name";
            this.setprocessNameToolStripMenuItem.Click += new System.EventHandler(this.setprocessNameToolStripMenuItem_Click);
            // 
            // lbl
            // 
            this.lbl.Location = new System.Drawing.Point(0, 35);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(121, 40);
            this.lbl.TabIndex = 2;
            this.lbl.Text = "Is process \'\' running?";
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctlIsProcRunning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.lbl);
            this.Name = "ctlIsProcRunning";
            this.Size = new System.Drawing.Size(121, 75);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.ToolStripMenuItem setprocessNameToolStripMenuItem;
    }
}