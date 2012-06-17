namespace webGui.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using lavalamp;
    using ruleEngine;
    using System.Configuration;

    [HandleError]
    public class HomeController : Controller
    {
        private readonly ruleClient client = new ruleClient(); 
        public HomeController()
        {
            client.SetBaseUri(ConfigurationManager.AppSettings["ServerURL"]);
        }

        public ActionResult Index()
        {
            var r = client.Get<List<lavalampRuleInfo>>("rule");
            return View(r);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult changeRuleState(lavalampRuleInfo item)
        {
            item.state = item.state.Value == ruleState.stopped ? ruleState.running : ruleState.stopped;
            var r = client.Put<List<lavalampRuleInfo>>("rule", item);
             
            return View("Index",r);
        }



        public ActionResult deleteRule(lavalampRuleInfo toDel)
        {
            
            var r = client.Delete<List<lavalampRuleInfo>>("rule/" + toDel.name);
            return View("Index", r);
        }

    }
}
