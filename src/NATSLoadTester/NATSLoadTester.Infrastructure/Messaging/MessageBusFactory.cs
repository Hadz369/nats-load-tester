using NATSLoadTester.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NATSLoadTester.Infrastructure.Messaging
{
    public sealed class MessageBusProviderFactory
    {
        public static IMessageBusProvider GetProvider(string providerName = "nats")
        {
            return NATSProvider.Instance;
        }
    }
}
