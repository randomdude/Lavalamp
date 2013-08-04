namespace webGui.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using lavalamp;

    using ruleEngine;

    public class ruleOverviewModel : IEnumerable<IRule>
    {
        private readonly List<IRule> _list;

        public ruleOverviewModel(List<IRule> ruleList)
        {
            Contract.Requires(ruleList != null);
            this._list = ruleList;
        }

        public IEnumerator<IRule> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [ContractInvariantMethod]
        public string getStateClass(IRule info)
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