namespace netGui.sensorControls
{
    partial class ctlOnOff
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
            this.cmdOn = new System.Windows.Forms.Button();
            this.cmdOff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdOn
            // 
            this.cmdOn.Location = new System.Drawing.Point(3, 3);
            this.cmdOn.Name = "cmdOn";
            this.cmdOn.Size = new System.Drawing.Size(75, 23);
            this.cmdOn.TabIndex = 0;
            this.cmdOn.Text = "O&n";
            this.cmdOn.UseVisualStyleBackColor = true;
            this.cmdOn.Click += new System.EventHandler(this.cmdOn_Click);
            // 
            // cmdOff
            // 
            this.cmdOff.Location = new System.Drawing.Point(3, 32);
            this.cmdOff.Name = "cmdOff";
            this.cmdOff.Size = new System.Drawing.Size(75, 23);
            this.cmdOff.TabIndex = 1;
            this.cmdOff.Text = "O&ff";
            this.cmdOff.UseVisualStyleBackColor = true;
            this.cmdOff.Click += new System.EventHandler(this.cmdOff_Click);
            // 
            // ctlOnOff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.cmdOff);
            this.Controls.Add(this.cmdOn);
            this.Name = "ctlOnOff";
            this.Size = new System.Drawing.Size(82, 59);
            this.Resize += new System.EventHandler(this.ctlOnOff_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOn;
        private System.Windows.Forms.Button cmdOff;

    }
}
