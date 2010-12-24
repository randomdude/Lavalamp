using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ruleEngine
{
    public partial class frmPythonParameters : Form
    {
        private List<Control> newControls = new List<Control>();
        public Dictionary<string, string> newParams;

        public frmPythonParameters()
        {
            InitializeComponent();
        }

        public frmPythonParameters(Dictionary<string, string> parameters)
        {
            InitializeComponent();

            Point cursor = new Point(12, 12);

            foreach( String paramName in parameters.Keys )
            {
                ctlPythonSetting newcontrol = new ctlPythonSetting(paramName, parameters[paramName]);

                newcontrol.Visible = true;
                newcontrol.Location = cursor;
                cursor.Y += newcontrol.Height;

                newControls.Add(newcontrol);
                Controls.Add(newcontrol);
            }

            cursor.Y += 20;

            this.btnOK.Top = cursor.Y ;
            this.btnCancel.Top = cursor.Y ;

            this.Height = cursor.Y + 60;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            newParams = new Dictionary<string, string>();
            foreach(ctlPythonSetting thisSetting in newControls)
                newParams.Add(thisSetting.prompt, thisSetting.newVal);

            this.Close();
        }
    }
}
