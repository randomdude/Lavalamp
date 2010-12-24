using System.Collections.Generic;

namespace ruleEngine.ruleItems
{
    public interface IScriptEngine
    {
        string getDescription();
        string getCategory();
        void evaluateScript();
        Dictionary<string, string> parameters { get; set; }
        Dictionary<string, ruleEngine.pin> getPinInfo();
    }
}