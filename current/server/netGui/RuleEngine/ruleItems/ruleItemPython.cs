using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IronPython.Hosting;
using netGui.Properties;
using netGui.RuleEngine.ruleItems.windows;

namespace netGui.RuleEngine.ruleItems
{
    // This is a special wrapper class, and so it isn't visible in the toolbar box. It is called specifically
    // when loading a python script.
    public class ruleItem_python : ruleItemBase
    {
        public pythonEngine myEng;
        //private Dictionary<String, pin> pinList;
        public Dictionary<String, String> parameters = new Dictionary<string,string>();
        private String ruleNameString;
        public string category;

        public override string ruleName()
        {
            return ruleNameString;
        }

        public override System.Drawing.Image background()
        {
            return null; // todo - make image, possibly returned by python script
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            return myEng.pinList;
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = base.addMenus(strip1);

            if (toRet.Items.Count > 0)
                toRet.Items.Add("-");

            if (parameters.Count > 0)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem("Item options..");
                newItem.Click += setParameters;
                toRet.Items.Add(newItem);
            }

            return toRet;
        }

        private void setParameters(object sender, EventArgs e)
        {
            frmPythonParameters paramForm = new frmPythonParameters(parameters);
            paramForm.ShowDialog();

            if (paramForm.newParams != null)
                this.parameters = paramForm.newParams;
        }

        public override void evaluate()
        {
            myEng.runPythonFile();
        }

        public ruleItem_python(pythonEngine newEng)
        {
            myEng = newEng;
            // hook up things from the python engine
            ruleNameString = myEng.description;
            category = myEng.category;
            parameters = myEng.Parameters;

            // Initialise the Pins on the control. This will generate a new guid for each pin.
            base.initPins();

            // Create our label
            Label caption = new Label();
            caption.Text = ruleNameString;
            caption.AutoSize = false;
            caption.Size = this.preferredSize();
            caption.Location = new Point(0, 0);
            caption.TextAlign = ContentAlignment.BottomCenter;
            caption.Visible = true;
            controls.Add(caption);
        }
    }
}
