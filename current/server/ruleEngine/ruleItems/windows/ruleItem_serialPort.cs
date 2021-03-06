using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Misc")]
    public class ruleItem_serialPort : ruleItemBase
    {
        public override string ruleName() { return "Serial port"; }

        public override string caption() { return "Serial port " + opts.portName; }
        [XmlElement]
        public serialPortOptions opts = new serialPortOptions();

        private SerialPort commport = null;
        private string lastInput = "";

        public override Dictionary<string, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("dataIn", new pin { name = "dataIn", description = "Data to send to serial port", direction = pinDirection.input, valueType = typeof(pinDataString) });

            return pinList;
        }

        public override void start()
        {
            commport = new SerialPort(opts.portName, opts.baudRate, opts.Parity, opts.dataBits);
            commport.Handshake = opts.handshake;
            commport.Open();
        }

        public override void stop()
        {
            commport.Close();
            commport.Dispose();
        }

        public override void evaluate()
        {
            IPinData inputData = pinInfo["dataIn"].value;

            if (lastInput == (string)inputData.data)
                return;
            lastInput = (string) inputData.data;

            writeWithDelay(opts.preSend);
            writeWithDelay(lastInput);
            writeWithDelay(opts.postSend);
        }

        private void writeWithDelay(string toWrite)
        {
            // Convert input in to bytes and throw them out the serial port
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(toWrite);
            foreach (byte toSend in bytes)
            {
                commport.Write(new byte[] { toSend }, 0, 1);
                Thread.Sleep(opts.interCharDelayMS);
            }            
        }

        public override IFormOptions setupOptions()
        {
            return opts;
        }
    }
    [Serializable]
    public class serialPortOptions : BaseOptions
    {
        public string portName = "com2";
        public int baudRate = 2400;
        public Parity Parity = Parity.None;
        public int dataBits = 8;
        public Handshake handshake = Handshake.None;
        public int interCharDelayMS = 0;

        public string preSend = "";
        public string postSend = "";

        public override string typedName
        {
            get { return "SerialPort"; }
        }

    }
}