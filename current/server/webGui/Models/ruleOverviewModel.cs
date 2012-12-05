namespace webGui.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using lavalamp;

    using ruleEngine;

    public class ruleOverviewModel : IEnumerable<lavalampRuleInfo>
    {
        private readonly List<lavalampRuleInfo> _list;

        public ruleOverviewModel(List<lavalampRuleInfo> ruleList)
        {
            Contract.Requires(ruleList != null);
            this._list = ruleList;
        }
       
        public IEnumerator<lavalampRuleInfo> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [ContractInvariantMethod]
        public string getStateClass(lavalampRuleInfo info)
        {
            Contract.Requires(info != null);
            if (!info.state.HasValue)
                return @"uncertain";
            switch (info.state)
            {
                case ruleState.running:
                    return @"ok";
                case ruleState.stopped:
                    return @"uncertain";
                case ruleState.errored:
                    return @"error";
                default:
                    return "";
            }

        }

    }
}