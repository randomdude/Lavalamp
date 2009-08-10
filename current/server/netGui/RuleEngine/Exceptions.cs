using System;

namespace netGui.RuleEngine
{
    class Exceptions : Exception { }
    class PinNotFoundException : Exception
    {
        public PinNotFoundException() : base () { }
        public PinNotFoundException(string message) : base(message) { }        
    }
    class ruleLoadException : Exception
    {
        public ruleLoadException() : base () { }
        public ruleLoadException(string message) : base(message) {}
    }
}
