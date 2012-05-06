namespace netGui.helpers
{
    partial class inputTime
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
            this.components = new System.ComponentModel.Container();
            this.txtTimeBox = new System.Windows.Forms.TextBox();
            this.inputTimeHelpText = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // txtTimeBox
            // 
            this.txtTimeBox.Location = new System.Drawing.Point(0, -1);
            this.txtTimeBox.Name = "txtTimeBox";
            this.txtTimeBox.Size = new System.Drawing.Size(145, 20);
            this.txtTimeBox.TabIndex = 0;
            this.inputTimeHelpText.SetToolTip(this.txtTimeBox, "Format: HH:MM");
            this.txtTimeBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ensureInputFormat);
            // 
            // inputTimeHelpText
            // 
            this.inputTimeHelpText.ToolTipTitle = "Format: HH:MM";
            // 
            // inputTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTimeBox);
            this.Name = "inputTime";
            this.Size = new System.Drawing.Size(147, 21);
            this.inputTimeHelpText.SetToolTip(this, "Format: HH:MM");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTimeBox;
        private System.Windows.Forms.ToolTip inputTimeHelpText;
    }
}
