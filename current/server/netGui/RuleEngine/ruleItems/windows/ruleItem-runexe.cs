using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Windows.Forms;
using netGui.RuleEngine.ruleItems.itemControls;
using netGui.RuleEngine.ruleItems.windows;

namespace netGui.RuleEngine.ruleItems 
{
    [RuleEngine.ToolboxRule]
    [RuleEngine.ToolboxRuleCategory("Windows tools")]
    public class ruleItem_runexe : ruleItemBase
    {
        public override string ruleName() { return "Execute program"; }

        public delegate void executeItNowDelegate();    // this is used by the control, when the user asks to 'test' configuration by running the target

        private tristate lastState;
        private ctlRunFile controlwidget = null;

        public override System.Drawing.Image background()
        {
            return null;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "trigger to start program", direction = pinDirection.input, type = typeof (pinDataTristate) });

            return pinList;
        }

        public override void evaluate()
        {
            tristate newState = (tristate)pinStates["input1"].getData();

            if (newState != lastState && newState == tristate.yes)
                executeIt();

            lastState = newState;
        }

        private void executeIt()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(controlwidget.filename);
                if (controlwidget.doImpersonate)
                {
                    startInfo.UseShellExecute = false;

                    startInfo.UserName = controlwidget.username;

                    // todo: is using SecureStrings in my app worth it?
                    startInfo.Password = new SecureString();
                    foreach (char c in controlwidget.password.ToCharArray())
                        startInfo.Password.AppendChar(c);
                }
                else
                {
                    startInfo.UseShellExecute = true;
                    startInfo.WindowStyle = controlwidget.windowStyle;
                }
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                throw new Exception("Starting program '" + controlwidget.filename + "' failed, with the error '" + e.Message + "'");
            }
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = base.addMenus( mnuParent );

            return controlwidget.addMenus(toRet);
        }
        
        public ruleItem_runexe()
        {
            controlwidget = new ctlRunFile(new executeItNowDelegate(executeItAsTest));
            this.controls.Add(controlwidget);
        }

        private void executeItAsTest()
        {
            try
            {
                executeIt();
            } 
            catch (Exception ex)
            {
                frmException ohnoes = new frmException(ex);
                ohnoes.ShowDialog();
            }
        }

    }
}
