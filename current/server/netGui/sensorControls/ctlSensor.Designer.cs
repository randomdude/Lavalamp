namespace netGui
{
    partial class ctlSensor
    {
        private graph graphTarget
        {
            get
            {
                return _graphTarget;
            }

            set
            {
                graph newGraph = value;
                this.Controls.Remove(_graphTarget);

                if (_graphTarget != null && newGraph != null)
                {
                    // Set up the new control similarly to the previous
                    newGraph.Left = _graphTarget.Left;
                    newGraph.Top = _graphTarget.Top;
                    newGraph.Width = _graphTarget.Width;
                    newGraph.Height = _graphTarget.Height;
                    newGraph.BorderStyle = _graphTarget.BorderStyle;
                }
                _graphTarget = newGraph;
                _graphTarget.sendValueToNodeDelegate = sendValueToNode;// Wire up the send-value delegate
                this.Controls.Add(_graphTarget);
            }
        }

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.graphtypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simplereadoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateEveryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuItemupdateFreq_1s = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuItemupdateFreq_5s = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuItemupdateFreq_10s = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuItemupdateFreq_60s = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuItemupdateFreq_none = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.updateNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimiseToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans0 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans10 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans20 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans30 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans40 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans50 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans60 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans70 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans80 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrans90 = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToNewFloatingWindowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToNewfloatingWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this._graphTarget = new netGui.graph();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(6, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(137, 22);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "lblTitle";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.VisibleChanged += new System.EventHandler(this.lblTitle_VisibleChanged);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(6, 22);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(137, 20);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "lblType";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblType.VisibleChanged += new System.EventHandler(this.lblType_VisibleChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateNowToolStripMenuItem,
            this.toolStripMenuItem5,
            this.showIDToolStripMenuItem,
            this.showTypeToolStripMenuItem,
            this.showStatusToolStripMenuItem,
            this.toolStripMenuItem2,
            this.graphtypeToolStripMenuItem,
            this.updateEveryToolStripMenuItem,
            this.toolStripMenuItem3,
            this.alwaysOnTopToolStripMenuItem,
            this.minimiseToTrayToolStripMenuItem,
            this.toolStripMenuItem6,
            this.transparencyToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(186, 270);
            // 
            // showIDToolStripMenuItem
            // 
            this.showIDToolStripMenuItem.Checked = true;
            this.showIDToolStripMenuItem.CheckOnClick = true;
            this.showIDToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showIDToolStripMenuItem.Name = "showIDToolStripMenuItem";
            this.showIDToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.showIDToolStripMenuItem.Text = "Show &ID";
            this.showIDToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showIDToolStripMenuItem_CheckedChanged);
            // 
            // showTypeToolStripMenuItem
            // 
            this.showTypeToolStripMenuItem.Checked = true;
            this.showTypeToolStripMenuItem.CheckOnClick = true;
            this.showTypeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTypeToolStripMenuItem.Name = "showTypeToolStripMenuItem";
            this.showTypeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.showTypeToolStripMenuItem.Text = "Show &Type";
            this.showTypeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showTypeToolStripMenuItem_CheckedChanged);
            // 
            // showStatusToolStripMenuItem
            // 
            this.showStatusToolStripMenuItem.Checked = true;
            this.showStatusToolStripMenuItem.CheckOnClick = true;
            this.showStatusToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showStatusToolStripMenuItem.Name = "showStatusToolStripMenuItem";
            this.showStatusToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.showStatusToolStripMenuItem.Text = "Show &Status";
            this.showStatusToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showStatusToolStripMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(182, 6);
            // 
            // graphtypeToolStripMenuItem
            // 
            this.graphtypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simplereadoutToolStripMenuItem});
            this.graphtypeToolStripMenuItem.Name = "graphtypeToolStripMenuItem";
            this.graphtypeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.graphtypeToolStripMenuItem.Text = "Graph &type";
            // 
            // simplereadoutToolStripMenuItem
            // 
            this.simplereadoutToolStripMenuItem.Name = "simplereadoutToolStripMenuItem";
            this.simplereadoutToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.simplereadoutToolStripMenuItem.Text = "Simple &readout";
            // 
            // updateEveryToolStripMenuItem
            // 
            this.updateEveryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuItemupdateFreq_1s,
            this.MnuItemupdateFreq_5s,
            this.MnuItemupdateFreq_10s,
            this.MnuItemupdateFreq_60s,
            this.toolStripMenuItem1,
            this.MnuItemupdateFreq_none});
            this.updateEveryToolStripMenuItem.Name = "updateEveryToolStripMenuItem";
            this.updateEveryToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.updateEveryToolStripMenuItem.Text = "&Update every";
            // 
            // MnuItemupdateFreq_1s
            // 
            this.MnuItemupdateFreq_1s.Name = "MnuItemupdateFreq_1s";
            this.MnuItemupdateFreq_1s.Size = new System.Drawing.Size(139, 22);
            this.MnuItemupdateFreq_1s.Text = "1 second";
            this.MnuItemupdateFreq_1s.Click += new System.EventHandler(this.MnuItemInupdateFreq_Clicked);
            // 
            // MnuItemupdateFreq_5s
            // 
            this.MnuItemupdateFreq_5s.Name = "MnuItemupdateFreq_5s";
            this.MnuItemupdateFreq_5s.Size = new System.Drawing.Size(139, 22);
            this.MnuItemupdateFreq_5s.Text = "&5 seconds";
            this.MnuItemupdateFreq_5s.Click += new System.EventHandler(this.MnuItemInupdateFreq_Clicked);
            // 
            // MnuItemupdateFreq_10s
            // 
            this.MnuItemupdateFreq_10s.Name = "MnuItemupdateFreq_10s";
            this.MnuItemupdateFreq_10s.Size = new System.Drawing.Size(139, 22);
            this.MnuItemupdateFreq_10s.Text = "&10 seconds";
            this.MnuItemupdateFreq_10s.Click += new System.EventHandler(this.MnuItemInupdateFreq_Clicked);
            // 
            // MnuItemupdateFreq_60s
            // 
            this.MnuItemupdateFreq_60s.Name = "MnuItemupdateFreq_60s";
            this.MnuItemupdateFreq_60s.Size = new System.Drawing.Size(139, 22);
            this.MnuItemupdateFreq_60s.Text = "&60 seconds";
            this.MnuItemupdateFreq_60s.Click += new System.EventHandler(this.MnuItemInupdateFreq_Clicked);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(136, 6);
            // 
            // MnuItemupdateFreq_none
            // 
            this.MnuItemupdateFreq_none.Checked = true;
            this.MnuItemupdateFreq_none.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MnuItemupdateFreq_none.Name = "MnuItemupdateFreq_none";
            this.MnuItemupdateFreq_none.Size = new System.Drawing.Size(139, 22);
            this.MnuItemupdateFreq_none.Text = "&Off";
            this.MnuItemupdateFreq_none.Click += new System.EventHandler(this.MnuItemInupdateFreq_Clicked);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(182, 6);
            // 
            // updateNowToolStripMenuItem
            // 
            this.updateNowToolStripMenuItem.Name = "updateNowToolStripMenuItem";
            this.updateNowToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.updateNowToolStripMenuItem.Text = "Update &Now";
            this.updateNowToolStripMenuItem.Click += new System.EventHandler(this.updateNowToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(182, 6);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.alwaysOnTopToolStripMenuItem.Text = "&Always on top";
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem_Click);
            // 
            // minimiseToTrayToolStripMenuItem
            // 
            this.minimiseToTrayToolStripMenuItem.Name = "minimiseToTrayToolStripMenuItem";
            this.minimiseToTrayToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.minimiseToTrayToolStripMenuItem.Text = "Minimise to &Tray";
            this.minimiseToTrayToolStripMenuItem.Click += new System.EventHandler(this.minimiseToTrayToolStripMenuItem_Click);
            // 
            // transparencyToolStripMenuItem
            // 
            this.transparencyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTrans0,
            this.mnuTrans10,
            this.mnuTrans20,
            this.mnuTrans30,
            this.mnuTrans40,
            this.mnuTrans50,
            this.mnuTrans60,
            this.mnuTrans70,
            this.mnuTrans80,
            this.mnuTrans90});
            this.transparencyToolStripMenuItem.Name = "transparencyToolStripMenuItem";
            this.transparencyToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.transparencyToolStripMenuItem.Text = "&Transparency";
            // 
            // mnuTrans0
            // 
            this.mnuTrans0.Name = "mnuTrans0";
            this.mnuTrans0.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans0.Text = "&0%";
            this.mnuTrans0.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans10
            // 
            this.mnuTrans10.Name = "mnuTrans10";
            this.mnuTrans10.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans10.Text = "&10%";
            this.mnuTrans10.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans20
            // 
            this.mnuTrans20.Name = "mnuTrans20";
            this.mnuTrans20.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans20.Text = "&20%";
            this.mnuTrans20.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans30
            // 
            this.mnuTrans30.Name = "mnuTrans30";
            this.mnuTrans30.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans30.Text = "&30%";
            this.mnuTrans30.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans40
            // 
            this.mnuTrans40.Name = "mnuTrans40";
            this.mnuTrans40.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans40.Text = "&40%";
            this.mnuTrans40.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans50
            // 
            this.mnuTrans50.Name = "mnuTrans50";
            this.mnuTrans50.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans50.Text = "&50%";
            this.mnuTrans50.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans60
            // 
            this.mnuTrans60.Name = "mnuTrans60";
            this.mnuTrans60.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans60.Text = "&60%";
            this.mnuTrans60.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans70
            // 
            this.mnuTrans70.Name = "mnuTrans70";
            this.mnuTrans70.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans70.Text = "&70%";
            this.mnuTrans70.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans80
            // 
            this.mnuTrans80.Name = "mnuTrans80";
            this.mnuTrans80.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans80.Text = "&80%";
            this.mnuTrans80.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // mnuTrans90
            // 
            this.mnuTrans90.Name = "mnuTrans90";
            this.mnuTrans90.Size = new System.Drawing.Size(108, 22);
            this.mnuTrans90.Text = "&90%";
            this.mnuTrans90.Click += new System.EventHandler(this.setTrans_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToNewFloatingWindowToolStripMenuItem1,
            this.copyToNewfloatingWindowToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.windowToolStripMenuItem.Text = "Snap-out this &Sensor";
            // 
            // moveToNewFloatingWindowToolStripMenuItem1
            // 
            this.moveToNewFloatingWindowToolStripMenuItem1.Name = "moveToNewFloatingWindowToolStripMenuItem1";
            this.moveToNewFloatingWindowToolStripMenuItem1.Size = new System.Drawing.Size(225, 22);
            this.moveToNewFloatingWindowToolStripMenuItem1.Text = "&Move to new floating window";
            this.moveToNewFloatingWindowToolStripMenuItem1.Click += new System.EventHandler(this.moveToNewFloatingWindowToolStripMenuItem1_Click);
            // 
            // copyToNewfloatingWindowToolStripMenuItem
            // 
            this.copyToNewfloatingWindowToolStripMenuItem.Name = "copyToNewfloatingWindowToolStripMenuItem";
            this.copyToNewfloatingWindowToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.copyToNewfloatingWindowToolStripMenuItem.Text = "Copy to new &floating window";
            this.copyToNewfloatingWindowToolStripMenuItem.Click += new System.EventHandler(this.copyToNewfloatingWindowToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 124);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(156, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.VisibleChanged += new System.EventHandler(this.statusStrip1_VisibleChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(182, 6);
            // 
            // _graphTarget
            // 
            this._graphTarget.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._graphTarget.Location = new System.Drawing.Point(6, 45);
            this._graphTarget.Name = "_graphTarget";
            this._graphTarget.Size = new System.Drawing.Size(147, 76);
            this._graphTarget.TabIndex = 2;
            // 
            // ctlSensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._graphTarget);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblTitle);
            this.Name = "ctlSensor";
            this.Size = new System.Drawing.Size(156, 146);
            this.Load += new System.EventHandler(this.ctlSensor_Load);
            this.VisibleChanged += new System.EventHandler(this.ctlSensor_VisibleChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem showIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTypeToolStripMenuItem;
        private graph _graphTarget;
        private System.Windows.Forms.ToolStripMenuItem graphtypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simplereadoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateEveryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MnuItemupdateFreq_1s;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MnuItemupdateFreq_none;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem updateNowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MnuItemupdateFreq_5s;
        private System.Windows.Forms.ToolStripMenuItem MnuItemupdateFreq_10s;
        private System.Windows.Forms.ToolStripMenuItem MnuItemupdateFreq_60s;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem showStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        public System.Windows.Forms.Label lblType;
        public System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToNewFloatingWindowToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToNewfloatingWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transparencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans0;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans90;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans80;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans70;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans60;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans50;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans40;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans30;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans20;
        private System.Windows.Forms.ToolStripMenuItem mnuTrans10;
        private System.Windows.Forms.ToolStripMenuItem minimiseToTrayToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
    }
}
