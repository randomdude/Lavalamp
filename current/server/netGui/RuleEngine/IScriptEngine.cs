using System.Collections.Generic;

namespace netGui.RuleEngine
{
    public interface IScriptEngine
    {
        string getDescription();
        string getCategory();
        void evaluateScript();
        Dictionary<string, string> parameters { get; set; }
        Dictionary<string, pin> getPinInfo();
    }
}