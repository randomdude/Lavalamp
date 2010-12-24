namespace ruleEngine
{
    // TODO: WHAT IN THE HOLY NAME OF FUCK AND ALL THAT IS GOOD IS THIS HIDEOUS PROFANITY?!
    public class delegatePack
    {
        // bwahhaha silly labels!
        public delegatePack(rule.GetLineChainFromGuidDelegate a, rule.GetRuleItemFromGuidDelegate b, rule.PinFromGuidDelegate c, rule.AddLineChainToGlobalPoolDelegate d, rule.AddRuleItemToGlobalPoolDelegate e, rule.AddPinToGlobalPoolDelegate f, rule.AddctlRuleItemWidgetToGlobalPoolDelegate g, rule.GetPinFromNameDelegate h, rule.GetctlRuleItemWidgetFromGuidDelegate i)
        {
            GetLineChainFromGuid = a;
            GetRuleItemFromGuid=b;
            GetPinFromGuid=c;
            AddLineChainToGlobalPool=d;
            AddRuleItemToGlobalPool=e;
            AddPinToGlobalPool=f;
            AddctlRuleItemWidgetToGlobalPool = g;
            GetPinFromName = h;
            GetCtlRuleFromGuid = i;
        }
        public rule.GetLineChainFromGuidDelegate GetLineChainFromGuid;
        public rule.GetRuleItemFromGuidDelegate GetRuleItemFromGuid;
        public rule.PinFromGuidDelegate GetPinFromGuid;
        public rule.GetctlRuleItemWidgetFromGuidDelegate GetCtlRuleFromGuid;

        public rule.AddLineChainToGlobalPoolDelegate AddLineChainToGlobalPool;
        public rule.AddRuleItemToGlobalPoolDelegate AddRuleItemToGlobalPool;
        public rule.AddPinToGlobalPoolDelegate AddPinToGlobalPool;
        public rule.AddctlRuleItemWidgetToGlobalPoolDelegate AddctlRuleItemWidgetToGlobalPool;

        public rule.GetPinFromNameDelegate GetPinFromName;
    }
}