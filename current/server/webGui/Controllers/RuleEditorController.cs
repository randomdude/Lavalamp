using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace webGui.Controllers
{
    using dform.NET;

    using lavalamp;

    using webGui.Controllers.Models;
    using System.Configuration;

    public class RuleEditorController : baseController
    {
        readonly ruleClient _client = new ruleClient();
        public RuleEditorController()
        {
            this._client.SetBaseUri(ConfigurationManager.AppSettings["ServerURL"]);
        }

        // GET: /RuleEditor/
        [HttpGet]
        public ActionResult Index()
        {
            var ruleItems = _client.Get<List<lavalampRuleItemInfo>>("ruleItem");
            return this.View(new ruleEditorModel(true)
                {
                    ruleItems = ruleItems
                }
            );
        }

        //
        // POST: /RuleEditor
        [HttpPost]
        public ActionResult Index(string selectedRule)
        {
            var ruleItems = _client.Get<List<lavalampRuleItemInfo>>("ruleItem");
            var item = _client.Get<lavalampRuleInfo>("rule/" +selectedRule);
            ruleEditorModel model = new ruleEditorModel(false)
            {
                currentRule = item,
                ruleItems = ruleItems
            };
            return this.View(model);
        }

        [HttpGet]
        public ContentResult getRuleItems(string selectedRule)
        {
            var rule = _client.Get<lavalampRuleInfo>("rule/" + selectedRule);
           
            var data =
                rule.getRuleItems().Select(
                    i =>
                    "{ \"name\" : \"" + i.ruleName() + "\", \"caption\": \"" + i.caption()
                    + "\", \"backgroundImg\": \"\" , \"optionsForm\": {"
                    + DFormSerializer.serialize(i.setupOptions()) + "},\"position\":{\"x\":"
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
