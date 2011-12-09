using ruleEngine.ruleItems;

namespace ruleEngine.pinDataTypes
{
    class pinDataTrigger : pinDataBool
    {
        public pinDataTrigger(ruleItemBase parentRuleItem, pin newParentPin) : base(parentRuleItem, newParentPin)
        {
        }

        public pinDataTrigger(bool initalVal, ruleItemBase parentRuleItem, pin newParentPin) : base(initalVal, parentRuleItem, newParentPin)
        {
        }
        public  pinDataTrigger(pinDataTrigger cpy) :base(cpy) {}
    }
}
