namespace netGui.RuleEngine.RuleItemOptionForms
{
    partial class frmSwitchOptions
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
            this.chkPinTrue = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFalseVal = new System.Windows.Forms.TextBox();
            this.txtTrueVal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkPinFalse = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cboDataTypeTrue = new System.Windows.Forms.ComboBox();
            this.cboDataTypeFalse = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // chkPinTrue
            // 
            this.chkPinTrue.AutoSize = true;
            this.chkPinTrue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPinTrue.Checked = true;
            this.chkPinTrue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPinTrue.Location = new System.Drawing.Point(12, 30);
            this.chkPinTrue.Name = "chkPinTrue";
            this.chkPinTrue.Size = new System.Drawing.Size(63, 17);
            this.chkPinTrue.TabIndex = 0;
            this.chkPinTrue.Tag = "True";
            this.chkPinTrue.Text = "Use Pin";
            this.chkPinTrue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPinTrue.UseVisualStyleBackColor = true;
            this.chkPinTrue.CheckedChanged += new System.EventHandler(this.chkPin_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Value when true";
            // 
            // txtFalseVal
            // 
            this.txtFalseVal.Enabled = false;
            this.txtFalseVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFalseVal.Location = new System.Drawing.Point(76, 143);
            this.txtFalseVal.Name = "txtFalseVal";
            this.txtFalseVal.Size = new System.Drawing.Size(187, 20);
            this.txtFalseVal.TabIndex = 4;
            this.txtFalseVal.Text = "Using pin value";
            // 
            // txtTrueVal
            // 
            this.txtTrueVal.Enabled = false;
            this.txtTrueVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTrueVal.Location = new System.Drawing.Point(76, 53);
            this.txtTrueVal.Name = "txtTrueVal";
            this.txtTrueVal.Size = new System.Drawing.Size(187, 20);
            this.txtTrueVal.TabIndex = 5;
            this.txtTrueVal.Text = "Using pin value";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Value";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Value when false";
            // 
            // chkPinFalse
            // 
            this.chkPinFalse.AutoSize = true;
            this.chkPinFalse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPinFalse.Checked = true;
            this.chkPinFalse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPinFalse.Location = new System.Drawing.Point(12, 120);
            this.chkPinFalse.Name = "chkPinFalse";
            this.chkPinFalse.Size = new System.Drawing.Size(63, 17);
            this.chkPinFalse.TabIndex = 7;
            this.chkPinFalse.Tag = "False";
            this.chkPinFalse.Text = "Use Pin";
            this.chkPinFalse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPinFalse.UseVisualStyleBackColor = true;
            this.chkPinFalse.CheckedChanged += new System.EventHandler(this.chkPin_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Value";
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(12, 212);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 12;
            this.btnAccept.Text = "OK";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(205, 212);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Data Type";
            // 
            // cboDataTypeTrue
            // 
            this.cboDataTypeTrue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataTypeTrue.FormattingEnabled = true;
            this.cboDataTypeTrue.Items.AddRange(new object[] {
            "Text",
            "Numeric",
            "Boolean",
            "File (TODO)"});
            this.cboDataTypeTrue.Location = new System.Drawing.Point(76, 80);
            this.cboDataTypeTrue.Name = "cboDataTypeTrue";
            this.cboDataTypeTrue.Size = new System.Drawing.Size(187, 21);
            this.cboDataTypeTrue.TabIndex = 15;
            this.cboDataTypeTrue.SelectedIndexChanged += new System.EventHandler(this.cboDataType_SelectedIndexChanged);
            // 
            // cboDataTypeFalse
            // 
            this.cboDataTypeFalse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataTypeFalse.FormattingEnabled = true;
            this.cboDataTypeFalse.Items.AddRange(new object[] {
            "Text",
            "Numeric",
            "Boolean",
            "File (TODO)"});
            this.cboDataTypeFalse.Location = new System.Drawing.Point(76, 169);
            this.cboDataTypeFalse.Name = "cboDataTypeFalse";
            this.cboDataTypeFalse.Size = new System.Drawing.Size(187, 21);
            this.cboDataTypeFalse.TabIndex = 17;
            this.cboDataTypeFalse.SelectedIndexChanged += new System.EventHandler(this.cboDataType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Data Type";
            // 
            // frmSwitchOptions
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 247);
            this.ControlBox = false;
            this.Controls.Add(this.cboDataTypeFalse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboDataTypeTrue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkPinFalse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTrueVal);
            this.Controls.Add(this.txtFalseVal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkPinTrue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSwitchOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPinTrue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFalseVal;
        private System.Windows.Forms.TextBox txtTrueVal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkPinFalse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox cboDataTypeTrue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboDataTypeFalse;
        private System.Windows.Forms.Label label6;
    }
}