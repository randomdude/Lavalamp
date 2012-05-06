using System.Collections.Generic;
using ruleEngine.Properties;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.basicLogic
{
    [ToolboxRule]
    [ToolboxRuleCategory("unfinished stuff")]
    public class ruleItem_switch : ruleItems.ruleItemBase
    {
        private tristate _lastState = tristate.noValue;

        public override string ruleName()
        {
            return "Switch";
        }

        public override System.Drawing.Image background()
        {
            return Resources.switch_false;
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            var pins = base.getPinInfo();
            pins.Add("inputTrue", new pin { name = "inputTrue", description = "is outputted when true", direction = pinDirection.input, dynamic = true });
            pins.Add("switch", new pin { name = "switch", description = "switches between inputs", direction = pinDirection.input, valueType = typeof(pinDataTypes.pinDataTristate) });
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
            return null;
        }

        public override void evaluate()
        {
            //this is tristate because it will start off as having noValue
            tristate state = (tristate) pinInfo["switch"].value.data;

            if (_lastState == state || state == tristate.noValue)
                return;
   
            _lastState = state;

            pin switchedPin;
            if (state == tristate.yes)
            {
                switchedPin = pinInfo["inputTrue"];
                setBackground(Resources.switch_true);
            }
            else
            {
                switchedPin = pinInfo["inputFalse"];
                setBackground(Resources.switch_false);
            }
            if (switchedPin.valueType != pinInfo["output"].valueType)
               
            {
                pinInfo["output"].valueType = switchedPin.valueType;
                pinInfo["output"].recreateValue();
            }

            var constructorInfo = switchedPin.valueType.GetConstructor(new[]
                                                                           {
                                                                               switchedPin.value.getDataType() , typeof (ruleItemBase) , typeof (pin)
                                                                           });
            if (constructorInfo != null) 
            {
                IPinData pinData = (IPinData)constructorInfo.Invoke(new [] { switchedPin.value.data, this, pinInfo["output"] });
                onRequestNewTimelineEvent(new timelineEventArgs(pinData));
            }
        }

    }
}
