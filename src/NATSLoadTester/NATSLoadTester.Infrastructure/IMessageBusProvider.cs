using System;
using System.Collections.Generic;
using System.Text;

namespace NATSLoadTester.Infrastructure
{
    public interface IMessageBusProvider: IDisposable
    {
        void Publish(string subject, byte[]? data = null, Dictionary<string, string>? headers = null);
        
        void PublishString(string subject, string message, Dictionary<string, string>? headers = null);
    }
}
