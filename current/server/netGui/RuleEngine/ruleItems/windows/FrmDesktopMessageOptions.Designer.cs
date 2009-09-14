namespace netGui.RuleEngine.ruleItems.windows
{
    partial class FrmDesktopMessageOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.trackbarFadeInSpeed = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblholdSpeed = new System.Windows.Forms.Label();
            this.CmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdPreview = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarFadeInSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hold speed:";
            // 
            // trackbarFadeInSpeed
            // 
            this.trackbarFadeInSpeed.LargeChange = 1;
            this.trackbarFadeInSpeed.Location = new System.Drawing.Point(46, 25);
            this.trackbarFadeInSpeed.Name = "trackbarFadeInSpeed";
            this.trackbarFadeInSpeed.Size = new System.Drawing.Size(212, 42);
            this.trackbarFadeInSpeed.TabIndex = 1;
            this.trackbarFadeInSpeed.Value = 1;
            this.trackbarFadeInSpeed.Scroll += new System.EventHandler(this.trackbarFadeInSpeed_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Slow";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Fast";
            // 
            // lblholdSpeed
            // 
            this.lblholdSpeed.AutoSize = true;
            this.lblholdSpeed.Location = new System.Drawing.Point(126, 9);
            this.lblholdSpeed.Name = "lblholdSpeed";
            this.lblholdSpeed.Size = new System.Drawing.Size(18, 13);
            this.lblholdSpeed.TabIndex = 4;
            this.lblholdSpeed.Text = "1s";
            // 
            // CmdCancel
            // 
            this.CmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CmdCancel.Location = new System.Drawing.Point(12, 193);
            this.CmdCancel.Name = "CmdCancel";
            this.CmdCancel.Size = new System.Drawing.Size(75, 23);
            this.CmdCancel.TabIndex = 2;
            this.CmdCancel.Text = "Cancel";
            this.CmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(220, 193);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdPreview
            // 
            this.cmdPreview.Location = new System.Drawing.Point(116, 193);
            this.cmdPreview.Name = "cmdPreview";
            this.cmdPreview.Size = new System.Drawing.Size(75, 23);
            this.cmdPreview.TabIndex = 3;
            this.cmdPreview.Text = "&Preview";
            this.cmdPreview.UseVisualStyleBackColor = true;
            this.cmdPreview.Click += new System.EventHandler(this.cmdPreview_Click);
            // 
            // FrmDesktopMessageOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CmdCancel;
            this.ClientSize = new System.Drawing.Size(307, 228);
            this.Controls.Add(this.cmdPreview);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.CmdCancel);
            this.Controls.Add(this.lblholdSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackbarFadeInSpeed);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmDesktopMessageOptions";
            this.Text = "Desktop message options";
            ((System.ComponentModel.ISupportInitialize)(this.trackbarFadeInSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackbarFadeInSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblholdSpeed;
        private System.Windows.Forms.Button CmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdPreview;
    }
}