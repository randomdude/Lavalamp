namespace ruleEngine
{
    partial class frmDebug
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
            this.lblDebugInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDebugInfo
            // 
            this.lblDebugInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDebugInfo.Location = new System.Drawing.Point(6, 9);
            this.lblDebugInfo.Name = "lblDebugInfo";
            this.lblDebugInfo.Size = new System.Drawing.Size(429, 489);
            this.lblDebugInfo.TabIndex = 0;
            // 
            // frmDebugLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 507);
            this.Controls.Add(this.lblDebugInfo);
            this.Name = "frmDebugLine";
            this.Text = "frmDebugLine";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDebugInfo;
    }
}