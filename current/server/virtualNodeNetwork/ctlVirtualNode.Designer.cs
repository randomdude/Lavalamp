namespace virtualNodeNetwork
{
    partial class ctlVirtualNode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlVirtualNode));
            this.grpNodeInfo = new System.Windows.Forms.GroupBox();
            this.lblNodeState = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNodeID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblNodeName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.picNode = new System.Windows.Forms.PictureBox();
            this.grpNodeSensors = new System.Windows.Forms.GroupBox();
            this.grpNodeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNode)).BeginInit();
            this.SuspendLayout();
            // 
            // grpNodeInfo
            // 
            this.grpNodeInfo.Controls.Add(this.lblNodeState);
            this.grpNodeInfo.Controls.Add(this.label4);
            this.grpNodeInfo.Controls.Add(this.lblNodeID);
            this.grpNodeInfo.Controls.Add(this.label3);
            this.grpNodeInfo.Controls.Add(this.lblNodeName);
            this.grpNodeInfo.Controls.Add(this.label1);
            this.grpNodeInfo.Controls.Add(this.picNode);
            this.grpNodeInfo.Location = new System.Drawing.Point(3, 3);
            this.grpNodeInfo.Name = "grpNodeInfo";
            this.grpNodeInfo.Size = new System.Drawing.Size(465, 81);
            this.grpNodeInfo.TabIndex = 7;
            this.grpNodeInfo.TabStop = false;
            this.grpNodeInfo.Text = "Node info";
            // 
            // lblNodeState
            // 
            this.lblNodeState.AutoSize = true;
            this.lblNodeState.Location = new System.Drawing.Point(131, 44);
            this.lblNodeState.Name = "lblNodeState";
            this.lblNodeState.Size = new System.Drawing.Size(16, 13);
            this.lblNodeState.TabIndex = 13;
            this.lblNodeState.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Node state:";
            // 
            // lblNodeID
            // 
            this.lblNodeID.AutoSize = true;
            this.lblNodeID.Location = new System.Drawing.Point(131, 31);
            this.lblNodeID.Name = "lblNodeID";
            this.lblNodeID.Size = new System.Drawing.Size(16, 13);
            this.lblNodeID.TabIndex = 11;
            this.lblNodeID.Text = "...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Node ID:";
            // 
            // lblNodeName
            // 
            this.lblNodeName.AutoSize = true;
            this.lblNodeName.Location = new System.Drawing.Point(131, 18);
            this.lblNodeName.Name = "lblNodeName";
            this.lblNodeName.Size = new System.Drawing.Size(16, 13);
            this.lblNodeName.TabIndex = 9;
            this.lblNodeName.Text = "...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Node name:";
            // 
            // picNode
            // 
            this.picNode.Image = ((System.Drawing.Image)(resources.GetObject("picNode.Image")));
            this.picNode.Location = new System.Drawing.Point(6, 18);
            this.picNode.Name = "picNode";
            this.picNode.Size = new System.Drawing.Size(48, 48);
            this.picNode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picNode.TabIndex = 7;
            this.picNode.TabStop = false;
            // 
            // grpNodeSensors
            // 
            this.grpNodeSensors.Location = new System.Drawing.Point(474, 3);
            this.grpNodeSensors.Name = "grpNodeSensors";
            this.grpNodeSensors.Size = new System.Drawing.Size(309, 166);
            this.grpNodeSensors.TabIndex = 8;
            this.grpNodeSensors.TabStop = false;
            this.grpNodeSensors.Text = "Node sensors";
            // 
            // ctlVirtualNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpNodeSensors);
            this.Controls.Add(this.grpNodeInfo);
            this.Name = "ctlVirtualNode";
            this.Size = new System.Drawing.Size(786, 172);
            this.grpNodeInfo.ResumeLayout(false);
            this.grpNodeInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpNodeInfo;
        private System.Windows.Forms.Label lblNodeState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNodeID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblNodeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picNode;
        private System.Windows.Forms.GroupBox grpNodeSensors;

    }
}
