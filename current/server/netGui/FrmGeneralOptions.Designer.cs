namespace netGui
{
    partial class FrmGeneralOptions
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
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdReload = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtRulePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUseEncryption = new System.Windows.Forms.CheckBox();
			this.lblStatus = new System.Windows.Forms.Label();
			
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboPort
            // 
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(92, 13);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(146, 21);
            this.cboPort.TabIndex = 1;
			this.cboPort.SelectedIndexChanged += new System.EventHandler(this.CheckVaildPort);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(300, 157);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(12, 157);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			//
			// cmdReload
            // 
			this.cmdReload.Location = new System.Drawing.Point(310, 12);
			this.cmdReload.Name = "cmdReload";
			this.cmdReload.Size = new System.Drawing.Size(75, 23);
			this.cmdReload.TabIndex = 4;
			this.cmdReload.Text = "&Reload";
			this.cmdReload.UseVisualStyleBackColor = true;
            this.cmdReload.Click += new System.EventHandler(this.cmdReload_Click);
			//
			// lblStatus
			//
			this.lblStatus.Location = new System.Drawing.Point(235, 12);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(75, 23);
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Network key:";
            // 
            // txtKey
            // 
            this.txtKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKey.Location = new System.Drawing.Point(93, 63);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(282, 20);
            this.txtKey.TabIndex = 5;
            this.txtKey.Text = "00112233445566778899AABBCCDDEEFF";
            // 
            // txtRulePath
            // 
            this.txtRulePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRulePath.Location = new System.Drawing.Point(93, 89);
            this.txtRulePath.Name = "txtRulePath";
            this.txtRulePath.Size = new System.Drawing.Size(282, 20);
            this.txtRulePath.TabIndex = 7;
            this.txtRulePath.Text = "C:\\lavalamp\\rules";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Rule path:";
            // 
            // chkUseEncryption
            // 
            this.chkUseEncryption.AutoSize = true;
            this.chkUseEncryption.Location = new System.Drawing.Point(93, 40);
            this.chkUseEncryption.Name = "chkUseEncryption";
            this.chkUseEncryption.Size = new System.Drawing.Size(131, 17);
            this.chkUseEncryption.TabIndex = 8;
            this.chkUseEncryption.Text = "Use &encryption on link";
            this.chkUseEncryption.UseVisualStyleBackColor = true;
            this.chkUseEncryption.CheckedChanged += new System.EventHandler(this.chkUseEncryption_CheckedChanged);
            // 
            // FrmGeneralOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(387,200);
            this.ControlBox = false;
            this.Controls.Add(this.chkUseEncryption);
            this.Controls.Add(this.txtRulePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.cmdReload);
			this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cboPort);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGeneralOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FrmGeneralOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdReload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtRulePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkUseEncryption;
		private System.Windows.Forms.Label lblStatus;
    }
}