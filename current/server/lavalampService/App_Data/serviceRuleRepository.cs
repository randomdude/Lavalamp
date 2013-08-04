namespace lavalamp
{
    using System.Collections.Generic;
    using AutoMapper;
    using ruleEngine;
    using ServiceStack.ServiceClient.Web;

    
    public class serviceRuleRepository : IRuleRepository
    {
        public string pathToRules { get; private set; }
        private List<IRule> _rules  = new List<IRule>(); 
        readonly ruleClient _client = new ruleClient();

        public serviceRuleRepository(string path)
        {
            pathToRules = path;
            _client.SetBaseUri(path);
 
        }

        public IRule getRule(string name)
        {
            
            var rules = this._client.Get<List<lavalampRuleInfo>>("rule/" + name);
            if (rules.Count == 0 || rules.Count > 1)
                return null;
            if (!_rules.Contains(rules[0]))
                _rules.Add(rules[0]);
            return Mapper.Map<lavalampRuleInfo,IRule>(rules[0]);
        }

        public List<IRule> getAllRules(bool forceReload)
        {
            var rules = this._client.Get<List<lavalampRuleInfo>>("rule");
            _rules = Mapper.Map<List<lavalampRuleInfo>, List<IRule>>(rules);
            
            return  _rules ?? new List<IRule>();
        }

        public void saveRule(IRule save)    
        {
            if (_rules.Contains(save))
                this._client.Put<lavalampRuleInfo>("rule", Mapper.Map<IRule, lavalampRuleInfo>(save));
            else
                this._client.Post<lavalampRuleInfo>("rule", Mapper.Map<IRule, lavalampRuleInfo>(save));
        }

        public void changeRuleName(IRule save, string newName)
        {
            lavalampRuleInfo item = Mapper.Map<IRule, lavalampRuleInfo>(save);
            item.newName = newName;
            this._client.Put<lavalampRuleInfo>("rule",item);
        }

        public void deleteRule(IRule toDelRule)
        {
            if (toDelRule == null || toDelRule.name == null)
                return;
            
            this._client.Delete<List<lavalampRuleInfo>>("rule/" + toDelRule.name);
        }
        private class ruleClient : JsonServiceClient
        {

        }
    }

}

