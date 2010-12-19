using System;
using System.Drawing;

namespace netGui.RuleEngine.ruleItems.windows
{
    public abstract class pinData
    {
        private readonly ruleItemBase parentRuleItem;
        private readonly pin parentPin;

// ReSharper disable PublicConstructorInAbstractClass
        // Explicity call this constructor in your inheriting classes!
        public pinData(ruleItemBase parentRuleItem, pin newParentPin)
// ReSharper restore PublicConstructorInAbstractClass
        {
            this.parentRuleItem = parentRuleItem;
            this.parentPin = newParentPin;
        }

        public abstract void setToDefault();
        public abstract object getData();
        public abstract void setData(object newData);
        public new abstract string ToString();
        public abstract Color getColour();

        protected void reevaluate()
        {
            // evaluate this ruleItem only if the pin we changed was an input pin
            if (parentPin.direction == pinDirection.input)
            {
                try
                {
                    parentRuleItem.evaluate();
                }
                catch (Exception e)
                {
                    parentRuleItem.errorHandler(e);
                }
            }

            if (parentPin.direction == pinDirection.output)
            {
                // If it was an output pin, or if we don't know what direction it was, we propagate the change out of the pin.
                // pass new pins to next ruleItem
                parentPin.invokeChangeHandlers();
            }

            // And now the aesthetic bit - set the pin background.
            parentPin.imageBox.BackColor = getColour();
        }

    }
}