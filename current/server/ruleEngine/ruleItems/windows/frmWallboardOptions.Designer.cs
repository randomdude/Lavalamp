﻿namespace ruleEngine.ruleItems.windows
{
    partial class frmWallboardOptions
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cboColour = new System.Windows.Forms.ComboBox();
            this.cboSpecial = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMode = new System.Windows.Forms.ComboBox();
            this.cboTextPosition = new System.Windows.Forms.ComboBox();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.trackBarTime = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(12, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(245, 249);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cboColour
            // 
            this.cboColour.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColour.FormattingEnabled = true;
            this.cboColour.Location = new System.Drawing.Point(113, 110);
            this.cboColour.Name = "cboColour";
            this.cboColour.Size = new System.Drawing.Size(213, 21);
            this.cboColour.TabIndex = 2;
            this.cboColour.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.drawColor);
            // 
            // cboSpecial
            // 
            this.cboSpecial.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboSpecial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpecial.FormattingEnabled = true;
            this.cboSpecial.Location = new System.Drawing.Point(113, 179);
            this.cboSpecial.Name = "cboSpecial";
            this.cboSpecial.Size = new System.Drawing.Size(213, 21);
            this.cboSpecial.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Location = new System.Drawing.Point(12, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Colour";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Location = new System.Drawing.Point(12, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Position";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.label4.Location = new System.Drawing.Point(12, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Special";
            // 
            // cboMode
            // 
            this.cboMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMode.FormattingEnabled = true;
            this.cboMode.Location = new System.Drawing.Point(113, 144);
            this.cboMode.Name = "cboMode";
            this.cboMode.Size = new System.Drawing.Size(213, 21);
            this.cboMode.TabIndex = 2;
            // 
            // cboTextPosition
            // 
            this.cboTextPosition.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboTextPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTextPosition.FormattingEnabled = true;
            this.cboTextPosition.Location = new System.Drawing.Point(113, 79);
            this.cboTextPosition.Name = "cboTextPosition";
            this.cboTextPosition.Size = new System.Drawing.Size(213, 21);
            this.cboTextPosition.TabIndex = 2;
            // 
            // cboPort
            // 
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(113, 45);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(213, 21);
            this.cboPort.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Port";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(194, 12);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(61, 23);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Wallboard State: ";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(104, 17);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(53, 13);
            this.lblState.TabIndex = 8;
            this.lblState.Text = "Unknown";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(261, 12);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(65, 23);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // trackBarTime
            // 
            this.trackBarTime.AutoSize = false;
            this.trackBarTime.LargeChange = 5000;
            this.trackBarTime.Location = new System.Drawing.Point(107, 206);
            this.trackBarTime.Maximum = 120000;
            this.trackBarTime.Name = "trackBarTime";
            this.trackBarTime.Size = new System.Drawing.Size(219, 42);
            this.trackBarTime.SmallChange = 1000;
            this.trackBarTime.TabIndex = 10;
            this.trackBarTime.TickFrequency = 5000;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.Location = new System.Drawing.Point(12, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 26);
            this.label7.TabIndex = 11;
            this.label7.Text = "Time before text \r\ncan be change";
            // 
            // frmWallboardOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 307);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.trackBarTime);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTextPosition);
            this.Controls.Add(this.cboMode);
            this.Controls.Add(this.cboSpecial);
            this.Controls.Add(this.cboColour);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.button1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "frmWallboardOptions";
            this.Text = "Wallboard Options";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboColour;
        private System.Windows.Forms.ComboBox cboSpecial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMode;
        private System.Windows.Forms.ComboBox cboTextPosition;
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TrackBar trackBarTime;
        private System.Windows.Forms.Label label7;
    }
}