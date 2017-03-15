using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lavalampService
{
    using System.Reflection;

    using ServiceStack.ServiceInterface;

    using lavalamp;

    using ruleEngine;
    using ruleEngine.ruleItems;

    public class ruleItemService : RestServiceBase<lavalampRuleItemInfo>
    {
        public override object OnGet(lavalampRuleItemInfo request)
        {
            IRuleItemRepository repo = this.GetAppHost().TryResolve<IRuleItemRepository>();
            if (string.IsNullOrEmpty(request.name))
            {
                
                var allSysRuleItems = repo.getAllRuleItems();

                return allSysRuleItems.Select(r => AutoMapper.Mapper.Map<ruleItemBase, lavalampRuleItemInfo>((ruleItemBase)r)).ToList();
            }
            return repo.getRuleItem(r => r.ruleName() == request.name);

        }
    }
}