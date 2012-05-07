using System.Runtime.Serialization;

namespace lavalamp
{
    [DataContract]
    public class ruleDoesNotExist
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