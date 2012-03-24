namespace ruleEngine.ruleItems.windows
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
            this.trackbarFadeInSpeed = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblholdSpeed = new System.Windows.Forms.Label();
            this.CmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdPreview = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnChangeFgd = new System.Windows.Forms.Button();
            this.btnChangeBkgd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarFadeInSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // trackbarFadeInSpeed
            // 
            this.trackbarFadeInSpeed.LargeChange = 1;
            this.trackbarFadeInSpeed.Location = new System.Drawing.Point(135, 68);
            this.trackbarFadeInSpeed.Name = "trackbarFadeInSpeed";
            this.trackbarFadeInSpeed.Size = new System.Drawing.Size(212, 42);
            this.trackbarFadeInSpeed.TabIndex = 11;
            this.trackbarFadeInSpeed.Value = 1;
            this.trackbarFadeInSpeed.Scroll += new System.EventHandler(this.trackbarFadeInSpeed_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(353, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Slower";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Faster";
            // 
            // lblholdSpeed
            // 
            this.lblholdSpeed.AutoSize = true;
            this.lblholdSpeed.Location = new System.Drawing.Point(102, 41);
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
            this.cmdOK.Location = new System.Drawing.Point(318, 193);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdPreview
            // 
            this.cmdPreview.Location = new System.Drawing.Point(165, 193);
            this.cmdPreview.Name = "cmdPreview";
            this.cmdPreview.Size = new System.Drawing.Size(75, 23);
            this.cmdPreview.TabIndex = 3;
            this.cmdPreview.Text = "&Preview";
            this.cmdPreview.UseVisualStyleBackColor = true;
            this.cmdPreview.Click += new System.EventHandler(this.cmdPreview_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Popup location:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Popup hold speed:";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(105, 12);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(135, 21);
            this.cmbLocation.TabIndex = 8;
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Text:";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(105, 116);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(135, 20);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            // 
            // btnChangeFgd
            // 
            this.btnChangeFgd.Location = new System.Drawing.Point(246, 114);
            this.btnChangeFgd.Name = "btnChangeFgd";
            this.btnChangeFgd.Size = new System.Drawing.Size(135, 23);
            this.btnChangeFgd.TabIndex = 11;
            this.btnChangeFgd.Text = "Change foreground col";
            this.btnChangeFgd.UseVisualStyleBackColor = true;
            this.btnChangeFgd.Click += new System.EventHandler(this.btnChangeFgd_Click);
            // 
            // btnChangeBkgd
            // 
            this.btnChangeBkgd.Location = new System.Drawing.Point(246, 142);
            this.btnChangeBkgd.Name = "btnChangeBkgd";
            this.btnChangeBkgd.Size = new System.Drawing.Size(135, 23);
            this.btnChangeBkgd.TabIndex = 12;
            this.btnChangeBkgd.Text = "Change background col";
            this.btnChangeBkgd.UseVisualStyleBackColor = true;
            this.btnChangeBkgd.Click += new System.EventHandler(this.btnChangeBkgd_Click);
            // 
            // FrmDesktopMessageOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CmdCancel;
            this.ClientSize = new System.Drawing.Size(405, 228);
            this.Controls.Add(this.btnChangeBkgd);
            this.Controls.Add(this.btnChangeFgd);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLocation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdPreview);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.CmdCancel);
            this.Controls.Add(this.lblholdSpeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackbarFadeInSpeed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmDesktopMessageOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options...";
            ((System.ComponentModel.ISupportInitialize)(this.trackbarFadeInSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackbarFadeInSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblholdSpeed;
        private System.Windows.Forms.Button CmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdPreview;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnChangeFgd;
        private System.Windows.Forms.Button btnChangeBkgd;
    }
}