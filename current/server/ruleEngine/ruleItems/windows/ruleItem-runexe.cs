using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Xml.Serialization;

namespace ruleEngine.ruleItems
{
    using ruleEngine.Properties;

    [ToolboxRule]
    [ToolboxRuleCategory("Windows tools")]
    public class ruleItem_runexe : ruleItemBase
    {
        public override string ruleName() { return "Execute program"; }
        public override string caption()
        {
            return "Execute " + options.filename;
        }
       

        public delegate void executeItNowDelegate();    // this is used by the control, when the user asks to 'test' configuration by running the target

        private bool lastState;

        RunExeOptions options = new RunExeOptions();

        [XmlElement("FileToRun")]
        public string fileToRun
        {
            get { return options.filename; }
            set { options.filename = value; }
        }

        public override System.Drawing.Image background()
        {
            return Resources.Shortcut.ToBitmap(); ;
        }

        public override IFormOptions setupOptions()
        {
            return options;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "trigger to start program", direction = pinDirection.input });

            return pinList;
        }

        public override void evaluate()
        {
            bool newState = pinInfo["input1"].value.asBoolean();

            if (newState != lastState && newState)
                executeIt();

            lastState = newState;
        }

        private void executeIt()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(options.filename);
                if (options.doImpersonate)
                {
                    startInfo.UseShellExecute = false;

                    startInfo.UserName = options.username;

                    // todo: is using SecureStrings in my app worth it?
                    startInfo.Password = new SecureString();
                    foreach (char c in this.options.password)
                        startInfo.Password.AppendChar(c);
                }
                else
                {
                    startInfo.UseShellExecute = true;
                    startInfo.WindowStyle = options.windowStyle;
                }
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                throw new Exception("Starting program '" + options.filename + "' failed, with the error '" + e.Message + "'");
            }
        }


        private void executeItAsTest()
        {
               executeIt();
        }

    }
    
    public class RunExeOptions : BaseOptions
    {
        public string filename;

        public ProcessWindowStyle windowStyle;

        public string password;

        public string username;

        public bool doImpersonate;

        public override string displayName
        {
            get
            {
                return "Choose Executable...";
            }
        }

        public override string typedName
        {
            get
            {
                return "RunExe";
            }
        }



    }
}
