using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace netGui.RuleEngine.ruleItems.windows
{
    public partial class frmPickProcess : Form
    {
        public bool cancelled = true;
        public string name = "";

        public frmPickProcess()
        {
            InitializeComponent();
        }

        public frmPickProcess(string newName)
        {
            InitializeComponent();
            cboProcessName.Text = name = newName;

            // add running processes to process list
            foreach (Process thisProc in Process.GetProcesses())
                cboProcessName.Items.Add(thisProc.ProcessName);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.name = cboProcessName.Text;

            this.cancelled = false;
            this.Close();
        }
    }
}
