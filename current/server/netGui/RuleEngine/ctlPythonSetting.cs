using System;
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
            textBox1.Text = defaultValue;
            prompt = label1.Text = newPrompt;
        }
    }
}
