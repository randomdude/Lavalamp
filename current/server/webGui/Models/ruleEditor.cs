using System.Collections.Generic;

namespace webGui.Controllers.Models
{
    using lavalamp;

    using ruleEngine;

    using System.Linq;
    using System.Text;
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

        public string ruleItemTree()
        {
            StringBuilder builder = new StringBuilder();
            List<string> seenCategories = new List<string>();
            if (ruleItems != null)
                foreach (IRuleItem item in ruleItems.OrderBy(i => i.category()))
                {
                    string category = string.IsNullOrEmpty(item.category()) ? "Misc" : item.category();
                    if(!seenCategories.Contains(category))
                    {
                        if (seenCategories.Any())
                            builder.Append(@"</ul></li>");
                        builder.AppendFormat(@"<li><a>{0}</a><ul>",category);
                        seenCategories.Add(category);
                    }
                    builder.AppendFormat(@"<li><a class='rule-item' data-background-img='{1}', data-caption='{2}' data-options-type-name='{3}'>{0}</a></li>", 
                                            item.ruleName(), 
                                            "",
                                            item.caption(),
                                            item.setupOptions() != null ? item.setupOptions().typedName : "");
                }
            builder.Append(@"</ul></li>");
            return builder.ToString();

        }

    }
}