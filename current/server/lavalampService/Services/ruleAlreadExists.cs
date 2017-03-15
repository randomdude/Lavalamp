using System.Runtime.Serialization;

namespace lavalamp
{
    public class ruleAlreadyExists
    {
        private string _rulename;

        public string message
        {
            get { return _rulename + " already exists"; }
        }

        public ruleAlreadyExists(string rulename)
        {
            _rulename = rulename;
        }

    }
}
