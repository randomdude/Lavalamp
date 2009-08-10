namespace netGui.sensorControls
{
    partial class ctlPWM
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
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.lblBrightness = new System.Windows.Forms.Label();
            this.lblFadeSpeed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.AutoSize = false;
            this.trackBarBrightness.Location = new System.Drawing.Point(20, 3);
            this.trackBarBrightness.Maximum = 255;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new System.Drawing.Size(137, 42);
            this.trackBarBrightness.TabIndex = 0;
            this.trackBarBrightness.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // trackBarSpeed
            // 
            this.trackBarSpeed.AutoSize = false;
            this.trackBarSpeed.Location = new System.Drawing.Point(20, 62);
            this.trackBarSpeed.Maximum = 100;
            this.trackBarSpeed.Minimum = 1;
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new System.Drawing.Size(137, 42);
            this.trackBarSpeed.TabIndex = 1;
            this.trackBarSpeed.Value = 1;
            this.trackBarSpeed.Scroll += new System.EventHandler(this.trackBarSpeed_Scroll);
            // 
            // lblBrightness
            // 
            this.lblBrightness.BackColor = System.Drawing.Color.Transparent;
            this.lblBrightness.Location = new System.Drawing.Point(28, 32);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(100, 13);
            this.lblBrightness.TabIndex = 2;
            this.lblBrightness.Text = "Brightness";
            this.lblBrightness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFadeSpeed
            // 
            this.lblFadeSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblFadeSpeed.Location = new System.Drawing.Point(41, 98);
            this.lblFadeSpeed.Name = "lblFadeSpeed";
            this.lblFadeSpeed.Size = new System.Drawing.Size(100, 16);
            this.lblFadeSpeed.TabIndex = 3;
            this.lblFadeSpeed.Text = "Fade delay";
            this.lblFadeSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctlPWM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblFadeSpeed);
            this.Controls.Add(this.lblBrightness);
            this.Controls.Add(this.trackBarSpeed);
            this.Controls.Add(this.trackBarBrightness);
            this.Name = "ctlPWM";
            this.Size = new System.Drawing.Size(176, 130);
            this.Resize += new System.EventHandler(this.ctlPWM_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.Label lblBrightness;
        private System.Windows.Forms.Label lblFadeSpeed;
    }
}
