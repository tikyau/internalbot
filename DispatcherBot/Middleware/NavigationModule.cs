//using Autofac;
//using DispatcherBot.Dialogs;
//using Microsoft.Bot.Builder.Dialogs.Internals;
//using Microsoft.Bot.Builder.Scorables;
//using Microsoft.Bot.Connector;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace DispatcherBot.Middleware
//{
//    public class NavigationModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            base.Load(builder);

//            // Register NavigationScorable as middleware to intercept every message to the conversation.
//            builder
//                .Register(c => new NavigationScorable(c.Resolve<IDialogStack>(),c.Resolve<IDialogTask>()))
//                .As<IScorable<IActivity,double>>()
//                .InstancePerLifetimeScope();
//        }
//    }
//}