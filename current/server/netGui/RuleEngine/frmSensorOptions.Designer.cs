namespace netGui.RuleEngine.windows
{
    partial class frmSensorOptions
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
            this.cboNodes = new System.Windows.Forms.ComboBox();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cboSensors = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblNode = new System.Windows.Forms.Label();
            this.lblSensors = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboNodes
            // 
            this.cboNodes.DisplayMember = "name";
            this.cboNodes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNodes.FormattingEnabled = true;
            this.cboNodes.Location = new System.Drawing.Point(32, 43);
            this.cboNodes.Name = "cboNodes";
            this.cboNodes.Size = new System.Drawing.Size(182, 21);
            this.cboNodes.TabIndex = 0;
            this.cboNodes.SelectedIndexChanged += new System.EventHandler(this.cboNodes_SelectedIndexChanged);
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Location = new System.Drawing.Point(140, 79);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 1;
            this.cmdOk.Text = "OK";
            this.cmdOk.UseVisualStyleBackColor = true;
            // 
            // cboSensors
            // 
            this.cboSensors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSensors.FormattingEnabled = true;
            this.cboSensors.Location = new System.Drawing.Point(33, 83);
            this.cboSensors.Name = "cboSensors";
            this.cboSensors.Size = new System.Drawing.Size(182, 21);
            this.cboSensors.TabIndex = 2;
            this.cboSensors.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 79);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblNode
            // 
            this.lblNode.AutoSize = true;
            this.lblNode.Location = new System.Drawing.Point(9, 27);
            this.lblNode.Name = "lblNode";
            this.lblNode.Size = new System.Drawing.Size(154, 13);
            this.lblNode.TabIndex = 4;
            this.lblNode.Text = "Select a node this sensor is on:";
            // 
            // lblSensors
            // 
            this.lblSensors.AutoSize = true;
            this.lblSensors.Location = new System.Drawing.Point(9, 67);
            this.lblSensors.Name = "lblSensors";
            this.lblSensors.Size = new System.Drawing.Size(101, 13);
            this.lblSensors.TabIndex = 5;
            this.lblSensors.Text = "Choose the sensor: ";
            this.lblSensors.Visible = false;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(9, 9);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(83, 13);
            this.lblError.TabIndex = 6;
            this.lblError.Text = "(filled at runtime)";
            // 
            // frmSensorOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(226, 110);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblSensors);
            this.Controls.Add(this.lblNode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cboSensors);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cboNodes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSensorOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sensor Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboNodes;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.ComboBox cboSensors;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNode;
        private System.Windows.Forms.Label lblSensors;
        private System.Windows.Forms.Label lblError;
    }
}