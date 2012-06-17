using System.Runtime.Serialization;

namespace lavalamp
{
    using System;

    public class ruleDoesNotExist : Exception
    {
        private string _ruleName ;
        public string message
        {
            get { return "Rule does not exist" + _ruleName; }
        }

        public ruleDoesNotExist(string ruleName)
        {
            _ruleName = ruleName;
        }

    }
}