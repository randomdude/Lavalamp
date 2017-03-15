using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Windows.Forms;
using ruleEngine;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems;

namespace ruleItems_winamp
{
    [ToolboxRule]
    [ToolboxRuleCategory("Winamp")]
    public class ruleItem_winamp_currentSong : ruleItemBase
    {
        private object lastInput;

        private NamedPipeClientStream myPipe = null;
        public virtual char[] Cmd
        {
            get { return new char[] { (char)0x06 }; }
        }

        public override string typedName
        {
            get
            {
                return "winampCurrentSong";
            }
        }

        public override string ruleName() { return "Get Current song"; }

        public override Image background()
        {
            return Properties.Resources.winamp_current;
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            Dictionary<string, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("trigger", new pin { name = "trigger", description = "input pin", direction = pinDirection.input });
            pinList.Add("current_song", new pin { name = "current_song", description = "Currently Playing Song", direction = pinDirection.output, valueType = typeof(pinDataString)});
            return pinList;
        }

        public override void evaluate()
        {
            try
            {
                if ((bool)pinInfo["trigger"].value.data && lastInput != pinInfo["trigger"].value.data)
                {
                    myPipe = new NamedPipeClientStream(".", "lavalamp winamp control", PipeDirection.InOut,
                                                       PipeOptions.None);
                    myPipe.Connect(500);
                    StreamWriter myWriter = new StreamWriter(myPipe);
                    StreamReader reader = new StreamReader(myPipe);
                    myWriter.AutoFlush = true;
                    myWriter.Write(Cmd);
                    pinInfo["current_song"].value.data = reader.ReadToEnd();
                    myPipe.Close();
                    onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(pinInfo["current_song"].value)));
                }
                lastInput = pinInfo["trigger"].value;
            }
            catch (ObjectDisposedException e)
            {
                // todo - add option to ignore errors / errored state / etc
                MessageBox.Show("Unable to contact winamp. Is it running? Is the plugin installed OK?");
                errorHandler(e);
            }
            catch (IOException e)
            {
                // todo - add option to ignore errors / errored state / etc
                MessageBox.Show("Unable to contact winamp. Is it running? Is the plugin installed OK?");
                errorHandler(e);
            }
            catch (System.TimeoutException e)
            {
                MessageBox.Show("Unable to contact winamp. Is it running? Is the plugin installed OK?");
                errorHandler(e);
            }
        }

        public override void stop()
        {
        }

        public override IFormOptions setupOptions()
        {
            return null;
        }
    }
}