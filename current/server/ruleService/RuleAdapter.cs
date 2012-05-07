using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ruleEngine;
using ruleEngine.ruleItems;

namespace lavalamp
{
    //adapter... for the rulEngines rule
    [DataContract]
    public class lavalampRuleInfo
    {
        [DataMember]
        public ruleState state{ get; set; }

        [DataMember]
        public string name {get; set; }
    }

    [DataContract]
    public class lavalampRuleItemInfo
    {
        [DataMember]
        public ruleState state { get; set; }

        [DataMember]
        public string name { get; set; }
    }


    public class RuleService : IRule
    {
        private rule _adapter = new rule();

        public void stop()
        {
            _adapter.stop();
           
        }
        public void start()
        {
            _adapter.start();
        }
        public void saveToDisk(string file)
        {
            _adapter.saveToDisk(file);
            
        }
        public IEnumerable<ruleItemBase> getRuleItems()
        {
            return _adapter.getRuleItems();
        }
    }
}
