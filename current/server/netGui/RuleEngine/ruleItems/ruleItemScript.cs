using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace netGui.RuleEngine.ruleItems
{
    // This is a special wrapper class, and so it isn't visible in the toolbar box. It is called specifically
    // when loading a python script.
    public class ruleItem_script : ruleItemBase
    {
        /// <summary>
        /// The script 'engine' which does all the work.
        /// </summary>
        private IScriptEngine myEng;

        /// <summary>
        /// Expose parameter array, entirely for the use of unit tests.
        /// </summary>
        public Dictionary<string, string> parameters
        {
            get
            {
                return myEng.parameters;
            }
        }

        /// <summary>
        /// the UI toolbox Category of this item.
        /// </summary>
        /// <returns>A string representing the UI category</returns>
        public String getCategory()
        {
            // Since we can't tag the assembly at design-time, we find categories at runtime
            return myEng.getCategory();
        }

        public override string ruleName()
        {
            return myEng.getDescription();
        }

        public override System.Drawing.Image background()
        {
            return null; // todo - make image, possibly returned by python script
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            return myEng.getPinInfo();
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = base.addMenus(strip1);

            if (toRet.Items.Count > 0)
                toRet.Items.Add("-");

            if (myEng.parameters.Count > 0)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem("Item options..");
                newItem.Click += setParameters;
                toRet.Items.Add(newItem);
            }

            return toRet;
        }

        private void setParameters(object sender, EventArgs e)
        {
            frmPythonParameters paramForm = new frmPythonParameters(myEng.parameters);
            paramForm.ShowDialog();

            if (paramForm.newParams != null)
                myEng.parameters = paramForm.newParams;
        }

        public override void evaluate()
        {
            myEng.evaluateScript();
        }

        public ruleItem_script(string sourceFilename)
        {
            // Create a new engine for our python script
            myEng = new PythonEngine(sourceFilename);
 
            // Initialise the Pins on the control. This will generate a new guid for each pin.
            base.initPins();

            // Create our label
            Label caption = new Label();
            caption.Text = this.ruleName();
            caption.AutoSize = false;
            caption.Size = this.preferredSize();
            caption.Location = new Point(0, 0);
            caption.TextAlign = ContentAlignment.BottomCenter;
            caption.Visible = true;
            controls.Add(caption);
        }
    }
}
