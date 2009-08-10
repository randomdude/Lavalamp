namespace netGui.sensorControls
{
    partial class ctlReadout
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
            this.lblReadout = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblReadout
            // 
            this.lblReadout.Location = new System.Drawing.Point(0, 0);
            this.lblReadout.Name = "lblReadout";
            this.lblReadout.Size = new System.Drawing.Size(46, 36);
            this.lblReadout.TabIndex = 0;
            this.lblReadout.Text = "000";
            this.lblReadout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctlReadout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblReadout);
            this.Name = "ctlReadout";
            this.Size = new System.Drawing.Size(45, 36);
            this.Resize += new System.EventHandler(this.ctlReadout_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblReadout;
    }
}
