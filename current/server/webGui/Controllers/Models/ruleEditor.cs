using System.Collections.Generic;

namespace webGui.Controllers.Models
{
    using lavalamp;

    using ruleEngine;

    public class ruleEditorModel
    {
        public ruleEditorModel(bool newRule)
        {
            this.newRule = newRule;
        }

        public IRule currentRule { get; set; }

        /// <summary>
        /// Is this rule just being created
        /// </summary>
        public bool newRule { get; private set; }

        /// <summary>
        /// All the rule Items available to add to the rule
        /// </summary>
        public List<lavalampRuleItemInfo> ruleItems { get; set; }
    }
}