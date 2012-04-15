using System;
using System.Collections.Generic;
using ruleEngine;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_splitter : ruleItemBase
    {
        public override string ruleName() { return "Splitter"; }
        private object _prevValue;
        public override System.Drawing.Image background()
        {
            return Properties.Resources.ruleItem_splitter;
        }

        public override string caption()
        {
            return "Splitter";
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "input pin", direction = pinDirection.input, dynamic = true});
            pinList.Add("output2", new pin { name = "output2", description = "input pin is true", direction = pinDirection.output, dynamic =true});
            pinList.Add("output1", new pin { name = "output1", description = "input pin is true", direction = pinDirection.output, dynamic = true});

            return pinList;
        }

        public override void evaluate()
        {
            var input1 = pinInfo["input1"].value.data;
            if (input1 == _prevValue)
                return;
            _prevValue = input1;
            foreach (var pin in pinInfo.Values)
            {

                if (pin.direction == pinDirection.output)
                {
                    IPinData pinData;
                    if (pin.valueType != pinInfo["input1"].valueType)
                    {
                        pin.valueType = pinInfo["input1"].valueType;
                        pin.recreateValue();
                    }
                    pinData = (IPinData)pin.valueType.GetConstructor(new[]
                                                                              {
                                                                                  pin.value.getDataType(),
                                                                                  typeof (ruleItemBase), typeof (pin)
                                                                              })
                                                 .Invoke(new object[] { input1, this, pin });
                    onRequestNewTimelineEvent(new timelineEventArgs(pinData));
                }
                    
            }
        }
    }
}
