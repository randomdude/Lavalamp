using System;

namespace lavalampService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using ServiceStack.Text;
    using ServiceStack.WebHost.Endpoints;
    
    using lavalamp;

    using ruleEngine;
    using ruleEngine.ruleItems;

    public class lavalampHost : ServiceStack.WebHost.Endpoints.AppHostBase
    {
        public lavalampHost()
            : base("lavalampHost", typeof(lavalampRuleInfo).Assembly)
        {
        }

        private static T DeserializeAnonymousType<T>(T template, string json) where T : class
        {
            TypeConfig<T>.EnableAnonymousFieldSetters = true;
            return JsonSerializer.DeserializeFromString(json, template.GetType()) as T;
        }

        public override void Configure(Funq.Container container)
        {
            IDictionary<rule, Timer> runningRules = new Dictionary<rule, Timer>();
            IRuleRepository ruleRepo = new ruleRepository(@"C:\lavalamp\rules");
            IRuleItemRepository ruleItemRepo = new ruleItemRepository();
            this.Register(ruleRepo);
            this.Register(ruleItemRepo);
            this.Register(runningRules);
            this.Config.UseBclJsonSerializers = true;
            JsConfig.Reset();
            JsConfig<pin>.SerializeFn = p => string.Format("{{\"direction\":\"{0}\",\"name\":\"{1}\",\"linkedto\":\"{2}\",\"description\":\"{3}\" }}",
                                                            p.direction,p.name, p.linkedTo.id.ToString(), p.description);
            JsConfig<pin>.DeSerializeFn = s =>
                {
                   var anomData = DeserializeAnonymousType(
                        new
                            {
                                direction = default(string),
                                name = default(string),
                                linkedto = default(string),
                                description = default(string)
                            },
                        s); return new pin() 
                                {
                                    direction = (pinDirection)Enum.Parse(typeof(pinDirection),anomData.direction),
                                    description = anomData.description,
                                    linkedTo = new pinGuid(anomData.linkedto),
                                    name =  anomData.name
                                }; 
                };
            JsConfig<pinGuid>.SerializeFn = guid => guid.id.ToString();
            JsConfig<pinGuid>.DeSerializeFn = s => new pinGuid(s);
            Routes.Add<lavalampRuleInfo>("/rule");
            Routes.Add<lavalampRuleInfo>("/rule/{name}");
            Routes.Add<lavalampRuleItemInfo>("/ruleItem");
            Routes.Add<lavalampRuleItemInfo>("/ruleItem/{name}");

            /*     RequestFilters.Add((httpReq,httpRes,requestDto) =>
                     {

                         var sessionId = httpReq.GetCookieValue("user-session");
                         if (sessionId == null)
                         {
                             httpRes.ReturnAuthRequired();
                             httpRes.Close();
                         }

                         if(httpReq.HttpMethod == "POST")
                         {

                             string callback = httpReq.GetParam("Register") ?? httpReq.GetParam("Unregister");
                             if (string.IsNullOrEmpty(callback))
                                 return;
                             Type callbackRegister = Type.GetType(callback.Substring(0,callback.IndexOf('.')));
                             if (callbackRegister == null || !callbackRegister.IsDefined(typeof(IHooked), true))
                                 return;
                             MethodInfo callbackHandlerMethod = callbackRegister.GetMethod(callback.Substring(callback.IndexOf('.') + 1, callback.Length - 1));
                             callbackHandlerMethod.Invoke();
                         }
                     });*/
        }
        public override void  Dispose()
        {
            try
            {
                IDictionary<rule, Timer> runningRules = this.TryResolve<IDictionary<rule, Timer>>();
                foreach (var runningRule in runningRules)
                {
                    runningRule.Key.stop();
                    runningRule.Value.Dispose();
                }
            }
            finally
            {
                base.Dispose();
            }
        }
    }

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //setup mappings
            AutoMapper.Mapper.CreateMap<ruleItemBase, lavalampRuleItemInfo>().ForMember(
                    dest => dest.name, opt => opt.MapFrom(source => source.ruleName())).ForMember(
                    dest => dest.state, opt => opt.MapFrom(source => source.isErrored ? ruleState.errored : ruleState.running)).ForMember(
                    dest => dest.opts, opt => opt.MapFrom(source => source.ruleItemOptions())).ForMember(
                    dest => dest.guid, opt => opt.MapFrom(source => source.serial.ToString())).ForMember(
                    dest => dest.caption, opt => opt.MapFrom(source => source.caption())).ForMember(
                    dest => dest.background, opt => opt.Ignore()).ForMember(
                    dest => dest._category, opt => opt.MapFrom(source => source.category())).ForMember(
                    dest => dest.location, opt => opt.MapFrom(source => source.location)).ForMember(
                    dest => dest.pinInfo, opt=> opt.MapFrom(source => source.pinInfo)) ;

            AutoMapper.Mapper.CreateMap<rule, lavalampRuleInfo>().ForMember(
                    dest => dest.name, opt => opt.MapFrom(source => source.name)).ForMember(
                        dest => dest.state, opt => opt.MapFrom(source => source.state)).ForMember(
                        dest => dest.details, opt => opt.MapFrom(source => source.details)).ForMember(
                        dest => dest.ruleItems, opt => opt.MapFrom(source => source.ruleItems.Values.ToList())).ForMember(
                        dest => dest.preferredHeight, opt => opt.MapFrom(source => source.preferredHeight)).ForMember(
                        dest => dest.preferredWidth, opt => opt.MapFrom(source => source.preferredWidth));

                new lavalampHost().Init();
        }

    }
}