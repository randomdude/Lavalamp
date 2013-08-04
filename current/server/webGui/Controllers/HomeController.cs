namespace webGui.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Ninject;

    using lavalamp;
    using ruleEngine;
    using System.Configuration;
    using webGui.Models;

    [HandleError]
    public class HomeController : baseController
    {

        public ActionResult Index()
        {
            var client = MvcApplication.Kernal.Get<IRuleRepository>();
            ruleOverviewModel overview = new ruleOverviewModel(client.getAllRules(false));
            return View(overview);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult changeRuleState(lavalampRuleInfo item)
        {
             var client = MvcApplication.Kernal.Get<IRuleRepository>();
            item.state = item.state.Value == ruleState.stopped ? ruleState.running : ruleState.stopped;
            client.saveRule(item);
            ruleOverviewModel overview = new ruleOverviewModel(new List<IRule>());

           return View("Index", overview);
        }



        public ActionResult deleteRule(lavalampRuleInfo toDel)
        {
            var client = MvcApplication.Kernal.Get<IRuleRepository>();

            client.deleteRule(toDel);
            return View("Index");
        }

    }
}
