using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webGui.Controllers
{
    public abstract class baseController : Controller
    {
       protected baseController()
       {
           
       }
       protected override void Initialize(System.Web.Routing.RequestContext requestContext)
       {
           base.Initialize(requestContext);
           ViewData["controller"] = ValueProvider.GetValue("controller").RawValue.ToString();
           ViewData["action"] = ValueProvider.GetValue("controller").RawValue.ToString();
       }
    }
}
