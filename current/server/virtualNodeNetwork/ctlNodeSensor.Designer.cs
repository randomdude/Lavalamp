namespace virtualNodeNetwork
{
    partial class ctlNodeSensor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblSensorID = new System.Windows.Forms.Label();
            this.lblSensorType = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sensor ID:";
            // 
            // lblSensorID
            // 
            this.lblSensorID.AutoSize = true;
            this.lblSensorID.Location = new System.Drawing.Point(75, 0);
            this.lblSensorID.Name = "lblSensorID";
            this.lblSensorID.Size = new System.Drawing.Size(13, 13);
            this.lblSensorID.TabIndex = 1;
            this.lblSensorID.Text = "..";
            // 
            // lblSensorType
            // 
            this.lblSensorType.AutoSize = true;
            this.lblSensorType.Location = new System.Drawing.Point(75, 13);
            this.lblSensorType.Name = "lblSensorType";
            this.lblSensorType.Size = new System.Drawing.Size(13, 13);
            this.lblSensorType.TabIndex = 3;
            this.lblSensorType.Text = "..";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Sensor type:";
            // 
            // ctlNodeSensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.lblSensorType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSensorID);
            this.Controls.Add(this.label1);
            this.Name = "ctlNodeSensor";
            this.Size = new System.Drawing.Size(261, 68);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSensorID;
        private System.Windows.Forms.Label lblSensorType;
        private System.Windows.Forms.Label label3;
    }
}
