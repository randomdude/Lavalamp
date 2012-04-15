using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
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

        public override Form ruleItemOptions()
        {
            frmWeatherOptions frm = new frmWeatherOptions(options);
            frm.Closed += frm_close;
            return frm;
        }

        private void frm_close(object sender, EventArgs e)
        {
            frmWeatherOptions frm = (frmWeatherOptions) sender;
            if (frm.DialogResult == DialogResult.OK)
                options = frm.SelectedOption;
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

            HttpWebRequest weatherRequest = (HttpWebRequest) WebRequest.Create(@"http://www.google.com/ig/api?weather=" + options.city);
            HttpWebResponse response = (HttpWebResponse) weatherRequest.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                errorHandler(new HttpException((int)response.StatusCode,"Could not connect to web service"));
            string toRet = "";
            using(XmlTextReader reader = new XmlTextReader(response.GetResponseStream()))
            {
                while(reader.Read())
                {
                    if (reader.Name == options.selectRead)
                    {
                        toRet = reader.GetAttribute(0);
                        break;
                    }
                } 
            }
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(toRet, this, pinInfo["weather"])));

        }
    }

    public class WeatherOptions
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
    }
    
}
