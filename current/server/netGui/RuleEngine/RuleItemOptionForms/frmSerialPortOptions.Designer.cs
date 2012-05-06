namespace netGui.RuleItemOptionForms
{
    partial class frmSerialPortOptions
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmboPortName = new System.Windows.Forms.ComboBox();
            this.cmboBaudRate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmboParity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmboHandshaking = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCharDelay = new System.Windows.Forms.TextBox();
            this.chkUseInterCharDelay = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmboPreData = new System.Windows.Forms.ComboBox();
            this.cmboPostData = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(194, 270);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(5, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(49, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmboPortName
            // 
            this.cmboPortName.FormattingEnabled = true;
            this.cmboPortName.Location = new System.Drawing.Point(124, 12);
            this.cmboPortName.Name = "cmboPortName";
            this.cmboPortName.Size = new System.Drawing.Size(121, 21);
            this.cmboPortName.TabIndex = 3;
            // 
            // cmboBaudRate
            // 
            this.cmboBaudRate.FormattingEnabled = true;
            this.cmboBaudRate.Location = new System.Drawing.Point(124, 39);
            this.cmboBaudRate.Name = "cmboBaudRate";
            this.cmboBaudRate.Size = new System.Drawing.Size(121, 21);
            this.cmboBaudRate.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(49, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baud rate:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmboParity
            // 
            this.cmboParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboParity.FormattingEnabled = true;
            this.cmboParity.Location = new System.Drawing.Point(124, 66);
            this.cmboParity.Name = "cmboParity";
            this.cmboParity.Size = new System.Drawing.Size(121, 21);
            this.cmboParity.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(49, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "Parity:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmboHandshaking
            // 
            this.cmboHandshaking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboHandshaking.FormattingEnabled = true;
            this.cmboHandshaking.Location = new System.Drawing.Point(124, 93);
            this.cmboHandshaking.Name = "cmboHandshaking";
            this.cmboHandshaking.Size = new System.Drawing.Size(121, 21);
            this.cmboHandshaking.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(39, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 21);
            this.label4.TabIndex = 8;
            this.label4.Text = "Handshaking:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(41, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 21);
            this.label5.TabIndex = 10;
            this.label5.Text = "Delay time (ms):";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCharDelay
            // 
            this.txtCharDelay.Location = new System.Drawing.Point(148, 141);
            this.txtCharDelay.Name = "txtCharDelay";
            this.txtCharDelay.Size = new System.Drawing.Size(49, 20);
            this.txtCharDelay.TabIndex = 11;
            // 
            // chkUseInterCharDelay
            // 
            this.chkUseInterCharDelay.AutoSize = true;
            this.chkUseInterCharDelay.Location = new System.Drawing.Point(12, 120);
            this.chkUseInterCharDelay.Name = "chkUseInterCharDelay";
            this.chkUseInterCharDelay.Size = new System.Drawing.Size(185, 17);
            this.chkUseInterCharDelay.TabIndex = 12;
            this.chkUseInterCharDelay.Text = "Delay after each character is sent";
            this.chkUseInterCharDelay.UseVisualStyleBackColor = true;
            this.chkUseInterCharDelay.CheckedChanged += new System.EventHandler(this.chkUseInterCharDelay_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(2, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 21);
            this.label6.TabIndex = 13;
            this.label6.Text = "Before sending data, send:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmboPreData
            // 
            this.cmboPreData.FormattingEnabled = true;
            this.cmboPreData.Location = new System.Drawing.Point(148, 167);
            this.cmboPreData.Name = "cmboPreData";
            this.cmboPreData.Size = new System.Drawing.Size(121, 21);
            this.cmboPreData.TabIndex = 14;
            // 
            // cmboPostData
            // 
            this.cmboPostData.FormattingEnabled = true;
            this.cmboPostData.Location = new System.Drawing.Point(148, 194);
            this.cmboPostData.Name = "cmboPostData";
            this.cmboPostData.Size = new System.Drawing.Size(121, 21);
            this.cmboPostData.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(2, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 21);
            this.label7.TabIndex = 15;
            this.label7.Text = "After sending data, send:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 218);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(257, 49);
            this.label8.TabIndex = 17;
            this.label8.Text = "Enter a string to send before or after the data. Enter {newline} or {linefeed} to" +
                " insert a new line (the \'enter\' key) or a line feed (\\r).";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmSerialPortOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 306);
            this.ControlBox = false;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmboPostData);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmboPreData);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkUseInterCharDelay);
            this.Controls.Add(this.txtCharDelay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmboHandshaking);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmboParity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmboBaudRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmboPortName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSerialPortOptions";
            this.Text = "Serial port options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmboPortName;
        private System.Windows.Forms.ComboBox cmboBaudRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmboParity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmboHandshaking;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCharDelay;
        private System.Windows.Forms.CheckBox chkUseInterCharDelay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmboPreData;
        private System.Windows.Forms.ComboBox cmboPostData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}