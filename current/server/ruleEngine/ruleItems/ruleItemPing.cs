
// -----------------------------------------------------------------------
// <copyright file="ruleItemPingp.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine.ruleItems
{
    using System.Net.NetworkInformation;
    using System.Net;
    using ruleEngine.pinDataTypes;
    using dform.NET;
    using System;

    /// <summary>
    /// 
    /// </summary>
    [ToolboxRule]
    [ToolboxRuleCategory("Internet")]
    public class ruleItemPing : ruleItemBase
    {
        public pingOptions options = new pingOptions();

        private bool _lastVal;

        public override string typedName
        {
            get
            {
                return "Ping";
            }
        }

        public override string ruleName()
        {
            return "Ping";
        }

        public override string caption()
        {
            return "Ping";
        }

        public override System.Collections.Generic.Dictionary<string, pin> getPinInfo()
        {
            var pins =  base.getPinInfo();

            pins.Add("trigger", new pin {direction = pinDirection.input, name = "trigger", valueType = typeof(pinDataTrigger)});
            pins.Add("pingSuccessful", new pin { direction = pinDirection.output, name = "pingSuccessful", valueType = typeof(pinDataBool),description = "Succeeded"});
            pins.Add("pingInfomation", new pin { direction = pinDirection.output, name = "pingInfomation", valueType = typeof(pinDataObject),description = "Ping Infomation"});

            return pins;
        }

        public override IFormOptions setupOptions()
        {
            return this.options;
        }

        public override void evaluate()
        {
            bool thisState = pinInfo["trigger"].value.asBoolean();

            if ((_lastVal != thisState) && thisState)
            {
                Ping check = new Ping();
                
                PingReply reply = check.Send(this.options.addressToPing);

                this.onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(reply.Status == IPStatus.Success, this, pinInfo["pingSuccessful"])));
                string info = "";

                PingDataType data = new PingDataType()
                    {
                        IPAddress = reply.Address.ToString(),
                        DomainName = Dns.GetHostEntry(reply.Address).HostName,
                        PingRecived = reply.Status == IPStatus.Success,
                        TimeTaken = reply.RoundtripTime
                    };

                this.onRequestNewTimelineEvent(new timelineEventArgs(new pinDataObject(data, this, pinInfo["pingInfomation"])));
            }
            _lastVal = thisState;
        }
    }

    public class pingOptions : BaseOptions
    {
        public enum PingInfomation
        {
            Full,
            TimeOnly,
            ResolvedNameOnly,
            ResolvedNameAndTime,
            IPOnly,
            IPAndTime,
            IPAndResolvedName
        }

        public pingOptions()
        {
            addressToPing = "localhost";
        }

        [DFormInput(name = "Address",caption = "Address")]
        public string addressToPing { get; set; }
        [DFormSelection(name = "Infomation",caption = "Information")]
        public PingInfomation pingInfo { get; set; }
        public override string typedName
        {
            get
            {
                return "Ping";
            }
        }
    }

}
