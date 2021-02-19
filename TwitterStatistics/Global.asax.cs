using Autofac;
using Autofac.Integration.WebApi;
using JH.Twitter;
using JH.Twitter.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TwitterStatistics
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DIConfig.RegisterDependencies();
            var twitterManager = DIConfig.container.Resolve<ITweetManager<TwitterStreamResponse>>();
            HostingEnvironment.QueueBackgroundWorkItem(ct => twitterManager.InitiateQueueService(Endpoints.SampleStreamEndpoint));
            Task.Run(() => { twitterManager.ProcessQueueService(DIConfig.container.Resolve<IMessageProcessor>()); });
        }
    }
}
