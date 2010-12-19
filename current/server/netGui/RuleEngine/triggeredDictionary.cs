using System;
using System.Collections.Generic;
using netGui.RuleEngine.ruleItems.windows;

namespace netGui.RuleEngine
{
    // TODO: Remove this class, refactoring the pinInfo elsewhere.
    public class triggeredDictionary : Dictionary<string, pinData>
    {
        public Dictionary<String, pin> pinInfo = null;
        public bool enabled = true;
    }
}
