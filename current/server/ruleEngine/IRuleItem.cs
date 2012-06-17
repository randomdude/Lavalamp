// -----------------------------------------------------------------------
// <copyright file="IRuleItem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ruleEngine.ruleItems;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRuleItem
    {
        string ruleName();

        string caption();

        IFormOptions setupOptions();

    }
}
