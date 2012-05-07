using System.Runtime.Serialization;

namespace lavalamp
{
    [DataContract]
    public class ruleAlreadExists
    {
        private string _rulename;

        public string message
        {
            get { return _rulename + " already exists"; }
        }

        public ruleAlreadExists(string rulename)
        {
            _rulename = rulename;
        }

    }
}
