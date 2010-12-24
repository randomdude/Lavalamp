using System;

namespace ruleEngine
{
    class pinNotFoundException : Exception
    {
        public pinNotFoundException() { }
        public pinNotFoundException(string message) : base(message) { }        
    }
    class ruleLoadException : Exception
    {
        public ruleLoadException() { }
        public ruleLoadException(string message) : base(message) {}
    }
}
