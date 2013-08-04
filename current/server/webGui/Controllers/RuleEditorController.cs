

namespace webGui.Controllers
{
    using System.Linq;

    using Ninject;
    using dform.NET;
    using lavalamp;
    using ruleEngine;
    using webGui.Controllers.Models;

    using System.Web.Mvc;

    public class RuleEditorController : baseController
    {

        // GET: /RuleEditor/
        [HttpGet]
        public ActionResult Index()
        {
            var client = MvcApplication.Kernal.Get<IRuleRepository>();
          //  var ruleItems = client.getAllRules(false);
            return this.View(new ruleEditorModel(true));
        }

        //
        // POST: /RuleEditor
        [HttpPost]
        public ActionResult Index(string selectedRule)
        {
            var ruleReposistory = MvcApplication.Kernal.Get<IRuleRepository>();
            var item = ruleReposistory.getRule(selectedRule);

            var ruleItems = item.getRuleItems();
            
            
            ruleEditorModel model = new ruleEditorModel(false)
            {
                currentRule = item,
                ruleItems = ruleItems.ToList()
            };
            return this.View(model);
        }

        [HttpGet]
        public ContentResult getRuleItems(string selectedRule)
        {
            var ruleReposistory = MvcApplication.Kernal.Get<IRuleRepository>();
            var rule = ruleReposistory.getRule(selectedRule);
           
            var data =
                rule.getRuleItems().Select(
                    i =>
                    "{ \"name\" : \"" + i.ruleName() + "\", \"caption\": \"" + i.caption()
                    + "\", \"backgroundImg\": \"\" , \"optionsForm\": "
                    + (i.setupOptions() != null ? DFormSerializer.serialize(i.setupOptions()) : "{}") + ",\"position\":{\"x\":"
                    + i.location.X + ",\"y\":" + i.location.Y + " }, \"pins\":[" + i.pinInfo.Select(p => "{\"pinid\": \"" + p.Value.serial.id.ToString() + 
                        "\", \"pin\":{ \"direction\":\"" + p.Value.direction + "\", \"description\":\"" + p.Value.description + "\", \"connected\":\"" + p.Value.linkedTo.id.ToString() + "\"} }")
                        .Aggregate((x,y) => x + "," + y) + 
                    "] } ").Aggregate((i,n) => i + "," + n);

            return new ContentResult
            {
                Content = "[" + data + "]",
                ContentType = "application/json"
            };
        }

        [HttpPost]
        public ActionResult saveRule(ruleEditorModel model)
        {
            if (this.ModelState.IsValid)
            {
                ruleClient client = new ruleClient();
                if (model.newRule)
                    client.Post<lavalampRuleInfo>("rule", model.currentRule);
                else
                    client.Put<lavalampRuleInfo>("rule", model.currentRule);
            }
            return this.View("Index", model);
        }

    }
}
