// -----------------------------------------------------------------------
// <copyright file="RuleRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// A file system repository for lavalamp rules.
    /// files are saved rulesPath\ruleName.rule
    /// </summary>
    public class ruleRepository : IRuleRepository
    {
        private List<rule> _ruleList;
        private readonly object _ruleLock = new object();
        private readonly string _rulesPath;

        private const string RULE_EXT = ".rule";
        /// <summary>
        /// Initializes the repository
        /// </summary>
        /// <param name="rulesPath">the path where lavalamp rules are to be stored. If null the current director is used</param>
        public ruleRepository(string rulesPath)
        {
            _rulesPath = string.IsNullOrEmpty(rulesPath) ? @"\" : rulesPath;
        }

        public string pathToRules 
        { 
            get
            {
                return _rulesPath;
            }
        } 

        public rule getRule(string name)
        {

            if (_ruleList == null)
                _ruleList = this.getAllRules(false);

            return this._ruleList.Exists(r => r.name == name) ? 
                   this._ruleList.First(r => r.name == name) : null;
            
        
        }

        public List<rule> getAllRules(bool forceReload)
        {
            if (_ruleList != null && !forceReload)
                return _ruleList;

            FileInfo[] fileList;
            try
            {
                DirectoryInfo rulesDir = new DirectoryInfo(_rulesPath);
                fileList = rulesDir.GetFiles();
            }
            catch(Exception ex)
            {
                throw new IOException("Unable to read rule files from " + _rulesPath, ex);
            }
            List<rule> newList = new List<rule>(fileList.Length);

            foreach (FileInfo thisFile in fileList)
            {
                try
                {
                    StreamReader thisFileReader;
                    XmlSerializer mySer = new XmlSerializer(typeof(rule));
                    using (thisFileReader = new StreamReader(thisFile.FullName))
                    {
                        // Add our  rule name to our listView, with a .tag() set to the rule object itself.
                        newList.Add((rule)mySer.Deserialize(thisFileReader));
                    }
                }
                catch
                {
                    rule r = new rule(thisFile.Name.Substring(0,thisFile.Name.IndexOf('.')));
                    r.state = ruleState.errored;
                    r.addError(new fileReadError(thisFile.FullName));
                    newList.Add(r);
                }
            }


            lock (_ruleLock)
                _ruleList = newList;

            return _ruleList;
        }

        public void saveRule(rule save)
        {
            save.saveToDisk(_rulesPath);
            lock (_ruleLock)
            {
                if (_ruleList == null)
                    _ruleList = new List<rule>(1);
                _ruleList.Add(save);
            }
        }

        public void changeRuleName(rule save, string newName)
        {
            string newRulePath = _rulesPath + newName + RULE_EXT;
            
            if (File.Exists(newRulePath))
                throw new IOException(newName + " already exists!");
            // if the rule was saved to disk we delete the old file name and recreate it else we just rename the object in memory
            string oldName = save.name;
            string oldRulePath = _rulesPath + oldName + RULE_EXT;
            save.name = newName;
            try
            {
                if (File.Exists(oldRulePath))
                {
                    File.Delete(oldRulePath);
                    save.saveToDisk(newRulePath);
                }
            } // if for some reason this fails we roll back to the previous state
            catch (Exception)
            {
                save.name = oldName;
                if (File.Exists(newRulePath))
                {
                    File.Delete(newRulePath);
                    if (!File.Exists(oldRulePath))
                        save.saveToDisk(oldRulePath);
                }
                throw;
            }
        }

        public void deleteRule(rule toDelRule)
        {
            string fileToDel = _rulesPath + toDelRule.name + RULE_EXT;
            lock (_ruleLock)
            {
                if (File.Exists(fileToDel))
                    File.Delete(fileToDel);
                _ruleList.Remove(toDelRule);
            }
        }
    }
    
    [ContractClass(typeof(ruleRepositoryContract))]
    public interface IRuleRepository
    {

        /// <summary>
        /// Returns the path to the current location where the repository stores its rules
        /// </summary>
        string pathToRules { get; }

        /// <summary>
        /// Returns the rule of this name.
        /// </summary>
        /// <param name="name">the rule item</param>
        /// <returns>The rule item with the matching name or null if it wasn't found</returns>
        rule getRule(string name);

        /// <summary>
        /// returns all the rules in the system
        /// </summary>
        /// <param name="forceReload">this flushes any cached rules</param>
        /// <returns>a list containing all the rules in the system</returns>
        List<rule> getAllRules(bool forceReload);
        
        /// <summary>
        /// Updates or saves a rule. 
        /// </summary>
        /// <param name="save">rule to save</param>
        void saveRule(rule save);

        /// <summary>
        /// Updates a rules name both on the disk and on the rule itself
        /// </summary>
        void changeRuleName(rule save,string newName);



        void deleteRule(rule toDelRule);
    }

    [ContractClassFor(typeof(IRuleRepository))]
    public abstract class ruleRepositoryContract : IRuleRepository
    {
        public string pathToRules
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                throw new NotImplementedException();
            }
        }

        [Pure]
        public rule getRule(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            throw new NotImplementedException();
        }

        [Pure]
        public List<rule> getAllRules(bool forceReload)
        {
            Contract.Ensures(Contract.Result<List<rule>>() != null && Contract.Result<List<rule>>().All(r => r != null));

            throw new NotImplementedException();
        }

        public void saveRule(rule save)
        {
            Contract.Requires(save != null);

            throw new NotImplementedException();
        }

        public void changeRuleName(rule toChange, string newName)
        {
            Contract.Requires(!String.IsNullOrEmpty(newName) && toChange != null);
            Contract.Ensures(toChange.name == newName);
            Contract.EnsuresOnThrow<Exception>(toChange.name == Contract.OldValue(toChange.name));
            Contract.EnsuresOnThrow<Exception>(!File.Exists(pathToRules + newName + ".rule") && File.Exists(pathToRules + toChange.name + ".rule"));
            
            throw new NotImplementedException();
        }

        public void deleteRule(rule toDelRule)
        {
            Contract.Requires(toDelRule != null);
            throw new NotImplementedException();
        }
    }
}
