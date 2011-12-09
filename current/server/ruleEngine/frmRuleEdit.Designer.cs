namespace ruleEngine
{
    partial class frmRuleEdit
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRuleEdit));
            this.tvToolbox = new System.Windows.Forms.TreeView();
            this.ctxMnuToolbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadFromnetAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysSnapToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctlRuleEditor = new ruleEngine.ctlRule();
            this.ctxMnuToolbox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvToolbox
            // 
            this.tvToolbox.ContextMenuStrip = this.ctxMnuToolbox;
            this.tvToolbox.Location = new System.Drawing.Point(11, 43);
            this.tvToolbox.Name = "tvToolbox";
            this.tvToolbox.Size = new System.Drawing.Size(135, 318);
            this.tvToolbox.TabIndex = 5;
            this.tvToolbox.DoubleClick += new System.EventHandler(this.tvToolbox_DoubleClick);
            // 
            // ctxMnuToolbox
            // 
            this.ctxMnuToolbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFromnetAssemblyToolStripMenuItem});
            this.ctxMnuToolbox.Name = "ctxMnuToolbox";
            this.ctxMnuToolbox.Size = new System.Drawing.Size(269, 26);
            // 
            // loadFromnetAssemblyToolStripMenuItem
            // 
            this.loadFromnetAssemblyToolStripMenuItem.Name = "loadFromnetAssemblyToolStripMenuItem";
            this.loadFromnetAssemblyToolStripMenuItem.Size = new System.Drawing.Size(268, 22);
            this.loadFromnetAssemblyToolStripMenuItem.Text = "&Load from .Net assembly or Python file..";
            this.loadFromnetAssemblyToolStripMenuItem.Click += new System.EventHandler(this.loadFromnetAssemblyToolStripMenuItem_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStop.Location = new System.Drawing.Point(590, 367);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 55);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.btnStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRun
            // 
            this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRun.Location = new System.Drawing.Point(680, 367);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(84, 55);
            this.btnRun.TabIndex = 7;
            this.btnRun.Text = "&Test run";
            this.btnRun.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Toolbox:";
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveClose.Image")));
            this.btnSaveClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveClose.Location = new System.Drawing.Point(241, 367);
            this.btnSaveClose.Name = "btnSaveClose";
            this.btnSaveClose.Size = new System.Drawing.Size(84, 55);
            this.btnSaveClose.TabIndex = 9;
            this.btnSaveClose.Text = "&Save && close";
            this.btnSaveClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveClose.UseVisualStyleBackColor = true;
            this.btnSaveClose.Click += new System.EventHandler(this.btnSaveClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.Location = new System.Drawing.Point(151, 367);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 55);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Close without saving";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(331, 368);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 55);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "&Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(777, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gridToolStripMenuItem
            // 
            this.gridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snapToGridToolStripMenuItem,
            this.alwaysSnapToGridToolStripMenuItem});
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.gridToolStripMenuItem.Text = "&Rule";
            // 
            // snapToGridToolStripMenuItem
            // 
            this.snapToGridToolStripMenuItem.Name = "snapToGridToolStripMenuItem";
            this.snapToGridToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.snapToGridToolStripMenuItem.Text = "&Snap to grid";
            this.snapToGridToolStripMenuItem.Click += new System.EventHandler(this.snapToGridToolStripMenuItem_Click_1);
            // 
            // alwaysSnapToGridToolStripMenuItem
            // 
            this.alwaysSnapToGridToolStripMenuItem.Name = "alwaysSnapToGridToolStripMenuItem";
            this.alwaysSnapToGridToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.alwaysSnapToGridToolStripMenuItem.Text = "Always snap to grid";
            this.alwaysSnapToGridToolStripMenuItem.Click += new System.EventHandler(this.alwaysSnapToGridToolStripMenuItem_Click);
            // 
            // ctlRuleEditor
            // 
            this.ctlRuleEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ctlRuleEditor.Location = new System.Drawing.Point(153, 12);
            this.ctlRuleEditor.Name = "ctlRuleEditor";
            this.ctlRuleEditor.Size = new System.Drawing.Size(613, 334);
            this.ctlRuleEditor.TabIndex = 4;
            // 
            // btnPromote
            // 
            /*this.btnPromote.Location = new System.Drawing.Point(12, 354);
            this.btnPromote.Name = "btnPromote";
            this.btnPromote.Size = new System.Drawing.Size(75, 54);
            this.btnPromote.TabIndex = 12;
            this.btnPromote.Text = "Promote Rule To Toolbox";
            this.btnPromote.UseVisualStyleBackColor = true;*/
            // 
            // frmRuleEdit
            // 
            this.AcceptButton = this.btnRun;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(777, 429);
            this.Controls.Add(this.menuStrip1);
            //this.Controls.Add(this.btnPromote);
            this.Controls.Add(this.ctlRuleEditor);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tvToolbox);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSaveClose);
            this.Controls.Add(this.btnCancel);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmRuleEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rule editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRuleEdit_FormClosing);
            this.Load += new System.EventHandler(this.frmRuleEdit_Load);
            this.ResizeBegin += new System.EventHandler(this.frmRuleEdit_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.frmRuleEdit_ResizeEnd);
            this.ctxMnuToolbox.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ctlRule ctlRuleEditor;
        private System.Windows.Forms.TreeView tvToolbox;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ContextMenuStrip ctxMnuToolbox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem loadFromnetAssemblyToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snapToGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysSnapToGridToolStripMenuItem;
    }
}