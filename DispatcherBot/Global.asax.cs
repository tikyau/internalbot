using Autofac;
using DispatcherBot.Middleware;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace DispatcherBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.RegisterModules();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        private void RegisterModules()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterModule(new ReflectionSurrogateModule());

            // Wire up all global navigation commands via scorables.
            //builder.RegisterModule<NavigationModule>();

            //builder.RegisterType<NavigationScorable>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.Register(c => new NavigationScorable(c.Resolve<IDialogStack>(), c.Resolve<IDialogTask>()))
                    .As<IScorable<Activity, double>>()
                    .InstancePerLifetimeScope();
            //builder.Update(Conversation.Container);
        }
    }
}
