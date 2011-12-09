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

        public override System.Drawing.Image background()
        {
            return Properties.Resources.ruleItem_splitter;
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
                    pin.value = pinData;
                    onRequestNewTimelineEvent(new timelineEventArgs(pinData));
                }
                    
            }
        }
    }
}
