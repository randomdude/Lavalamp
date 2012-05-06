namespace netGui.RuleItemOptionForms
{
    partial class frmFileWriteOptions
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblAdditionalTitle = new System.Windows.Forms.Label();
            this.txtAdditonalData = new System.Windows.Forms.TextBox();
            this.lblPublishURI = new System.Windows.Forms.Label();
            this.txtPublishURI = new System.Windows.Forms.TextBox();
            this.btnDir = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(17, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(210, 256);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cboFormat
            // 
            this.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Location = new System.Drawing.Point(109, 39);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(192, 21);
            this.cboFormat.TabIndex = 2;
            this.cboFormat.SelectedIndexChanged += new System.EventHandler(this.cboFormat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select File Format";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "File name:";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(109, 9);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(159, 20);
            this.txtFileName.TabIndex = 5;
            // 
            // lblAdditionalTitle
            // 
            this.lblAdditionalTitle.AutoSize = true;
            this.lblAdditionalTitle.Location = new System.Drawing.Point(16, 79);
            this.lblAdditionalTitle.Name = "lblAdditionalTitle";
            this.lblAdditionalTitle.Size = new System.Drawing.Size(54, 13);
            this.lblAdditionalTitle.TabIndex = 6;
            this.lblAdditionalTitle.Text = "Feed Title";
            // 
            // txtAdditonalData
            // 
            this.txtAdditonalData.Location = new System.Drawing.Point(109, 76);
            this.txtAdditonalData.Name = "txtAdditonalData";
            this.txtAdditonalData.Size = new System.Drawing.Size(192, 20);
            this.txtAdditonalData.TabIndex = 7;
            // 
            // lblPublishURI
            // 
            this.lblPublishURI.AutoSize = true;
            this.lblPublishURI.Location = new System.Drawing.Point(16, 114);
            this.lblPublishURI.Name = "lblPublishURI";
            this.lblPublishURI.Size = new System.Drawing.Size(84, 13);
            this.lblPublishURI.TabIndex = 8;
            this.lblPublishURI.Text = "Feed Publish Url";
            // 
            // txtPublishURI
            // 
            this.txtPublishURI.Location = new System.Drawing.Point(109, 107);
            this.txtPublishURI.Name = "txtPublishURI";
            this.txtPublishURI.Size = new System.Drawing.Size(192, 20);
            this.txtPublishURI.TabIndex = 9;
            // 
            // btnDir
            // 
            this.btnDir.Location = new System.Drawing.Point(274, 9);
            this.btnDir.Name = "btnDir";
            this.btnDir.Size = new System.Drawing.Size(27, 23);
            this.btnDir.TabIndex = 10;
            this.btnDir.Text = "...";
            this.btnDir.UseVisualStyleBackColor = true;
            this.btnDir.Click += new System.EventHandler(this.btnDir_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(109, 152);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(192, 87);
            this.txtDescription.TabIndex = 11;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(16, 152);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(87, 13);
            this.lblDescription.TabIndex = 12;
            this.lblDescription.Text = "Feed Description";
            // 
            // frmFileWriteOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(303, 286);
            this.ControlBox = false;
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.btnDir);
            this.Controls.Add(this.txtPublishURI);
            this.Controls.Add(this.lblPublishURI);
            this.Controls.Add(this.txtAdditonalData);
            this.Controls.Add(this.lblAdditionalTitle);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboFormat);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFileWriteOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblAdditionalTitle;
        private System.Windows.Forms.TextBox txtAdditonalData;
        private System.Windows.Forms.Label lblPublishURI;
        private System.Windows.Forms.TextBox txtPublishURI;
        private System.Windows.Forms.Button btnDir;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
    }
}