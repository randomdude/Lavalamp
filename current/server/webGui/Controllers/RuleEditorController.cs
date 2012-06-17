using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webGui.Controllers
{
    using lavalamp;
    using ruleEngine;
    using webGui.Controllers.Models;
    using System.Configuration;

    public class RuleEditorController : Controller
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
        public ActionResult Index(lavalampRuleInfo item)
        {
            var ruleItems = _client.Get<List<lavalampRuleItemInfo>>("ruleItem");
            ruleEditorModel model = new ruleEditorModel(false)
            {
                currentRule = item,
                ruleItems = ruleItems
            };
            return this.View(model);
        }

        [HttpGet]
        public JsonResult getRuleItems(string selectedRule)
        {
            ruleClient client = new ruleClient();
            var rule = client.Get<lavalampRuleInfo>("rule/" + selectedRule);
            return new JsonResult {Data = rule.ruleItems.Select(i => "{ name: '" + i.name + "', caption: '" + i.caption +
                "', backgroundImg: '" + i.background + "' , optionsTypeName: '" + i.opts.typedName + "'}"), JsonRequestBehavior = JsonRequestBehavior.AllowGet};
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
