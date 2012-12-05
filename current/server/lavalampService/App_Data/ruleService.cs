

namespace lavalamp
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;

    using ruleEngine;
    using ServiceStack.ServiceInterface;

    using ruleEngine.ruleItems;

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

            lavalampRuleInfo serverRule = _rules.Find(i => i.name == save.name);
            IRuleRepository repo = this.GetAppHost().TryResolve<IRuleRepository>();

            if (!string.IsNullOrEmpty(save.newName))
            {
                repo.changeRuleName(repo.getRule(save.name),save.newName);
                serverRule.name = save.newName;
            }

            if (save.state != serverRule.state)
            {
                
                IDictionary<rule, Timer> runningRules = this.GetAppHost().TryResolve<IDictionary<rule, Timer>>();
                rule r = repo.getRule(save.name);
                switch (save.state)
                {
                    case ruleState.running:
                        {
                            r.start();
                            Timer ruling = new Timer(this.startRule, r, 0, 100);
                            runningRules.Add(r,ruling);
                    
                        }
                        break;
                    case ruleState.stopped:
                        {
                            r.stop();
                            Timer timer =  runningRules[r];
                            timer.Dispose();
                            runningRules.Remove(r);
                        }
                        break;
                    default:
                        r.state = ruleState.errored;
                        break;
                }
                serverRule.state = r.state;
            }
            return _rules;
        }
        
        public override object OnDelete(lavalampRuleInfo toDelRule)
        {
            int indexToDel;
            if (_rules == null)
                _rules = loadRules();
            if ((indexToDel =  _rules.FindIndex(r => r.name == toDelRule.name)) < 0)
                throw new ruleDoesNotExist(toDelRule.name);
                // throw new FaultException<ruleDoesNotExist>(new ruleDoesNotExist(toDelRule.name));
            IRuleRepository repo = this.GetAppHost().TryResolve<IRuleRepository>();
            rule actualToDel = repo.getRule(toDelRule.name);
            repo.deleteRule(actualToDel);
            _rules.RemoveAt(indexToDel);

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
                rule currentlySaved = repo.getRule(rule.name);
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
            
            List<rule> rules = repo.getAllRules(false);
            

            List<lavalampRuleInfo> ruleAdapted = new List<lavalampRuleInfo>(rules.Count);


            ruleAdapted.AddRange(rules.Select(r => AutoMapper.Mapper.Map<rule,lavalampRuleInfo>(r)));
            return ruleAdapted;
        }

        public void InformOtherClients(object obj)
        {
      //     ICallbackRegister register = this.GetAppHost().TryResolve<ICallbackRegister>();
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
