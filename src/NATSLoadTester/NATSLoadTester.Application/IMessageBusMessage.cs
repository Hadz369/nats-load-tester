using System;
using System.Collections.Generic;
using System.Text;

namespace NATSLoadTester.Application
{
    public interface IMessageBusMessage
    {
        public string Subject { get; }

        public string ReplyTo { get; }

        public Dictionary<string, string>? Headers { get; }

        public object? Data { get; }
    }
}
