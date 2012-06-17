using System.Collections.Generic;

namespace lavalamp
{
    public interface IRuleRepositoryService
    {
        /// <summary>
        /// Returns all the rules which are stored on this server
        /// todo: limit by user or host?
        /// </summary>
        /// <returns></returns>
        List<lavalampRuleInfo> GetAllRules();

        /// <summary>
        /// adds a new rule to the server
        /// todo store against host/user
        /// </summary>
        /// <param name="newRule"></param>

        void AddNewRule(lavalampRuleInfo newRule);

        /// <summary>
        /// deletes a rule on the server
        /// </summary>
        /// <param name="toDelRule"></param>
        void DeleteRule(lavalampRuleInfo toDelRule);


        /// <summary>
        /// saves a list of rules from a client the intended use of this is if
        /// a client has been disconnected from the server and has created more rules
        /// or updated rules locally.
        /// </summary>
        /// <param name="rules"></param>
        void SaveListOfRules(List<lavalampRuleInfo> rules);
    }
}
