using Autofac;
using Autofac.Integration.WebApi;
using JH.Twitter;
using JH.Twitter.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace TwitterStatistics
{
    public class DIConfig
    {
        public static IContainer container;
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();
            
            RegisterProjectSpecificDependencies(builder);

            // Set the dependency resolver to be Autofac.
            container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterProjectSpecificDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<QueueStorageService<TwitterStreamResponse>>().As<IQueueStorageService<TwitterStreamResponse>>().SingleInstance();
            builder.RegisterType<TwitterStatisticsModel>().As<TwitterStatisticsModel>().SingleInstance();
            
            builder.RegisterType<TwitterMetadata>().As<TwitterMetadata>();
            builder.RegisterType<TwitterRepository>().As<ITwitterRepository>();
            builder.RegisterType<MessageProcessor>().As<IMessageProcessor>();
            builder.RegisterType<TwitterAnalyticsGenerator>().As<IAnalyticsGenerator>();
            builder.RegisterType<QueueStorageService<TwitterStreamResponse>>().As<IQueueStorageService<TwitterStreamResponse>>();
            builder.RegisterType<TweetManager<TwitterStreamResponse>>().As<ITweetManager<TwitterStreamResponse>>();
        }
    }
}