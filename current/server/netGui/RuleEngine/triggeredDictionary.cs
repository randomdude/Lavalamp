using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public class triggeredDictionary : Dictionary<string, Object>
    {
        public Dictionary<String, ruleItemBase.changeNotifyDelegate> pinChangeHandlers =
            new Dictionary<String, ruleItemBase.changeNotifyDelegate>();
        public ruleItemBase.evaluateDelegate evaluate = null;
        private ruleItemBase.errorDelegate errorDelegate;

        public Dictionary<String, pin> pinInfo = null;
        public bool enabled = true;

        public new Object this[string key]
        {
            set
            {
                // todo- Beware (ie, protect against at some point) stack overflows/stupid users/loops. 

                base[key] = value;

                // evaluate this ruleItem only if the pin we changed was an input pin
                if (pinInfo == null || pinInfo[key].direction == pinDirection.input)
                {
                    if (evaluate != null)
                    {
                        try
                        {
                            evaluate.Invoke();
                        }
                        catch (Exception e)
                        {
                            if (errorDelegate != null)
                                errorDelegate.Invoke(e);
                        }
                    }
                }

                if (pinInfo == null || pinInfo[key].direction == pinDirection.output)
                {
                    // If it was an output pin, or if we don't know what direction it was, we propogate the change out of the pin.

                    // pass new pins to next ruleItem
                    if (pinChangeHandlers != null)
                    {
                        if (pinChangeHandlers.ContainsKey(key) && pinChangeHandlers[key] != null)
                            pinChangeHandlers[key].Invoke();
                    }
                }

                // And now the aesthtic bit - set the pin background to red or transparent when pin is true or false, respectively.
                if (pinInfo!= null && pinInfo[key].imageBox != null)
                {
                    if ((bool)value)
                        pinInfo[key].imageBox.BackColor = Color.Green;
                    else
                        pinInfo[key].imageBox.BackColor = Color.Transparent;
                }

            }
            get
            {
                return base[key];
            }
        }

        public void setErrorHandler(ruleItemBase.errorDelegate newHandler)
        {
            errorDelegate = newHandler;
        }
    }
}
