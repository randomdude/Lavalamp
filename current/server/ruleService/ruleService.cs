using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;
using lavalamp.Properties;
using ruleEngine;

namespace lavalamp
{
    public class ruleService : IRuleRepositoryService
    {
        private List<lavalampRuleInfo> _rules;
        public List<lavalampRuleInfo> GetAllRules()
        {
            return _rules ?? (_rules = loadRules());
        }

        public void AddNewRule(lavalampRuleInfo newRule)
        {
            if (_rules == null)
               _rules = loadRules();
            _rules.Add(newRule);
            saveToDisk(newRule);
        }

        private void saveToDisk(lavalampRuleInfo newRule)
        {
            newRule;
            throw new System.NotImplementedException();
        }


        public void DeleteRule(lavalampRuleInfo toDelRule)
        {
            if (_rules == null)
                _rules = loadRules();
            if (!_rules.Any(r => r.name == toDelRule.name))
                throw new FaultException<ruleDoesNotExist>(new ruleDoesNotExist(toDelRule.name));

            _rules.Remove(toDelRule);
            // we presume it hasn't actually been saved on the server if it doesn't exist
            if (File.Exists(ConfigurationManager.AppSettings["RulePath"] + @"\" + toDelRule.name + ".rule"))
                File.Delete(ConfigurationManager.AppSettings["RulePath"] + @"\" + toDelRule.name + ".rule");
        }

        public void SaveListOfRules(List<lavalampRuleInfo> rules)
        {
            if (_rules == null)
                _rules = loadRules();
            
            foreach (var rule in rules)
            {
                IRule currentlySaved = _rules.FirstOrDefault(r => r.name == rule.name);
                if (currentlySaved == null)
                {
                    AddNewRule(rule);
                }
                else
                {
                    //todo check versioning here and error if more recent.
                    currentlySaved = rule;
                    currentlySaved.saveToDisk(ConfigurationManager.AppSettings["RulePath"]);
                }
            }

        }

        private List<lavalampRuleInfo> loadRules()
        {
            
            if (!Directory.Exists((string) Settings.Default["RulePath"]))
                Directory.CreateDirectory(ConfigurationManager.AppSettings["RulePath"]);

            DirectoryInfo rulesDir = new DirectoryInfo((string)Settings.Default["RulePath"]);

            FileInfo[] files = rulesDir.GetFiles();

            List<lavalampRuleInfo> rules = new List<lavalampRuleInfo>(files.Length);

            foreach (var file in files)
            {
                try
                {
                    StreamReader thisFileReader;
                    XmlSerializer mySer = new XmlSerializer(typeof(rule));
                    AutoMapper.Mapper.CreateMap<rule , lavalampRuleInfo>()
                        .ForMember(dest => dest.name , opt => opt.MapFrom(source => source.name))
                        .ForMember(dest => dest.state , opt => opt.MapFrom(source => source.state));
                    using (thisFileReader = new StreamReader(file.FullName))
                    {
                        rule toMap = (rule) mySer.Deserialize(thisFileReader);
                        rules.Add(AutoMapper.Mapper.Map<lavalampRuleInfo>(toMap));
                    }
                }
                catch (SerializationException ex) {}
            }
            return rules;
        }
    }
}
