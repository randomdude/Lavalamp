
// -----------------------------------------------------------------------
// <copyright file="ruleItemPingp.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine.ruleItems
{
    using System.Globalization;
    using System.Net.NetworkInformation;
    using System.Net;
    using ruleEngine.pinDataTypes;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ruleItemPing : ruleItemBase
    {
        public pingOptions options = new pingOptions();

        private bool _lastVal;

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
            pins.Add("pingSuccessful", new pin { direction = pinDirection.output, name = "pingSuccessful", valueType = typeof(pinDataBool) });
            pins.Add("pingInfomation", new pin { direction = pinDirection.output, name = "pingInfomation", valueType = typeof(pinDataString) });

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
                if(reply == null || reply.Status != IPStatus.Success)
                {
                    this.onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(false,this, pinInfo["pingSuccessful"])));
                    this.onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(reply == null ? string.Empty : this.options.addressToPing + " failed: " + reply.Status.ToString(), this, pinInfo["pingInfomation"])));
                    
                }
                else
                {
                    this.onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(true, this, pinInfo["pingSuccessful"])));
                    string info = "";
                    switch (this.options.pingInfo)
                    {
                        case pingOptions.PingInfomation.Full:
                            info = Dns.GetHostEntry(reply.Address).HostName + " " + reply.Address + " time=" +
                                   reply.RoundtripTime;
                            break;
                        case pingOptions.PingInfomation.IPOnly:
                            info = reply.Address.ToString();
                        break;
                        case pingOptions.PingInfomation.ResolvedNameOnly:
                            info = Dns.GetHostEntry(reply.Address).HostName;
                        break;
                        case pingOptions.PingInfomation.ResolvedNameAndTime:
                            info = Dns.GetHostEntry(reply.Address).HostName + " time=" + reply.RoundtripTime;
                            break;
                        case pingOptions.PingInfomation.TimeOnly:
                            info = reply.RoundtripTime.ToString(CultureInfo.CurrentUICulture);
                        break;
                        case pingOptions.PingInfomation.IPAndResolvedName:
                            info = Dns.GetHostEntry(reply.Address).HostName + " " + reply.Address;
                        break;
                        case pingOptions.PingInfomation.IPAndTime:
                            info = reply.Address + " time=" + reply.RoundtripTime;
                        break;
                            
                    }
                    this.onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(info, this, pinInfo["pingInfomation"])));
                }
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
        public string addressToPing { get; set; }

        public PingInfomation pingInfo { get; set; }
    }

}
