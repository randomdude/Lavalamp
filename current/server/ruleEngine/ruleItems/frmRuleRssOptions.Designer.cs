namespace ruleEngine.ruleItems.windows
{
    partial class frmRuleRssOptions
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFeed = new System.Windows.Forms.TextBox();
            this.lblWarning = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFeedTitle = new System.Windows.Forms.LinkLabel();
            this.lblFeedDescription = new System.Windows.Forms.Label();
            this.label = new System.Windows.Forms.Label();
            this.lblFeedAuthors = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(250, 157);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 157);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(250, 25);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 20);
            this.btnCheck.TabIndex = 5;
            this.btnCheck.Text = "Check feed";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter the rss feed url (eg. http://example.com/myfeed.xml):";
            // 
            // txtFeed
            // 
            this.txtFeed.Location = new System.Drawing.Point(15, 27);
            this.txtFeed.Name = "txtFeed";
            this.txtFeed.Size = new System.Drawing.Size(227, 20);
            this.txtFeed.TabIndex = 4;
            // 
            // lblWarning
            // 
            this.lblWarning.ForeColor = System.Drawing.Color.DarkRed;
            this.lblWarning.Location = new System.Drawing.Point(93, 50);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(233, 27);
            this.lblWarning.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Feed title:";
            // 
            // lblFeedTitle
            // 
            this.lblFeedTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFeedTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeedTitle.Location = new System.Drawing.Point(93, 77);
            this.lblFeedTitle.Name = "lblFeedTitle";
            this.lblFeedTitle.Size = new System.Drawing.Size(232, 24);
            this.lblFeedTitle.TabIndex = 8;
            this.lblFeedTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.feedTitleClicked);
            // 
            // lblFeedDescription
            // 
            this.lblFeedDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFeedDescription.Location = new System.Drawing.Point(93, 123);
            this.lblFeedDescription.Name = "lblFeedDescription";
            this.lblFeedDescription.Size = new System.Drawing.Size(232, 22);
            this.lblFeedDescription.TabIndex = 10;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(15, 102);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(72, 13);
            this.label.TabIndex = 11;
            this.label.Text = "Feed authors:";
            // 
            // lblFeedAuthors
            // 
            this.lblFeedAuthors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFeedAuthors.Location = new System.Drawing.Point(93, 101);
            this.lblFeedAuthors.Name = "lblFeedAuthors";
            this.lblFeedAuthors.Size = new System.Drawing.Size(232, 22);
            this.lblFeedAuthors.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Feed description:";
            // 
            // frmRuleRssOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(337, 193);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFeedAuthors);
            this.Controls.Add(this.label);
            this.Controls.Add(this.lblFeedDescription);
            this.Controls.Add(this.lblFeedTitle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.txtFeed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRuleRssOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RSS reader options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFeed;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lblFeedTitle;
        private System.Windows.Forms.Label lblFeedDescription;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label lblFeedAuthors;
        private System.Windows.Forms.Label label3;
    }
}