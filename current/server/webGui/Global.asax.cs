namespace webGui
{
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Ninject;
    using lavalamp;
    using ruleEngine;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static IKernel Kernal { get; private set; }
        public static void registerRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "RuleEditor", // Route name
                "{controller}/{action}",
                new { controller = "RuleEditor", action = "Index"} // Parameter defaults
            );

        }
        public void initDependencyInjection()
        {
            Kernal = new StandardKernel();
            Kernal.Bind<IRuleRepository>().To<serviceRuleRepository>()
                                          .InSingletonScope()
                                          .WithConstructorArgument("path", ConfigurationManager.AppSettings["ServerURL"]);
        }
        protected void Application_Start()
        {
           // AreaRegistration.RegisterAllAreas();

            registerRoutes(RouteTable.Routes);
            initDependencyInjection();
        }
    }
}