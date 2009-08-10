namespace netGui.sensorControls
{
    partial class sensorFrm
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctlSensor1 = new netGui.ctlSensor();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ctlSensor1
            // 
            this.ctlSensor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ctlSensor1.Location = new System.Drawing.Point(0, 0);
            this.ctlSensor1.Name = "ctlSensor1";
            this.ctlSensor1.Size = new System.Drawing.Size(160, 149);
            this.ctlSensor1.TabIndex = 0;
            // 
            // sensorFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(157, 146);
            this.Controls.Add(this.ctlSensor1);
            this.Name = "sensorFrm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "sensor";
            this.Resize += new System.EventHandler(this.sensorFrm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        public ctlSensor ctlSensor1;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}