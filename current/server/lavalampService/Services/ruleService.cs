

namespace lavalamp
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;

    using ruleEngine;
    using ServiceStack.ServiceInterface;

    public class ruleService : RestServiceBase<lavalampRuleInfo>, IPushService
    {
        private static List<lavalampRuleInfo> _rules;

        public override object OnPost(lavalampRuleInfo newRule)
        {
            if (_rules == null)
                _rules = loadRules();
            _rules.Add(newRule);
            IRuleRepository repo = this.GetAppHost().TryResolve<IRuleRepository>();
            
            rule actual = new rule(newRule.name);
            repo.saveRule(actual);
            InformOtherClients(newRule);
            return null;
        }

        public override object OnPut(lavalampRuleInfo save)
        {
            if (_rules == null)
                _rules = loadRules();

            if (save == null || string.IsNullOrEmpty(save.name))
                return _rules;

            lavalampRuleInfo serverRule = _rules.Find(i => i.name == save.name);
            IRuleRepository repo = this.GetAppHost().TryResolve<IRuleRepository>();
            var ruleToPut = repo.getRule(save.name);

            if (ruleToPut == null)
                return _rules;

            if (!string.IsNullOrEmpty(save.newName))
            {
                repo.changeRuleName(ruleToPut, save.newName);
                serverRule.name = save.newName;
            }

            if (save.state != serverRule.state)
            {
                IDictionary<IRule, Timer> runningRules = this.GetAppHost().TryResolve<IDictionary<IRule, Timer>>();
                switch (save.state)
                {
                    case ruleState.running:
                        {
                            ruleToPut.start();
                            Timer ruling = new Timer(this.startRule, ruleToPut, 0, 100);
                            runningRules.Add(ruleToPut, ruling);
                    
                        }
                        break;
                    case ruleState.stopped:
                        {
                            ruleToPut.stop();
                            Timer timer = runningRules[ruleToPut];
                            timer.Dispose();
                            runningRules.Remove(ruleToPut);
                        }
                        break;
                    default:
                        ruleToPut.state = ruleState.errored;
                        break;
                }
                serverRule.state = ruleToPut.state;
            }
            return _rules;
        }
        
        public override object OnDelete(lavalampRuleInfo toDelRule)
        {
            if (_rules == null)
                _rules = loadRules();
            if (toDelRule == null || string.IsNullOrEmpty(toDelRule.name))
                throw new ruleDoesNotExist("no name specified");

            IRuleRepository repo = this.GetAppHost().TryResolve<IRuleRepository>();
            IRule actualToDel = repo.getRule(toDelRule.name);

            if (actualToDel == null)
                throw new ruleDoesNotExist("no name specified");

            repo.deleteRule(actualToDel);

            InformOtherClients(toDelRule);
            return _rules;
        }

        public void saveListOfRules(List<lavalampRuleInfo> rules)
        {
            if (_rules == null)
                _rules = loadRules();
            IRuleRepository repo = this.GetAppHost().TryResolve<IRuleRepository>();
            foreach (var rule in rules)
            {
                if (string.IsNullOrEmpty(rule.name))
                    continue;

                IRule currentlySaved = repo.getRule(rule.name);
                if (currentlySaved == null)
                {
                    currentlySaved = new rule(rule.name);
                    repo.saveRule(currentlySaved);
                    InformOtherClients(rule);
                }
                else
                {
                    //to-do check versioning here and error if more recent.
                    currentlySaved.saveToDisk(ConfigurationManager.AppSettings["RulePath"]);
                }
            }
          

        }


        public override object OnGet(lavalampRuleInfo request)
        {
            //if(_rules == null) 
                _rules = this.loadRules();
            if (!string.IsNullOrEmpty(request.name))
            { 
                var toRet = _rules.FirstOrDefault(l => (string.IsNullOrEmpty(request.name) || l.name == request.name) && 
                                                  (!request.state.HasValue || l.state == request.state));
                return toRet;
            }
            
            return _rules;
        }

        private List<lavalampRuleInfo> loadRules()
        {
            //if (!Directory.Exists((string)Default["RulePath"])) Directory.CreateDirectory(ConfigurationManager.AppSettings["RulePath"]);

            IRuleRepository repo = this.GetAppHost().TryResolve<IRuleRepository>();

            var rules = repo.getAllRules(false).Cast<rule>();

            return AutoMapper.Mapper.Map<IEnumerable<rule>, List<lavalampRuleInfo>>(rules);
        }

        public void InformOtherClients(object obj)
        {
          //  ICallbackRegister register = this.GetAppHost().TryResolve<ICallbackRegister>();
       //    InformOtherClients(obj, register.Get().where(r => r.Id != this.GetSession().Id && r.CallbackService == this);
           //throw new System.NotImplementedException();
        }
        /*private void InformOtherClients(object obj, List<registee> register)
        {
         //   foreach(registee toInform in register)
          //      registee.push(obj);
            
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void startRule(object obj)
        {
            rule r = (rule)obj;
            r.advanceDelta();
        }

    }
}
