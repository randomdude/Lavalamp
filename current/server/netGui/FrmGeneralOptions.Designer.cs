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
            this.lblPathOrServer = new System.Windows.Forms.Label();
            this.chkUseEncryption = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dlgRulePath = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOpenDlg = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkServer = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboPort
            // 
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(89, 89);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(132, 21);
            this.cboPort.TabIndex = 1;
            this.cboPort.Text = "com1";
            this.cboPort.TextChanged += new System.EventHandler(this.CheckVaildPort);
            this.cboPort.Leave += new System.EventHandler(this.CheckVaildPort);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(297, 218);
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
            this.cmdCancel.Location = new System.Drawing.Point(8, 218);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdReload
            // 
            this.cmdReload.Location = new System.Drawing.Point(283, 89);
            this.cmdReload.Name = "cmdReload";
            this.cmdReload.Size = new System.Drawing.Size(89, 23);
            this.cmdReload.TabIndex = 4;
            this.cmdReload.Text = "Rescan &port list";
            this.cmdReload.UseVisualStyleBackColor = true;
            this.cmdReload.Click += new System.EventHandler(this.cmdReload_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Network key:";
            // 
            // txtKey
            // 
            this.txtKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKey.Location = new System.Drawing.Point(90, 141);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(282, 20);
            this.txtKey.TabIndex = 5;
            // 
            // txtRulePath
            // 
            this.txtRulePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::netGui.Properties.Settings.Default, "rulesPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtRulePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRulePath.Location = new System.Drawing.Point(89, 191);
            this.txtRulePath.Name = "txtRulePath";
            this.txtRulePath.Size = new System.Drawing.Size(238, 20);
            this.txtRulePath.TabIndex = 7;
            this.txtRulePath.Text = global::netGui.Properties.Settings.Default.rulesPath;
            // 
            // lblPathOrServer
            // 
            this.lblPathOrServer.AutoSize = true;
            this.lblPathOrServer.Location = new System.Drawing.Point(15, 194);
            this.lblPathOrServer.Name = "lblPathOrServer";
            this.lblPathOrServer.Size = new System.Drawing.Size(56, 13);
            this.lblPathOrServer.TabIndex = 6;
            this.lblPathOrServer.Text = "Rule path:";
            // 
            // chkUseEncryption
            // 
            this.chkUseEncryption.AutoSize = true;
            this.chkUseEncryption.Location = new System.Drawing.Point(90, 116);
            this.chkUseEncryption.Name = "chkUseEncryption";
            this.chkUseEncryption.Size = new System.Drawing.Size(131, 17);
            this.chkUseEncryption.TabIndex = 8;
            this.chkUseEncryption.Text = "Use &encryption on link";
            this.chkUseEncryption.UseVisualStyleBackColor = true;
            this.chkUseEncryption.CheckedChanged += new System.EventHandler(this.chkUseEncryption_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(227, 81);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 34);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dlgRulePath
            // 
            this.dlgRulePath.Description = "Select the rule path";
            this.dlgRulePath.SelectedPath = global::netGui.Properties.Settings.Default.rulesPath;
            // 
            // btnOpenDlg
            // 
            this.btnOpenDlg.Location = new System.Drawing.Point(333, 189);
            this.btnOpenDlg.Name = "btnOpenDlg";
            this.btnOpenDlg.Size = new System.Drawing.Size(38, 23);
            this.btnOpenDlg.TabIndex = 10;
            this.btnOpenDlg.Text = "...";
            this.btnOpenDlg.UseVisualStyleBackColor = true;
            this.btnOpenDlg.Click += new System.EventHandler(this.btnOpenDlg_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(282, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Change Password";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Username:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(89, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(187, 20);
            this.textBox1.TabIndex = 13;
            // 
            // chkServer
            // 
            this.chkServer.AutoSize = true;
            this.chkServer.Location = new System.Drawing.Point(90, 168);
            this.chkServer.Name = "chkServer";
            this.chkServer.Size = new System.Drawing.Size(77, 17);
            this.chkServer.TabIndex = 16;
            this.chkServer.Text = "Use &server";
            this.chkServer.UseVisualStyleBackColor = true;
            // 
            // FrmGeneralOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(387, 247);
            this.ControlBox = false;
            this.Controls.Add(this.chkServer);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOpenDlg);
            this.Controls.Add(this.chkUseEncryption);
            this.Controls.Add(this.txtRulePath);
            this.Controls.Add(this.lblPathOrServer);
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
        private System.Windows.Forms.Label lblPathOrServer;
        private System.Windows.Forms.CheckBox chkUseEncryption;
		private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.FolderBrowserDialog dlgRulePath;
        private System.Windows.Forms.Button btnOpenDlg;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox chkServer;
    }
}