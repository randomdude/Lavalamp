namespace netGui
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuItemConnectToTrans = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuItemDisconnectFromTrans = new System.Windows.Forms.ToolStripMenuItem();
            this.rulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showRuleEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAllRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstNodes = new System.Windows.Forms.ListView();
            this.ColCaption = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSensorCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nodeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshThisNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolNodeSeperater1 = new System.Windows.Forms.ToolStripSeparator();
            this.thisNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePadvancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolNodeSeperator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuAddNode = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgLstNodes = new System.Windows.Forms.ImageList(this.components);
            this.gpNodeInfo = new System.Windows.Forms.GroupBox();
            this.lblSensorCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNodeName = new System.Windows.Forms.Label();
            this.lblNodeId = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstRules = new System.Windows.Forms.ListView();
            this.colHdrState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdrName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdrIsOpenInEditor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdrDetail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripRules = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.newRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgLstStates = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1.SuspendLayout();
            this.nodeMenuStrip.SuspendLayout();
            this.gpNodeInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStripRules.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.rulesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(542, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generalToolStripMenuItem,
            this.toolStripMenuItem2,
            this.MnuItemConnectToTrans,
            this.MnuItemDisconnectFromTrans});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // generalToolStripMenuItem
            // 
            this.generalToolStripMenuItem.Name = "generalToolStripMenuItem";
            this.generalToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.generalToolStripMenuItem.Text = "&General";
            this.generalToolStripMenuItem.Click += new System.EventHandler(this.mnuItemGeneralOpts);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(204, 6);
            // 
            // MnuItemConnectToTrans
            // 
            this.MnuItemConnectToTrans.Name = "MnuItemConnectToTrans";
            this.MnuItemConnectToTrans.Size = new System.Drawing.Size(207, 22);
            this.MnuItemConnectToTrans.Text = "&Connect to transmitter";
            this.MnuItemConnectToTrans.Click += new System.EventHandler(this.MnuItemConnectToTrans_Click);
            // 
            // MnuItemDisconnectFromTrans
            // 
            this.MnuItemDisconnectFromTrans.Name = "MnuItemDisconnectFromTrans";
            this.MnuItemDisconnectFromTrans.Size = new System.Drawing.Size(207, 22);
            this.MnuItemDisconnectFromTrans.Text = "&Disconnect from transmitter";
            this.MnuItemDisconnectFromTrans.Click += new System.EventHandler(this.MnuItemDisconnectFromTrans_Click);
            // 
            // rulesToolStripMenuItem
            // 
            this.rulesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showRuleEditorToolStripMenuItem,
            this.saveAllRulesToolStripMenuItem,
            this.loadAllRulesToolStripMenuItem});
            this.rulesToolStripMenuItem.Name = "rulesToolStripMenuItem";
            this.rulesToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.rulesToolStripMenuItem.Text = "&Rules";
            // 
            // showRuleEditorToolStripMenuItem
            // 
            this.showRuleEditorToolStripMenuItem.Name = "showRuleEditorToolStripMenuItem";
            this.showRuleEditorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showRuleEditorToolStripMenuItem.Text = "Show rule &Editor";
            this.showRuleEditorToolStripMenuItem.Click += new System.EventHandler(this.showRuleEditorToolStripMenuItem_Click);
            // 
            // saveAllRulesToolStripMenuItem
            // 
            this.saveAllRulesToolStripMenuItem.Name = "saveAllRulesToolStripMenuItem";
            this.saveAllRulesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAllRulesToolStripMenuItem.Text = "&Save all rules";
            this.saveAllRulesToolStripMenuItem.Click += new System.EventHandler(this.saveAllRulesToolStripMenuItem_Click);
            // 
            // loadAllRulesToolStripMenuItem
            // 
            this.loadAllRulesToolStripMenuItem.Name = "loadAllRulesToolStripMenuItem";
            this.loadAllRulesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadAllRulesToolStripMenuItem.Text = "&Load all rules";
            this.loadAllRulesToolStripMenuItem.Click += new System.EventHandler(this.loadAllRulesToolStripMenuItem_Click);
            // 
            // lstNodes
            // 
            this.lstNodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColCaption,
            this.colId,
            this.colName,
            this.colSensorCount});
            this.lstNodes.ContextMenuStrip = this.nodeMenuStrip;
            this.lstNodes.GridLines = true;
            this.lstNodes.LabelEdit = true;
            this.lstNodes.LargeImageList = this.imgLstNodes;
            this.lstNodes.Location = new System.Drawing.Point(16, 20);
            this.lstNodes.Name = "lstNodes";
            this.lstNodes.Size = new System.Drawing.Size(495, 262);
            this.lstNodes.SmallImageList = this.imgLstNodes;
            this.lstNodes.TabIndex = 1;
            this.lstNodes.UseCompatibleStateImageBehavior = false;
            this.lstNodes.SelectedIndexChanged += new System.EventHandler(this.lstNodes_SelectedIndexChanged);
            this.lstNodes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstNodes_KeyPress);
            this.lstNodes.Leave += new System.EventHandler(this.lstNodes_Leave);
            this.lstNodes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstNodes_MouseDoubleClick);
            // 
            // ColCaption
            // 
            this.ColCaption.DisplayIndex = 2;
            this.ColCaption.Text = "Caption";
            // 
            // colId
            // 
            this.colId.DisplayIndex = 0;
            this.colId.Text = "ID";
            // 
            // colName
            // 
            this.colName.DisplayIndex = 1;
            this.colName.Text = "Name";
            // 
            // colSensorCount
            // 
            this.colSensorCount.Text = "Sensor count";
            // 
            // nodeMenuStrip
            // 
            this.nodeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.refreshThisNodeToolStripMenuItem,
            this.toolNodeSeperater1,
            this.thisNodeToolStripMenuItem,
            this.toolNodeSeperator2,
            this.cmnuAddNode,
            this.viewToolStripMenuItem});
            this.nodeMenuStrip.Name = "contextMenuStrip1";
            this.nodeMenuStrip.Size = new System.Drawing.Size(160, 126);
            this.nodeMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.nodeMenuStrip_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // refreshThisNodeToolStripMenuItem
            // 
            this.refreshThisNodeToolStripMenuItem.Name = "refreshThisNodeToolStripMenuItem";
            this.refreshThisNodeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.refreshThisNodeToolStripMenuItem.Text = "Refresh this node";
            this.refreshThisNodeToolStripMenuItem.Click += new System.EventHandler(this.refreshThisNodeToolStripMenuItem_Click);
            // 
            // toolNodeSeperater1
            // 
            this.toolNodeSeperater1.Name = "toolNodeSeperater1";
            this.toolNodeSeperater1.Size = new System.Drawing.Size(156, 6);
            // 
            // thisNodeToolStripMenuItem
            // 
            this.thisNodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeIDToolStripMenuItem,
            this.changeKeyToolStripMenuItem,
            this.changeNameToolStripMenuItem,
            this.changePadvancedToolStripMenuItem});
            this.thisNodeToolStripMenuItem.Name = "thisNodeToolStripMenuItem";
            this.thisNodeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.thisNodeToolStripMenuItem.Text = "This &Node";
            // 
            // changeIDToolStripMenuItem
            // 
            this.changeIDToolStripMenuItem.Name = "changeIDToolStripMenuItem";
            this.changeIDToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.changeIDToolStripMenuItem.Text = "Change &ID";
            this.changeIDToolStripMenuItem.Click += new System.EventHandler(this.changeIDToolStripMenuItem_Click);
            // 
            // changeKeyToolStripMenuItem
            // 
            this.changeKeyToolStripMenuItem.Name = "changeKeyToolStripMenuItem";
            this.changeKeyToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.changeKeyToolStripMenuItem.Text = "Change &Key";
            this.changeKeyToolStripMenuItem.Click += new System.EventHandler(this.changeKeyToolStripMenuItem_Click);
            // 
            // changeNameToolStripMenuItem
            // 
            this.changeNameToolStripMenuItem.Name = "changeNameToolStripMenuItem";
            this.changeNameToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.changeNameToolStripMenuItem.Text = "Change &Name";
            this.changeNameToolStripMenuItem.Click += new System.EventHandler(this.changeNameToolStripMenuItem_Click);
            // 
            // changePadvancedToolStripMenuItem
            // 
            this.changePadvancedToolStripMenuItem.Name = "changePadvancedToolStripMenuItem";
            this.changePadvancedToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.changePadvancedToolStripMenuItem.Text = "Change P (advanced!)";
            this.changePadvancedToolStripMenuItem.Click += new System.EventHandler(this.changePadvancedToolStripMenuItem_Click);
            // 
            // toolNodeSeperator2
            // 
            this.toolNodeSeperator2.Name = "toolNodeSeperator2";
            this.toolNodeSeperator2.Size = new System.Drawing.Size(156, 6);
            // 
            // cmnuAddNode
            // 
            this.cmnuAddNode.Name = "cmnuAddNode";
            this.cmnuAddNode.Size = new System.Drawing.Size(159, 22);
            this.cmnuAddNode.Text = "&Add node";
            this.cmnuAddNode.Click += new System.EventHandler(this.cmnuAddNode_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.largeIconsToolStripMenuItem,
            this.smallIconsToolStripMenuItem,
            this.detailsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // largeIconsToolStripMenuItem
            // 
            this.largeIconsToolStripMenuItem.Checked = true;
            this.largeIconsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.largeIconsToolStripMenuItem.Name = "largeIconsToolStripMenuItem";
            this.largeIconsToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.largeIconsToolStripMenuItem.Text = "Large icons";
            this.largeIconsToolStripMenuItem.Click += new System.EventHandler(this.largeIconsToolStripMenuItem_Click);
            // 
            // smallIconsToolStripMenuItem
            // 
            this.smallIconsToolStripMenuItem.Name = "smallIconsToolStripMenuItem";
            this.smallIconsToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.smallIconsToolStripMenuItem.Text = "Small icons";
            this.smallIconsToolStripMenuItem.Click += new System.EventHandler(this.smallIconsToolStripMenuItem_Click);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // imgLstNodes
            // 
            this.imgLstNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstNodes.ImageStream")));
            this.imgLstNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstNodes.Images.SetKeyName(0, "ram.ico");
            // 
            // gpNodeInfo
            // 
            this.gpNodeInfo.Controls.Add(this.lblSensorCount);
            this.gpNodeInfo.Controls.Add(this.label4);
            this.gpNodeInfo.Controls.Add(this.lblNodeName);
            this.gpNodeInfo.Controls.Add(this.lblNodeId);
            this.gpNodeInfo.Controls.Add(this.label2);
            this.gpNodeInfo.Controls.Add(this.label1);
            this.gpNodeInfo.Location = new System.Drawing.Point(338, 19);
            this.gpNodeInfo.Name = "gpNodeInfo";
            this.gpNodeInfo.Size = new System.Drawing.Size(173, 262);
            this.gpNodeInfo.TabIndex = 2;
            this.gpNodeInfo.TabStop = false;
            this.gpNodeInfo.Text = "Node information";
            this.gpNodeInfo.Visible = false;
            // 
            // lblSensorCount
            // 
            this.lblSensorCount.AutoSize = true;
            this.lblSensorCount.Location = new System.Drawing.Point(88, 58);
            this.lblSensorCount.Name = "lblSensorCount";
            this.lblSensorCount.Size = new System.Drawing.Size(0, 13);
            this.lblSensorCount.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Sensor count:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNodeName
            // 
            this.lblNodeName.AutoSize = true;
            this.lblNodeName.Location = new System.Drawing.Point(88, 41);
            this.lblNodeName.Name = "lblNodeName";
            this.lblNodeName.Size = new System.Drawing.Size(0, 13);
            this.lblNodeName.TabIndex = 3;
            // 
            // lblNodeId
            // 
            this.lblNodeId.AutoSize = true;
            this.lblNodeId.Location = new System.Drawing.Point(88, 25);
            this.lblNodeId.Name = "lblNodeId";
            this.lblNodeId.Size = new System.Drawing.Size(0, 13);
            this.lblNodeId.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(38, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(38, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gpNodeInfo);
            this.groupBox2.Controls.Add(this.lstNodes);
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 296);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connected nodes";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstRules);
            this.groupBox3.Location = new System.Drawing.Point(12, 329);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(517, 306);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rules";
            // 
            // lstRules
            // 
            this.lstRules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHdrState,
            this.colHdrName,
            this.colHdrIsOpenInEditor,
            this.colHdrDetail});
            this.lstRules.ContextMenuStrip = this.contextMenuStripRules;
            this.lstRules.FullRowSelect = true;
            this.lstRules.GridLines = true;
            this.lstRules.Location = new System.Drawing.Point(6, 19);
            this.lstRules.Name = "lstRules";
            this.lstRules.Size = new System.Drawing.Size(505, 227);
            this.lstRules.SmallImageList = this.imgLstStates;
            this.lstRules.StateImageList = this.imgLstStates;
            this.lstRules.TabIndex = 5;
            this.lstRules.UseCompatibleStateImageBehavior = false;
            this.lstRules.View = System.Windows.Forms.View.Details;
            this.lstRules.ItemActivate += new System.EventHandler(this.lstRules_ItemActivate);
            // 
            // colHdrState
            // 
            this.colHdrState.Text = "State";
            this.colHdrState.Width = 133;
            // 
            // colHdrName
            // 
            this.colHdrName.Text = "Name";
            this.colHdrName.Width = 180;
            // 
            // colHdrIsOpenInEditor
            // 
            this.colHdrIsOpenInEditor.Text = "Open in editor?";
            // 
            // colHdrDetail
            // 
            this.colHdrDetail.Text = "Detail";
            this.colHdrDetail.Width = 187;
            // 
            // contextMenuStripRules
            // 
            this.contextMenuStripRules.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runRuleToolStripMenuItem,
            this.stopRuleToolStripMenuItem,
            this.toolStripMenuItem4,
            this.newRuleToolStripMenuItem,
            this.deleteRuleToolStripMenuItem,
            this.editRuleToolStripMenuItem,
            this.renameRuleToolStripMenuItem});
            this.contextMenuStripRules.Name = "contextMenuStrip1";
            this.contextMenuStripRules.Size = new System.Drawing.Size(135, 142);
            // 
            // runRuleToolStripMenuItem
            // 
            this.runRuleToolStripMenuItem.Image = global::netGui.Properties.Resources.Run;
            this.runRuleToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.runRuleToolStripMenuItem.Name = "runRuleToolStripMenuItem";
            this.runRuleToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.runRuleToolStripMenuItem.Text = "&Run rule";
            this.runRuleToolStripMenuItem.Click += new System.EventHandler(this.runRuleToolStripMenuItem_Click);
            // 
            // stopRuleToolStripMenuItem
            // 
            this.stopRuleToolStripMenuItem.Image = global::netGui.Properties.Resources.Pause;
            this.stopRuleToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.stopRuleToolStripMenuItem.Name = "stopRuleToolStripMenuItem";
            this.stopRuleToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.stopRuleToolStripMenuItem.Text = "&Stop rule";
            this.stopRuleToolStripMenuItem.Click += new System.EventHandler(this.stopRuleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(131, 6);
            // 
            // newRuleToolStripMenuItem
            // 
            this.newRuleToolStripMenuItem.Name = "newRuleToolStripMenuItem";
            this.newRuleToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.newRuleToolStripMenuItem.Text = "&New rule";
            this.newRuleToolStripMenuItem.Click += new System.EventHandler(this.newRuleToolStripMenuItem_Click);
            // 
            // deleteRuleToolStripMenuItem
            // 
            this.deleteRuleToolStripMenuItem.Name = "deleteRuleToolStripMenuItem";
            this.deleteRuleToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.deleteRuleToolStripMenuItem.Text = "&Delete rule";
            this.deleteRuleToolStripMenuItem.Click += new System.EventHandler(this.deleteRuleToolStripMenuItem_Click);
            // 
            // editRuleToolStripMenuItem
            // 
            this.editRuleToolStripMenuItem.Name = "editRuleToolStripMenuItem";
            this.editRuleToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.editRuleToolStripMenuItem.Text = "&Edit rule";
            this.editRuleToolStripMenuItem.Click += new System.EventHandler(this.editRuleToolStripMenuItem_Click);
            // 
            // renameRuleToolStripMenuItem
            // 
            this.renameRuleToolStripMenuItem.Name = "renameRuleToolStripMenuItem";
            this.renameRuleToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.renameRuleToolStripMenuItem.Text = "&Rename rule";
            this.renameRuleToolStripMenuItem.Click += new System.EventHandler(this.renameRuleToolStripMenuItem_Click);
            // 
            // imgLstStates
            // 
            this.imgLstStates.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstStates.ImageStream")));
            this.imgLstStates.TransparentColor = System.Drawing.Color.Magenta;
            this.imgLstStates.Images.SetKeyName(0, "Pause.bmp");
            this.imgLstStates.Images.SetKeyName(1, "Critical.bmp");
            this.imgLstStates.Images.SetKeyName(2, "Run.bmp");
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 587);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.Text = "Lavalamp GUI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.nodeMenuStrip.ResumeLayout(false);
            this.gpNodeInfo.ResumeLayout(false);
            this.gpNodeInfo.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.contextMenuStripRules.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MnuItemConnectToTrans;
        private System.Windows.Forms.ToolStripMenuItem MnuItemDisconnectFromTrans;
        private System.Windows.Forms.ListView lstNodes;
        private System.Windows.Forms.ContextMenuStrip nodeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cmnuAddNode;
        private System.Windows.Forms.GroupBox gpNodeInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNodeName;
        private System.Windows.Forms.Label lblNodeId;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader ColCaption;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolNodeSeperater1;
        private System.Windows.Forms.ToolStripMenuItem refreshThisNodeToolStripMenuItem;
        private System.Windows.Forms.Label lblSensorCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader colSensorCount;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem thisNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePadvancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolNodeSeperator2;
        private System.Windows.Forms.ToolStripMenuItem rulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showRuleEditorToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lstRules;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRules;
        private System.Windows.Forms.ToolStripMenuItem newRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllRulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAllRulesToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colHdrName;
        private System.Windows.Forms.ColumnHeader colHdrState;
        private System.Windows.Forms.ImageList imgLstStates;
        private System.Windows.Forms.ColumnHeader colHdrDetail;
        private System.Windows.Forms.ColumnHeader colHdrIsOpenInEditor;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem runRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopRuleToolStripMenuItem;
        private System.Windows.Forms.ImageList imgLstNodes;
        private System.Windows.Forms.ToolStripMenuItem editRuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameRuleToolStripMenuItem;
    }
}

