//using Microsoft.Extensions.DependencyInjection;
//using NATSLoadTester.Application;
//using NATSLoadTester.Infrastructure.Messaging;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace NATSLoadTester.Infrastructure
//{
//    public static class DependencyRegistrar
//    {
//        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
//        {
//            services.AddSingleton<MessageBusBroker>();
//            services.AddSingleton<IMessageBusProvider, NATSProvider>();
//            return services;
//        }
//    }
//}
