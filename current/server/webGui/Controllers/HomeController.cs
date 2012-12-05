namespace webGui.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using lavalamp;
    using ruleEngine;
    using System.Configuration;

    using webGui.Models;

    [HandleError]
    public class HomeController : baseController
    {
        private readonly ruleClient client = new ruleClient(); 
        public HomeController()
        {
            client.SetBaseUri(ConfigurationManager.AppSettings["ServerURL"]);
            
        }

        public ActionResult Index()
        {
            ruleOverviewModel overview = new ruleOverviewModel(client.Get<List<lavalampRuleInfo>>("rule"));
            return View(overview);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult changeRuleState(lavalampRuleInfo item)
        {
            item.state = item.state.Value == ruleState.stopped ? ruleState.running : ruleState.stopped;
           ruleOverviewModel overview = new ruleOverviewModel(client.Put<List<lavalampRuleInfo>>("rule", item));

           return View("Index", overview);
        }



        public ActionResult deleteRule(lavalampRuleInfo toDel)
        {
            
            var r = client.Delete<List<lavalampRuleInfo>>("rule/" + toDel.name);
            return View("Index", r);
        }

    }
}
