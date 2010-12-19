﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using netGui.RuleEngine;
using netGui.RuleEngine.ruleItems;

namespace ruleItems_winamp
{
    public class ruleItem_winamp_base : ruleItemBase
    {
        private object lastInput;

        private NamedPipeClientStream myPipe = null ;
        public virtual char[] Cmd
        {
            get { return new char[] { (char)0x01 };}
        }

        public override string ruleName() { return "Stop current song"; }

        public override Image background()
        {
            return Properties.Resources.winamp_stop;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("trigger", new pin { name = "trigger", description = "input pin", direction = pinDirection.input });
            return pinList;
        }

        public override void evaluate()
        {
            try
            {
                if ((bool)pinInfo["trigger"].value.getData() == true && lastInput != pinInfo["trigger"].value.getData())
                {
                    myPipe = new NamedPipeClientStream(".", "lavalamp winamp control", PipeDirection.InOut,
                                                       PipeOptions.None);
                    myPipe.Connect(500);
                    StreamWriter myWriter = new StreamWriter(myPipe);
                    myWriter.AutoFlush = true;
                    myWriter.Write(Cmd);
                    myPipe.Close();
                }
                lastInput = pinInfo["trigger"].value;
            }
            catch (ObjectDisposedException)
            {
                // todo - add option to ignore errors / errored state / etc
                MessageBox.Show("Unable to contact winamp. Is it running? Is the plugin installed OK?");
            }
            catch (IOException)
            {
                // todo - add option to ignore errors / errored state / etc
                MessageBox.Show("Unable to contact winamp. Is it running? Is the plugin installed OK?");
            }
            catch (System.TimeoutException)
            {
                MessageBox.Show("Unable to contact winamp. Is it running? Is the plugin installed OK?");
            }
        }

        public ruleItem_winamp_base()
        {
            //Dictionary<String, pin> pinInfo = getPinInfo();
            //pinStates.pinInfo = getPinInfo();
            //foreach (String pinName in pinInfo.Keys)
            //    this.pinStates.Add(pinName, false);
        }

        public override void stop()
        {
        }
 
    }
}