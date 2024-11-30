using NATSLoadTester.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace NATSLoadTester.Infrastructure.Messaging
{
    public sealed class MessageBusMessage : IMessageBusMessage
    {
        public MessageBusMessage(string subject, string replyTo, Dictionary<string, string>? headers = null, object? data = null) 
        { 
            Subject = subject;
            ReplyTo = replyTo;
            Headers = headers;
            Data = data;
        }

        public string Subject { get; private set; }

        public string ReplyTo { get; private set; }

        public Dictionary<string,string>? Headers { get; private set; }

        public object? Data { get; private set; }
    }
}
