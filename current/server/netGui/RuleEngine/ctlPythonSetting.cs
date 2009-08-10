using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace netGui.RuleEngine
{
    public partial class ctlPythonSetting : UserControl
    {
        public String newVal
        {
            get { return textBox1.Text; }
        }

        public string prompt;

        public ctlPythonSetting()
        {
            InitializeComponent();
        }

        public ctlPythonSetting(String newPrompt, String defaultValue)
        {
            InitializeComponent();
            this.textBox1.Text = defaultValue;
            prompt = this.label1.Text = newPrompt;
        }
    }
}
