namespace ruleEngine
{
    partial class ctlRule
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
            this.mnuStripHandles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemAddJunc = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemDelJunc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemDelLine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.showDebugInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addJunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tmrStep = new System.Windows.Forms.Timer(this.components);
            this.mnuStripHandles.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuStripHandles
            // 
            this.mnuStripHandles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemAddJunc,
            this.mnuItemDelJunc,
            this.toolStripMenuItem2,
            this.mnuItemDelLine,
            this.toolStripMenuItem3,
            this.showDebugInfoToolStripMenuItem});
            this.mnuStripHandles.Name = "mnuStripHandles";
            this.mnuStripHandles.Size = new System.Drawing.Size(162, 104);
            this.mnuStripHandles.Text = "menuStrip1";
            // 
            // mnuItemAddJunc
            // 
            this.mnuItemAddJunc.Name = "mnuItemAddJunc";
            this.mnuItemAddJunc.Size = new System.Drawing.Size(161, 22);
            this.mnuItemAddJunc.Text = "&Add junction";
            this.mnuItemAddJunc.Click += new System.EventHandler(this.mnuItemAddJunc_Click);
            // 
            // mnuItemDelJunc
            // 
            this.mnuItemDelJunc.Name = "mnuItemDelJunc";
            this.mnuItemDelJunc.Size = new System.Drawing.Size(161, 22);
            this.mnuItemDelJunc.Text = "&Delete junction";
            this.mnuItemDelJunc.Click += new System.EventHandler(this.mnuItemDelJunc_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(158, 6);
            // 
            // mnuItemDelLine
            // 
            this.mnuItemDelLine.Name = "mnuItemDelLine";
            this.mnuItemDelLine.Size = new System.Drawing.Size(161, 22);
            this.mnuItemDelLine.Text = "&Delete whole &Wire";
            this.mnuItemDelLine.Click += new System.EventHandler(this.mnuItemDelLine_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(158, 6);
            // 
            // showDebugInfoToolStripMenuItem
            // 
            this.showDebugInfoToolStripMenuItem.Name = "showDebugInfoToolStripMenuItem";
            this.showDebugInfoToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.showDebugInfoToolStripMenuItem.Text = "Show &Debug info";
            this.showDebugInfoToolStripMenuItem.Visible = false;
            this.showDebugInfoToolStripMenuItem.Click += new System.EventHandler(this.showDebugInfoToolStripMenuItem_Click);
            // 
            // addJunctionToolStripMenuItem
            // 
            this.addJunctionToolStripMenuItem.Name = "addJunctionToolStripMenuItem";
            this.addJunctionToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(169, 6);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus,
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 402);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(496, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.MarqueeAnimationSpeed = 0;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar.Visible = false;
            // 
            // tmrStep
            // 
            this.tmrStep.Tick += new System.EventHandler(this.tmrStep_Tick);
            // 
            // ctlRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.Name = "ctlRule";
            this.Size = new System.Drawing.Size(496, 424);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmRule_Paint);
            this.Click += new System.EventHandler(this.FrmRule_Click);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmRule_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmRule_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmRule_MouseUp);
            this.SizeChanged += new System.EventHandler(this.ctlRule_SizeChanged);
            this.mnuStripHandles.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuStripHandles;
        private System.Windows.Forms.ToolStripMenuItem addJunctionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDelJunc;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDelLine;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAddJunc;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem showDebugInfoToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Timer tmrStep;
    }
}
