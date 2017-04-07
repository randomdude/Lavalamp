using System;
using System.Collections.Generic;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;
using ruleEngine.ruleItems.Basic_logic;

namespace ruleEngine.basicLogic
{
    using System.Xml.Serialization;

    [ToolboxRule]
    [ToolboxRuleCategory("unfinished stuff")]
    public class ruleItem_switch : ruleItems.ruleItemBase
    {
        private tristate _lastState = tristate.noValue;

        [XmlElement("SwitchOptions")]
        public switchRuleItemOptions _options;

        public ruleItem_switch()
        {
            _options = new switchRuleItemOptions();
        }

        public ruleItem_switch(switchRuleItemOptions options)
        {
            _options = options; 
        }

        public override string typedName
        {
            get
            {
                return "Switch";
            }
        }

        public override string ruleName()
        {
            return "Switch";
        }

        public override System.Drawing.Image background()
        {
            return null; /*Resources.switch_false;*/
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            var pins = base.getPinInfo();
            if (_options.UseTruePin)
                pins.Add("inputTrue", new pin { name = "inputTrue", description = "is outputted when true", direction = pinDirection.input, dynamic = true });
            pins.Add("switch", new pin { name = "switch", description = "switches between inputs", direction = pinDirection.input, valueType = typeof(pinDataTypes.pinDataTristate) });
            if (_options.UseFalsePin)
                pins.Add("inputFalse", new pin { name = "inputFalse", description = "is outputted when false", direction = pinDirection.input, dynamic = true });
            pins.Add("output", new pin { name = "output", description = "switch's output", direction = pinDirection.output, valueType = typeof(pinDataTypes.pinDataTristate), dynamic = true });
            return pins;
        }

        public override string caption()
        {
            return "Switch";
        }

        public override IFormOptions setupOptions()
        {
            return _options;
        }

        public override void evaluate()
        {
            //this is tristate because it will start off as having noValue
            tristate state = (tristate) pinInfo["switch"].value.data;

            if (_lastState == state || state == tristate.noValue)
                return;
   
            _lastState = state;

            pin switchedPin = null;
            object value = null;
            Type dataType = null;
            if (state == tristate.yes)
            {
                if (_options.UseTruePin)
                {
                    switchedPin = pinInfo["inputTrue"];
                    dataType = switchedPin.valueType;

                }
                else
                {
                    dataType = this._options.DataTypeTrue;
                    value = _options.TrueValue;
                }
            }
            
            else
            {
                if (_options.UseFalsePin)
                {
                    switchedPin = pinInfo["inputFalse"];
                    dataType = switchedPin.valueType;
                }
                else
                {
                    value = _options.FalseValue;
                    dataType = this._options.DataTypeFalse;
                }
            }
            IPinData pinData = null;
 

            if (dataType != pinInfo["output"].valueType)
            {
                pinInfo["output"].valueType = dataType;
                pinInfo["output"].recreateValue();
            }

            if (switchedPin != null)
            {

                var constructorInfo = dataType.GetConstructor(
                    new[] { switchedPin.value.getDataType(), typeof(ruleItemBase), typeof(pin) });

                if (constructorInfo != null)
                {
                    pinData =
                        (IPinData)constructorInfo.Invoke(new[] { switchedPin.value.data, this, pinInfo["output"] });
                }
            }
            else
            {
                var constructorInfo = dataType.GetConstructor(
                    new[] { value.GetType() , typeof(ruleItemBase), typeof(pin) });

                if (constructorInfo != null)
                {
                    pinData =
                        (IPinData)constructorInfo.Invoke(new[] { value, this, pinInfo["output"] });
                }
            }
            if (pinData != null)
            {
                switchTimeLineEventArgs eventData = new switchTimeLineEventArgs(state, pinData);
                onRequestNewTimelineEvent(eventData);
            }

        }

    }

}
