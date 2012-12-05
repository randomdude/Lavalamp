// -----------------------------------------------------------------------
// <copyright file="IRuleItem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    using ruleEngine.ruleItems;
using System.Diagnostics.Contracts;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ContractClass(typeof(ruleItemContract))]
    public interface IRuleItem
    {
        string ruleName();

        string category();

        string caption();

        IFormOptions setupOptions();

        Point location { get; set; }

        Dictionary<string, pin> pinInfo { get; set; }
    }

    [ContractClassFor(typeof(IRuleItem))]
    public abstract class ruleItemContract : IRuleItem
    {
        
       public string ruleName()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            throw new NotImplementedException();
        }

        [Pure]
       public string category()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            throw new NotImplementedException();
        }

        public string caption()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            throw new NotImplementedException();
        }

        [Pure]
        public IFormOptions setupOptions()
        {
            throw new NotImplementedException();
        }

        public Point location
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, pin> pinInfo
        {
            get
            {
                Contract.Ensures(Contract.Result<Dictionary<string, pin>>() != null);
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

}
