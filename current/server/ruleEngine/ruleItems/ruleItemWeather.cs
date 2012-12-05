using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Xml;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    using System;

    [ToolboxRule]
    [ToolboxRuleCategory("Environment")]
    public class ruleItemWeather :ruleItemBase
    {
        public WeatherOptions options = new WeatherOptions();

        public override string ruleName()
        {
            return "Weather";
        }

        public override string caption()
        {
            return "Weather";
        }

        public override IFormOptions setupOptions()
        {
            return options;
        }


        public override Dictionary<string, pin> getPinInfo()
        {
            var pins = base.getPinInfo();
            pins.Add("trigger",new pin{name = "trigger", description = "poll", direction = pinDirection.input});
            pins.Add("weather", new pin {name = "weather", description = "", direction = pinDirection.output,valueType = typeof(pinDataString) });
            return pins;
        }

        public override void evaluate()
        {
            if (!pinInfo["trigger"].value.asBoolean())
                return;
            string toRet = "";
            try
            {
                HttpWebRequest weatherRequest = (HttpWebRequest)WebRequest.Create(@"http://www.google.com/ig/api?weather=" + options.city);
                HttpWebResponse response = (HttpWebResponse)weatherRequest.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    errorHandler(new HttpException((int)response.StatusCode, "Could not connect to web service"));
                    return;
                }
                


                using (XmlTextReader reader = new XmlTextReader(response.GetResponseStream()))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == options.selectRead)
                        {
                            toRet = reader.GetAttribute(0);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandler(ex);
                return;
            }
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(toRet, this, pinInfo["weather"])));

        }
    }

    public class WeatherOptions : BaseOptions
    {
        private string _stringRead;

        public string selectRead
        {
            get
            {
                if (string.IsNullOrEmpty(_stringRead))
                    return "temp_c";
                return _stringRead;
            }
            set
            {
                _stringRead = value;
            } 
        }
        private string _city;
        public string city
        {
            get
            {
                if (string.IsNullOrEmpty(_city))
                    return "London";
                return _city;
            }
            set
            {
                _city = value;
            } 
        }


        public override string typedName
        {
            get { return "Weather"; }
        }

    }
    
}
