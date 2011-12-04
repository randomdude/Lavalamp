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
            this.ctlRule1 = new ruleEngine.ctlRule();
            this.button1 = new System.Windows.Forms.Button();
            this.ctxMnuToolbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvToolbox
            // 
            this.tvToolbox.ContextMenuStrip = this.ctxMnuToolbox;
            this.tvToolbox.Location = new System.Drawing.Point(12, 28);
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
            this.btnStop.Location = new System.Drawing.Point(591, 352);
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
            this.btnRun.Location = new System.Drawing.Point(681, 352);
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
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Toolbox:";
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveClose.Image")));
            this.btnSaveClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveClose.Location = new System.Drawing.Point(242, 352);
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
            this.btnCancel.Location = new System.Drawing.Point(152, 352);
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
            this.btnSave.Location = new System.Drawing.Point(332, 353);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 55);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "&Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ctlRule1
            // 
            this.ctlRule1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ctlRule1.Location = new System.Drawing.Point(153, 12);
            this.ctlRule1.Name = "ctlRule1";
            this.ctlRule1.Size = new System.Drawing.Size(613, 334);
            this.ctlRule1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(510, 353);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmRuleEdit
            // 
            this.AcceptButton = this.btnRun;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(777, 420);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ctlRule1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tvToolbox);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSaveClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "frmRuleEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rule editor";
            this.Load += new System.EventHandler(this.frmRuleEdit_Load);
            this.ResizeBegin += new System.EventHandler(this.frmRuleEdit_ResizeBegin);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRuleEdit_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.frmRuleEdit_ResizeEnd);
            this.ctxMnuToolbox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ctlRule ctlRule1;
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
        private System.Windows.Forms.Button button1;
    }
}